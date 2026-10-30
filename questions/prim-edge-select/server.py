import random

NODES = ['A', 'B', 'C', 'D', 'E']

def generate(data):
    graph = _random_connected_graph()
    mst = _prims(graph)
    mst_edges = [[u, v] for u, v, w in mst]

    data['params']['nodes'] = NODES
    data['params']['edges'] = [[u, v, w] for u, v, w in graph]
    data['correct_answers']['mst_edges'] = mst_edges


def _random_connected_graph():
    all_pairs = [(NODES[i], NODES[j]) for i in range(len(NODES)) for j in range(i + 1, len(NODES))]
    weights = random.sample(range(1, 11), len(all_pairs))
    edge_pool = [(all_pairs[i][0], all_pairs[i][1], weights[i]) for i in range(len(all_pairs))]
    random.shuffle(edge_pool)

    # Build a random spanning tree first (guarantees connectivity)
    parent = {n: n for n in NODES}

    def find(x):
        while parent[x] != x:
            parent[x] = parent[parent[x]]
            x = parent[x]
        return x

    spanning, extra = [], []
    for u, v, w in edge_pool:
        if find(u) != find(v):
            parent[find(u)] = find(v)
            spanning.append((u, v, w))
        else:
            extra.append((u, v, w))

    chosen = spanning + extra[:3]
    return sorted(chosen, key=lambda e: (e[0], e[1]))


def _prims(edges):
    adj = {n: [] for n in NODES}
    for u, v, w in edges:
        adj[u].append((v, w))
        adj[v].append((u, w))

    visited = {'A'}
    mst = []
    while len(visited) < len(NODES):
        best = None
        for u in visited:
            for v, w in adj[u]:
                if v not in visited:
                    if best is None or w < best[2]:
                        best = (u, v, w)
        if best:
            mst.append(best)
            visited.add(best[1])
    return mst
