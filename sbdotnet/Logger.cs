using sbdotnet.parallel;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace sbdotnet
{
    /// <summary>
    /// This is exposed so that the user can easily create a viewer.
    /// My advice would be a DataGrid bound to Logger.Events
    /// </summary>
    public class LogEvent
    {
        public enum EventCategory
        {
            Information,
            Warning,
            Error,
            Notify,
            Log,
            Debug
        }
        public EventCategory Category { get; set; }
        public DateTime Timestamp { get; set; } = DateTime.Now;
        public string Message { get; set; } = string.Empty;

        public override string ToString()
        {
            return $"Logger:{Category}:{Timestamp.ToString("dd.MM.~yy::hh.mm.ss")}:{Message}";
        }
    }


    public class Logger
    {
        ///////////////////////////////////////////////////////////
        #region Properties

        public static LockedObservableCollection<LogEvent> Events { get; private set; } = [];
        public static bool UseConsole { get; set; } = false;
        public static bool UseTrace { get; set; } = false;

        #endregion Properties
        ///////////////////////////////////////////////////////////
        

        ///////////////////////////////////////////////////////////
        #region Internal

        private static void NewEvent(LogEvent @event)
        {
            Events.Add(@event);
        }

        private static void NewEvent(LogEvent.EventCategory category, string message)
        {
            LogEvent ev = new()
            {
                Category = category,
                Message = message,
            };
            Events.Add(ev);

            if (UseConsole)
            {
                LogToConsole(ev);
            }
            if (UseTrace)
            {
                LogToTrace(ev);
            }
        }

        private static void LogToConsole(LogEvent logEvent)
        {
            Console.WriteLine($"Logger:{logEvent.Category}:{logEvent.Message}");
        }

        private static void LogToTrace(LogEvent logEvent)
        {
            Trace.WriteLine($"Logger:{logEvent.Category}:{logEvent.Message}");
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
            LogEvent @event = new()
            {
                Category = LogEvent.EventCategory.Warning,
                Message = ex.Message
            };
            if (ex.StackTrace is not null)
            {
                @event.Message += $"{Environment.NewLine}{ex.StackTrace}";
            }
            NewEvent(@event);
        }

        public static void Error(string message)
        {
            NewEvent(LogEvent.EventCategory.Error, message);
        }

        public static void Error(Exception ex)
        {
            LogEvent @event = new()
            {
                Category = LogEvent.EventCategory.Error,
                Message = ex.Message
            };
            if (ex.StackTrace is not null)
            {
                @event.Message += $"{Environment.NewLine}{ex.StackTrace}";
            }
            NewEvent(@event);
        }

        public static void Notify(string message)
        {
            NewEvent(LogEvent.EventCategory.Notify, message);
        }

        public static void Log(string message)
        {
            NewEvent(LogEvent.EventCategory.Log, message);
        }

        #endregion Interface
        ///////////////////////////////////////////////////////////

    }
}
