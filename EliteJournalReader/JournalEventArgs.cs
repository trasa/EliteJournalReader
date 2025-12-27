using Newtonsoft.Json.Linq;
using System;
using System.Globalization;

namespace EliteJournalReader
{
    public class JournalEventArgs : EventArgs
    {
        public JObject OriginalEvent { get; set; }

        public DateTime Timestamp { get; set; }

        public JournalEventArgs()
        {
        }

        internal static void Populate(JournalEventArgs eventArgs, JObject evt)
        {
            eventArgs.OriginalEvent = evt;
            eventArgs.Timestamp = DateTime.Parse(evt.Value<string>("timestamp"),
                CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal | DateTimeStyles.AdjustToUniversal);
        }

        public virtual void PostProcess(JObject evt, JournalWatcher journalWatcher) { }

        public virtual JournalEventArgs Clone() => (JournalEventArgs)MemberwiseClone();
    }
}
