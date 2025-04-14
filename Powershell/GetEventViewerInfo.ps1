$AppName = "NombreDeTuAplicacion"
$LastWeek = (Get-Date).AddDays(-7)

Get-EventLog -LogName Application |
Where-Object {$_.Source -eq $AppName -and ($_.EntryType -eq "Error" -or $_.EntryType -eq "Warning") -and $_.TimeGenerated -ge $LastWeek} |
Group-Object -Property Message, EntryType |
Select-Object @{Name='Mensaje';Expression={$_.Group[0].Message}},
              @{Name='Tipo';Expression={$_.Group[0].EntryType}},
              @{Name='Cantidad';Expression={$_.Count}},
              @{Name='Última Vez';Expression={($_.Group | Sort-Object -Property TimeGenerated -Descending | Select-Object -First 1).TimeGenerated}} |
Sort-Object -Property Cantidad -Descending |
ConvertTo-Json |
Out-File -Path "C:\ruta\a\tus\datos_eventos.json" -Encoding UTF8
