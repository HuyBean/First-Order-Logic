import re

# Load the knowledge base
knowledge_base = {}

def load_knowledge_base(file_name):
    with open(file_name, 'r') as file:
        for line in file:
            line = line.strip()
            if line and not line.startswith('%'):
                if line.endswith('.'):
                    match = re.match(r'([\w]+)\(([^)]+)\)\s*\.', line)
                    if match:
                        predicate = match.group(1)
                        arguments = match.group(2).split(',')
                        knowledge_base.setdefault(predicate, []).append(arguments)
                else:
                    match = re.match(r'([\w]+)\(([^)]+)\)\s*:-\s*([^)]+)', line)
                    if match:
                        predicate = match.group(1)
                        arguments = match.group(2).split(',')
                        rule = match.group(3)
                        knowledge_base.setdefault(predicate, []).append((arguments, rule))

# Backward reasoning
def query(predicate, arguments, query_type, bindings=None):
    if bindings is None:
        bindings = {}

    if predicate in knowledge_base:
        for fact in knowledge_base[predicate]:
            if isinstance(fact, list):
                if query_type == 'true_false':
                    substitutions = unify(fact, arguments, bindings)
                    if substitutions is not None:
                        return True
                elif query_type == 'variable_search':
                    substitutions = unify(fact, arguments, bindings)
                    if substitutions is not None:
                        return apply_substitutions(arguments[0], substitutions)
            else:
                rule_arguments, rule = fact
                rule_substitutions = unify(rule_arguments, arguments, bindings)
                if rule_substitutions is not None:
                    sub_query = re.findall(r'(\w+)\(([^\)]+)\)', rule)
                    sub_bindings = {k: v for k, v in bindings.items()}
                    sub_bindings.update(rule_substitutions)
                    for sub_predicate, sub_arguments in sub_query:
                        sub_predicate = sub_predicate.strip()
                        sub_arguments = [arg.strip() for arg in sub_arguments.split(',')]
                        sub_arguments = [apply_substitutions(arg, sub_bindings) for arg in sub_arguments]
                        result = query(sub_predicate, sub_arguments, query_type, sub_bindings)
                        if result is not None:
                            return result

    if query_type == 'true_false':
        return False

def unify(rule, arguments, bindings):
    substitutions = {k: v for k, v in bindings.items()}
    for i in range(len(rule)):
        if rule[i].startswith('_'):
            substitutions[rule[i]] = arguments[i]
        elif rule[i] != arguments[i]:
            return None
    return substitutions

def apply_substitutions(argument, substitutions):
    for key, value in substitutions.items():
        argument = argument.replace(key, value)
    return argument

# Main program
def main():
    load_knowledge_base('royal_family.pl')

    # User question
    question_predicate = input('Enter the predicate: ')
    question_arguments = input('Enter the arguments (comma-separated): ').split(',')
    query_type = input('Enter the query type (true_false/variable_search): ')

    # Perform the query
    result = query(question_predicate, question_arguments, query_type)
    print('Result:', result)

if __name__ == '__main__':
    main()
