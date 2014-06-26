using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ReplaceTokens
{
    class Program
    {
        static int Main(string[] args)
        {
            if (args.Count() != 3)
            {
                Console.WriteLine("Invalid arguments. Usage: ReplaceTokens <PathAndFilename> <TokenToReplace> <ReplacementValue>");
                return 1;
            }

            try
            {
                List<string> fileIn = File.ReadAllLines(args[0]).ToList();

                List<string> fileOut = fileIn.Select(s => s.Replace(args[1], args[2])).ToList();

                File.WriteAllLines(args[0], fileOut);
            }
            catch (Exception ex)
            {
                Console.WriteLine("An exception has occurred:\n{0}", ex.Message);
                return 1;
            }

            return 0;
        }
    }
}
