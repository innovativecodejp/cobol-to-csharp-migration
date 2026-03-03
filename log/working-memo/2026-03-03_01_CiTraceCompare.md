# 2026-03-03_01_CiTraceCompare

## 🎯 今日の目的
- ゴールデンケース比較を手動運用から CI 自動判定へ移行する。

## 🛠 実施内容
- `tools/trace/testdata/expected.trace.log` / `actual.trace.log` を追加
- `tools/trace/run-compare.ps1` を追加し、比較実行を一本化
- `.github/workflows/trace-compare.yml` を追加して CI ジョブ化
- `tools/trace/README.md` に CI 回帰用 testdata の前提を追記
- `compare-trace.ps1` の AUTO 判定で 1行ケースを安定動作させる修正を実施

## 🔍 結果
- `run-compare.ps1` の PASS ケースで `exit 0` を確認
- 差分を入れた FAIL ケースで `exit 1` を確認
- `trace-compare.yml` により push / pull_request で自動実行可能な状態になった

## 💡 学び
- 比較スクリプトは「正常系 testdata を固定」しておくことで CI 導入時の摩擦を減らせる。

## 🧠 思考整理
- Phase2 は仕様・実装・比較ツール・CI を同時に揃えることで、
  同型性検証を継続可能な開発フローへ昇格できる。

## ⏭ 次のアクション
- FAIL 専用 testdata を追加して差分検出の回帰テストを強化
- COBOL実測ログの自動取り込み手順を整備
