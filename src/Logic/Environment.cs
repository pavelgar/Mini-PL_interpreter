using System.Collections.Generic;

namespace miniPL {
    public class Environment {
        private readonly Dictionary<string, object> values = new Dictionary<string, object>();

        void Define(string name, object value) {
            values[name] = value;
        }

        object Get(Token token) {
            if (values.ContainsKey(token.rawValue))
                return values[token.rawValue];

            throw new RuntimeError(token, $"Undefined variable '{token.rawValue}'.");
        }
    }
}
