namespace DaxxnLoggerLibrary.Models
{
   /// <summary>
   /// Interface for a single log.
   /// </summary>
   public interface ILog
   {
      /// <inheritdoc/>
      LogType Type { get; }
      /// <summary>
      /// The severity of the log. Higher is more severe.
      /// </summary>
      int Severity { get; }
      /// <inheritdoc/>
      string Message { get; }
      /// <summary>
      /// Extra data to give context to the log.
      /// </summary>
      object Data { get; }

      /// <summary>
      /// Serializes the log for saving in binary format.
      /// </summary>
      /// <returns>A binary array containing the data.</returns>
      byte[] Serialize();
   }
}
