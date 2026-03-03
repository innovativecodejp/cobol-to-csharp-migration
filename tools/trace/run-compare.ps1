#Requires -Version 7.0
param(
    [string]$Expected,
    [string]$Actual
)

$scriptPath = Join-Path $PSScriptRoot "compare-trace.ps1"

& $scriptPath -Expected $Expected -Actual $Actual -IgnoreRunId -KeyMode AUTO

if ($LASTEXITCODE -ne 0) {
    Write-Host "Trace comparison failed."
    exit 1
}

Write-Host "Trace comparison passed."
exit 0
