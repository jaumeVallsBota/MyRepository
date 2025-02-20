param (
    [string]$csvPath
)

# Verificar si se proporcionó un parámetro
if (-not $csvPath) {
    Write-Host "Error: Debes proporcionar la ruta del archivo CSV como parámetro." -ForegroundColor Red
    exit 1
}

try {
    # Verificar si el archivo existe antes de importarlo
    if (-Not (Test-Path -Path $csvPath -PathType Leaf)) {
        throw "El archivo CSV no existe en la ruta especificada: $csvPath"
    }

    # Intentar importar el archivo CSV con delimitador específico
    $csvData = Import-Csv -Path $csvPath -Delimiter ";" -ErrorAction Stop

    # Verificar si el archivo está vacío
    if ($csvData.Count -eq 0) {
        throw "El archivo CSV está vacío o no contiene datos válidos."
    }

    Write-Host "El archivo CSV se importó correctamente." -ForegroundColor Green
} 
catch [System.IO.FileNotFoundException] {
    Write-Host "Error: No se encontró el archivo CSV en la ruta: $csvPath" -ForegroundColor Red
} 
catch [System.Management.Automation.RuntimeException] {
    Write-Host "Error de ejecución al importar el CSV: $_" -ForegroundColor Red
} 
catch {
    Write-Host "Error inesperado: $_" -ForegroundColor Red
} 
finally {
    if ($csvData) {
        Write-Host "El CSV contiene $($csvData.Count) registros." -ForegroundColor Cyan
    } else {
        Write-Host "No se pudo importar el archivo CSV." -ForegroundColor Yellow
    }
}
