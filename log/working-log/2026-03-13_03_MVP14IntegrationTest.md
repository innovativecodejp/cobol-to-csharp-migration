# 2026-03-13_03_MVP14IntegrationTest

## 🎯 目的
MVP14: READ → WRITE の I/O 通し処理統合テスト

## 🛠 実施内容
- Mvp14Integration.cbl 追加（READ→WRITE パターンの COBOL サンプル）
- mvp13 fixture 拡張（input_empty, input_withspaces）
- CobolMvpRuntimeReadWriteIntegrationTests 新設
- ProcessFile テストに empty / withspaces ケース追加

## 📂 変更ファイル
- `samples/cobol/Mvp14Integration.cbl`（新規）
- `samples/data/mvp13/input_empty.txt`, `expected_empty.txt`（新規）
- `samples/data/mvp13/input_withspaces.txt`, `expected_withspaces.txt`（新規）
- `apps/backend/tests/CobolMvpRuntimeReadWriteIntegrationTests.cs`（新規）
- `apps/backend/tests/CobolMvpRuntimeMvp13Tests.cs`（修正）

## 🔍 結果
- 全62テスト成功
- 空 / 1行 / 複数行 / 空白含み の通し検証を確立
- I/O が「点」ではなく「線」で動くことを証明

## 💡 学び
- ProcessFile による READ→WRITE 統合が回帰基盤として有効
- 統合テストを専用クラスに分離すると意図が明確になる

## ⏭ 次のアクション
- record mapping 強化
- READ INTO / WRITE FROM 対応
- MVP14（indexed file）: START / READ NEXT
