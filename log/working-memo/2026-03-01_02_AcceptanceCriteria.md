# 2026-03-01 Acceptance Criteria

## 対象
Phase2（変換保証レイヤ）拡張

## Acceptance Criteria

### 1) `docs/trace/TraceSpec.md` が追加されている
- [x] 1行1レコード形式（`|` 区切り、`KEY=VALUE`）を明文化
- [x] キー規約（`RUN` / `STEP` / `TYPE`）を明文化
- [x] エスケープ規約（`\r`, `\n`, `|`, `=`）を明文化
- [x] `STEP` と `STMT` の実装方針を明文化
- [x] TYPEカタログ（ASSIGN/IF/READ/WRITE/START/DISPLAY）を表で定義

### 2) C# Runtime に I/O トレースAPIが追加されている
- [x] `MigrationTrace.LogRead(...)` を追加
- [x] `MigrationTrace.LogWrite(...)` を追加
- [x] `MigrationTrace.LogStart(...)` を追加

### 3) テストが追加され、実行で通る
- [x] I/Oログの期待フォーマット検証テストを追加
- [x] Disabled時に出力しない検証テストを追加
- [x] 既存のASSIGN/IFテストを維持
- [x] `dotnet test --filter MigrationTraceTests` で通過（3 tests passed）

### 4) `tools/trace/compare-trace.ps1` が CI 向けに改善される
- [x] 差分件数（`DiffCount`）を出力
- [x] 最初の差分行（`FirstMismatchIndex`）を出力
- [x] 先頭N件差分（`-MaxDiffs`）を出力
- [x] `-IgnoreRunId` を維持
- [x] `-FailFast` を実装
- [x] 差分あり `exit 1` / 差分なし `exit 0`

## 補足
- 比較スクリプトは `pwsh` 実行を前提
- COBOL側トレース生成は後続フェーズで実装予定
