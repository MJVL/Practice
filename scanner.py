import requests
from urllib.parse import urlsplit
import time

def normalize_url(url):
    # if no scheme, we'll default to https
    if "://" not in url:
        url = f"https://{url}"

    # we want to normalize away query params, fragments, and other things
    parsed = urlsplit(url)

    # handle bad urls
    if not parsed.hostname:
        return None

    # back to https
    return f"https://{parsed.hostname.lower()}"

def make_unique_urls(targets):
    unique = set()
    for t in targets:
        norm = normalize_url(t)
        if norm:
            unique.add(norm)
    return unique

def scan(targets):
    for t in make_unique_urls(targets):
        try:
            print(f"Reaching out to {t}")
            r = requests.get(t)
            print(f"Resp: {r.status_code}")
        except Exception as e:
            print(f"Failed to poll: {e}")


if __name__ == "__main__":
    #test_cases = ["https://www.google.com", "http://www.google.com", "www.google.com", "www.google.com?s=5", "www.google.com#hello!"]
    #print(make_unique_urls(test_cases))
    start_time = time.perf_counter()
    scan(["https://www.google.com", "www.github.com", "www.mjvl.zip", "www.amazon.com"])
    end_time = time.perf_counter()
    print(f"Exec time: {end_time - start_time:.4f} seconds")