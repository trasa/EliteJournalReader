using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;

namespace EliteJournalReader
{
    public class JournalEventArgs : EventArgs, IFormattable
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

        public string ShortEventArgsName => GetType().Name.Replace("EventArgs", "");

        protected virtual string ToCompact() => $"{ShortEventArgsName}({EventName})";

        protected virtual string ToSummary() => ToFull();

        /// <summary>
        /// Don't include these properties in the Full string format
        /// </summary>
        /// <returns></returns>
        protected virtual IEnumerable<string> PropertiesToSkip() => new[]
        {
            nameof(OriginalEvent),
            nameof(Timestamp),
            nameof(EventName),
            nameof(ShortEventArgsName)
        };

        protected virtual string ToFull()
        {
            var properties = GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Where(p => !PropertiesToSkip().Contains(p.Name))
                .OrderBy(p => p.Name);

            // exclude nulls
            var parts = properties
                .Select(p => (Name: p.Name, Value: p.GetValue(this)))
                .Where(x => x.Value != null)
                .Select(x => $"{x.Name}: {FormatValue(x.Value)}");

            return string.Join(", ", parts);
        }

        protected virtual string FormatValue(object value) =>
            value switch
            {
                null => "null",
                double d => $"{d:F2}",
                float f => $"{f:F2}",
                DateTime dt => dt.ToString("o"),
                _ => value.ToString()
            };


        /// <summary>Formats the value of the current instance using the specified format.</summary>
        /// <param name="format">
        /// "G" or "SUMMARY" - General summary format (default)<br/>
        /// "F" or "FULL" - Full format including all ranks<br/>
        /// "C" or "COMPACT" - Compact format showing only Combat, Trade, and Explore ranks<br/>
        /// "J" or "JSON" - JSON format
        /// </param>
        /// <param name="formatProvider" />
        /// <returns>The value of the current instance in the specified format.</returns>
        public virtual string ToString(string format, IFormatProvider formatProvider = null)
        {
            format = format?.ToUpperInvariant() ?? "G";

            return format switch
            {
                "G" or "SUMMARY" => ToSummary(),
                "F" or "FULL" => ToFull(),
                "C" or "COMPACT" => ToCompact(),
                "J" or "JSON" => ToJson(),
                _ => throw new FormatException($"Format '{format}' is not supported")
            };
        }
    }
}
