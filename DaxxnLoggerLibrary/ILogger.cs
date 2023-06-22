using System.Collections.Generic;
using System.Threading.Tasks;

using DaxxnLoggerLibrary.Models;

namespace DaxxnLoggerLibrary
{
   /// <summary>
   /// Interface for deriving new loggers from <see cref="LoggerBase"/>.
   /// </summary>
   public interface ILogger
   {
      /// <summary>
      /// Defines the priority level for the logger. If the log has a lower priority, it will not be used with this logger.
      /// </summary>
      int SeverityLevel { get; set; }

      /// <summary>
      /// <see cref="ILog"/>s Buffer.
      /// </summary>
      List<ILog> Logs { get; }

      /// <summary>
      /// Saves the logs.
      /// <para>
      /// Do nothing if saving is not required.
      /// </para>
      /// </summary>
      void Save();

      /// <summary>
      /// Add an <see cref="ILog"/>.
      /// <para/>
      /// Will also check the log file size.
      /// </summary>
      /// <param name="log"><see cref="ILog"/> to add.</param>
      void Log(ILog log);

      /// <summary>
      /// Send a log.
      /// </summary>
      /// <param name="message">Log message</param>
      /// <param name="type">Log type</param>
      void Log(string message, LogType type);

      /// <summary>
      /// Send a log.
      /// </summary>
      /// <param name="message">Log message</param>
      /// <param name="type">Log type</param>
      /// <param name="severity">Log severity</param>
      void Log(string message, LogType type, int severity);

      /// <summary>
      /// Add an <see cref="ILog"/> async.
      /// <para/>
      /// Will also check the log file size.
      /// </summary>
      /// <param name="log"><see cref="ILog"/> to add.</param>
      Task LogAsync(ILog log);
   }
}
