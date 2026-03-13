# 2026-03-13_01_MVP12Implementation

## 🎯 目的
MVP12: `READ ... AT END`（SEQUENTIAL入力）の最小実装

## 🛠 実施内容
- SequentialFileReader ランタイム追加（ReadNext / CurrentRecord）
- Mvp12Program を SequentialFileReader 利用にリファクタ
- READ INTO 未対応を NotSupportedException で明示
- 空ファイル・1行・複数行の fixture 追加（input_empty, input_1line, input.txt）
- 自動テスト拡張（Theory 3ケース + CurrentRecord + ReadInto 検出）

## 📂 変更ファイル
- `apps/backend/src/CobolMvpRuntime/SequentialFileReader.cs`（新規）
- `apps/backend/src/CobolMvpRuntime/Mvp12Program.cs`（修正）
- `samples/data/mvp12/input_empty.txt`, `expected_empty.txt`（新規）
- `samples/data/mvp12/input_1line.txt`, `expected_1line.txt`（新規）
- `apps/backend/tests/CobolMvpRuntimeMvp12Tests.cs`（修正）

## 🔍 結果
- 全53テスト成功
- READ ... AT END / NOT AT END の変換パターン確立
- I/O基盤の最初の1本を通す

## 💡 学び
- SequentialFileReader を分離することで、変換ロジックとランタイムの責務が明確化
- 空/1行/複数行の3ケースで回帰テストを担保

## ⏭ 次のアクション
- MVP13: WRITE（順次ファイル出力）
- MVP14: START / READ NEXT（インデックスファイル）
