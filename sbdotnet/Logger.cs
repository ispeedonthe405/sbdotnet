using System.Collections.ObjectModel;

namespace sbdotnet
{
    public class LogEvent
    {
        public enum EventCategory
        {
            Information,
            Warning,
            Error,
            Notify,
            Debug
        }
        public EventCategory Category { get; set; }
        public DateTime Timestamp { get; set; } = DateTime.Now;
        public string Message { get; set; } = string.Empty;
    }

    public class Logger
    {
        ///////////////////////////////////////////////////////////
        #region Properties

        public static ObservableCollection<LogEvent> Events { get; private set; } = [];
        public static bool UseConsole { get; set; } = false;

        #endregion Properties
        ///////////////////////////////////////////////////////////


        ///////////////////////////////////////////////////////////
        #region Internal

        private static void NewEvent(LogEvent.EventCategory category, string message)
        {
            LogEvent ev = new()
            {
                Category = category,
                Message = message,
            };
            Events.Add(ev);

            if(UseConsole)
            {
                LogToConsole(ev);
            }
        }

        private static void LogToConsole(LogEvent logEvent)
        {
            Console.Write($"Logger:{logEvent.Category}:{Environment.NewLine}{logEvent.Message}{Environment.NewLine}");
        }

        #endregion Internal
        ///////////////////////////////////////////////////////////
        

        ///////////////////////////////////////////////////////////
        #region Interface

        public static void Information(string message)
        {
            NewEvent(LogEvent.EventCategory.Information, message);
        }

        public static void Warning(string message)
        {
            NewEvent(LogEvent.EventCategory.Warning, message);
        }

        public static void Warning(Exception ex)
        {
            NewEvent(LogEvent.EventCategory.Warning, ex.Message);
        }

        public static void Error(string message)
        {
            NewEvent(LogEvent.EventCategory.Error, message);
        }

        public static void Error(Exception ex)
        {
            NewEvent(LogEvent.EventCategory.Error, ex.Message);
        }

        public static void Notify(string message)
        {
            NewEvent(LogEvent.EventCategory.Notify, message);
        }

        public static void Debug(string message)
        {
#if DEBUG
            NewEvent(LogEvent.EventCategory.Debug, message);
#endif
        }

        public static void Debug(Exception ex)
        {
#if DEBUG
            NewEvent(LogEvent.EventCategory.Debug, ex.Message);
#endif
        }

        #endregion Interface
        ///////////////////////////////////////////////////////////

    }
}
