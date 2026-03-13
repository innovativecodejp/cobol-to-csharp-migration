# 2026-03-13_02_MVP13Implementation

## 🎯 目的
MVP13: WRITE（SEQUENTIAL出力）の最小実装、MigrationTrace テスト失敗の修正

## 🛠 実施内容
- SequentialFileWriter ランタイム追加（WriteLine / Flush / Dispose）
- Mvp13Program を SequentialFileWriter 利用にリファクタ
- ProcessFile(inPath, outPath) 追加（READ→WRITE コピー）
- WRITE FROM / AFTER ADVANCING / BEFORE ADVANCING 未対応を NotSupportedException で明示
- mvp13 fixture 追加（input_1line, input_multiline）
- MigrationTrace 並列実行による干渉を修正（Collection で直列化）

## 📂 変更ファイル
- `apps/backend/src/CobolMvpRuntime/SequentialFileWriter.cs`（新規）
- `apps/backend/src/CobolMvpRuntime/Mvp13Program.cs`（修正）
- `apps/backend/tests/MigrationTraceCollection.cs`（新規）
- `apps/backend/tests/MigrationTraceTests.cs`（修正）
- `apps/backend/tests/GoldenIoTraceRunnerTests.cs`（修正）
- `samples/data/mvp13/input_1line.txt`, `expected_1line.txt`（新規）
- `samples/data/mvp13/input_multiline.txt`, `expected_multiline.txt`（新規）
- `docs/prompts/exec/2026-03-13_04_MVP13_Codex.md`（新規）

## 🔍 結果
- 全56テスト成功
- MigrationTraceTests / GoldenIoTraceRunnerTests の並列干渉を解消
- READ→WRITE 連携の土台確立

## 💡 学び
- 静的シングルトン（MigrationTrace）を使うテストは Collection で直列化が必要
- SequentialFileReader / SequentialFileWriter の対称設計で I/O 基盤が整理された

## ⏭ 次のアクション
- MVP14: READ → 処理 → WRITE の I/O 通し処理テスト
