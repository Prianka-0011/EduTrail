import random

def generate(data):
    correct = "7"
    distractors = ["4", "6", "9"]
    options = [correct] + distractors
    random.shuffle(options)
    data['params']['opts'] = options          # plain list, NOT json.dumps
    data['correct_answers']['ans'] = correct
