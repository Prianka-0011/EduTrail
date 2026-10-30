import random

def generate(data):
    primes = [2, 3, 5, 7, 11, 13, 17, 19, 23, 29]
    non_primes = [4, 6, 8, 9, 10, 12, 14, 15, 16, 18, 20, 21, 22, 25]

    correct = str(random.choice(primes))
    distractors = [str(x) for x in random.sample(non_primes, 3)]
    options = [correct] + distractors
    random.shuffle(options)
    data['params']['opts'] = options
    data['correct_answers']['ans'] = correct
