# Logger Library

My own simple logging library. It cant do super fancy things but thats the point. You need something that can do more? Use a more advanced logger. =) I may implement a DI logger but there's more effective options out there, hell even the built in logger would be better.

## Features

- Log to File
- Log to Console
- Exposed base model for custom loggers

## Usage With WPF

app.xaml.cs
```cs
public class App : Application
{
   public static ILogger Logger { get; init; }

   protected override void OnStartup(StartupEventArgs e)
   {
      // Recommend saving the logs file in the users AppData/local folder.
      string appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
      string appFolderPath = Path.Combine(appDataPath, "NameOfApp");
      string path = Path.Combine(appDataPath, "NameOfApp", "Logs.txt");

      
      // Setup logger chain of responibility. End of chain is passed null.
      var ConsoleLog = new(null);
      var MessageBoxLog = new(ConsoleLog);
      Logger = new(MessageBoxLog, path);

      Logger.Log(new Log(LogType.Information, "Startup"));
   }

   protected override void OnExit(ExitEventArgs e)
   {
      Logger.Log(new Log(LogType.Information, "Exit"));
      Logger.Save();
      base.OnExit(e);
   }
}
```

In ViewModel Files

```cs
App.Logger.Log(new Log(LogType.Error, e.Message));
```