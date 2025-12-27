using Newtonsoft.Json;
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

        public virtual string ToJson(Formatting formatting = Formatting.Indented) => OriginalEvent?.ToString(formatting) ?? "{}";

        public virtual string EventName => OriginalEvent?["event"]?.Value<string>() ?? "";

        public virtual void PostProcess(JObject evt, JournalWatcher journalWatcher) { }

        public virtual JournalEventArgs Clone() => (JournalEventArgs)MemberwiseClone();
    }
}
