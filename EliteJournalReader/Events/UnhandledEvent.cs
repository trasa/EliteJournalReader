using Newtonsoft.Json.Linq;

namespace EliteJournalReader.Events
{
    public class UnhandledEvent : JournalEvent<UnhandledEvent.UnhandledEventArgs>
    {
        public UnhandledEvent(string eventType) : base(eventType)
        {
        }

        protected override UnhandledEventArgs BuildEventArgs(string eventType, JObject evt)
        {
            var args = new UnhandledEventArgs(eventType, evt);
            JournalEventArgs.Populate(args, evt);
            return args;
        }

        public class UnhandledEventArgs : JournalEventArgs
        {
            public UnhandledEventArgs()
            {

            }

            public UnhandledEventArgs(string eventType, JObject evt)
            {
                EventType = eventType;
                Data = evt.ToString();
            }

            public string EventType { get; set; } = "";
            public string Data { get; set; } = "";
        }
    }
}
