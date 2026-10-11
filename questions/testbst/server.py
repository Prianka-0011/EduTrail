import random

def generate(data):
    # Root in the middle range so the tree isn't degenerate
    root_val = random.randint(20, 35)
    pool = [v for v in range(1, 51) if v != root_val]
    rest = random.sample(pool, 6)
    nodes = [root_val] + rest

    root = _build_bst(nodes)
    traversal = _inorder(root)   # sorted order — list of strings
    height = _height(root)

    # Pick an insertion target not already in the tree
    in_tree = set(nodes)
    target = random.choice([v for v in range(1, 51) if v not in in_tree])
    insert_parent = _find_parent(root, target)

    # Shuffled options for the traversal drag-and-drop
    traversal_opts = traversal[:]
    random.shuffle(traversal_opts)

    # MC choices for insert-parent: correct + 3 wrong
    wrong = random.sample([n for n in nodes if n != insert_parent], 3)
    choices = [str(w) for w in wrong] + [str(insert_parent)]
    random.shuffle(choices)

    data['params']['nodes'] = nodes
    data['params']['traversal_opts'] = traversal_opts
    data['params']['target'] = target
    data['params']['choices'] = choices

    data['correct_answers']['traversal'] = traversal
    data['correct_answers']['height'] = height
    data['correct_answers']['insert_parent'] = str(insert_parent)


def _build_bst(vals):
    root = None
    for v in vals:
        root = _insert(root, v)
    return root


def _insert(node, val):
    if node is None:
        return {'val': val, 'left': None, 'right': None}
    if val < node['val']:
        node['left'] = _insert(node['left'], val)
    else:
        node['right'] = _insert(node['right'], val)
    return node


def _inorder(node):
    if node is None:
        return []
    return _inorder(node['left']) + [str(node['val'])] + _inorder(node['right'])


def _height(node):
    if node is None:
        return -1
    return 1 + max(_height(node['left']), _height(node['right']))


def _find_parent(node, val):
    if node is None:
        return None
    if val < node['val']:
        if node['left'] is None:
            return node['val']
        return _find_parent(node['left'], val)
    else:
        if node['right'] is None:
            return node['val']
        return _find_parent(node['right'], val)
