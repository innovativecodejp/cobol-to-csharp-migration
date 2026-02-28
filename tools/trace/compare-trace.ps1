#Requires -Version 7.0
param(
  [Parameter(Mandatory = $true)]
  [string]$Expected,

  [Parameter(Mandatory = $true)]
  [string]$Actual,

  [switch]$IgnoreRunId,

  [switch]$FailFast,

  [int]$MaxDiffs = 20,

  [ValidateSet("AUTO", "STEP", "STMT")]
  [string]$KeyMode = "AUTO"
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

function Get-TraceKey {
  param(
    [string]$Line,
    [string]$KeyName
  )

  if ($null -eq $Line) {
    return ""
  }

  $m = [regex]::Match($Line, "(?:^|\|)$KeyName=([^|]+)")
  if ($m.Success) {
    return $m.Groups[1].Value
  }

  return ""
}

function Sort-ByTraceKey {
  param(
    [string[]]$Lines,
    [string]$KeyName
  )

  if ($null -eq $Lines -or $Lines.Count -eq 0) {
    return @()
  }

  $indexed = for ($i = 0; $i -lt $Lines.Count; $i++) {
    $k = Get-TraceKey -Line $Lines[$i] -KeyName $KeyName
    [pscustomobject]@{
      Index      = $i
      Key        = $k
      IsKeyEmpty = [string]::IsNullOrEmpty($k)
      Line       = $Lines[$i]
    }
  }

  return @($indexed | Sort-Object IsKeyEmpty, Key, Index | ForEach-Object { $_.Line })
}

function Remove-Field {
  param(
    [string]$Line,
    [string]$Field
  )

  if ([string]::IsNullOrEmpty($Line)) {
    return $Line
  }

  $parts = @($Line -split '\|')
  $filtered = @($parts | Where-Object { $_ -notmatch "^$Field=" })
  return ($filtered -join "|")
}

$effectiveKeyMode = $KeyMode
if ($KeyMode -eq "AUTO") {
  $expectedHasStmt = ($expectedLines | Where-Object { (Get-TraceKey -Line $_ -KeyName "STMT") -ne "" }).Count -gt 0
  $actualHasStmt = ($actualLines | Where-Object { (Get-TraceKey -Line $_ -KeyName "STMT") -ne "" }).Count -gt 0
  if ($expectedHasStmt -and $actualHasStmt) {
    $effectiveKeyMode = "STMT"
  }
  else {
    $effectiveKeyMode = "STEP"
  }
}

if ($effectiveKeyMode -eq "STMT") {
  $expectedLines = @($expectedLines | ForEach-Object { Remove-Field -Line $_ -Field "STEP" })
  $actualLines = @($actualLines | ForEach-Object { Remove-Field -Line $_ -Field "STEP" })
  $expectedLines = Sort-ByTraceKey -Lines $expectedLines -KeyName "STMT"
  $actualLines = Sort-ByTraceKey -Lines $actualLines -KeyName "STMT"
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
  Write-Host "Trace files match. Lines=$($expectedLines.Count) KeyMode=$effectiveKeyMode"
  exit 0
}

Write-Host "Trace files differ."
Write-Host "DiffCount=$($diffs.Count)"
Write-Host "FirstMismatchIndex=$($diffs[0].Index)"
Write-Host "KeyMode=$effectiveKeyMode"

$show = [Math]::Min($MaxDiffs, $diffs.Count)
Write-Host "DiffSamples(Top $show):"
for ($j = 0; $j -lt $show; $j++) {
  $d = $diffs[$j]
  Write-Host ("[{0:D6}] EXP: {1}" -f $d.Index, $d.Expected)
  Write-Host ("[{0:D6}] ACT: {1}" -f $d.Index, $d.Actual)
}

exit 1
