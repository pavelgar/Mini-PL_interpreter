using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace miniPL {
    class Program {
        static bool hadError = false;

        // Define the interpreter here to persist the state. Matters only in prompt mode.
        static readonly Interpreter interpreter = new Interpreter();
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
            if (hadError) Environment.Exit(65);
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
            List<Statement> statements = parser.Parse();

            if (hadError) return;

            interpreter.Interpret(statements);
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

        public static void RuntimeError(RuntimeError error) {
            Token token = error.token;
            Report(token.line, $"at '{token.rawValue}'", error.Message);
        }

        private static void Report(int line, string at, string message) {
            Console.Error.WriteLine($"[{line}] Error: {at}: {message}");
            hadError = true;
        }
    }
}
