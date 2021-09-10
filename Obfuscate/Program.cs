using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Obfuscate
{
    class Program
    {
        static Random _random = new Random();

        static void Main(string[] args)
        {
            try
            {
                //Console.Write("Enter path: ");
                //var path = Console.ReadLine();
                var path = @"Z:\атмта\a.txt";

                //Console.Write("Enter path for save: ");
                // var path1 = Console.ReadLine();
                var path1 = @"Z:\атмта\a1.txt";

                DoWithFile(path, path1);
                /*var txt = new StringBuilder(File.ReadAllText(path));
                Console.WriteLine(txt);*/
            }
            catch
            {
                Console.WriteLine("error");
            }

            Console.Read();
        }

        static void DoWithFile(string path, string pathToSave)
        {
            var txt = ObfuscateCode(File.ReadAllText(path));

            Console.WriteLine(txt);
            File.WriteAllText(pathToSave, txt);
            
        }

        static string ObfuscateCode(string input)
        {
            var blockComments = @"/\*(.*?)\*/";
            var lineComments = @"//(.*?)\r?\n";
            var strings = @"""((\\[^\n]|[^""\n])*)""";
            var verbatimStrings = @"@(""[^""]*"")+";

            string noComments = Regex.Replace(input,
                blockComments + "|" + lineComments + "|" + strings + "|" + verbatimStrings,
                me => {
                    if (me.Value.StartsWith("/*") || me.Value.StartsWith("//"))
                        return me.Value.StartsWith("//") ? Environment.NewLine : "";
                    return me.Value;
                },
            RegexOptions.Singleline);

            var nc = Regex.Replace(noComments, Environment.NewLine, " ");

            var words = nc.Split(new char[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);
            var elems = new HashSet<string>();
            
            for (var _i = 0; _i < words.Length; _i++)
            {
                var w = words[_i];
                if (IsAVarOrFuncName(w)) elems.Add(getVarName(words[_i + 1]));
                _i++;
            }

            var txt = string.Join(" ", words);

            foreach (var e in elems)
            {
                var n = GenName();
                txt = Regex.Replace(txt, $@"\b{e}\b", n);
            }

            var _t0 = Regex.Replace(txt, " = ", "=");
            var _t1 = Regex.Replace(_t0, "; ", ";");

            return _t1;
        }

        static string getVarName(string input) =>
            Regex.Match(input, "[A-Za-z0-9]+").Value;

        static bool IsAVarOrFuncName(string word) {
            return word == "var" ||
                word == "let" ||
                word == "const" ||
                word == "function";
        }

        static string GenName()
        {
            var num = _random.Next(0, 0xFFFF);
            var str = "_0X" + num.ToString("X4");
            return str;
        }
    }
}
