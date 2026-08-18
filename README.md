# 検証プロジェクト：AI駆動ゲーム開発におけるエンジン別トークン効率・自己完結性の比較

**〜 Unity vs Godot vs Axmol で作る「ツムツム風パズルゲーム」〜**

本リポジトリは、生成AI（LLM）を活用したゲーム開発において、ゲームエンジン自体のアーキテクチャ（言語仕様、データフォーマット、エディタ依存度）の違いが、LLMのトークン消費量やコンテキスト圧迫、デバッグ往復ターン数、自己完結性に与える影響を定量的に検証し、Qiita記事として発表するための検証プロジェクトです。

---

## 📄 ドキュメント一覧

- 📍 [VERIFICATION_ROADMAP.md](./VERIFICATION_ROADMAP.md) : 今後実施する検証手順・各フェーズの詳細・Qiita記事化に向けたロードマップ
- 📍 [implementation_plan.md](file:///Users/ooharayukio/.gemini/antigravity/brain/722df093-2fa8-4c77-ae24-1b8d958fcaff/implementation_plan.md) : AIシステム側の詳細実装計画書
- 📝 `QIITA_ARTICLE_DRAFT.md` *(検証進行に伴い作成)* : Qiita投稿用の記事原稿

---

## 🛠 比較対象エンジン

1. **Unity (C#)** : 独自YAML / バイナリメタデータ・GUIインスペクター依存
2. **Godot 4.6 (GDScript)** : `.tscn` テキストシーン・簡潔な構文
3. **Axmol 2.11 (C++)** : コードベース（エディタ不要・C++完結）

---

## 📊 計測項目

- **入力トークン数 (Input Tokens)**
- **出力トークン数 (Output Tokens)**
- **プロンプト往復回数 (Turns)**
- **エラー内訳 (文法・参照切れ・物理/座標系)**
- **推定 API コスト ($)**
