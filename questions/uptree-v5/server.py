import random

def sentinel(parent, x):
    while parent[x] != -1:
        x = parent[x]
    return x

def find_compress(parent, x):
    root = sentinel(parent, x)
    while parent[x] != -1:
        nxt = parent[x]
        parent[x] = root
        x = nxt
    return root

def do_union(parent, x, y):
    sx = find_compress(parent, x)
    sy = find_compress(parent, y)
    if sx != sy:
        parent[sx] = sy

def generate(data):
    n = 6
    parent = [-1] * n
    ops = []
    attempts = 0

    while len(ops) < 5 and attempts < 200:
        x = random.randint(0, n - 1)
        y = random.randint(0, n - 1)
        if x != y and sentinel(parent, x) != sentinel(parent, y):
            do_union(parent, x, y)
            ops.append('union({},{})'.format(x, y))
        attempts += 1

    non_roots = [i for i in range(n) if parent[i] != -1]
    find_node = random.choice(non_roots) if non_roots else random.randint(0, n - 1)
    find_compress(parent, find_node)

    data['params']['operations'] = ' '.join(ops) + ' find({})'.format(find_node)

    for i in range(n):
        data['correct_answers']['p{}'.format(i)] = parent[i]