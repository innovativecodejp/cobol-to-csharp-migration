# Trace Compare Guide (Phase2)

## 目的

COBOL側トレースとC#側トレースを `compare-trace.ps1` で比較し、  
変換後挙動の同型性を確認する。

## 前提

- PowerShell 7 (`pwsh`) が利用可能であること
- トレースは `TraceSpec.md` 準拠（1行1レコード, `|`, `KEY=VALUE`）
- COBOLコンパイル/実行環境は実行者側依存（本リポジトリでは手順のみ提示）

## ゴールデンケース（固定）

- 入力: `samples/golden-io-trace/input/input.txt`
- COBOLサンプル: `samples/golden-io-trace/cobol/GoldenIoTrace.cbl`
- C#サンプル: `samples/golden-io-trace/csharp/GoldenIoTraceRunner.cs`
- 比較用テストデータ:
  - `tools/trace/testdata/golden/cobol.trace.log`
  - `tools/trace/testdata/golden/csharp.trace.log`

## 実行手順

### 1) COBOL側トレース取得（環境依存）

以下は例。実際のコンパイラ/ランタイムに置き換える。

```powershell
# 例: COBOLプログラム実行結果を標準出力からtraceへ保存
# cobc -x ".\samples\golden-io-trace\cobol\GoldenIoTrace.cbl"
# .\GoldenIoTrace.exe > ".\samples\golden-io-trace\cobol.trace.log"
```

### 2) C#側トレース取得

`MigrationTrace` を有効にして `GoldenIoTraceRunner` を呼び出し、`trace.log` を生成する。

```powershell
# 実行例（呼び出し側は任意のホストで可）
# GoldenIoTraceRunner.Run(
#   ".\samples\golden-io-trace\input\input.txt",
#   ".\samples\golden-io-trace\output.txt",
#   ".\samples\golden-io-trace\csharp.trace.log"
# )
```

### 3) 比較（AUTO）

```powershell
pwsh -NoProfile -File ".\tools\trace\compare-trace.ps1" `
  -Expected ".\tools\trace\testdata\golden\cobol.trace.log" `
  -Actual ".\tools\trace\testdata\golden\csharp.trace.log" `
  -IgnoreRunId -KeyMode AUTO
```

期待結果:
- `Trace files match...` が表示される
- 終了コード `0`

### 4) 差分確認（FailFast）

```powershell
pwsh -NoProfile -File ".\tools\trace\compare-trace.ps1" `
  -Expected ".\tools\trace\testdata\golden\cobol.trace.log" `
  -Actual ".\tools\trace\testdata\golden\csharp.trace.log" `
  -IgnoreRunId -KeyMode STEP -FailFast -MaxDiffs 5
```

期待結果:
- `Trace files differ` が表示される
- `DiffCount`, `FirstMismatchIndex`, 差分サンプルが表示される
- 終了コード `1`

## KeyMode の使い分け

- `AUTO`（既定）:
  - Expected/Actual の両方で `STMT` が1件以上あれば `STMT` 比較を優先
  - 無ければ `STEP` 比較
- `STMT`:
  - `STMT` キーで安定ソートして比較（実行順差異に耐性）
- `STEP`:
  - 行順比較（実行順を厳密比較）

## CI 回帰用の最小比較データ

Phase2の回帰判定を自動化するため、以下の固定ファイルを用意している。

- `tools/trace/testdata/expected.trace.log`
- `tools/trace/testdata/actual.trace.log`

この2ファイルは初期状態では同一内容（PASSケース）であり、  
`run-compare.ps1` から `compare-trace.ps1` を呼ぶことで CI 判定に利用する。

仮定:
- CIの最小目的は「比較ロジックが常時実行されること」の担保である。
- FAILケースは必要に応じて別testdata（または一時改変）で検証する。
