namespace EliteJournalReader.Events
{
    //When written: Permission to dock at carrier
    //Parameters:
    //CarrierID: The ID of the carrier
    //DockingAccess:
    //AllowNotorious:
    public class CarrierDockingPermissionEvent : JournalEvent<CarrierDockingPermissionEvent.CarrierDockingPermissionEventArgs>
    {
        public CarrierDockingPermissionEvent() : base("CarrierDockingPermission") { }

        public class CarrierDockingPermissionEventArgs : JournalEventArgs
        {
            public long CarrierID { get; set; }
            public string DockingAccess { get; set; }
            public bool AllowNotorious { get; set; }
        }
    }
}
