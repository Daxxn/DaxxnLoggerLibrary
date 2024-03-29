﻿using System;
using System.Collections.Generic;

namespace DaxxnLoggerLibrary.Models
{
   /// <summary>
   /// The general type of log generated.
   /// </summary>
   public enum LogType
   {
      /// <inheritdoc/>
      Error = 0,
      /// <inheritdoc/>
      Warning = 1,
      /// <summary>
      /// General informational message.
      /// </summary>
      Information = 2,
      /// <summary>
      /// Something not specified. Not recommended to be used that often.
      /// </summary>
      Other = 3,
      /// <summary>
      /// The app has performed an action.
      /// </summary>
      Action = 4,
      /// <inheritdoc/>
      FileManagement = 5,
   }

   /// <summary>
   /// The base class for a single log.
   /// </summary>
   public class Log : ILog
   {
      /// <summary>
      /// The type of log this <see cref="Log"/> is.
      /// </summary>
      public LogType Type { get; private set; }
      /// <summary>
      /// The severity of the log. Higher is more severe.
      /// </summary>
      public int Severity { get; private set; }
      /// <summary>
      /// A message to explain what this log is for.
      /// </summary>
      public string Message { get; private set; }
      /// <summary>
      /// Optional data to give context to the log.
      /// <para>
      /// Usually meant for error logs.
      /// </para>
      /// </summary>
      public object Data { get; private set; }
      /// <summary>
      /// Time the log was generated.
      /// </summary>
      public DateTime Time { get; private set; }

      /// <inheritdoc/>
      public Log(LogType type, string message)
      {
         Type = type;
         Message = message;
         Time = DateTime.Now;
      }
      /// <inheritdoc/>
      public Log(LogType type, int severity, string message)
      {
         Type = type;
         Severity = severity;
         Message = message;
         Time = DateTime.Now;
      }
      /// <inheritdoc/>
      public Log(LogType type, string message, object data)
      {
         Type = type;
         Message = message;
         Data = data;
         Time = DateTime.Now;
      }
      /// <inheritdoc/>
      public Log(LogType type, int severity, string message, object data)
      {
         Type = type;
         Severity = severity;
         Message = message;
         Data = data;
         Time = DateTime.Now;
      }

      /// <summary>
      /// Returns a <see cref="string"/> representation of the <see cref="Log"/>.
      /// <para/>
      /// Used by <see cref="LoggerBase"/> to format the log.
      /// </summary>
      /// <returns>A formatted <see cref="string"/> representing the <see cref="Log"/>.</returns>
      public override string ToString() =>
         $"{Type} {Severity} {Message} {Time:MM/dd/yy_HH:mm:ss:fff} | {Data}";

      /// <summary>
      /// Serializes the log data to be saved to a file.
      /// <para>
      /// <list type="table">
      ///   <listheader>
      ///      <term>Type Codes</term>
      ///      <description>Byte codes for reconstructing the data.</description>
      ///   </listheader>
      ///   <item>
      ///      <term>0xF1</term>
      ///      <description>String</description>
      ///   </item>
      ///   <item>
      ///      <term>0xF2</term>
      ///      <description>Int or Byte</description>
      ///   </item>
      ///   <item>
      ///      <term>0xF3</term>
      ///      <description>Double</description>
      ///   </item>
      ///   <item>
      ///      <term>0xF4</term>
      ///      <description>Exception</description>
      ///   </item>
      ///   <item>
      ///      <term>0xF5</term>
      ///      <description>Object <para>Object.ToString() needs to be configured for proper reconstruction.</para></description>
      ///   </item>
      /// </list>
      /// </para>
      /// </summary>
      /// <returns>Byte array to be saved to a file</returns>
      public byte[] Serialize()
      {
         List<byte> output = new List<byte>()
         {
            (byte)Type,
            (byte)Severity
         };
         output.AddRange(ConvertString(Message));

         if (Data != null)
         {
            if (Data is string dataStr)
            {
               output.Add(0xF1);
               output.AddRange(ConvertString(dataStr));
            }
            else if (Data is byte b)
            {
               output.Add(0xF2);
               output.Add(b);
            }
            else if (Data is int i)
            {
               output.Add(0xF2);
               output.Add((byte)i);
            }
            else if (Data is double d)
            {
               output.Add(0xF3);
               output.Add((byte)d);
            }
            else if (Data is Exception ex)
            {
               output.Add(0xF4);
               output.AddRange(ConvertString(ex.Message));
               output.AddRange(ConvertString(ex.Source));
            }
            else
            {
               output.Add(0xF5);
               output.AddRange(ConvertString(Data.ToString()));
            }
         }

         return output.ToArray();
      }

      /// <summary>
      /// Converts a <see cref="string"/> into a <see cref="byte"/> <see cref="Array"/>.
      /// <para/>
      /// Adds line termination <see cref="byte"/>s to the start and end of the <see cref="string"/>.
      /// </summary>
      /// <param name="input"><see cref="string"/> to convert.</param>
      /// <returns>A <see cref="byte"/> <see cref="Array"/> representing the <see cref="string"/>.</returns>
      private static IEnumerable<byte> ConvertString(string input)
      {
         List<byte> output = new List<byte>() { 2 };
         if (!string.IsNullOrEmpty(input))
         {
            foreach (var c in input)
            {
               output.Add((byte)c);
            }
         }
         output.Add(3);

         return output;
      }
   }
}