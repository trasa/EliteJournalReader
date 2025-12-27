namespace EliteJournalReader.Events
{
    //When written: Transferring funds to your carrier's bank
    //Parameters:
    //CarrierID
    //Deposit
    //PlayerBalance
    //CarrierBalance
    public class CarrierBankTransferEvent : JournalEvent<CarrierBankTransferEvent.CarrierBankTransferEventArgs>
    {
        public CarrierBankTransferEvent() : base("CarrierBankTransfer") { }

        public class CarrierBankTransferEventArgs : JournalEventArgs
        {
            public long CarrierID { get; set; }
            public long Deposit { get; set; }
            public long PlayerBalance { get; set; }
            public long CarrierBalance { get; set; }
        }
    }
}
