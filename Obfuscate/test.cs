//Rextester.Program.Main is the entry point for your code. Don't change it.
//Microsoft (R) Visual C# Compiler version 2.9.0.63208 (958f2354)

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Rextester
{
    public class Program
    {
        static Random _random = new Random();
        static Regex _regex = new Regex("[A-Za-z0-9]");
        
        public static void Main(string[] args) {
            var c = ObfuscateCode(_code);
            Console.WriteLine(c);
        }
        
        static string ObfuscateCode(string txt)
        {
            Regex.Replace(txt, "//[.]*[\n]$", string.Empty);

            var words = txt.Split(new char[] { ' ', '\n', '\t' }, StringSplitOptions.RemoveEmptyEntries);

            var elemsPos = words.Where(_w => IsAVarOrFuncName(_w))
                .Select(word =>
                    Array.FindIndex(words, w => w == word)
                ).ToList();


            var vars = new HashSet<string>();
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
            txt = string.Join(" ", words);
            return txt;
        }

        static bool IsAVarOrFuncName(string word) {
            return word == "var" ||
                word == "let" ||
                word == "const" ||
                word == "function";
        }

        static string GenName()
        {
            var num = _random.Next(0, 0xFFFF);
            var str = "_0X" + num.ToString("4X");
            return str;
        }
        
        static string _code = @"
    //initial
var w = c.width = window.innerWidth;
var h = c.height = window.innerHeight,
var ctx = c.getContext('2d'),
    
    //parameters
var total = w,
var accelleration = .05,
    
    //afterinitial calculations
var size = w/total,
var occupation = w/total,
var repaintColor = 'rgba(0, 0, 0, .04)'
var colors = [],
var dots = [],
var dotsVel = [];

//setting the colors' hue
//and y level for all dots
var portion = 360/total;
for(var i = 0; i < total; ++i){
  colors[i] = portion * i;
  
  dots[i] = h;
  dotsVel[i] = 10;
}

function anim(){
  window.requestAnimationFrame(anim);
  
  ctx.fillStyle = repaintColor;
  ctx.fillRect(0, 0, w, h);
  
  for(var i = 0; i < total; ++i){
    var currentY = dots[i] - 1;
    dots[i] += dotsVel[i] += accelleration;
    
    ctx.fillStyle = 'hsl('+ colors[i] + ', 80%, 50%)';
    ctx.fillRect(occupation * i, currentY, size, dotsVel[i] + 1);
    
    if(dots[i] > h && Math.random() < .01){
      dots[i] = dotsVel[i] = 0;
    }
  }
}

anim();";
    }
}