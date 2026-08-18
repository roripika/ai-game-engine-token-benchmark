#!/usr/bin/env python3
import os
import sys
import json
import re

# tiktoken が利用可能なら使用、不可なら高精度フォールバック
try:
    import tiktoken
    HAS_TIKTOKEN = True
    ENCODER = tiktoken.get_encoding("o200k_base") # GPT-4o / modern LLMs
except ImportError:
    HAS_TIKTOKEN = False
    ENCODER = None

def count_tokens(text: str) -> int:
    """テキストのトークン数を計算する"""
    if not text:
        return 0
    if HAS_TIKTOKEN and ENCODER:
        return len(ENCODER.encode(text))
    
    # フォールバック高精度トークン計算アルゴリズム
    # 日本語/全角文字: 1文字 ≒ 1.3 トークン
    # C#/GDScript/C++ コード・英数字: 1単語 ≒ 1.2 トークン / 約3.5文字 ≒ 1トークン
    cjk_chars = len(re.findall(r'[\u3000-\u9fff\uf900-\ufaff\uff00-\uffef]', text))
    other_text = re.sub(r'[\u3000-\u9fff\uf900-\ufaff\uff00-\uffef]', '', text)
    
    words = len(other_text.split())
    ascii_chars = len(other_text)
    
    # コード特有の記号類
    symbols = len(re.findall(r'[{}\[\]();:,.<>=+\-*/&|^%!~?]', other_text))
    
    token_est = int(cjk_chars * 1.35 + (ascii_chars - symbols) / 4.0 + symbols * 0.8)
    return max(token_est, 1)

def count_file_tokens(file_path: str) -> int:
    """単一ファイルのトークン数を計算"""
    if not os.path.exists(file_path):
        return 0
    with open(file_path, "r", encoding="utf-8", errors="ignore") as f:
        content = f.read()
    return count_tokens(content)

def count_dir_tokens(dir_path: str, extensions=None) -> dict:
    """ディレクトリ内の対象拡張子ファイルの合計トークン数を計算"""
    total_tokens = 0
    file_details = {}
    
    if not os.path.exists(dir_path):
        return {"total_tokens": 0, "files": {}}

    for root, _, files in os.walk(dir_path):
        for file in files:
            if extensions:
                ext = os.path.splitext(file)[1]
                if ext not in extensions:
                    continue
            full_path = os.path.join(root, file)
            tokens = count_file_tokens(full_path)
            rel_path = os.path.relpath(full_path, dir_path)
            file_details[rel_path] = tokens
            total_tokens += tokens
            
    return {"total_tokens": total_tokens, "files": file_details}

def save_metrics(engine: str, data: dict, output_dir="metrics"):
    """計測メトリクスをJSON保存"""
    os.makedirs(output_dir, exist_ok=True)
    file_path = os.path.join(output_dir, f"{engine}_metrics.json")
    with open(file_path, "w", encoding="utf-8") as f:
        json.dump(data, f, ensure_ascii=False, indent=2)
    print(f"Saved metrics to {file_path}")

if __name__ == "__main__":
    if len(sys.argv) > 1:
        target = sys.argv[1]
        if os.path.isfile(target):
            print(f"File: {target} | Tokens: {count_file_tokens(target)}")
        elif os.path.isdir(target):
            res = count_dir_tokens(target)
            print(f"Directory: {target} | Total Tokens: {res['total_tokens']}")
            for fpath, t in res['files'].items():
                print(f"  - {fpath}: {t} tokens")
    else:
        sample_text = "Hello World! こんにちは世界。2D物理パズルゲームのツムツム風実装です。"
        print(f"Tiktoken Active: {HAS_TIKTOKEN}")
        print(f"Sample Token Count: {count_tokens(sample_text)}")
