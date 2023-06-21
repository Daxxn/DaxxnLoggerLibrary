using System.Collections.Generic;
using System.Threading.Tasks;

using DaxxnLoggerLibrary.Models;

namespace DaxxnLoggerLibrary
{
   /// <summary>
   /// Base Logger class for derriving loggers.
   /// </summary>
   public abstract class LoggerBase : ILogger
   {
      #region Local Props

      /// <inheritdoc/>
      public int SeverityLevel { get; set; } = 0;

      /// <summary>
      /// <see cref="ILog"/>s Buffer.
      /// </summary>
      public List<ILog> Logs { get; private set; } = new List<ILog>();

      /// <summary>
      /// Next logger in the chain.
      /// <see langword="null"/> if end of chain.
      /// </summary>
      protected ILogger Next { get; private set; } = null;
      #endregion

      #region Constructors
      /// <summary>
      /// Constructs a base logger without a logger next in the chain.
      /// <para>
      /// See <see href="https://en.wikipedia.org/wiki/Chain-of-responsibility_pattern">Chain of Responibility Pattern.</see>
      /// </para>
      /// </summary>
      public LoggerBase() { }
      /// <summary>
      /// Construct logger and append next logger in chain.
      /// <para>
      /// See <see href="https://en.wikipedia.org/wiki/Chain-of-responsibility_pattern">Chain of Responibility Pattern.</see>
      /// </para>
      /// </summary>
      /// <param name="next">Next logger in the chain</param>
      public LoggerBase(ILogger next) => Next = next;
      /// <summary>
      /// Construct logger and append next logger in chain. Sets the priority index as well.
      /// <para>
      /// See <see href="https://en.wikipedia.org/wiki/Chain-of-responsibility_pattern">Chain of Responibility Pattern.</see>
      /// </para>
      /// </summary>
      /// <param name="next">Next logger in the chain</param>
      /// <param name="severityLevel">The severity for this logger</param>
      public LoggerBase(ILogger next, int severityLevel)
      {
         Next = next;
         SeverityLevel = severityLevel;
      }
      #endregion

      #region Methods
      /// <summary>
      /// Saves the logs.
      /// <para>
      /// Do nothing if saving is not required.
      /// </para>
      /// </summary>
      public void Save()
      {
         AbstSave();
         Next?.Save();
      }

      /// <summary>
      /// Send a log.
      /// </summary>
      /// <param name="log"><see cref="ILog"/> to add</param>
      public void Log(ILog log)
      {
         if (log.Severity < SeverityLevel) return;
         AbstLog(log);
         Next?.Log(log);
      }

      /// <summary>
      /// Send a log asyncronously.
      /// </summary>
      /// <param name="log"><see cref="ILog"/> to add</param>
      public async Task LogAsync(ILog log)
      {
         if (log.Severity < SeverityLevel) return;
         await AbstLogAsync(log);
         Next?.LogAsync(log);
      }

      /// <summary>
      /// Send a log.
      /// </summary>
      /// <param name="type">Log type</param>
      /// <param name="message">Log message</param>
      public void Log(string message, LogType type)
      {
         Log(new Log(type, message));
      }

      /// <summary>
      /// Send a log.
      /// </summary>
      /// <param name="message">log message</param>
      /// <param name="type">log type</param>
      /// <param name="severity">log severity level</param>
      public void Log(string message, LogType type, int severity)
      {
         Log(new Log(type, severity, message));
      }

      /// <summary>
      /// When overriden in a derived class, defines what the logger will do when an <see cref="ILog"/> is generated.
      /// </summary>
      /// <param name="log"><see cref="ILog"/> to pass down the chain</param>
      protected abstract void AbstLog(ILog log);

      /// <summary>
      /// When overriden in a derived class, defines what the logger will do when an <see cref="ILog"/> is generated.
      /// </summary>
      /// <param name="log"><see cref="ILog"/> to pass down the chain</param>
      protected abstract Task AbstLogAsync(ILog log);

      /// <summary>
      /// When overriden in a derived class, defines what the logger will do when the log buffer is saved.
      /// </summary>
      protected abstract void AbstSave();

      /// <summary>
      /// When overriden in a derived class, defines what the logger will do when the log buffer is saved asyncronously.
      /// </summary>
      protected abstract Task AbstSaveAsync();
      #endregion
   }
}