# Trace Specification (Phase2 MVP)

## 1. 目的

本仕様は、COBOL実行とC#実行の**振る舞い同型性**を比較可能にするための共通トレース形式を定義する。  
差分比較を最優先にし、1行1レコードのテキストログで保存する。

## 2. レコード形式

- 1行 = 1レコード
- 区切り文字: `|`
- キーと値: `KEY=VALUE`
- 改行: `Environment.NewLine`（実行環境依存）

例:

```text
RUN=20260301-0001|STEP=000001|TYPE=ASSIGN|VAR=WS-COUNT|VAL=1
RUN=20260301-0001|STEP=000002|TYPE=IF|COND=WS-TEXT(1:1)=="A"|RESULT=TRUE
```

## 3. 必須キー

- `RUN`: 実行ID（例: `yyyyMMdd-HHmmss`）
- `STEP`: 実行順連番（6桁ゼロ埋め、例: `000123`）
- `TYPE`: 命令種別（例: `ASSIGN`, `IF`, `READ`）

## 4. TYPE別必須フィールド

| TYPE | 必須フィールド | 補足 |
|---|---|---|
| ASSIGN | `VAR`, `VAL` | changed-only を基本とする |
| IF | `COND`, `RESULT` | `RESULT` は `TRUE/FALSE` |
| READ | `FILE`, `RESULT` | `RESULT` は `OK/EOF/ERROR` |
| WRITE | `FILE` | `RECNO`, `LEN` は任意 |
| START | `FILE`, `KEY`, `RESULT` | `RESULT` は `OK/INVALID/ERROR` |
| DISPLAY | `TEXT` | 表示文字列 |

## 5. エスケープ規約

値（VALUE）に対して以下を適用する。

- `\` -> `\\`
- `\r` -> `\\r`
- `\n` -> `\\n`
- `|` -> `\|`
- `=` -> `\=`

注記:
- 本MVPでは、復元（unescape）処理は比較ツール側で必須としない。
- 比較の基準は「同一規約で生成されたトレース文字列一致」とする。

## 6. STEP vs STMT の方針

- C#側は `STEP`（実行順）を採用する。
- COBOL側は将来 `STMT`（stmt-id）を必須化する方針とし、可能であれば実行順も併記する。
- compare-trace は現時点で `RUN` 無視比較を提供し、将来の `STMT` ベース比較へ移行可能な設計余地を残す。

## 7. 研究接続点（最小）

破綻パターン分類に備え、将来 `CLASS`, `RULE`, `STMT` などのキー追加を許容する。  
本MVPでは追加キーは任意（後方互換）とする。

## 8. 実装方針（MVP仮定）

- 仮定1: まずは C#側のみが本仕様でトレースを生成する。
- 仮定2: 比較対象の差分主因は `RUN` の変動であるため、比較ツールで `-IgnoreRunId` を提供する。
- 仮定3: 書き込みは単一プロセス内 lock 排他で十分とし、背景Writerは導入しない。

## 9. compare-trace 検証手順（最小）

Pester未導入環境では、以下を手動検証手順とする。

1. 同一内容・異なるRUNの2ファイルを作成する  
2. `-IgnoreRunId` 付きで比較し、`exit 0` になることを確認する  
3. 1行変更して再比較し、`DiffCount` / `FirstMismatchIndex` / サンプル差分が出て `exit 1` になることを確認する  
4. `-FailFast` 指定時に最初の差分のみ表示されることを確認する
