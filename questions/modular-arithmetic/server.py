import random

PRIMES = [3, 5, 7, 11, 13]

def generate(data):
    a = random.randint(5, 15)
    b = random.randint(5, 15)
    m = random.choice(PRIMES)

    data['params']['a'] = a
    data['params']['b'] = b
    data['params']['m'] = m
    data['correct_answers']['result'] = (a * b) % m
