
using System;
using System.IO;
using System.Text.RegularExpressions;
using SWI_Simulation.DataType;

namespace SWI_Simulation
{
    internal class Program
    {
        static void Main(string[] args)
        {
            try
            {
                string? inputPath;
                string? outputPath;

                do
                {
                    Console.Write("Enter input KB path: ");
                    inputPath = Console.ReadLine();
                }
                while(string.IsNullOrEmpty(inputPath));
                Console.WriteLine(Path.GetFullPath(inputPath));
                
                Console.Write("Enter output result path: ");
                outputPath = Console.ReadLine(); 
                if (string.IsNullOrEmpty(outputPath))
                    outputPath = "default.txt";
                Console.WriteLine(Path.GetFullPath(outputPath));
                Console.WriteLine();
                Console.WriteLine();

                using (var rootStream = new FileStream(outputPath, FileMode.Create))
                {
                    using (var file = new StreamWriter(rootStream))
                    {
                        Console.Write("Starting at ");
                        Console.WriteLine(DateTime.Now.ToString("HH:mm:ss"));
                        var startTime = DateTime.Now;
                        KnowledgeBase KB = new KnowledgeBase();
                        readAndAnswerFromFile(KB, inputPath, file);
                        var consumeTime = (DateTime.Now - startTime).TotalSeconds;
                        file?.WriteLine($"Used {consumeTime} second(s)!");
                        Console.WriteLine($"Used {consumeTime} second(s)!");
                        Console.Write("End at ");
                        Console.WriteLine(DateTime.Now.ToString("HH:mm:ss"));
                    }
                }

                Console.WriteLine("Press any key to exit...");
                Console.ReadKey();
            }
            catch (Exception e)
            {
                Console.WriteLine("An exception has thrown!!!!!");
                Console.WriteLine(e);
                Console.ReadKey();
            }
        }
        public static void readAndAnswerFromFile(KnowledgeBase KB, string path, StreamWriter? file = null)
        {
            var lines = Regex.Replace
                        (
                            Regex.Replace(
                                File.ReadAllText(path),
                                RegexPattern.COMMENT_PATTERN,
                                "").Replace("\r", "\n")
                            ,
                            RegexPattern.MULTI_NEWLINE,
                            "\n"
                        )
                        .TrimStart('\n')
                        .TrimEnd('\n')
                        .Split("\n");
            foreach (var line in lines)
            {
                try
                {
                    if (Regex.Matches(line, RegexPattern.IGNORE_LINE).Count > 0)
                        continue;
                    if (Query.isQuery(line))
                    {
                        KB.addQuerries(line);
                        var q = KB.Queries[KB.Queries.Count - 1];
                        file?.WriteLine(q);
                        Console.WriteLine(q);
                        var Result = LogicProcess.ForwardChaning(KB, q, file).ToString() + ".";
                        file?.WriteLine(Result);
                        Console.WriteLine(Result);
                        file?.WriteLine($"Current fact: {KB.FactsCount}");
                        Console.WriteLine($"Current fact: {KB.FactsCount}");
                        file?.WriteLine();
                        Console.WriteLine();
                    }
                    else if (KB.isRule(line))
                        KB.addRule(line);
                    else if (KB.isFact(line))
                        KB.addFact(line);
                }
                catch
                {
                    continue;
                }

            }
        }
    }
}