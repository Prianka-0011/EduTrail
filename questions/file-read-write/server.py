import random


def generate(data):
    cases = []
    for _ in range(5):
        n = random.randint(3, 6)
        nums = [random.randint(1, 99) for _ in range(n)]
        inp = str(n) + "\n" + " ".join(str(x) for x in nums) + "\n"
        expected = "Sum: " + str(sum(nums)) + "\nMax: " + str(max(nums))
        cases.append({"input": inp, "expected": expected})

    data["params"]["prompt"] = (
        "Read an integer N from standard input, then read N integers. "
        "Write those N integers to a file named numbers.txt, one integer per line. "
        "Then reopen numbers.txt, read the integers back from the file, and print their "
        "sum and their maximum."
    )
    data["params"]["sample_input"] = cases[0]["input"].strip()
    data["params"]["starter_code"] = (
        "#include <iostream>\n"
        "#include <fstream>\n"
        "using namespace std;\n"
        "int main() {\n"
        "    // 1) read N, then N integers from standard input\n"
        "    // 2) write them to numbers.txt (one integer per line)\n"
        "    // 3) reopen numbers.txt and read the integers back\n"
        "    // 4) print two lines, exactly:\n"
        "    //       Sum: (the sum)\n"
        "    //       Max: (the maximum)\n"
        "    return 0;\n"
        "}\n"
    )
    # Test cases keyed by the answers-name (never exposed to the student).
    data["correct_answers"]["code"] = cases
