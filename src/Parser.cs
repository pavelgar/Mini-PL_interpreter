using System.Collections.Generic;

namespace miniPL {
    public class Parser {
        private readonly Scanner scanner;
        private Token currentToken;

        public Parser(Scanner scanner) {
            this.scanner = scanner;
            this.currentToken = this.scanner.ScanToken();
        }
    }
}
