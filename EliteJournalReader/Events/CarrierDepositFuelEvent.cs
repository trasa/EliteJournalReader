namespace EliteJournalReader.Events
{
    //When written: Depositing tritium fuel into your carrier
    //Parameters:
    //CarrierID: The ID of the carrier
    //Amount: The amount of fuel deposited
    //Total: The total amount of fuel in the carrier after the deposit
    public class CarrierDepositFuelEvent : JournalEvent<CarrierDepositFuelEvent.CarrierDepositFuelEventArgs>
    {
        public CarrierDepositFuelEvent() : base("CarrierDepositFuel") { }

        public class CarrierDepositFuelEventArgs : JournalEventArgs
        {
            public long CarrierID { get; set; }
            public long Amount { get; set; }
            public long Total { get; set; }
        }
    }
}
