using System;
using System.Collections.Generic;

namespace PhotoWF.Common
{
    /// <summary>
    /// Immutable argument class for handling entry inseratition into the log.
    /// </summary>
    public class LogEntryAddedEventArgs : EventArgs
    {
        /// <summary>
        /// The entry that was added to the log.
        /// </summary>
        public LogEntry Entry {get; private set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="entry_">The entry that was added to the log</param>
        public LogEntryAddedEventArgs(LogEntry entry_)
        {
            Entry = entry_;
        }
    }

    /// <summary>
    /// Delegate for handling entry insertation into the log
    /// </summary>
    /// <param name="source_">Log where the new entry was inserted</param>
    /// <param name="args_">Event argument class that contains the newly inserted entry.</param>
    public delegate void LogEntryAddedEventHandler(AssertLog source_, LogEntryAddedEventArgs args_);

    /// <summary>
    /// Log class storing Error, Warning and/or Info entries.
    /// </summary>
    public class AssertLog
    {
        /// <summary>
        /// Collection storing the entries
        /// </summary>
        private List<LogEntry> _items = new List<LogEntry>();

        /// <summary>
        /// if true, log info is also logged
        /// false: only warnings and failures 
        /// </summary>
        private AssertLevel _logLevel = AssertLevel.Warn;
        /// <summary>
        /// Getter for all the asserts
        /// </summary>
        public LogEntry[] Items
        {
            get { return _items.ToArray(); }
        }

        /// <summary>
        /// Filtered asserts - Warnings
        /// </summary>
        public LogEntry[] Warnings
        {
            get
            {
                return Array.FindAll<LogEntry>(this.Items, delegate(LogEntry a) { return a.Level == AssertLevel.Warn; });
            }
        }

        /// <summary>
        /// Filtered asserts - Failures 
        /// </summary>
        public LogEntry[] Failures
        {
            get
            {
                return Array.FindAll<LogEntry>(this.Items, delegate(LogEntry a) { return a.Level == AssertLevel.Fail; });
            }
        }

        /// <summary>
        /// Event which is raised after a new entry is added.
        /// </summary>
        public event LogEntryAddedEventHandler EntryAdded;

        /// <summary>
        /// Constructor for the log
        /// </summary>
        /// <param name="logLevel_">Log level to use</param>
        public AssertLog(AssertLevel logLevel_)
        {
            _logLevel = logLevel_;
        }



        /// <summary>
        /// Method that raises the <see cref="EntryAdded"/> event.
        /// This is called after the new entry is inserted.
        /// </summary>
        /// <param name="entry_">The new log entry</param>
        protected void OnEntryAdded(LogEntry entry_)
        {
            if (this.EntryAdded != null && entry_.Level >= _logLevel)
            {
                EntryAdded(this, new LogEntryAddedEventArgs(entry_));
            }
        }

        /// <summary>
        /// General error message
        /// </summary>
        /// <param name="level_">Warning/Failure</param>
        /// <param name="message_">Assertation message</param>
        public void Add(AssertLevel level_, string message_)
        {
            LogEntry entry = new LogEntry(level_, message_);
            _items.Add(entry);

            OnEntryAdded(entry);
        }

          

    }

    /// <summary>
    /// Enumeration that specifies how serious the log entry is
    /// </summary>
    public enum AssertLevel
    {
        /// <summary>
        /// Simple info
        /// </summary>
        Info,

        /// <summary>
        /// Warning
        /// </summary>
        Warn,

        /// <summary>
        /// Critical problem
        /// </summary>
        Fail
    }

    /// <summary>
    /// Assertation entry
    /// </summary>
    public class LogEntry
    {
        /// <summary>
        /// Member field storing the seriousness of the entry
        /// </summary>
        private AssertLevel _level;

        /// <summary>
        /// Text message of the log entry
        /// </summary>
        private string _message;

        /// <summary>
        /// Retrieves the level of the seriousness
        /// </summary>
        public AssertLevel Level { get { return _level; } }

        /// <summary>
        /// The Text of the entry
        /// </summary>
        public string Message { get { return _message; } }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="level_">Seriousness of the entry</param>
        /// <param name="message_">Text of the log entry</param>
        internal LogEntry(AssertLevel level_, string message_)
        {
            _level = level_;
            _message = string.Format("{0} - {1}", _level, message_);
        }
    }
}
