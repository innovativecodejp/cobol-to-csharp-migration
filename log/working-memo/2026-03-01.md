# 2026-03-01

## 🎯 今日の目的
- Phase2として「変換保証レイヤ（Trace）」のMVPを最小差分で導入し、C#実行の振る舞い同型比較の土台を作る。

## 🛠 実施内容
- `apps/backend/src/CobolMvpRuntime/TraceOptions.cs` を追加（Enabled/OutputPath/RunId/ResetOnStart）
- `apps/backend/src/CobolMvpRuntime/TraceRecord.cs` を追加（1行レコード組み立て・改行エスケープ）
- `apps/backend/src/CobolMvpRuntime/MigrationTrace.cs` を追加（Start/NextStep/LogAssign/LogIf/Log/Stop）
- `apps/backend/src/CobolMvpRuntime/Program.cs` に代表差し込み（`LogAssign` / `LogIf`）
- `tools/trace/compare-trace.ps1` を追加（`-Expected` / `-Actual` / `-IgnoreRunId`）
- `apps/backend/tests/MigrationTraceTests.cs` を追加（trace出力/無効時の2ケース）
- `dotnet test --filter MigrationTraceTests` と `pwsh compare-trace.ps1` 実行で動作確認

## 🔍 結果
- Traceログを 1行1レコード形式（`RUN|STEP|TYPE|...`）で出力可能
- STEPは `000001` 形式で連番採番、比較しやすい形式を維持
- ON/OFF切替（Enabled）と出力先指定（OutputPath）を確認
- 比較スクリプトで RunId差分を無視した比較が可能（`-IgnoreRunId`）
- 追加テスト2件は合格（失敗0）

## 💡 学び
- 変換保証レイヤは「ログ責務をAPIに集中」させることで、既存ロジックへの影響を局所化できる。
- 差分比較前提では、最初からフォーマットを固定しておくことが運用コストを下げる。

## 🧠 思考整理
- Phase1のMVP成果（構文別実装）に対して、Phase2は「実行同型性の検証導線」を重ねるフェーズ。
- まずは changed-only の最小トレースで検証ループを成立させ、後続で full-dump/命令種拡張を段階投入する。

## ⏭ 次のアクション
- COBOL側トレース（後続実装）とキー揃えの仕様を決定する
- compare-trace の差分出力をCIで扱いやすい形に整備する
- `READ/WRITE/START` 系MVPシナリオへトレース適用範囲を広げる
