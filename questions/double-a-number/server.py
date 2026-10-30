import random

def generate(data):
    x = random.randint(1, 10)
    data['params']['x'] = x
    data['correct_answers']['result'] = x * 2
