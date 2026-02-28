#Requires -Version 7.0
param(
  [Parameter(Mandatory = $true)]
  [string]$Expected,

  [Parameter(Mandatory = $true)]
  [string]$Actual,

  [switch]$IgnoreRunId
)

Set-StrictMode -Version Latest
$ErrorActionPreference = "Stop"

if (-not (Test-Path -LiteralPath $Expected)) {
  throw "Expected file not found: $Expected"
}

if (-not (Test-Path -LiteralPath $Actual)) {
  throw "Actual file not found: $Actual"
}

$expectedLines = Get-Content -LiteralPath $Expected
$actualLines = Get-Content -LiteralPath $Actual

if ($IgnoreRunId) {
  $expectedLines = $expectedLines | ForEach-Object { $_ -replace '^RUN=[^|]+\|', '' }
  $actualLines = $actualLines | ForEach-Object { $_ -replace '^RUN=[^|]+\|', '' }
}

$diff = Compare-Object -ReferenceObject $expectedLines -DifferenceObject $actualLines

if ($null -ne $diff -and $diff.Count -gt 0) {
  $diff | Format-Table -AutoSize | Out-String | Write-Host
  exit 1
}

Write-Host "Trace files match."
exit 0
