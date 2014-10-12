using System;
using System.IO;
using System.Net;

namespace ProCon2014.Client
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        private static void Main(string[] args)
        {
            if (args.Length <= 2)
            {
                PrintUsage();
                return;
            }
            var cmd = args[0];
            var server = args[1];
            switch (cmd)
            {
                case "GetProblem":
                    byte[] res;
                    try
                    {
                        res = ClientLib.GetProblem(server, args[2]);
                    }
                    catch (SystemException e)
                    {
                        Console.WriteLine(e.Message);
                        return;
                    }
                    if (args.Length >= 4)
                    {
                        using (var s = File.OpenWrite(args[3]))
                        {
                            s.Write(res, 0, res.Length);
                        }
                    }
                    else
                    {
                        using (var s = Console.OpenStandardOutput())
                        {
                            s.Write(res, 0, res.Length);
                        }
                    }
                    break;

                case "SubmitAnswer":
                    if (args.Length <= 3)
                    {
                        PrintUsage();
                        break;
                    }

                    string answer;
                    if (args.Length >= 5)
                    {
                        using (var s = File.OpenRead(args[4]))
                        {
                            answer = ReadString(s);
                        }
                    }
                    else
                    {
                        using (var s = Console.OpenStandardInput())
                        {
                            answer = ReadString(s);
                        }
                    }
                    string resp;
                    try
                    {
                        resp = ClientLib.SubmitAnswer(server, args[2], args[3], answer);
                    }
                    catch (WebException e)
                    {
                        Console.WriteLine(e.Message);
                        return;
                    }
                    Console.WriteLine(resp);
                    break;

                default:
                    PrintUsage();
                    break;
            }
        }

        private static string ReadString(Stream s)
        {
            StreamReader sr = new StreamReader(s);
            return sr.ReadToEnd();
        }

        private static void PrintUsage()
        {
            Console.WriteLine("Usage");
            Console.WriteLine("GetProblem (server) (problemid) [filename]");
            Console.WriteLine("    Get prob(problemid).ppm from (server).");
            Console.WriteLine("    Save to [filename] if [filename] is specified.");
            Console.WriteLine("    Otherwise just print out to the standard output.");
            Console.WriteLine("SubmitAnswer (server) (problemid) (playerid) [filename]");
            Console.WriteLine("    Put the answer to (server).");
            Console.WriteLine("    Read the answer from [filename] if [filename] is specified.");
            Console.WriteLine("    Otherwise read the answer from standard input.");
            Console.WriteLine("    Submission status (i.e. ACCEPT, ERROR or so forth.) will be print out to the standard output.");
        }
    }
}