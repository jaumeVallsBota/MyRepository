function Get-DiskUsage {
    param (
        [string]$Path = "C:\"
    )
    Get-ChildItem -Path $Path -Recurse | Measure-Object -Property Length -Sum
}
Get-DiskUsage -Path "C:\temp\"