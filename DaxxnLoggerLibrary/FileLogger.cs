using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

using DaxxnLoggerLibrary.Models;

namespace DaxxnLoggerLibrary
{
   /// <summary>
   /// <see cref="LoggerBase"/> implementation for saving <see cref="ILog"/>s to a file.
   /// </summary>
   public class FileLogger : LoggerBase
   {
      #region Local Props
      /// <summary>
      /// Log file save path
      /// </summary>
      public string SavePath { get; set; }
      private FileInfo _file;

      /// <summary>
      /// Line count read from the log file.
      /// </summary>
      private long FileLineCount { get; set; }

      /// <summary>
      /// Sets the threshold when to save the current logs to the log file.
      /// <para/>
      /// Default = 100 lines
      /// <para/>
      /// Should be lower or equal to <see cref="MaxFileLines"/>.
      /// </summary>
      public static long SaveLogsThreshold { get; set; } = 100;

      /// <summary>
      /// Maximum line count before the old logs are dropped from the log file.
      /// <para/>
      /// Default = 500 lines
      /// <para/>
      /// Should be higher or equal to <see cref="SaveLogsThreshold"/>.
      /// </summary>
      public static long MaxFileLines { get; set; } = 500;
      #endregion

      #region Constructors
      /// <summary>
      /// Create a new <see cref="FileLogger"/> at the end of the chain.
      /// <para>
      /// See <see href="https://en.wikipedia.org/wiki/Chain-of-responsibility_pattern">Chain of Responibility Pattern.</see>
      /// </para>
      /// </summary>
      public FileLogger(string savePath) : base(null)
      {
         SavePath = savePath;
         _file = new FileInfo(savePath);
      }

      /// <summary>
      /// Create a new <see cref="FileLogger"/> at the end of the chain.
      /// <para>
      /// See <see href="https://en.wikipedia.org/wiki/Chain-of-responsibility_pattern">Chain of Responibility Pattern.</see>
      /// </para>
      /// </summary>
      /// <param name="savePath">Log file save path</param>
      /// <param name="severityLevel">Severity level for this logger</param>
      public FileLogger(string savePath, int severityLevel) : base(null, severityLevel)
      {
         SavePath = savePath;
         _file = new FileInfo(SavePath);
      }

      /// <summary>
      /// Create a new <see cref="FileLogger"/>.
      /// <para>
      /// See <see href="https://en.wikipedia.org/wiki/Chain-of-responsibility_pattern">Chain of Responibility Pattern.</see>
      /// </para>
      /// </summary>
      /// <param name="next">Next logger in the chain</param>
      /// <param name="savePath">Log file save path</param>
      public FileLogger(ILogger next, string savePath) : base(next)
      {
         SavePath = savePath;
         _file = new FileInfo(savePath);
      }

      /// <summary>
      /// Create a new <see cref="FileLogger"/>.
      /// <para>
      /// See <see href="https://en.wikipedia.org/wiki/Chain-of-responsibility_pattern">Chain of Responibility Pattern.</see>
      /// </para>
      /// </summary>
      /// <param name="next">Next logger in the chain</param>
      /// <param name="savePath">Log file save path</param>
      /// <param name="severityLevel">Severity index for this logger</param>
      public FileLogger(ILogger next, string savePath, int severityLevel) : base(next, severityLevel)
      {
         SavePath = savePath;
         _file = new FileInfo(savePath);
      }

      /// <summary>
      /// Create a new <see cref="FileLogger"/>.
      /// <para>
      /// See <see href="https://en.wikipedia.org/wiki/Chain-of-responsibility_pattern">Chain of Responibility Pattern.</see>
      /// </para>
      /// </summary>
      /// <param name="next">Next logger in the chain</param>
      /// <param name="savePath">Log file save path</param>
      /// <param name="maxFileSize">Maximum line count of the log file</param>
      public FileLogger(ILogger next, string savePath, long maxFileSize) : base(next)
      {
         SavePath = savePath;
         _file = new FileInfo(savePath);
         MaxFileLines = maxFileSize;
      }

      /// <summary>
      /// Create a new <see cref="FileLogger"/>.
      /// <para>
      /// See <see href="https://en.wikipedia.org/wiki/Chain-of-responsibility_pattern">Chain of Responibility Pattern.</see>
      /// </para>
      /// </summary>
      /// <param name="next">Next logger in the chain</param>
      /// <param name="savePath">Log file save path</param>
      /// <param name="maxFileSize">Maximum line count of the log file</param>
      /// <param name="severityLevel">Severity index for this logger</param>
      public FileLogger(ILogger next, string savePath, long maxFileSize, int severityLevel) : base(next, severityLevel)
      {
         SavePath = savePath;
         _file = new FileInfo(savePath);
         MaxFileLines = maxFileSize;
      }
      #endregion

      #region Methods
      private bool CheckFileSize()
      {
         if (!_file.Exists) return true;
         using (var reader = _file.OpenText())
         {
            FileLineCount = 0;
            while (!reader.EndOfStream)
            {
               if (reader.Read() == '\n')
               {
                  FileLineCount++;
               }
            }
            return FileLineCount < MaxFileLines;
         }
      }

      /// <summary>
      /// Shortens the log file to keep the size of the file from becoming too large.
      /// </summary>
      /// <returns>Shortened log file lines.</returns>
      private void ShortenFile()
      {
         var lines = new List<string>();
         using (StreamReader reader = new StreamReader(SavePath))
         {
            long currentReadLength = 0;
            while (!reader.EndOfStream)
            {
               var line = reader.ReadLine();
               currentReadLength++;
               if (line == null)
                  continue;
               lines.Add(line);
            }
            if (currentReadLength + Logs.Count > MaxFileLines)
            {
               lines.RemoveRange(0, (lines.Count - (int)MaxFileLines) + Logs.Count);
            }
         }

         using (var writer = new StreamWriter(SavePath))
         {
            foreach (var line in lines)
            {
               writer.WriteLine(line);
            }
         }
      }

      /// <inheritdoc/>
      protected override void AbstSave()
      {
         if (CheckFileSize())
         {
            _file = new FileInfo(SavePath);
            using (var writer = _file.AppendText())
            {
               foreach (var log in Logs)
               {
                  writer.WriteLine(log.ToString());
               }
               writer.Flush();

               Logs.Clear();
            }
         }
         else
         {
            ShortenFile();
            _file = new FileInfo(SavePath);
            using (var writer = _file.AppendText())
            {
               foreach (var log in Logs)
               {
                  writer.WriteLine(log.ToString());
               }
               writer.Flush();

               Logs.Clear();
            }
         }
      }

      /// <inheritdoc/>
      protected override async Task AbstSaveAsync()
      {
         await Task.Run(() => AbstSave());
      }

      /// <inheritdoc/>
      protected override void AbstLog(ILog log)
      {
         Logs.Add(log);
         if (Logs.Count > SaveLogsThreshold)
         {
            AbstSave();
         }
      }

      /// <inheritdoc/>
      protected override async Task AbstLogAsync(ILog log)
      {
         await Task.Run(async () =>
         {
            Logs.Add(log);

            if (Logs.Count > SaveLogsThreshold)
            {
               await AbstSaveAsync();
            }
         });
      }
      #endregion
   }
}