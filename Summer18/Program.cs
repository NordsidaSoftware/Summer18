using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


//=====================================================================================
//                                SUMMER '18
//           
//                            A Compiler project 
//   Main starter uten argumenter for REPL
//   Vedlagt text.txt som kan startes fra build.options
// 
//=====================================================================================

namespace Summer18
{
//=====================================================================================
//                             CLASS PROGRAM
    class Program
    {
        static void Main(string[] args)
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("Summer 18 Compiler\n");
            if (args.Length > 1 ) { Console.WriteLine("Usage : Summer18 [sourcefile.txt]"); }
            if (args.Length == 1) { LoadSource(args[0]);}
            else { Run(); }
        }

        static void LoadSource(string path)
        {
            if (File.Exists(path))
            {
                var source = File.ReadAllLines(path);
                StringBuilder sb = new StringBuilder();
                foreach (string s in source) { sb.Append(s + " "); }

                Run(sb.ToString());
            }

            else Console.WriteLine("Error reading file : " + path);

            Console.Read();
            
        }

        public static void Run()
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Summer'18 REPL. Ctrl - C to exit.\n");
            Console.ForegroundColor = ConsoleColor.White;
            while (true)
            {
                Console.Write(">>>");
                string input = Console.ReadLine();
                Run(input);
            }
        }

        public static void Run(string source)
        {
            Lexer lex = new Lexer(source);
            Parser parser = new Parser(lex);
            parser.Parse();
        }
    }
//============================END CLASS PROGRAM=========================================
}
