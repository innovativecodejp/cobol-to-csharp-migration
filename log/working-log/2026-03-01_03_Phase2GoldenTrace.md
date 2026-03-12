# 2026-03-01_Phase2GoldenTrace

## 🎯 今日の目的
- Phase2の比較成立に向け、COBOL/C#トレースのゴールデンケースを固定化する。

## 🛠 実施内容
- `compare-trace.ps1` に `-KeyMode AUTO|STEP|STMT` を追加
- `docs/trace/TraceSpec.md` に比較キー運用（AUTO/STMT/STEP）を追記
- `samples/golden-io-trace/` に COBOL/C# サンプルと入力データを追加
- `tools/trace/testdata/golden/` に比較用トレースデータを追加
- `tools/trace/README.md` に再現手順と期待結果（exit code）を整理

## 🔍 結果
- `KeyMode=AUTO` で `STMT` 比較が成立し、差分なし（exit 0）を確認
- `KeyMode=STEP` では意図どおり差分検出（exit 1）を確認
- `MigrationTraceTests` は通過し、回帰基盤として維持できる状態

## 💡 学び
- 実行順（STEP）が異なるケースでも、`STMT` 比較を導入すると同型比較の実運用性が上がる。

## 🧠 思考整理
- 仕様（TraceSpec）・実装（Runtime/Script）・検証（Test/README）を同時に揃えると、
  後続のCOBOL側計測拡張へ接続しやすい。

## ⏭ 次のアクション
- COBOL実行環境で実トレースを採取し、ゴールデンケースの実測更新を行う
- compare-trace に Pester テストを追加し、CI自動検証を強化する
