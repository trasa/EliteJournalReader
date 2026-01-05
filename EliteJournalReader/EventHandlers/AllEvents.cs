#nullable enable
using System.Reflection;

namespace EliteJournalReader.EventHandlers;

public class AllEvents
{
    private static object? CreateTypedHandler<TJournalEventArgs>(EventHandler<TJournalEventArgs> handler, Type eventArgType) where TJournalEventArgs : EventArgs, new()
    {
        var handlerType = typeof(EventHandler<>).MakeGenericType(eventArgType);
        var delegateMethod = handler.GetType().GetMethod(nameof(handler.Invoke));
        if (delegateMethod != null)
        {
            return Delegate.CreateDelegate(handlerType, handler.Target, delegateMethod);
        }
        return null;
    }

    public static void HandleAllAsync<TJournalEventArgs>(JournalWatcher watcher, EventHandler<TJournalEventArgs> handler) where TJournalEventArgs : JournalEventArgs, new()
    {
        var eventArgTypes = Assembly.GetExecutingAssembly().GetTypes().Where(t => t.IsSubclassOf(typeof(JournalEventArgs)) && !t.IsAbstract);
        foreach (var eventArgType in eventArgTypes)
        {
            var getEventMethod = typeof(JournalWatcher).GetMethod(nameof(JournalWatcher.GetEvent))?.MakeGenericMethod(eventArgType);
            if (getEventMethod != null)
            {
                object? journalEvent = getEventMethod.Invoke(watcher, null);
                if (journalEvent != null)
                {
                    var addHandlerMethod = journalEvent.GetType().GetMethod(nameof(JournalEvent<JournalEventArgs>.AddHandler));
                    if (addHandlerMethod != null)
                    {
                        object? typedHandler = CreateTypedHandler(handler, eventArgType);
                        if (typedHandler != null)
                        {
                            addHandlerMethod.Invoke(journalEvent, new[] { typedHandler });
                        }
                    }
                }
            }
        }
/*
        watcher.GetEvent<AfmuRepairsEvent>()?.AddHandler((_, e) => handler(e));

        watcher.GetEvent<AppliedToSquadronEvent>()?.AddHandler((_, e) => PrintSimpleEvent(e));
        watcher.GetEvent<ApproachBodyEvent>()?.AddHandler((_, e) => PrintSimpleEvent(e));
        watcher.GetEvent<ApproachSettlementEvent>()?.AddHandler((_, e) => PrintSimpleEvent(e));
        watcher.GetEvent<AsteroidCrackedEvent>()?.AddHandler((_, e) => PrintSimpleEvent(e));
        watcher.GetEvent<BackpackEvent>()?.AddHandler((_, e) => PrintSimpleEvent(e));
        watcher.GetEvent<BackpackMaterialsEvent>()?.AddHandler((_, e) => PrintSimpleEvent(e));
        watcher.GetEvent<BookDropshipEvent>()?.AddHandler((_, e) => PrintSimpleEvent(e));
        watcher.GetEvent<BookTaxiEvent>()?.AddHandler((_, e) => PrintSimpleEvent(e));
        watcher.GetEvent<BountyEvent>()?.AddHandler((_, e) => PrintSimpleEvent(e));
        watcher.GetEvent<BuyAmmoEvent>()?.AddHandler((_, e) => PrintSimpleEvent(e));
        watcher.GetEvent<BuyDronesEvent>()?.AddHandler((_, e) => PrintSimpleEvent(e));
        watcher.GetEvent<BuyExplorationDataEvent>()?.AddHandler((_, e) => PrintSimpleEvent(e));
        watcher.GetEvent<BuyMicroResourcesEvent>()?.AddHandler((_, e) => PrintSimpleEvent(e));
        watcher.GetEvent<BuySuitEvent>()?.AddHandler((_, e) => PrintSimpleEvent(e));
        watcher.GetEvent<BuyTradeDataEvent>()?.AddHandler((_, e) => PrintSimpleEvent(e));
        watcher.GetEvent<BuyWeaponEvent>()?.AddHandler((_, e) => PrintSimpleEvent(e));
        watcher.GetEvent<CancelDropshipEvent>()?.AddHandler((_, e) => PrintSimpleEvent(e));
        watcher.GetEvent<CancelTaxiEvent>()?.AddHandler((_, e) => PrintSimpleEvent(e));
        watcher.GetEvent<CapShipBondEvent>()?.AddHandler((_, e) => PrintSimpleEvent(e));
        watcher.GetEvent<CargoDepotEvent>()?.AddHandler((_, e) => PrintSimpleEvent(e));
        watcher.GetEvent<CargoEvent>()?.AddHandler((_, e) => PrintSimpleEvent(e));
        watcher.GetEvent<CargoTransferEvent>()?.AddHandler((_, e) => PrintSimpleEvent(e));
        watcher.GetEvent<CarrierBankTransferEvent>()?.AddHandler((_, e) => PrintSimpleEvent(e));
        watcher.GetEvent<CarrierBuyEvent>()?.AddHandler((_, e) => PrintSimpleEvent(e));
        watcher.GetEvent<CarrierCancelDecommissionEvent>()?.AddHandler((_, e) => PrintSimpleEvent(e));
        watcher.GetEvent<CarrierCrewServicesEvent>()?.AddHandler((_, e) => PrintSimpleEvent(e));
        watcher.GetEvent<CarrierDecommissionEvent>()?.AddHandler((_, e) => PrintSimpleEvent(e));
        watcher.GetEvent<CarrierDepositFuelEvent>()?.AddHandler((_, e) => PrintSimpleEvent(e));
        watcher.GetEvent<CarrierDockingPermissionEvent>()?.AddHandler((_, e) => PrintSimpleEvent(e));
        watcher.GetEvent<CarrierFinanceEvent>()?.AddHandler((_, e) => PrintSimpleEvent(e));
        watcher.GetEvent<CarrierJumpCancelledEvent>()?.AddHandler((_, e) => PrintSimpleEvent(e));
        watcher.GetEvent<CarrierJumpEvent>()?.AddHandler((_, e) => PrintSimpleEvent(e));
        watcher.GetEvent<CarrierJumpRequestEvent>()?.AddHandler((_, e) => PrintSimpleEvent(e));
        watcher.GetEvent<CarrierLocationEvent>()?.AddHandler((_, e) => PrintSimpleEvent(e));
        watcher.GetEvent<CarrierModulePackEvent>()?.AddHandler((_, e) => PrintSimpleEvent(e));
        watcher.GetEvent<CarrierNameChangedEvent>()?.AddHandler((_, e) => PrintSimpleEvent(e));
        watcher.GetEvent<CarrierShipPackEvent>()?.AddHandler((_, e) => PrintSimpleEvent(e));
        watcher.GetEvent<CarrierTradeOrderEvent>()?.AddHandler((_, e) => PrintSimpleEvent(e));
        watcher.GetEvent<ChangeCrewRoleEvent>()?.AddHandler((_, e) => PrintSimpleEvent(e));
        watcher.GetEvent<ClearSavedGameEvent>()?.AddHandler((_, e) => PrintSimpleEvent(e));
        watcher.GetEvent<CockpitBreachedEvent>()?.AddHandler((_, e) => PrintSimpleEvent(e));
        watcher.GetEvent<CodexEntryEvent>()?.AddHandler((_, e) => PrintSimpleEvent(e));
        watcher.GetEvent<CollectCargoEvent>()?.AddHandler((_, e) => PrintSimpleEvent(e));
        watcher.GetEvent<CollectItemsEvent>()?.AddHandler((_, e) => PrintSimpleEvent(e));
        watcher.GetEvent<ColonisationConstructionDepotEvent>()?.AddHandler((_, e) => PrintSimpleEvent(e));
        watcher.GetEvent<ColonisationContributionEvent>()?.AddHandler((_, e) => PrintSimpleEvent(e));
        watcher.GetEvent<CommanderEvent>()?.AddHandler((_, e) => PrintSimpleEvent(e));
        watcher.GetEvent<CommitCrimeEvent>()?.AddHandler((_, e) => PrintSimpleEvent(e));
        watcher.GetEvent<CommunityGoalDiscardEvent>()?.AddHandler((_, e) => PrintSimpleEvent(e));
        watcher.GetEvent<CommunityGoalEvent>()?.AddHandler((_, e) => PrintSimpleEvent(e));
        watcher.GetEvent<CommunityGoalJoinEvent>()?.AddHandler((_, e) => PrintSimpleEvent(e));
        watcher.GetEvent<CommunityGoalRewardEvent>()?.AddHandler((_, e) => PrintSimpleEvent(e));
        watcher.GetEvent<ContinuedEvent>()?.AddHandler((_, e) => PrintSimpleEvent(e));
        watcher.GetEvent<CreateSuitLoadoutEvent>()?.AddHandler((_, e) => PrintSimpleEvent(e));
        watcher.GetEvent<CrewAssignEvent>()?.AddHandler((_, e) => PrintSimpleEvent(e));
        watcher.GetEvent<CrewFireEvent>()?.AddHandler((_, e) => PrintSimpleEvent(e));
        watcher.GetEvent<CrewHireEvent>()?.AddHandler((_, e) => PrintSimpleEvent(e));
        watcher.GetEvent<CrewLaunchFighterEvent>()?.AddHandler((_, e) => PrintSimpleEvent(e));
        watcher.GetEvent<CrewMemberJoinsEvent>()?.AddHandler((_, e) => PrintSimpleEvent(e));
        watcher.GetEvent<CrewMemberQuitsEvent>()?.AddHandler((_, e) => PrintSimpleEvent(e));
        watcher.GetEvent<CrewMemberRoleChangeEvent>()?.AddHandler((_, e) => PrintSimpleEvent(e));
        watcher.GetEvent<CrimeVictimEvent>()?.AddHandler((_, e) => PrintSimpleEvent(e));
        watcher.GetEvent<DatalinkScanEvent>()?.AddHandler((_, e) => PrintSimpleEvent(e));
        watcher.GetEvent<DatalinkVoucherEvent>()?.AddHandler((_, e) => PrintSimpleEvent(e));
        watcher.GetEvent<DataScannedEvent>()?.AddHandler((_, e) => PrintSimpleEvent(e));
        watcher.GetEvent<DeleteSuitLoadoutEvent>()?.AddHandler((_, e) => PrintSimpleEvent(e));
        watcher.GetEvent<DiedEvent>()?.AddHandler((_, e) => PrintSimpleEvent(e));
        watcher.GetEvent<DisbandedSquadronEvent>()?.AddHandler((_, e) => PrintSimpleEvent(e));
        watcher.GetEvent<DiscoveryScanEvent>()?.AddHandler((_, e) => PrintSimpleEvent(e));
        watcher.GetEvent<DisembarkEvent>()?.AddHandler((_, e) => PrintSimpleEvent(e));
        watcher.GetEvent<DockedEvent>()?.AddHandler((_, e) => PrintSimpleEvent(e));
        watcher.GetEvent<DockFighterEvent>()?.AddHandler((_, e) => PrintSimpleEvent(e));
        watcher.GetEvent<DockingCancelledEvent>()?.AddHandler((_, e) => PrintSimpleEvent(e));
        watcher.GetEvent<DockingDeniedEvent>()?.AddHandler((_, e) => PrintSimpleEvent(e));
        watcher.GetEvent<DockingGrantedEvent>()?.AddHandler((_, e) => PrintSimpleEvent(e));
        watcher.GetEvent<DockingRequestedEvent>()?.AddHandler((_, e) => PrintSimpleEvent(e));
        watcher.GetEvent<DockingTimeoutEvent>()?.AddHandler((_, e) => PrintSimpleEvent(e));
        watcher.GetEvent<DockSRVEvent>()?.AddHandler((_, e) => PrintSimpleEvent(e));
        watcher.GetEvent<DropItemsEvent>()?.AddHandler((_, e) => PrintSimpleEvent(e));
        watcher.GetEvent<DropShipDeployEvent>()?.AddHandler((_, e) => PrintSimpleEvent(e));
        watcher.GetEvent<EjectCargoEvent>()?.AddHandler((_, e) => PrintSimpleEvent(e));
        watcher.GetEvent<EmbarkEvent>()?.AddHandler((_, e) => PrintSimpleEvent(e));
        watcher.GetEvent<EndCrewSessionEvent>()?.AddHandler((_, e) => PrintSimpleEvent(e));
        watcher.GetEvent<EngineerContributionEvent>()?.AddHandler((_, e) => PrintSimpleEvent(e));
        watcher.GetEvent<EngineerCraftEvent>()?.AddHandler((_, e) => PrintSimpleEvent(e));
        watcher.GetEvent<EngineerLegacyConvertEvent>()?.AddHandler((_, e) => PrintSimpleEvent(e));
        watcher.GetEvent<EngineerProgressEvent>()?.AddHandler((_, e) => PrintSimpleEvent(e));
        watcher.GetEvent<EscapeInterdictionEvent>()?.AddHandler((_, e) => PrintSimpleEvent(e));
        watcher.GetEvent<FactionKillBondEvent>()?.AddHandler((_, e) => PrintSimpleEvent(e));
        watcher.GetEvent<FetchRemoteModuleEvent>()?.AddHandler((_, e) => PrintSimpleEvent(e));
        watcher.GetEvent<FighterDestroyedEvent>()?.AddHandler((_, e) => PrintSimpleEvent(e));
        watcher.GetEvent<FighterRebuiltEvent>()?.AddHandler((_, e) => PrintSimpleEvent(e));
        watcher.GetEvent<FileheaderEvent>()?.AddHandler((_, e) => PrintSimpleEvent(e));
        watcher.GetEvent<FriendsEvent>()?.AddHandler((_, e) => PrintSimpleEvent(e));
        watcher.GetEvent<FSDJumpEvent>()?.AddHandler((_, e) => PrintSimpleEvent(e));
        watcher.GetEvent<FSDTargetEvent>()?.AddHandler((_, e) => PrintSimpleEvent(e));
        watcher.GetEvent<FSSAllBodiesFoundEvent>()?.AddHandler((_, e) => PrintSimpleEvent(e));
        watcher.GetEvent<FSSBodySignalsEvent>()?.AddHandler((_, e) => PrintSimpleEvent(e));
        watcher.GetEvent<FSSDiscoveryScanEvent>()?.AddHandler((_, e) => PrintSimpleEvent(e));
        watcher.GetEvent<FSSSignalDiscoveredEvent>()?.AddHandler((_, e) => PrintSimpleEvent(e));
        watcher.GetEvent<FuelScoopEvent>()?.AddHandler((_, e) => PrintSimpleEvent(e));
        watcher.GetEvent<HeatDamageEvent>()?.AddHandler((_, e) => PrintSimpleEvent(e));
        watcher.GetEvent<HeatWarningEvent>()?.AddHandler((_, e) => PrintSimpleEvent(e));
        watcher.GetEvent<HullDamageEvent>()?.AddHandler((_, e) => PrintSimpleEvent(e));
        watcher.GetEvent<InterdictedEvent>()?.AddHandler((_, e) => PrintSimpleEvent(e));
        watcher.GetEvent<InterdictionEvent>()?.AddHandler((_, e) => PrintSimpleEvent(e));
        watcher.GetEvent<InvitedToSquadronEvent>()?.AddHandler((_, e) => PrintSimpleEvent(e));
        watcher.GetEvent<IsLiveEvent>()?.AddHandler((_, e) => PrintSimpleEvent(e));
        watcher.GetEvent<JetConeBoostEvent>()?.AddHandler((_, e) => PrintSimpleEvent(e));
        watcher.GetEvent<JetConeDamageEvent>()?.AddHandler((_, e) => PrintSimpleEvent(e));
        watcher.GetEvent<JoinACrewEvent>()?.AddHandler((_, e) => PrintSimpleEvent(e));
        watcher.GetEvent<JoinedSquadronEvent>()?.AddHandler((_, e) => PrintSimpleEvent(e));
        watcher.GetEvent<KickCrewMemberEvent>()?.AddHandler((_, e) => PrintSimpleEvent(e));
        watcher.GetEvent<KickedFromSquadronEvent>()?.AddHandler((_, e) => PrintSimpleEvent(e));
        watcher.GetEvent<LaunchDroneEvent>()?.AddHandler((_, e) => PrintSimpleEvent(e));
        watcher.GetEvent<LaunchFighterEvent>()?.AddHandler((_, e) => PrintSimpleEvent(e));
        watcher.GetEvent<LaunchSRVEvent>()?.AddHandler((_, e) => PrintSimpleEvent(e));
        watcher.GetEvent<LeaveBodyEvent>()?.AddHandler((_, e) => PrintSimpleEvent(e));
        watcher.GetEvent<LeftSquadronEvent>()?.AddHandler((_, e) => PrintSimpleEvent(e));
        watcher.GetEvent<LiftoffEvent>()?.AddHandler((_, e) => PrintSimpleEvent(e));
        watcher.GetEvent<LoadGameEvent>()?.AddHandler((_, e) => PrintSimpleEvent(e));
        watcher.GetEvent<LoadoutEquipModuleEvent>()?.AddHandler((_, e) => PrintSimpleEvent(e));
        watcher.GetEvent<LoadoutEvent>()?.AddHandler((_, e) => PrintSimpleEvent(e));
        watcher.GetEvent<LoadoutRemoveModuleEvent>()?.AddHandler((_, e) => PrintSimpleEvent(e));
        watcher.GetEvent<LocationEvent>()?.AddHandler((_, e) => PrintSimpleEvent(e));
        watcher.GetEvent<MarketBuyEvent>()?.AddHandler((_, e) => PrintSimpleEvent(e));
        watcher.GetEvent<MarketRefinedEvent>()?.AddHandler((_, e) => PrintSimpleEvent(e));
        watcher.GetEvent<MarketSellEvent>()?.AddHandler((_, e) => PrintSimpleEvent(e));
        watcher.GetEvent<MassModuleStoreEvent>()?.AddHandler((_, e) => PrintSimpleEvent(e));
        watcher.GetEvent<MaterialCollectedEvent>()?.AddHandler((_, e) => PrintSimpleEvent(e));
        watcher.GetEvent<MaterialDiscardedEvent>()?.AddHandler((_, e) => PrintSimpleEvent(e));
        watcher.GetEvent<MaterialDiscoveredEvent>()?.AddHandler((_, e) => PrintSimpleEvent(e));
        watcher.GetEvent<MaterialsEvent>()?.AddHandler((_, e) => PrintSimpleEvent(e));
        watcher.GetEvent<MaterialTradeEvent>()?.AddHandler((_, e) => PrintSimpleEvent(e));
        watcher.GetEvent<MiningRefinedEvent>()?.AddHandler((_, e) => PrintSimpleEvent(e));
        watcher.GetEvent<MissionAbandonedEvent>()?.AddHandler((_, e) => PrintSimpleEvent(e));
        watcher.GetEvent<MissionAcceptedEvent>()?.AddHandler((_, e) => PrintSimpleEvent(e));
        watcher.GetEvent<MissionCompletedEvent>()?.AddHandler((_, e) => PrintSimpleEvent(e));
        watcher.GetEvent<MissionFailedEvent>()?.AddHandler((_, e) => PrintSimpleEvent(e));
        watcher.GetEvent<MissionRedirectedEvent>()?.AddHandler((_, e) => PrintSimpleEvent(e));
        watcher.GetEvent<MissionsEvent>()?.AddHandler((_, e) => PrintSimpleEvent(e));
        watcher.GetEvent<ModuleBuyEvent>()?.AddHandler((_, e) => PrintSimpleEvent(e));
        watcher.GetEvent<ModuleInfoEvent>()?.AddHandler((_, e) => PrintSimpleEvent(e));
        watcher.GetEvent<ModuleRetrieveEvent>()?.AddHandler((_, e) => PrintSimpleEvent(e));
        watcher.GetEvent<ModuleSellEvent>()?.AddHandler((_, e) => PrintSimpleEvent(e));
        watcher.GetEvent<ModuleSellRemoteEvent>()?.AddHandler((_, e) => PrintSimpleEvent(e));
        watcher.GetEvent<ModulesInfoEvent>()?.AddHandler((_, e) => PrintSimpleEvent(e));
        watcher.GetEvent<ModuleStoreEvent>()?.AddHandler((_, e) => PrintSimpleEvent(e));
        watcher.GetEvent<ModuleSwapEvent>()?.AddHandler((_, e) => PrintSimpleEvent(e));
        watcher.GetEvent<MultiSellExplorationDataEvent>()?.AddHandler((_, e) => PrintSimpleEvent(e));
        watcher.GetEvent<MusicEvent>()?.AddHandler((_, e) => PrintSimpleEvent(e));
        watcher.GetEvent<NavBeaconScanEvent>()?.AddHandler((_, e) => PrintSimpleEvent(e));
        watcher.GetEvent<NavRouteClearEvent>()?.AddHandler((_, e) => PrintSimpleEvent(e));
        watcher.GetEvent<NavRouteEvent>()?.AddHandler((_, e) => PrintSimpleEvent(e));
        watcher.GetEvent<NewCommanderEvent>()?.AddHandler((_, e) => PrintSimpleEvent(e));
        watcher.GetEvent<NpcCrewPaidWageEvent>()?.AddHandler((_, e) => PrintSimpleEvent(e));
        watcher.GetEvent<NpcCrewRankEvent>()?.AddHandler((_, e) => PrintSimpleEvent(e));
        watcher.GetEvent<OutfittingEvent>()?.AddHandler((_, e) => PrintSimpleEvent(e));
        watcher.GetEvent<PassengersEvent>()?.AddHandler((_, e) => PrintSimpleEvent(e));
        watcher.GetEvent<PayBountiesEvent>()?.AddHandler((_, e) => PrintSimpleEvent(e));
        watcher.GetEvent<PayFinesEvent>()?.AddHandler((_, e) => PrintSimpleEvent(e));
        watcher.GetEvent<PayLegacyFinesEvent>()?.AddHandler((_, e) => PrintSimpleEvent(e));
        watcher.GetEvent<PowerplayCollectEvent>()?.AddHandler((_, e) => PrintSimpleEvent(e));
        watcher.GetEvent<PowerplayDefectEvent>()?.AddHandler((_, e) => PrintSimpleEvent(e));
        watcher.GetEvent<PowerplayDeliverEvent>()?.AddHandler((_, e) => PrintSimpleEvent(e));
        watcher.GetEvent<PowerplayEvent>()?.AddHandler((_, e) => PrintSimpleEvent(e));
        watcher.GetEvent<PowerplayFastTrackEvent>()?.AddHandler((_, e) => PrintSimpleEvent(e));
        watcher.GetEvent<PowerplayJoinEvent>()?.AddHandler((_, e) => PrintSimpleEvent(e));
        watcher.GetEvent<PowerplayLeaveEvent>()?.AddHandler((_, e) => PrintSimpleEvent(e));
        watcher.GetEvent<PowerplaySalaryEvent>()?.AddHandler((_, e) => PrintSimpleEvent(e));
        watcher.GetEvent<PowerplayVoteEvent>()?.AddHandler((_, e) => PrintSimpleEvent(e));
        watcher.GetEvent<PowerplayVoucherEvent>()?.AddHandler((_, e) => PrintSimpleEvent(e));
        watcher.GetEvent<ProgressEvent>()?.AddHandler((_, e) => PrintSimpleEvent(e));
        watcher.GetEvent<PromotionEvent>()?.AddHandler((_, e) => PrintSimpleEvent(e));
        watcher.GetEvent<ProspectedAsteroidEvent>()?.AddHandler((_, e) => PrintSimpleEvent(e));
        watcher.GetEvent<PVPKillEvent>()?.AddHandler((_, e) => PrintSimpleEvent(e));
        watcher.GetEvent<QuitACrewEvent>()?.AddHandler((_, e) => PrintSimpleEvent(e));
        watcher.GetEvent<RankEvent>()?.AddHandler((_, e) => PrintSimpleEvent(e));
        watcher.GetEvent<RebootRepairEvent>()?.AddHandler((_, e) => PrintSimpleEvent(e));
        watcher.GetEvent<ReceiveTextEvent>()?.AddHandler((_, e) => PrintSimpleEvent(e));
        watcher.GetEvent<RedeemVoucherEvent>()?.AddHandler((_, e) => PrintSimpleEvent(e));
        watcher.GetEvent<RefuelAllEvent>()?.AddHandler((_, e) => PrintSimpleEvent(e));
        watcher.GetEvent<RefuelPartialEvent>()?.AddHandler((_, e) => PrintSimpleEvent(e));
        watcher.GetEvent<RenameSuitLoadoutEvent>()?.AddHandler((_, e) => PrintSimpleEvent(e));
        watcher.GetEvent<RepairAllEvent>()?.AddHandler((_, e) => PrintSimpleEvent(e));
        watcher.GetEvent<RepairDroneEvent>()?.AddHandler((_, e) => PrintSimpleEvent(e));
        watcher.GetEvent<RepairEvent>()?.AddHandler((_, e) => PrintSimpleEvent(e));
        watcher.GetEvent<ReputationEvent>()?.AddHandler((_, e) => PrintSimpleEvent(e));
        watcher.GetEvent<ReservoirReplenishedEvent>()?.AddHandler((_, e) => PrintSimpleEvent(e));
        watcher.GetEvent<RestockVehicleEvent>()?.AddHandler((_, e) => PrintSimpleEvent(e));
        watcher.GetEvent<ResurrectEvent>()?.AddHandler((_, e) => PrintSimpleEvent(e));
        watcher.GetEvent<SAAScanCompleteEvent>()?.AddHandler((_, e) => PrintSimpleEvent(e));
        watcher.GetEvent<SAASignalsFoundEvent>()?.AddHandler((_, e) => PrintSimpleEvent(e));
        watcher.GetEvent<ScanBaryCentreEvent>()?.AddHandler((_, e) => PrintSimpleEvent(e));
        watcher.GetEvent<ScanEvent>()?.AddHandler((_, e) => PrintSimpleEvent(e, compact: true));
        watcher.GetEvent<ScannedEvent>()?.AddHandler((_, e) => PrintSimpleEvent(e));
        watcher.GetEvent<ScanOrganicEvent>()?.AddHandler((_, e) => PrintSimpleEvent(e));
        watcher.GetEvent<ScientificResearchEvent>()?.AddHandler((_, e) => PrintSimpleEvent(e));
        watcher.GetEvent<ScreenshotEvent>()?.AddHandler((_, e) => PrintSimpleEvent(e));
        watcher.GetEvent<SearchAndRescueEvent>()?.AddHandler((_, e) => PrintSimpleEvent(e));
        watcher.GetEvent<SelfDestructEvent>()?.AddHandler((_, e) => PrintSimpleEvent(e));
        watcher.GetEvent<SellDronesEvent>()?.AddHandler((_, e) => PrintSimpleEvent(e));
        watcher.GetEvent<SellExplorationDataEvent>()?.AddHandler((_, e) => PrintSimpleEvent(e));
        watcher.GetEvent<SellMicroResourcesEvent>()?.AddHandler((_, e) => PrintSimpleEvent(e));
        watcher.GetEvent<SellOrganicDataEvent>()?.AddHandler((_, e) => PrintSimpleEvent(e));
        watcher.GetEvent<SellShipOnRebuyEvent>()?.AddHandler((_, e) => PrintSimpleEvent(e));
        watcher.GetEvent<SellSuitEvent>()?.AddHandler((_, e) => PrintSimpleEvent(e));
        watcher.GetEvent<SellWeaponEvent>()?.AddHandler((_, e) => PrintSimpleEvent(e));
        watcher.GetEvent<SendTextEvent>()?.AddHandler((_, e) => PrintSimpleEvent(e));
        watcher.GetEvent<SetUserShipNameEvent>()?.AddHandler((_, e) => PrintSimpleEvent(e));
        watcher.GetEvent<SharedBookmarkToSquadronEvent>()?.AddHandler((_, e) => PrintSimpleEvent(e));
        watcher.GetEvent<ShieldStateEvent>()?.AddHandler((_, e) => PrintSimpleEvent(e));
        watcher.GetEvent<ShipLockerEvent>()?.AddHandler((_, e) => PrintSimpleEvent(e));
        watcher.GetEvent<ShipLockerMaterialsEvent>()?.AddHandler((_, e) => PrintSimpleEvent(e));
        watcher.GetEvent<ShipTargetedEvent>()?.AddHandler((_, e) => PrintSimpleEvent(e));
        watcher.GetEvent<ShipyardBuyEvent>()?.AddHandler((_, e) => PrintSimpleEvent(e));
        watcher.GetEvent<ShipyardEvent>()?.AddHandler((_, e) => PrintSimpleEvent(e));
        watcher.GetEvent<ShipyardNewEvent>()?.AddHandler((_, e) => PrintSimpleEvent(e));
        watcher.GetEvent<ShipyardSellEvent>()?.AddHandler((_, e) => PrintSimpleEvent(e));
        watcher.GetEvent<ShipyardSwapEvent>()?.AddHandler((_, e) => PrintSimpleEvent(e));
        watcher.GetEvent<ShipyardTransferEvent>()?.AddHandler((_, e) => PrintSimpleEvent(e));
        watcher.GetEvent<ShutdownEvent>()?.AddHandler((_, e) => PrintSimpleEvent(e));
        watcher.GetEvent<SquadronCreatedEvent>()?.AddHandler((_, e) => PrintSimpleEvent(e));
        watcher.GetEvent<SquadronDemotionEvent>()?.AddHandler((_, e) => PrintSimpleEvent(e));
        watcher.GetEvent<SquadronPromotionEvent>()?.AddHandler((_, e) => PrintSimpleEvent(e));
        watcher.GetEvent<SquadronStartupEvent>()?.AddHandler((_, e) => PrintSimpleEvent(e));
        watcher.GetEvent<SRVDestroyedEvent>()?.AddHandler((_, e) => PrintSimpleEvent(e));
        watcher.GetEvent<StartJumpEvent>()?.AddHandler((_, e) => PrintSimpleEvent(e));
        watcher.GetEvent<StatisticsEvent>()?.AddHandler((_, e) => PrintSimpleEvent(e));
        watcher.GetEvent<StatusEvent>()?.AddHandler((_, e) => PrintSimpleEvent(e));
        watcher.GetEvent<StoredModulesEvent>()?.AddHandler((_, e) => PrintSimpleEvent(e));
        watcher.GetEvent<StoredShipsEvent>()?.AddHandler((_, e) => PrintSimpleEvent(e));
        watcher.GetEvent<SuitLoadoutEvent>()?.AddHandler((_, e) => PrintSimpleEvent(e));
        watcher.GetEvent<SupercruiseDestinationDropEvent>()?.AddHandler((_, e) => PrintSimpleEvent(e));
        watcher.GetEvent<SupercruiseEntryEvent>()?.AddHandler((_, e) => PrintSimpleEvent(e));
        watcher.GetEvent<SupercruiseExitEvent>()?.AddHandler((_, e) => PrintSimpleEvent(e));
        watcher.GetEvent<SwitchSuitLoadoutEvent>()?.AddHandler((_, e) => PrintSimpleEvent(e));
        watcher.GetEvent<SynthesisEvent>()?.AddHandler((_, e) => PrintSimpleEvent(e));
        watcher.GetEvent<SystemsShutdownEvent>()?.AddHandler((_, e) => PrintSimpleEvent(e));
        watcher.GetEvent<TechnologyBrokerEvent>()?.AddHandler((_, e) => PrintSimpleEvent(e));
        watcher.GetEvent<TouchdownEvent>()?.AddHandler((_, e) => PrintSimpleEvent(e));
        watcher.GetEvent<TradeMicroResourcesEvent>()?.AddHandler((_, e) => PrintSimpleEvent(e));
        watcher.GetEvent<TransferMicroResourcesEvent>()?.AddHandler((_, e) => PrintSimpleEvent(e));
        watcher.GetEvent<UnderAttackEvent>()?.AddHandler((_, e) => PrintSimpleEvent(e));
        watcher.GetEvent<UndockedEvent>()?.AddHandler((_, e) => PrintSimpleEvent(e));
        watcher.GetEvent<UpgradeSuitEvent>()?.AddHandler((_, e) => PrintSimpleEvent(e));
        watcher.GetEvent<UpgradeWeaponEvent>()?.AddHandler((_, e) => PrintSimpleEvent(e));
        watcher.GetEvent<UseConsumableEvent>()?.AddHandler((_, e) => PrintSimpleEvent(e));
        watcher.GetEvent<USSDropEvent>()?.AddHandler((_, e) => PrintSimpleEvent(e));
        watcher.GetEvent<VehicleSwitchEvent>()?.AddHandler((_, e) => PrintSimpleEvent(e));
        watcher.GetEvent<WingAddEvent>()?.AddHandler((_, e) => PrintSimpleEvent(e));
        watcher.GetEvent<WingInviteEvent>()?.AddHandler((_, e) => PrintSimpleEvent(e));
        watcher.GetEvent<WingJoinEvent>()?.AddHandler((_, e) => PrintSimpleEvent(e));
        watcher.GetEvent<WingLeaveEvent>()?.AddHandler((_, e) => PrintSimpleEvent(e));
        watcher.GetEvent<WonATrophyForSquadronEvent>()?.AddHandler((_, e) => PrintSimpleEvent(e));
*/
    }
}
