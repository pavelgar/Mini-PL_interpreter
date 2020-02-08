using System;
using System.IO;

namespace miniPL {
    class Program {
        static void Main(string[] args) {
            string[] lines = File.ReadAllLines("./test_program.txt");

            Console.WriteLine("Contents of test_program.txt = ");
            foreach (string line in lines) {
                Console.WriteLine("\t" + line);
            }

            Console.WriteLine("Press any key to exit.");
            Console.ReadKey();
        }
    }
}
