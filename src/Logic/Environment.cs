using System.Collections.Generic;

namespace miniPL {
    public class Environment {
        private readonly Dictionary<string, object> values = new Dictionary<string, object>();

        public void Define(string name, object value) {
            values[name] = value;
        }

        public object Get(Token token) {
            if (values.ContainsKey(token.rawValue))
                return values[token.rawValue];

            throw new RuntimeError(token, $"Undefined variable '{token.rawValue}'.");
        }
    }
}
