

namespace EliteJournalReader.Events
{
    //When written: when a docking request is granted
    //Parameters:
    //�	StationName: name of station
    //�	LandingPad: pad number
    public class DockingGrantedEvent : JournalEvent<DockingGrantedEvent.DockingGrantedEventArgs>
    {
        public DockingGrantedEvent() : base("DockingGranted") { }

        public class DockingGrantedEventArgs : JournalEventArgs
        {
            public string StationName { get; set; }
            public string StationType { get; set; }
            public long MarketID { get; set; }
            public int LandingPad { get; set; }

            protected override string ToCompact() => $"Docking Permission Granted at {StationName} on pad {LandingPad}";
            protected override string ToSummary() => ToCompact();
        }
    }
}
