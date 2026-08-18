#!/usr/bin/env python3
import json
import urllib.request
import re
import sys
import subprocess

TOKEN = "06baa7e4b5520cceb2a3dc7058ce518b50d6471e"
ITEM_ID = "ea173f3df4ea85966b5f"
API_URL = f"https://qiita.com/api/v2/items/{ITEM_ID}"

def get_git_commit_hash():
    try:
        output = subprocess.check_output(["git", "rev-parse", "HEAD"], cwd="/Users/ooharayukio/ai_token_test_tsumutsumu")
        return output.decode("utf-8").strip()
    except Exception:
        return "main"

def prepare_qiita_body(file_path: str) -> str:
    commit_hash = get_git_commit_hash()
    github_raw_base = f"https://raw.githubusercontent.com/roripika/ai-game-engine-token-benchmark/{commit_hash}/metrics/screenshots/"

    with open(file_path, "r", encoding="utf-8") as f:
        content = f.read()

    # ローカルファイルパスを 50% 幅指定の Qiita 用 img タグに置換
    content = re.sub(
        r'<img src="file:///[^\s)]+godot_screenshot\.png"[^>]*>',
        f'<img src="{github_raw_base}godot_screenshot.png?v={commit_hash[:7]}" width="50%" alt="Godot 4.6 動作画面">',
        content
    )
    content = re.sub(
        r'<img src="file:///[^\s)]+axmol_screenshot\.png"[^>]*>',
        f'<img src="{github_raw_base}axmol_screenshot.png?v={commit_hash[:7]}" width="50%" alt="Axmol 2.11 動作画面">',
        content
    )
    content = re.sub(
        r'<img src="file:///[^\s)]+unity_screenshot\.png"[^>]*>',
        f'<img src="{github_raw_base}unity_screenshot.png?v={commit_hash[:7]}" width="50%" alt="Unity 6 動作画面">',
        content
    )

    return content.strip()

def update_article(title: str, body: str):
    payload = {
        "title": title,
        "body": body,
        "private": False,
        "tags": [
            {"name": "Unity", "versions": []},
            {"name": "Godot", "versions": []},
            {"name": "C++", "versions": []},
            {"name": "AI", "versions": []},
            {"name": "ゲーム開発", "versions": []}
        ]
    }

    data = json.dumps(payload).encode("utf-8")
    req = urllib.request.Request(
        API_URL,
        data=data,
        headers={
            "Authorization": f"Bearer {TOKEN}",
            "Content-Type": "application/json"
        },
        method="PATCH"
    )

    try:
        with urllib.request.urlopen(req) as res:
            res_body = res.read().decode("utf-8")
            item = json.loads(res_body)
            print("Successfully updated Qiita article with 50% width images and titles!")
            print(f"Article URL: {item.get('url')}")
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
    update_article(title_str, body_content)
