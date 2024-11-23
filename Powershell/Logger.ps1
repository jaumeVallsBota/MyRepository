# Función para inicializar el log
function Initialize-Log {
    param (
        [Parameter(Mandatory)]
        [string]$BasePath,  # Ruta base del directorio donde se guardarán los logs

        [string]$CustomName = "log"  # Nombre personalizado opcional
    )

    # Asegurarse de que el directorio existe
    if (!(Test-Path -Path $BasePath)) {
        New-Item -ItemType Directory -Path $BasePath -Force | Out-Null
    }

    # Generar el nombre del archivo de log con la fecha
    $date = Get-Date -Format "yyyy-MM-dd"
    $global:LogPath = Join-Path -Path $BasePath -ChildPath "$CustomName-$date.log"
    $global:LogHtmlPath = Join-Path -Path $BasePath -ChildPath "$CustomName-$date.html"

    Write-Host "Log file initialized: $global:LogPath"
    Write-Host "HTML log file initialized: $global:LogHtmlPath"
}

# Función para escribir logs con niveles y colores
function Write-Log {
    param (
        [Parameter(Mandatory)]
        [string]$Message,  # Mensaje a registrar

        [ValidateSet("INFO", "WARNING", "ERROR")]
        [string]$Level = "INFO",  # Nivel del log

        [switch]$AppendNewLine,  # Opción para añadir un salto de línea

        [string]$ForegroundColor = "White",  # Color del texto

        [string]$BackgroundColor = "Black"  # Color del fondo
    )

    # Verificar que $global:LogPath está inicializado
    if (-not $global:LogPath) {
        throw "The log file has not been initialized. Please call Initialize-Log first."
    }

    # Formatea el mensaje con la hora actual y el nivel
    $timestamp = Get-Date -Format "yyyy-MM-dd HH:mm:ss"
    $formattedMessage = if ($AppendNewLine) {
        "$timestamp [$Level] - $Message`r`n"
    } else {
        "$timestamp [$Level] - $Message"
    }

    # Escribe el mensaje al archivo de texto
    Add-Content -Path $global:LogPath -Value $formattedMessage

    # Crear o actualizar el archivo HTML
    $htmlMessage = "<p><b>$timestamp [$Level]:</b> $Message</p>"
    if (-not (Test-Path -Path $global:LogHtmlPath)) {
        $htmlHeader = "<html><head><title>Log Output</title></head><body><h1>Log for $timestamp</h1>"
        $htmlFooter = "</body></html>"
        $htmlMessage = "$htmlHeader$htmlMessage$htmlFooter"
        Set-Content -Path $global:LogHtmlPath -Value $htmlMessage
    } else {
        Add-Content -Path $global:LogHtmlPath -Value $htmlMessage
    }

    # Configura los colores y muestra el mensaje en consola
    Write-Host $formattedMessage -ForegroundColor $ForegroundColor -BackgroundColor $BackgroundColor
}

# Función para obtener colores predeterminados por nivel
function Get-LogColors {
    param (
        [ValidateSet("INFO", "WARNING", "ERROR")]
        [string]$Level
    )

    switch ($Level) {
        "INFO" { return @{ ForegroundColor = "Green"; BackgroundColor = "Black" } }
        "WARNING" { return @{ ForegroundColor = "Yellow"; BackgroundColor = "Black" } }
        "ERROR" { return @{ ForegroundColor = "Red"; BackgroundColor = "Black" } }
    }
}


# Inicializar el archivo de log en una carpeta específica con un nombre personalizado
Initialize-Log -BasePath "/home/jaume/Documents/" -CustomName "LogTest"

# Escribir logs con diferentes niveles y colores
# INFO
$colors = Get-LogColors -Level "INFO"
Write-Log -Message "Operación completada correctamente." -Level "INFO" -ForegroundColor $colors.ForegroundColor -BackgroundColor $colors.BackgroundColor 

# WARNING
$colors = Get-LogColors -Level "WARNING"
Write-Log -Message "Este es un aviso." -Level "WARNING" -ForegroundColor $colors.ForegroundColor -BackgroundColor $colors.BackgroundColor

# ERROR
$colors = Get-LogColors -Level "ERROR"
Write-Log -Message "Ocurrió un error crítico." -Level "ERROR" -ForegroundColor $colors.ForegroundColor -BackgroundColor $colors.BackgroundColor
