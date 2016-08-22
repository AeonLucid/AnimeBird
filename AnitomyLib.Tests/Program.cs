using System;
using System.IO;
using System.Linq;
using AnimeBird.Parser;
using AnimeBird.Parser.Tokens;
using Newtonsoft.Json.Linq;

namespace AnitomyLib.Tests
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var file = File.ReadAllText("Data/anime.json");
            var json = JArray.Parse(file);

            foreach (var anime in json)
            {
                var fileName = anime["file_name"].ToString();
                var fileNameParser = new FileNameParser(fileName);
                fileNameParser.Parse();

                if (fileNameParser.Tokens.Any(x => x.Category == TokenCategory.Unknown))
                {
                    WriteOutColor($"Oops, '{fileName}' was not parsed successfully.", ConsoleColor.Red);

                    WriteOutColor("==================================== Tokens", ConsoleColor.DarkYellow);
                    foreach (var token in fileNameParser.Tokens)
                    {
                        if (token.Category == TokenCategory.Unknown)
                        {
                            WriteOutColor(token.ToString(), ConsoleColor.DarkRed);
                        }
                        else if(token.Category == TokenCategory.Identifier)
                        {
                            WriteOutColor(token.ToString(), ConsoleColor.DarkCyan);
                        }
                        else
                        {
                            WriteOutColor(token.ToString(), ConsoleColor.DarkGreen);
                        }
                    }

                    WriteOutColor("==================================== Elements", ConsoleColor.DarkYellow);
                    foreach (var pair in fileNameParser.Elements)
                    {
                        WriteOutColor($"{pair.Key,-17}|{pair.Value}", ConsoleColor.DarkCyan);
                    }

                    Console.WriteLine("Press a key to exit.");
                    Console.ReadKey();
                    Environment.Exit(1);
                }

                break;
            }

            Console.WriteLine("Press a key to exit.");
            Console.ReadKey();
        }

        private static void WriteOutColor(string str, ConsoleColor color = ConsoleColor.Gray)
        {
            Console.ForegroundColor = color;
            Console.WriteLine(str);
            Console.ResetColor();
        }
    }
}
