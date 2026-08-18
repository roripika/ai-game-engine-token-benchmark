#!/usr/bin/env python3
import json
import urllib.request
import re
import sys

TOKEN = "06baa7e4b5520cceb2a3dc7058ce518b50d6471e"
API_URL = "https://qiita.com/api/v2/items"

# Qiita投稿用の代替画像URL
GITHUB_RAW_BASE = "https://raw.githubusercontent.com/roripika/ai-game-engine-token-benchmark/main/metrics/screenshots/"

def prepare_qiita_body(file_path: str) -> str:
    with open(file_path, "r", encoding="utf-8") as f:
        content = f.read()

    # カルーセルタグなどの独自記法を Qiita 標準 Markdown に置換
    content = content.replace("````carousel", "")
    content = content.replace("````", "")
    content = content.replace("<!-- slide -->", "\n")

    # ローカルファイルパスを GitHub raw URL に置換
    content = re.sub(r'file:///[^\s)]+godot_screenshot\.png', GITHUB_RAW_BASE + 'godot_screenshot.png', content)
    content = re.sub(r'file:///[^\s)]+axmol_screenshot\.png', GITHUB_RAW_BASE + 'axmol_screenshot.png', content)
    content = re.sub(r'file:///[^\s)]+unity_screenshot\.png', GITHUB_RAW_BASE + 'unity_screenshot.png', content)

    return content.strip()

def post_article(title: str, body: str, private: bool = False):
    payload = {
        "title": title,
        "body": body,
        "private": private,
        "tags": [
            {"name": "Unity", "versions": []},
            {"name": "Godot", "versions": []},
            {"name": "C++", "versions": []},
            {"name": "AI", "versions": []},
            {"name": "ゲーム開発", "versions": []}
        ],
        "tweet": False
    }

    data = json.dumps(payload).encode("utf-8")
    req = urllib.request.Request(
        API_URL,
        data=data,
        headers={
            "Authorization": f"Bearer {TOKEN}",
            "Content-Type": "application/json"
        },
        method="POST"
    )

    try:
        with urllib.request.urlopen(req) as res:
            res_body = res.read().decode("utf-8")
            item = json.loads(res_body)
            print("Successfully posted to Qiita!")
            print(f"Article URL: {item.get('url')}")
            print(f"Article ID: {item.get('id')}")
            return item
    except urllib.error.HTTPError as e:
        error_resp = e.read().decode("utf-8")
        print(f"HTTP Error {e.code}: {error_resp}", file=sys.stderr)
        return None
    except Exception as e:
        print(f"Error: {e}", file=sys.stderr)
        return None

if __name__ == "__main__":
    body_content = prepare_qiita_body("QIITA_ARTICLE_DRAFT.md")
    title_str = "AI駆動ゲーム開発におけるエンジン別トークン効率・自己完結性の比較 〜 Unity vs Godot vs Axmol 〜"
    
    # オプション: 引数に --publish がある場合は公開投稿、それ以外は下書き・検証
    post_article(title_str, body_content, private=False)
