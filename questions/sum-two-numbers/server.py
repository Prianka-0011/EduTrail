import random


def generate(data):
    cases = []
    for _ in range(5):
        a = random.randint(1, 99)
        b = random.randint(1, 99)
        cases.append({"input": str(a) + " " + str(b) + "\n", "expected": str(a + b)})

    data["params"]["prompt"] = "Read two integers from standard input and print their sum."
    data["params"]["sample_input"] = cases[0]["input"].strip()
    data["params"]["starter_code"] = (
        "#include <iostream>\n"
        "using namespace std;\n"
        "int main() {\n"
        "    // read two integers and print their sum\n"
        "    return 0;\n"
        "}\n"
    )
    # Test cases keyed by the answers-name, like the other graders expect.
    data["correct_answers"]["code"] = cases
    # Model solution shown by "Show Correct Answer" (test cases stay hidden).
    data["correct_answers"]["solution"] = r'''#include <iostream>
using namespace std;
int main() {
    int a, b;
    cin >> a >> b;
    cout << a + b;
    return 0;
}
'''
