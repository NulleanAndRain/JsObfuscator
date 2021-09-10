using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Obfuscate
{
    class Program
    {
        static Random _random = new Random();
        static Regex _regex = new Regex("[A-Za-z0-9]");

        static void Main(string[] args)
        {
            //Console.WriteLine(12.ToString("X4"));
            //Console.ReadKey();
            
            try
            {
                Console.WriteLine("Enter code: ");
                var code = Console.ReadLine();
                var txt = DoWithCode(code);
                /*var txt = new StringBuilder(File.ReadAllText(path));
                Console.WriteLine(txt);*/
                Console.WriteLine(txt);
            }
            catch
            {
                Console.WriteLine("error");
            }

            Console.Read();
        }

        static string DoWithCode(string path)
        {
            // var txt = File.ReadAllText(path);
            var txt = path;

            var vars = new HashSet<string>();
            var words = txt.Split(new char[] { ' ', '\n', '\t' }, StringSplitOptions.RemoveEmptyEntries);

            var elemsPos = words.Where(_w => IsAVarOrFuncName(_w))
                .Select(word =>
                    Array.FindIndex(words, w => w == word)
                ).ToList();


            foreach (var i in elemsPos)
            {
                string name;
                do
                {
                    name = GenName();
                } while (!vars.Add(name));

                var _w = words[i + 1].Split("()[] ".ToCharArray(), StringSplitOptions.RemoveEmptyEntries)[0];
                Regex.Replace(txt, _w, name);
            }

            // var res = string.Join(" ", words);

            //Console.WriteLine(res);
            
            // FileStream fs = new FileStream(pathToSave, FileMode.OpenOrCreate);
            // File.WriteAllText(pathToSave, txt);

            return txt;
            
        }

        static bool IsAVarOrFuncName(string word) =>
            word == "var" ||
            word == "let" ||
            word == "const" ||
            word == "function";

        static string GenName()
        {
            var num = _random.Next(0, 0xFFFF);
            var str = "_0X" + num.ToString("4X");
            return str;
        }
    }
}
