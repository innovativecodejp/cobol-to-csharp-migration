#Requires -Version 7.0
param(
  [Parameter(Mandatory = $true)]
  [string]$Expected,

  [Parameter(Mandatory = $true)]
  [string]$Actual,

  [switch]$IgnoreRunId,

  [switch]$FailFast,

  [int]$MaxDiffs = 20
)

Set-StrictMode -Version Latest
$ErrorActionPreference = "Stop"

if (-not (Test-Path -LiteralPath $Expected)) {
  throw "Expected file not found: $Expected"
}

if (-not (Test-Path -LiteralPath $Actual)) {
  throw "Actual file not found: $Actual"
}

if ($MaxDiffs -lt 1) {
  $MaxDiffs = 1
}

$expectedLines = @(Get-Content -LiteralPath $Expected)
$actualLines = @(Get-Content -LiteralPath $Actual)

if ($IgnoreRunId) {
  $expectedLines = @($expectedLines | ForEach-Object { $_ -replace '^RUN=[^|]+\|', '' })
  $actualLines = @($actualLines | ForEach-Object { $_ -replace '^RUN=[^|]+\|', '' })
}

$max = [Math]::Max($expectedLines.Count, $actualLines.Count)
$diffs = New-Object System.Collections.Generic.List[object]

for ($i = 0; $i -lt $max; $i++) {
  $exp = "<MISSING>"
  if ($i -lt $expectedLines.Count) {
    $exp = $expectedLines[$i]
  }

  $act = "<MISSING>"
  if ($i -lt $actualLines.Count) {
    $act = $actualLines[$i]
  }

  if ($exp -ne $act) {
    $diffs.Add([pscustomobject]@{
      Index    = $i + 1
      Expected = $exp
      Actual   = $act
    }) | Out-Null

    if ($FailFast) {
      break
    }
  }
}

if ($diffs.Count -eq 0) {
  Write-Host "Trace files match. Lines=$($expectedLines.Count)"
  exit 0
}

Write-Host "Trace files differ."
Write-Host "DiffCount=$($diffs.Count)"
Write-Host "FirstMismatchIndex=$($diffs[0].Index)"

$show = [Math]::Min($MaxDiffs, $diffs.Count)
Write-Host "DiffSamples(Top $show):"
for ($j = 0; $j -lt $show; $j++) {
  $d = $diffs[$j]
  Write-Host ("[{0:D6}] EXP: {1}" -f $d.Index, $d.Expected)
  Write-Host ("[{0:D6}] ACT: {1}" -f $d.Index, $d.Actual)
}

exit 1
