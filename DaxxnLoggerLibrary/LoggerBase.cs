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
      /// <param name="next">Next logger in chain.</param>
      public LoggerBase(ILogger next) => Next = next;
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
      /// Add a <see cref="ILog"/> to the buffer.
      /// </summary>
      /// <param name="log"><see cref="ILog"/> to add.</param>
      public void Log(ILog log)
      {
         AbstLog(log);
         Next?.Log(log);
      }

      /// <summary>
      /// Add a <see cref="ILog"/> to the buffer async.
      /// </summary>
      /// <param name="log"><see cref="ILog"/> to add.</param>
      public async Task LogAsync(ILog log)
      {
         await AbstLogAsync(log);
         Next?.LogAsync(log);
      }

      /// <summary>
      /// Add a <see cref="ILog"/> to the buffer.
      /// </summary>
      /// <param name="type">Log type</param>
      /// <param name="message">Log message</param>
      public void Log(LogType type, string message)
      {
         Log(new Log(type, message));
      }

      /// <summary>
      /// When overriden in a derived class, defines what the logger will do when an <see cref="ILog"/> is generated.
      /// </summary>
      /// <param name="log"><see cref="ILog"/> to pass down the chain.</param>
      protected abstract void AbstLog(ILog log);

      /// <summary>
      /// When overriden in a derived class, defines what the logger will do when an <see cref="ILog"/> is generated.
      /// </summary>
      /// <param name="log"><see cref="ILog"/> to pass down the chain.</param>
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