# 検証完了レポート：AI駆動ゲーム開発におけるエンジン別トークン効率・自己完結性の比較

**〜 Unity vs Godot vs Axmol で作る「ツムツム風パズルゲーム」〜**

本レポートは、同一仕様の2D物理パズルゲーム（ツムツム風）を AI にて 3つのゲームエンジンで実装し、トークン数・デバッグターン数・エラー率を比較計測した最終結果です。

---

## 📊 計測結果サマリー

```
┌─────────────────────────┬──────────────┬──────────────────────┬──────────────────┐
│ 項目                    │ Unity 6 (C#) │ Godot 4.6 (GDScript) │ Axmol 2.11 (C++) │
├─────────────────────────┼──────────────┼──────────────────────┼──────────────────┤
│ 初期生成 出力トークン   │ 2,089        │ 1,857                │ 2,648            │
│ シーン情報 入力トークン │ 1,879 (YAML) │ 304 (.tscn)          │ 0 (コード完結)   │
│ デバッグ往復数 (Turns)  │ 2 回         │ 1 回                 │ 0 回             │
├─────────────────────────┼──────────────┼──────────────────────┼──────────────────┤
│ 累計消費トークン (Total)│ 3,968        │ 2,161 (最安 🏆)       │ 2,648 (最高自律) │
│ 推定 API コスト ($)     │ $0.0119      │ $0.0065 (-45.5%)     │ $0.0079          │
└─────────────────────────┴──────────────┴──────────────────────┴──────────────────┘
```

---

## 📁 成果物と保存先一覧

1. 📝 **Qiita 記事ドラフト**: [QIITA_ARTICLE_DRAFT.md](./QIITA_ARTICLE_DRAFT.md)
2. 📍 **検証ロードマップ**: [VERIFICATION_ROADMAP.md](./VERIFICATION_ROADMAP.md)
3. ⚙️ **計測ツール & データ**:
   - 計測スクリプト: [metrics/token_counter.py](./metrics/token_counter.py)
   - Godot データ: [metrics/godot_metrics.json](./metrics/godot_metrics.json)
   - Axmol データ: [metrics/axmol_metrics.json](./metrics/axmol_metrics.json)
   - Unity データ: [metrics/unity_metrics.json](./metrics/unity_metrics.json)
4. 🎮 **各エンジン実装ソースコード**:
   - [godot_tsumutsumu/](./godot_tsumutsumu)
   - [axmol_tsumutsumu/](./axmol_tsumutsumu)
   - [unity_tsumutsumu/](./unity_tsumutsumu)
5. 🔗 **GitHub Repository**: [roripika/ai-game-engine-token-benchmark](https://github.com/roripika/ai-game-engine-token-benchmark)
