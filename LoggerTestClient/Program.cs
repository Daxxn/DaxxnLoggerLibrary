using DaxxnLoggerLibrary;
using DaxxnLoggerLibrary.Models;

namespace LoggerTestClient
{
   internal class Program
   {
      static void Main(string[] args)
      {
         Console.WriteLine("Testing Client for DaxxnLoggerLibrary");

         string logPath = @"F:\Code\C#\CSharpLibraries\DaxxnLoggerLibrary\LoggerTestClient\TestLog.log";

         ConsoleLogger console = new();
         FileLogger logger = new(console, logPath);

         for (int i = 0; i < 200; i++)
         {
            logger.Log(LogType.Information, $"Log {i}");
         }

         logger.Save();
      }
   }
}