using System;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;

namespace EliteJournalReader
{
    public abstract class JournalEvent
    {
        public string[] EventNames { get; }

        public string OriginalEvent { get; protected set; }

        protected JournalEvent(params string[] eventNames)
        {
            EventNames = eventNames;
        }

        internal abstract JournalEventArgs ParseEventArgs(JObject evt);

        internal abstract JournalEventArgs FireEvent(JournalWatcher journalWatcher, JObject evt, string eventType = null);
    }

    public abstract class JournalEvent<TJournalEventArgs> : JournalEvent
        where TJournalEventArgs : JournalEventArgs, new()
    {
        public event EventHandler<TJournalEventArgs> Fired;

        protected JournalEvent(params string[] eventNames) : base(eventNames)
        {
        }

        public void AddHandler(EventHandler<TJournalEventArgs> eventHandler) => Fired += eventHandler;

        public void RemoveHandler(EventHandler<TJournalEventArgs> eventHandler) => Fired -= eventHandler;

        internal override JournalEventArgs ParseEventArgs(JObject evt) => evt.ToObject<TJournalEventArgs>();

        internal override JournalEventArgs FireEvent(JournalWatcher journalWatcher, JObject evt, string eventType = null)
        {
            var eventArgs = BuildEventArgs(eventType, evt);

            eventArgs.PostProcess(evt, journalWatcher);

            Fired?.Invoke(journalWatcher, eventArgs);

            return eventArgs;
        }

        protected virtual TJournalEventArgs BuildEventArgs(string eventType, JObject evt)
        {
            var eventArgs = evt.ToObject<TJournalEventArgs>();
            JournalEventArgs.Populate(eventArgs, evt);

#if DEBUG
            Type argsType = typeof(TJournalEventArgs);
            var eventName = evt["event"];

            var argsPropertyNames = argsType.GetProperties().Select(p => p.Name).ToList();
            string[] ignoreProperties = new string[] { "event" };
            foreach (var jProperty in evt.Properties())
            {
                string jsonPropertyName = jProperty.Name;
                if (ignoreProperties.Contains(jsonPropertyName))
                {
                    // ignore anything in the ignore list
                }
                else if (jsonPropertyName.EndsWith("_Localised", StringComparison.CurrentCultureIgnoreCase))
                {
                    // ignore localised
                }
                else if (!argsPropertyNames.Any(x => string.Compare(jsonPropertyName, x, StringComparison.InvariantCultureIgnoreCase) == 0))
                {
                    // found something missing
                    Trace.TraceInformation($"EventArgs for {eventName} does not contain property {jsonPropertyName}");
                    //Debugger.Break();
                }
            }
#endif
            return eventArgs;
        }
    }
}
