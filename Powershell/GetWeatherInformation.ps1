# Obtener la ubicación del script
$scriptDirectory = $PSScriptRoot

# Cambiar al directorio del script
Set-Location -Path $scriptDirectory

$configPath = Join-Path $scriptDirectory "configWeather.json"
# Cargar configuración
if (-Not (Test-Path $configPath)) {
    throw "El archivo de configuración no existe. Por favor, crea un archivo config.json con la API Key."
}
$config = Get-Content -Path $configPath | ConvertFrom-Json

# Configuración de la clave de API y URL base
$apiKey = $config.apiKey
$baseUrl = "http://api.openweathermap.org/data/2.5/weather"

# Ciudad a consultar
$ciudad = "Sant Boi de Llobregat"
$unidad = "metric" # Cambia a "imperial" para Fahrenheit

# Construcción de la URL con parámetros
$url = $baseUrl+"?q=$ciudad&units=$unidad&appid=$apiKey"

 # Función para mostrar notificaciones
 function Mostrar-Notificacion {
    param (
        [string]$titulo,
        [string]$mensaje
    )
    # Usar notify-send para mostrar una notificación emergente
    $command = "notify-send '$titulo' '$mensaje'"
    Invoke-Expression $command
}

$minTemp = 11   # Temperatura mínima de alerta en °C
$maxTemp = 35  # Temperatura máxima de alerta en °C

# Solicitud HTTP GET
try {
    $response = Invoke-RestMethod -Uri $url -Method Get

    if ($response) {
        $temperatura = $response.main.temp

        # Condiciones de alerta
        if ($temperatura -lt $minTemp) {
            Mostrar-Notificacion -titulo "Alerta Meteorológica" -mensaje "¡La temperatura en $ciudad es muy baja: $temperatura°C!"
        } elseif ($temperatura -gt $maxTemp) {
            Mostrar-Notificacion -titulo "Alerta Meteorológica" -mensaje "¡La temperatura en $ciudad es muy alta: $temperatura°C!"
        } else {
            Write-Output "El clima está dentro de los rangos normales: $temperatura°C"
        }
    } else {
        Write-Output "No se pudo obtener la información del clima."
    }
} catch {
    Mostrar-Notificacion -titulo "Error" -mensaje "Error en la solicitud: $_"
}