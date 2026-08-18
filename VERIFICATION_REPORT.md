# 検証完了レポート：AI駆動ゲーム開発におけるエンジン別トークン効率・自己完結性の比較

**〜 Unity vs Godot vs Axmol で作る「ツムツム風パズルゲーム」〜**

本レポートは、同一仕様の2D物理パズルゲーム（ツムツム風）を AI にて 3つのゲームエンジンで実装し、トークン数・デバッグターン数・エラー率および実機スクリーンショットを比較計測した最終結果です。

---

## 🚀 Qiita 公開記事

- 📝 **Qiita 記事 URL**: [https://qiita.com/roripika/items/ea173f3df4ea85966b5f](https://qiita.com/roripika/items/ea173f3df4ea85966b5f)
- 📌 **タイトル**: AI駆動ゲーム開発におけるエンジン別トークン効率・自己完結性の比較 〜 Unity vs Godot vs Axmol 〜

---

## 📊 計測結果サマリー（実機スクショ撮影・完全動作到達）

```
┌─────────────────────────┬──────────────┬──────────────────────┬──────────────────┐
│ 項目                    │ Unity 6 (C#) │ Godot 4.6 (GDScript) │ Axmol 2.11 (C++) │
├─────────────────────────┼──────────────┼──────────────────────┼──────────────────┤
│ 最終コード (Tokens)     │ 3,544        │ 1,634                │ 3,249            │
│ シーン情報 入力トークン │ 1,434 (YAML) │ 304 (.tscn)          │ 0 (コード完結)   │
│ デバッグ往復数 (Turns)  │ 2 回         │ 1 回                 │ 1 回             │
├─────────────────────────┼──────────────┼──────────────────────┼──────────────────┤
│ 累計消費トークン (Total)│ 5,263        │ 2,019 (最安 🏆)       │ 3,391 (最高自律) │
│ 推定 API コスト ($)     │ $0.0158      │ $0.0061 (-61.4%)     │ $0.0101          │
└─────────────────────────┴──────────────┴──────────────────────┴──────────────────┘
```

---

## 📁 成果物と保存先一覧

1. 📝 **Qiita 記事原稿**: [QIITA_ARTICLE_DRAFT.md](./QIITA_ARTICLE_DRAFT.md)
2. 📍 **検証ロードマップ**: [VERIFICATION_ROADMAP.md](./VERIFICATION_ROADMAP.md)
3. 🖼 **実機動作画面スクリーンショット**:
   - [Godot 画面](./metrics/screenshots/godot_screenshot.png)
   - [Axmol 画面](./metrics/screenshots/axmol_screenshot.png)
   - [Unity 画面](./metrics/screenshots/unity_screenshot.png)
4. ⚙️ **計測ツール & データ**:
   - 計測スクリプト: [metrics/token_counter.py](./metrics/token_counter.py)
   - 投稿スクリプト: [metrics/post_to_qiita.py](./metrics/post_to_qiita.py)
   - Godot データ: [metrics/godot_metrics.json](./metrics/godot_metrics.json)
   - Axmol データ: [metrics/axmol_metrics.json](./metrics/axmol_metrics.json)
   - Unity データ: [metrics/unity_metrics.json](./metrics/unity_metrics.json)
5. 🔗 **GitHub Repository**: [roripika/ai-game-engine-token-benchmark](https://github.com/roripika/ai-game-engine-token-benchmark)
