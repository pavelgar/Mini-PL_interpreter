using System.Collections.Generic;

namespace miniPL {
    public class Environment {
        private readonly Dictionary<string, object> variables = new Dictionary<string, object>();
        private readonly HashSet<string> control = new HashSet<string>();

        public void Define(Token token, object value) {
            if (!variables.ContainsKey(token.rawValue)) {
                variables[token.rawValue] = value;
                return;
            }
            throw new RuntimeError(token, $"Variable '{token.rawValue}' already defined.");
        }

        public object Get(Token token) {
            if (variables.ContainsKey(token.rawValue)) {
                return variables[token.rawValue];
            }
            throw new RuntimeError(token, $"Undefined variable '{token.rawValue}'.");
        }

        public void Assign(Token token, object value) {
            if (control.Contains(token.rawValue)) {
                throw new RuntimeError(token, $"Can't assign to a control variable '{token.rawValue}'.");
            }
            if (variables.ContainsKey(token.rawValue)) {
                variables[token.rawValue] = value;
                return;
            }
            throw new RuntimeError(token, $"Undefined variable '{token.rawValue}'.");
        }

        public void ControlAssign(Token token, object value) {
            if (!control.Contains(token.rawValue)) {
                throw new RuntimeError(token, $"'{token.rawValue}' is not a control variable.");
            }
            if (!variables.ContainsKey(token.rawValue)) {
                throw new RuntimeError(token, $"Undefined variable '{token.rawValue}'.");
            }
            variables[token.rawValue] = value;

        }

        public void SetAsControl(Token token) {
            if (variables.ContainsKey(token.rawValue)) {
                if (!control.Add(token.rawValue)) {
                    throw new RuntimeError(token,
                        $"Variable '{token.rawValue}' is already in use as a control variable.");
                }
            } else {
                throw new RuntimeError(token, $"Undefined variable '{token.rawValue}'.");
            }
        }

        public void RemoveFromControl(Token token) {
            if (!control.Remove(token.rawValue)) {
                throw new RuntimeError(token, $"'{token.rawValue}' is not a control variable.");
            }
        }

    }
}
