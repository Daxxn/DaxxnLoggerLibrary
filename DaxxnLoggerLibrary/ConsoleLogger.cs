using System;
using System.Threading.Tasks;

using DaxxnLoggerLibrary.Models;

namespace DaxxnLoggerLibrary
{
   /// <summary>
   /// <see cref="LoggerBase"/> implementation for a logger that will send messages to the <see cref="Console"/>.
   /// </summary>
   public class ConsoleLogger : LoggerBase
   {
      #region Constructors
      /// <summary>
      /// Creates a new <see cref="ConsoleLogger"/> at the end of the chain.
      /// <para>
      /// See <see href="https://en.wikipedia.org/wiki/Chain-of-responsibility_pattern">Chain of Responibility Pattern.</see>
      /// </para>
      /// </summary>
      public ConsoleLogger() : base() { }

      /// <summary>
      /// Creates a new <see cref="ConsoleLogger"/> at the end of the chain.
      /// <para>
      /// See <see href="https://en.wikipedia.org/wiki/Chain-of-responsibility_pattern">Chain of Responibility Pattern.</see>
      /// </para>
      /// </summary>
      /// <param name="severityLevel">Logger severity level</param>
      /// <param name="verbose">Set the logger into verbose mode</param>
      public ConsoleLogger(int severityLevel, bool verbose = false) : base(null, severityLevel, verbose) { }

      /// <summary>
      /// Creates a new <see cref="ConsoleLogger"/>.
      /// <para>
      /// See <see href="https://en.wikipedia.org/wiki/Chain-of-responsibility_pattern">Chain of Responibility Pattern.</see>
      /// </para>
      /// </summary>
      /// <param name="next">Next logger in the chain</param>
      public ConsoleLogger(ILogger next) : base(next) { }

      /// <summary>
      /// Creates a new <see cref="ConsoleLogger"/>. Sets the severity level for the console logger.
      /// <para>
      /// See <see href="https://en.wikipedia.org/wiki/Chain-of-responsibility_pattern">Chain of Responibility Pattern.</see>
      /// </para>
      /// </summary>
      /// <param name="next">Next logger in the chain</param>
      /// <param name="severityLevel">Logger severity level</param>
      /// <param name="verbose">Set the logger into verbose mode</param>
      public ConsoleLogger(ILogger next, int severityLevel, bool verbose = false) : base(next, severityLevel, verbose) { }
      #endregion

      #region Methods
      /// <inheritdoc/>
      protected override void AbstLog(ILog log) => Console.WriteLine(log);

      /// <inheritdoc/>
      protected override async Task AbstLogAsync(ILog log) => await Task.Run(() => AbstLog(log));

      /// <inheritdoc/>
      protected override void AbstSave() => Console.WriteLine("Save Logs");

      /// <inheritdoc/>
      protected override async Task AbstSaveAsync() => await Task.Run(() => AbstSave());
      #endregion
   }
}
