import random
import math

def generate(data):
    # Use powers of 2 so n * log2(n) is always a whole number
    x = random.choice([2, 4, 8, 16, 32])
    data['params']['x'] = x
    data['correct_answers']['result'] = x * int(math.log2(x))
