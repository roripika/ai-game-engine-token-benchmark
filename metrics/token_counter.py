#!/usr/bin/env python3
import os
import sys
import json
import re

try:
    import tiktoken
    HAS_TIKTOKEN = True
    ENCODER = tiktoken.get_encoding("o200k_base")
except ImportError:
    HAS_TIKTOKEN = False
    ENCODER = None

VALID_EXTENSIONS = {".gd", ".tscn", ".godot", ".cs", ".unity", ".cpp", ".h", ".txt", ".json", ".txt"}

def count_tokens(text: str) -> int:
    if not text:
        return 0
    if HAS_TIKTOKEN and ENCODER:
        return len(ENCODER.encode(text))
    
    cjk_chars = len(re.findall(r'[\u3000-\u9fff\uf900-\ufaff\uff00-\uffef]', text))
    other_text = re.sub(r'[\u3000-\u9fff\uf900-\ufaff\uff00-\uffef]', '', text)
    
    ascii_chars = len(other_text)
    symbols = len(re.findall(r'[{}\[\]();:,.<>=+\-*/&|^%!~?]', other_text))
    
    token_est = int(cjk_chars * 1.35 + (ascii_chars - symbols) / 4.0 + symbols * 0.8)
    return max(token_est, 1)

def count_file_tokens(file_path: str) -> int:
    if not os.path.exists(file_path):
        return 0
    with open(file_path, "r", encoding="utf-8", errors="ignore") as f:
        content = f.read()
    return count_tokens(content)

def count_dir_tokens(dir_path: str) -> dict:
    total_tokens = 0
    file_details = {}
    
    if not os.path.exists(dir_path):
        return {"total_tokens": 0, "files": {}}

    for root, _, files in os.walk(dir_path):
        for file in files:
            ext = os.path.splitext(file)[1]
            filename = os.path.basename(file)
            if ext not in VALID_EXTENSIONS and filename != "CMakeLists.txt":
                continue
            full_path = os.path.join(root, file)
            tokens = count_file_tokens(full_path)
            rel_path = os.path.relpath(full_path, dir_path)
            file_details[rel_path] = tokens
            total_tokens += tokens
            
    return {"total_tokens": total_tokens, "files": file_details}

if __name__ == "__main__":
    if len(sys.argv) > 1:
        target = sys.argv[1]
        if os.path.isfile(target):
            print(f"File: {target} | Tokens: {count_file_tokens(target)}")
        elif os.path.isdir(target):
            res = count_dir_tokens(target)
            print(f"Directory: {target} | Total Code/Scene Tokens: {res['total_tokens']}")
            for fpath, t in res['files'].items():
                print(f"  - {fpath}: {t} tokens")
