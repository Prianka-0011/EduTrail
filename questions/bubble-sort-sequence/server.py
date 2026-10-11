import random

BLOCKS = [
    "def bubble_sort(arr):",
    "    n = len(arr)",
    "    for i in range(n):",
    "        for j in range(n - i - 1):",
    "            if arr[j] > arr[j + 1]:",
    "                arr[j], arr[j + 1] = arr[j + 1], arr[j]",
    "    return arr",
]

def generate(data):
    shuffled = BLOCKS[:]
    random.shuffle(shuffled)
    data['params']['options'] = shuffled
    data['correct_answers']['sequence'] = BLOCKS
