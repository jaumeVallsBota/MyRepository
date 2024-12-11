#Linux Part -> Pre-requisite: sudo apt-get install poppler-utils

$pdfPath = "/home/jaume/Documents/Python_scripts/Powershell/HarryPotter-PiedraFilosofal.pdf"
$textPath = "/home/jaume/Documents/Python_scripts/Powershell/HarryPotter-PiedraFilosofal.txt"
Start-Process -NoNewWindow -Wait -FilePath "pdftotext" -ArgumentList "$pdfPath $textPath"
$texto = Get-Content $textPath
Write-Output $texto

#windows Part 

Install-Module -Name PdfToText

$pdfPath = "/ruta/a/archivo.pdf"
$texto = Get-PdfText -Path $pdfPath
Write-Output $texto