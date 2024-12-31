$Harry = Get-Content -Path "/home/jaume/Documents/Python_scripts/Powershell/HarryPotter-PiedraFilosofal.txt" 

$harry |  ForEach-Object { $_ -replace "Potter", "Valls"   -replace "Harry", "Jaume"} 

