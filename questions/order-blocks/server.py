import random

def generate(data):
    correct = [
        "Set low = 0, high = len(arr) - 1",
        "While low <= high:",
        "    mid = (low + high) // 2",
        "    If arr[mid] == target: return mid",
        "    If arr[mid] < target: low = mid + 1",
        "    Else: high = mid - 1",
        "Return -1"
    ]
    shuffled = correct[:]
    random.shuffle(shuffled)
    data['params']['opts'] = shuffled
    data['correct_answers']['result'] = correct
