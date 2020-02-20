using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace miniPL {
    class Program {
        static bool hadError = false;
        static void Main(string[] args) {
            if (args.Length > 1) {
                Console.WriteLine("ERROR: Too many arguments");
                Environment.Exit(64);
            } else if (args.Length == 1) {
                RunFile(args[0]);
            } else {
                RunPrompt();
            }
        }

        private static void RunFile(string path) {
            byte[] bytes = File.ReadAllBytes(path);
            Run(Encoding.UTF8.GetString(bytes));
        }

        private static void RunPrompt() {
            for (;;) {
                Console.Write("> ");
                Run(Console.ReadLine());
            }
        }

        private static void Run(string source) {
            Scanner scanner = new Scanner(source);
            List<Token> tokens = scanner.ScanTokens();
            Parser parser = new Parser(tokens);
            List<Statement> result = parser.Stmts();
            result.ForEach(Console.WriteLine);
        }

        public static void Error(int line, string message) {
            Report(line, "", message);
        }

        public static void Error(Token token, String message) {
            if (token.type == TokenType.EOF) {
                Report(token.line, "at end", message);
            } else {
                Report(token.line, $"at '{token.rawValue}'", message);
            }
        }

        private static void Report(int line, string at, string message) {
            Console.Error.WriteLine($"[{line}] Error: {at}: {message}");
            hadError = true;
        }
    }
}
