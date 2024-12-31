Import-Module powershell-yaml

# Obtener la ubicación del script
$scriptDirectory = $PSScriptRoot

# Cambiar al directorio del script
Set-Location -Path $scriptDirectory
#Leer el Yaml de los horoscopos

$configPath = Join-Path $scriptDirectory "config.json"
# Cargar configuración
if (-Not (Test-Path $configPath)) {
    throw "El archivo de configuración no existe. Por favor, crea un archivo config.json con la API Key."
}
$config = Get-Content -Path $configPath | ConvertFrom-Json

# Usar la API Key del archivo de configuración
$pathYaml = $config.yamlPath 
# Ruta al archivo de configuración

$apiKey = $config.apiKey
$apiUrl = "https://api.telegram.org/bot$apiKey"

# Función para cargar el YAML
function Load-YAML {
    param (
        [string]$FilePath
    )
    if (-Not (Test-Path $FilePath)) {
        throw "El archivo YAML no se encuentra en la ruta especificada."
    }

    # Leer y convertir el contenido YAML
    $yamlContent = Get-Content -Path $FilePath -Raw
    $yamlData = ConvertFrom-Yaml -Yaml $yamlContent
    return $yamlData
}

# Función para determinar el signo zodiacal
function Get-ZodiacSign {
    param (
        [datetime]$BirthDate,
        [string]$YamlPath
    )
    # Cargar datos YAML
    $zodiacData = Load-YAML -FilePath $YamlPath

    # Función auxiliar para comprobar si una fecha está dentro de un rango
    function Is-DateInRange {
        param (
            [datetime]$Date,
            [string]$Range
        )
    
        # Dividir el rango en fechas de inicio y fin
        $dates = $Range -split " - "
        $startDate = [datetime]::ParseExact("$($dates[0])/$($Date.Year)", "dd/MM/yyyy", $null)
        $endDate = [datetime]::ParseExact("$($dates[1])/$($Date.Year)", "dd/MM/yyyy", $null)
    
        # Si el rango cruza el año (e.g., "22/12" a "19/01"), ajustar el fin del rango
        if ($endDate -lt $startDate) {
            $endDate = $endDate.AddYears(1)
        }
    
        # Normalizar el año de la fecha de entrada para la comparación
        $normalizedDate = [datetime]::ParseExact($Date.ToString("dd/MM") + "/$($startDate.Year)", "dd/MM/yyyy", $null)
    
        return ($normalizedDate -ge $startDate -and $normalizedDate -le $endDate)
    }

  # Determinar el signo tradicional
    $traditionalSign = $null
    foreach ($sign in $zodiacData.signos_tradicionales.GetEnumerator()) {
        if (Is-DateInRange -Date $BirthDate -Range $sign.Value) {
            $traditionalSign = $sign.Key
            break
        }
    }

    # Determinar el signo actualizado
    $newSign = $null
    foreach ($sign in $zodiacData.signos_con_nuevos.GetEnumerator()) {
        if (Is-DateInRange -Date $BirthDate -Range $sign.Value) {
            $newSign = $sign.Key
            break
        }
    }


    # Resultado
    [PSCustomObject]@{
        FechaNacimiento = $BirthDate.ToString("dd/MM/yyyy")
        SignoTradicional = $traditionalSign
        SignoActualizado = $newSign
    }
}



# Función para enviar mensajes
function Send-TelegramMessage {
    param (
        [string]$chatId,
        [string]$message
    )

    # Verifica que el mensaje no esté vacío
    if ([string]::IsNullOrWhiteSpace($message)) {
        Write-Host "Error: El mensaje está vacío. No se enviará nada."
        return
    }

    $url = "$apiUrl/sendMessage"
    $body = @{
        chat_id = $chatId
        text    = $message
    }

    try {
        Invoke-RestMethod -Uri $url -Method Post -Body ($body | ConvertTo-Json -Depth 2) -ContentType "application/json"
    } catch {
        Write-Host "Error al enviar mensaje: $_"
    }
}


# Función para obtener actualizaciones
function Get-TelegramUpdates {
    param (
        [int]$offset
    )
    $url = "$apiUrl/getUpdates"
    $parameters = @{
        offset = $offset
    }
    $urlfinal = $url+"?offset="+$offset
    try {
        # Añadir parámetros como query string
        $response = Invoke-RestMethod -Uri  $urlfinal -Method Get
        return $response
    } catch {
        Write-Host "Error al obtener actualizaciones: $_"
        return $null
    }
}

# Función para manejar comandos
# Función para manejar comandos
function Handle-Command {
    param (
        $chatId,
        [string]$command,
        [string]$additionalData
    )
    $username = $chatId.username
    if ($username -eq $null) {
        $username = $chatId.first_name
    }

    switch ($command) {
        "/start" {
            $response = "¡Hola $username! Soy tu bot de Telegram. Estoy aquí para ayudarte. Escribe /help para saber qué puedo hacer."
        }
        "/help" {
            $response = "Estos son los comandos disponibles:`n/start - Iniciar el bot`n/help - Mostrar esta ayuda`n/horoscopo - Descubre tu signo zodiacal."
        }
        "/horoscopo" {
            $response = "Dime tu fecha de nacimiento en formato dd/MM/yyyy para decirte tu signo zodiacal."
        }
        default {
            $response = "Lo siento, no reconozco el comando '$command'. Escribe /help para ver los comandos disponibles."
        }
    }
    Send-TelegramMessage -chatId $chatId.id -message $response
}
# Configuración inicial
$offset = 0
$botId = $null  # Se obtendrá automáticamente

# Función para obtener el ID del bot
function Get-BotInfo {
    $url = "$apiUrl/getMe"
    $response = Invoke-RestMethod -Uri $url -Method Get
    return $response.result.id
}

# Obtener el ID del bot al inicio
if (-not $botId) {
    $botId = Get-BotInfo
    Write-Host "ID del bot: $botId"
}

# Actualiza el Offset tras procesar cada mensaje
$lastUpdateId = $null

# Lógica principal
while ($true) {
    try {
        # Obtén actualizaciones desde el último offset
        $updates = Get-TelegramUpdates -offset $offset

        # Verifica si hay resultados
        if ($updates.result -and $updates.result.Count -gt 0) {
            foreach ($update in $updates.result) {
                # Obtén el ID del último mensaje
                $currentUpdateId = $update.update_id

                # Verifica si este update ya fue procesado
                if ($lastUpdateId -eq $currentUpdateId) {
                    continue
                }
                $lastUpdateId = $currentUpdateId

                # Ignorar mensajes enviados por el propio bot
                if ($update.message.from.id -eq $botId) {
                    Write-Host "Ignorando mensaje del bot: $($update.message.text)"
                    continue
                }

                # Información del mensaje
                $chatId = $update.message.chat
                $text   = $update.message.text

                # Manejo especial para el comando /horoscopo
                if ($text -eq "/horoscopo") {
                    Handle-Command -chatId $chatId -command "/horoscopo"
                } elseif ($text -match "^\d{2}/\d{2}/\d{4}$") {
                    try {
                        # Validar y convertir la fecha
                        $birthDate = [datetime]::ParseExact($text, "dd/MM/yyyy", $null)
                        
                        # Obtener los signos zodiacales
                        $zodiacResult = Get-ZodiacSign -BirthDate $birthDate -YamlPath $pathYaml

                        # Construir el mensaje de respuesta
                        $response = @"
Tu fecha de nacimiento es: $($zodiacResult.FechaNacimiento)
Tu signo zodiacal tradicional es: $($zodiacResult.SignoTradicional)
Tu signo zodiacal actualizado es: $($zodiacResult.SignoActualizado)
"@
                    } catch {
                        $response = "Parece que la fecha proporcionada no es válida. Por favor, intenta de nuevo con el formato dd/MM/yyyy."
                    }
                    Send-TelegramMessage -chatId $chatId.id -message $response
                } else {
                    # Maneja comandos y mensajes genéricos
                    Handle-Command -chatId $chatId -command $text
                }

                # Actualiza el offset al último mensaje procesado
                $offset = $currentUpdateId + 1
                Write-Host "Offset actualizado a: $offset"
            }
        } else {
            Write-Host "Sin mensajes nuevos."
        }
    } catch {
        Write-Host "Error: $_"
    }

    # Pausa antes de la siguiente consulta
    Start-Sleep -Seconds 2
}

