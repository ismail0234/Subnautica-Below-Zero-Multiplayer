namespace Subnautica.Events.Handlers
{
    using Subnautica.Events.EventArgs;

    using static Subnautica.API.Extensions.EventExtensions;

    public class Furnitures
    {
        /**
         *
         * ToiletSwitchToggle İşleyicisi
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static event SubnauticaPluginEventHandler<ToiletSwitchToggleEventArgs> ToiletSwitchToggle;

        /**
         *
         * ToiletSwitchToggle Olayı 
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void OnToiletSwitchToggle(ToiletSwitchToggleEventArgs ev) => ToiletSwitchToggle.CustomInvoke(ev);

        /**
         *
         * EmmanuelPendulumSwitchToggle İşleyicisi
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static event SubnauticaPluginEventHandler<EmmanuelPendulumSwitchToggleEventArgs> EmmanuelPendulumSwitchToggle;

        /**
         *
         * EmmanuelPendulumSwitchToggle Olayı 
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void OnEmmanuelPendulumSwitchToggle(EmmanuelPendulumSwitchToggleEventArgs ev) => EmmanuelPendulumSwitchToggle.CustomInvoke(ev);

        /**
         *
         * AromatherapyLampSwitchToggle İşleyicisi
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static event SubnauticaPluginEventHandler<AromatherapyLampSwitchToggleEventArgs> AromatherapyLampSwitchToggle;

        /**
         *
         * AromatherapyLampSwitchToggle Olayı 
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void OnAromatherapyLampSwitchToggle(AromatherapyLampSwitchToggleEventArgs ev) => AromatherapyLampSwitchToggle.CustomInvoke(ev);

        /**
         *
         * SmallStoveSwitchToggle İşleyicisi
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static event SubnauticaPluginEventHandler<SmallStoveSwitchToggleEventArgs> SmallStoveSwitchToggle;

        /**
         *
         * SmallStoveSwitchToggle Olayı 
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void OnSmallStoveSwitchToggle(SmallStoveSwitchToggleEventArgs ev) => SmallStoveSwitchToggle.CustomInvoke(ev);

        /**
         *
         * SinkSwitchToggle İşleyicisi
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static event SubnauticaPluginEventHandler<SinkSwitchToggleEventArgs> SinkSwitchToggle;

        /**
         *
         * SinkSwitchToggle Olayı 
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void OnSinkSwitchToggle(SinkSwitchToggleEventArgs ev) => SinkSwitchToggle.CustomInvoke(ev);

        /**
         *
         * ShowerSwitchToggle İşleyicisi
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static event SubnauticaPluginEventHandler<ShowerSwitchToggleEventArgs> ShowerSwitchToggle;

        /**
         *
         * ShowerSwitchToggle Olayı 
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void OnShowerSwitchToggle(ShowerSwitchToggleEventArgs ev) => ShowerSwitchToggle.CustomInvoke(ev);

        /**
         *
         * SnowmanDestroying İşleyicisi
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static event SubnauticaPluginEventHandler<SnowmanDestroyingEventArgs> SnowmanDestroying;

        /**
         *
         * SnowmanDestroying Olayı 
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void OnSnowmanDestroying(SnowmanDestroyingEventArgs ev) => SnowmanDestroying.CustomInvoke(ev);

        /**
         *
         * SignDataChanged İşleyicisi
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static event SubnauticaPluginEventHandler<SignDataChangedEventArgs> SignDataChanged;

        /**
         *
         * SignDataChanged Olayı 
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void OnSignDataChanged(SignDataChangedEventArgs ev) => SignDataChanged.CustomInvoke(ev);

        /**
         *
         * JukeboxUsed İşleyicisi
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static event SubnauticaPluginEventHandler<JukeboxUsedEventArgs> JukeboxUsed;

        /**
         *
         * JukeboxUsed Olayı 
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void OnJukeboxUsed(JukeboxUsedEventArgs ev) => JukeboxUsed.CustomInvoke(ev);

        /**
         *
         * PictureFrameImageSelecting İşleyicisi
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static event SubnauticaPluginEventHandler<PictureFrameImageSelectingEventArgs> PictureFrameImageSelecting;

        /**
         *
         * PictureFrameImageSelecting Olayı 
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void OnPictureFrameImageSelecting(PictureFrameImageSelectingEventArgs ev) => PictureFrameImageSelecting.CustomInvoke(ev);

        /**
         *
         * BedIsCanSleepChecking İşleyicisi
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static event SubnauticaPluginEventHandler<BedIsCanSleepCheckingEventArgs> BedIsCanSleepChecking;

        /**
         *
         * BedIsCanSleepChecking Olayı 
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void OnBedIsCanSleepChecking(BedIsCanSleepCheckingEventArgs ev) => BedIsCanSleepChecking.CustomInvoke(ev);

        /**
         *
         * BedEnterInUseMode İşleyicisi
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static event SubnauticaPluginEventHandler<BedEnterInUseModeEventArgs> BedEnterInUseMode;

        /**
         *
         * BedEnterInUseMode Olayı 
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void OnBedEnterInUseMode(BedEnterInUseModeEventArgs ev) => BedEnterInUseMode.CustomInvoke(ev);

        /**
         *
         * AquariumDataChanged İşleyicisi
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static event SubnauticaPluginEventHandler<AquariumDataChangedEventArgs> AquariumDataChanged;

        /**
         *
         * AquariumDataChanged Olayı 
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void OnAquariumDataChanged(AquariumDataChangedEventArgs ev) => AquariumDataChanged.CustomInvoke(ev);

        /**
         *
         * CrafterOpening İşleyicisi
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static event SubnauticaPluginEventHandler<CrafterOpeningEventArgs> CrafterOpening;

        /**
         *
         * CrafterOpening Olayı 
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void OnCrafterOpening(CrafterOpeningEventArgs ev) => CrafterOpening.CustomInvoke(ev);

        /**
         *
         * CrafterClosed İşleyicisi
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static event SubnauticaPluginEventHandler<CrafterClosedEventArgs> CrafterClosed;

        /**
         *
         * CrafterClosed Olayı 
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void OnCrafterClosed(CrafterClosedEventArgs ev) => CrafterClosed.CustomInvoke(ev);

        /**
         *
         * CrafterEnded İşleyicisi
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static event SubnauticaPluginEventHandler<CrafterEndedEventArgs> CrafterEnded;

        /**
         *
         * CrafterEnded Olayı 
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void OnCrafterEnded(CrafterEndedEventArgs ev) => CrafterEnded.CustomInvoke(ev);

        /**
         *
         * CrafterBegin İşleyicisi
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static event SubnauticaPluginEventHandler<CrafterBeginEventArgs> CrafterBegin;

        /**
         *
         * CrafterBegin Olayı 
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void OnCrafterBegin(CrafterBeginEventArgs ev) => CrafterBegin.CustomInvoke(ev);

        /**
         *
         * CrafterItemPickup İşleyicisi
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static event SubnauticaPluginEventHandler<CrafterItemPickupEventArgs> CrafterItemPickup;

        /**
         *
         * CrafterItemPickup Olayı 
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void OnCrafterItemPickup(CrafterItemPickupEventArgs ev) => CrafterItemPickup.CustomInvoke(ev);

        /**
         *
         * BenchStandup İşleyicisi
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static event SubnauticaPluginEventHandler<BenchStandupEventArgs> BenchStandup;

        /**
         *
         * BenchStandup Olayı 
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void OnBenchStandup(BenchStandupEventArgs ev) => BenchStandup.CustomInvoke(ev);

        /**
         *
         * BenchSitdown İşleyicisi
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static event SubnauticaPluginEventHandler<BenchSitdownEventArgs> BenchSitdown;

        /**
         *
         * BenchSitdown Olayı 
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void OnBenchSitdown(BenchSitdownEventArgs ev) => BenchSitdown.CustomInvoke(ev);

        /**
         *
         * SignSelect İşleyicisi
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static event SubnauticaPluginEventHandler<SignSelectEventArgs> SignSelect;

        /**
         *
         * SignSelect Olayı 
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void OnSignSelect(SignSelectEventArgs ev) => SignSelect.CustomInvoke(ev);

        /**
         *
         * SignDeselect İşleyicisi
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static event SubnauticaPluginEventHandler<SignDeselectEventArgs> SignDeselect;

        /**
         *
         * SignDeselect Olayı 
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void OnSignDeselect(SignDeselectEventArgs ev) => SignDeselect.CustomInvoke(ev);

        /**
         *
         * PictureFrameOpening İşleyicisi
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static event SubnauticaPluginEventHandler<PictureFrameOpeningEventArgs> PictureFrameOpening;

        /**
         *
         * PictureFrameOpening Olayı 
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void OnPictureFrameOpening(PictureFrameOpeningEventArgs ev) => PictureFrameOpening.CustomInvoke(ev);

        /**
         *
         * ChargerOpening İşleyicisi
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static event SubnauticaPluginEventHandler<ChargerOpeningEventArgs> ChargerOpening;

        /**
         *
         * ChargerOpening Olayı 
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void OnChargerOpening(ChargerOpeningEventArgs ev) => ChargerOpening.CustomInvoke(ev);

        /**
         *
         * HoverpadHoverbikeSpawning İşleyicisi
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static event SubnauticaPluginEventHandler<HoverpadHoverbikeSpawningEventArgs> HoverpadHoverbikeSpawning;

        /**
         *
         * HoverpadHoverbikeSpawning Olayı 
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void OnHoverpadHoverbikeSpawning(HoverpadHoverbikeSpawningEventArgs ev) => HoverpadHoverbikeSpawning.CustomInvoke(ev);

        /**
         *
         * RecyclotronRecycle İşleyicisi
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static event SubnauticaPluginEventHandler<RecyclotronRecycleEventArgs> RecyclotronRecycle;

        /**
         *
         * RecyclotronRecycle Olayı 
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void OnRecyclotronRecycle(RecyclotronRecycleEventArgs ev) => RecyclotronRecycle.CustomInvoke(ev);

        /**
         *
         * FiltrationMachineOpening İşleyicisi
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static event SubnauticaPluginEventHandler<FiltrationMachineOpeningEventArgs> FiltrationMachineOpening;

        /**
         *
         * FiltrationMachineOpening Olayı 
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void OnFiltrationMachineOpening(FiltrationMachineOpeningEventArgs ev) => FiltrationMachineOpening.CustomInvoke(ev);

        /**
         *
         * PlanterItemAdded İşleyicisi
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static event SubnauticaPluginEventHandler<PlanterItemAddedEventArgs> PlanterItemAdded;

        /**
         *
         * PlanterItemAdded Olayı 
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void OnPlanterItemAdded(PlanterItemAddedEventArgs ev) => PlanterItemAdded.CustomInvoke(ev);

        /**
         *
         * PlanterGrowned İşleyicisi
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static event SubnauticaPluginEventHandler<PlanterGrownedEventArgs> PlanterGrowned;

        /**
         *
         * PlanterGrowned Olayı 
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void OnPlanterGrowned(PlanterGrownedEventArgs ev) => PlanterGrowned.CustomInvoke(ev);

        /**
         *
         * PlanterProgressCompleted İşleyicisi
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static event SubnauticaPluginEventHandler<PlanterProgressCompletedEventArgs> PlanterProgressCompleted;

        /**
         *
         * PlanterProgressCompleted Olayı 
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void OnPlanterProgressCompleted(PlanterProgressCompletedEventArgs ev) => PlanterProgressCompleted.CustomInvoke(ev);

        /**
         *
         * BedExitInUseMode İşleyicisi
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static event SubnauticaPluginEventHandler<BedExitInUseModeEventArgs> BedExitInUseMode;

        /**
         *
         * BedExitInUseMode Olayı 
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void OnBedExitInUseMode(BedExitInUseModeEventArgs ev) => BedExitInUseMode.CustomInvoke(ev);

        /**
         *
         * BulkheadOpening İşleyicisi
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static event SubnauticaPluginEventHandler<BulkheadOpeningEventArgs> BulkheadOpening;

        /**
         *
         * BulkheadOpening Olayı 
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void OnBulkheadOpening(BulkheadOpeningEventArgs ev) => BulkheadOpening.CustomInvoke(ev);

        /**
         *
         * BulkheadClosing İşleyicisi
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static event SubnauticaPluginEventHandler<BulkheadClosingEventArgs> BulkheadClosing;

        /**
         *
         * BulkheadClosing Olayı 
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void OnBulkheadClosing(BulkheadClosingEventArgs ev) => BulkheadClosing.CustomInvoke(ev);

        /**
         *
         * BaseControlRoomMinimapUsing İşleyicisi
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static event SubnauticaPluginEventHandler<BaseControlRoomMinimapUsingEventArgs> BaseControlRoomMinimapUsing;

        /**
         *
         * BaseControlRoomMinimapUsing Olayı 
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void OnBaseControlRoomMinimapUsing(BaseControlRoomMinimapUsingEventArgs ev) => BaseControlRoomMinimapUsing.CustomInvoke(ev);

        /**
         *
         * BaseControlRoomMinimapExiting İşleyicisi
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static event SubnauticaPluginEventHandler<BaseControlRoomMinimapExitingEventArgs> BaseControlRoomMinimapExiting;

        /**
         *
         * BaseControlRoomMinimapExiting Olayı 
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void OnBaseControlRoomMinimapExiting(BaseControlRoomMinimapExitingEventArgs ev) => BaseControlRoomMinimapExiting.CustomInvoke(ev);

        /**
         *
         * BaseControlRoomCellPowerChanging İşleyicisi
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static event SubnauticaPluginEventHandler<BaseControlRoomCellPowerChangingEventArgs> BaseControlRoomCellPowerChanging;

        /**
         *
         * BaseControlRoomCellPowerChanging Olayı 
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void OnBaseControlRoomCellPowerChanging(BaseControlRoomCellPowerChangingEventArgs ev) => BaseControlRoomCellPowerChanging.CustomInvoke(ev);

        /**
         *
         * BaseControlRoomMinimapMoving İşleyicisi
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static event SubnauticaPluginEventHandler<BaseControlRoomMinimapMovingEventArgs> BaseControlRoomMinimapMoving;

        /**
         *
         * BaseControlRoomMinimapMoving Olayı 
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void OnBaseControlRoomMinimapMoving(BaseControlRoomMinimapMovingEventArgs ev) => BaseControlRoomMinimapMoving.CustomInvoke(ev);

        /**
         *
         * HoverpadDocking İşleyicisi
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static event SubnauticaPluginEventHandler<HoverpadDockingEventArgs> HoverpadDocking;

        /**
         *
         * HoverpadDocking Olayı 
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void OnHoverpadDocking(HoverpadDockingEventArgs ev) => HoverpadDocking.CustomInvoke(ev);

        /**
         *
         * HoverpadUnDocking İşleyicisi
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static event SubnauticaPluginEventHandler<HoverpadUnDockingEventArgs> HoverpadUnDocking;

        /**
         *
         * HoverpadUnDocking Olayı 
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void OnHoverpadUnDocking(HoverpadUnDockingEventArgs ev) => HoverpadUnDocking.CustomInvoke(ev);

        /**
         *
         * HoverpadShowroomTriggering İşleyicisi
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static event SubnauticaPluginEventHandler<HoverpadShowroomTriggeringEventArgs> HoverpadShowroomTriggering;

        /**
         *
         * HoverpadShowroomTriggering Olayı 
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void OnHoverpadShowroomTriggering(HoverpadShowroomTriggeringEventArgs ev) => HoverpadShowroomTriggering.CustomInvoke(ev);

        /**
         *
         * SpotLightInitialized İşleyicisi
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static event SubnauticaPluginEventHandler<SpotLightInitializedEventArgs> SpotLightInitialized;

        /**
         *
         * SpotLightInitialized Olayı 
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void OnSpotLightInitialized(SpotLightInitializedEventArgs ev) => SpotLightInitialized.CustomInvoke(ev);

        /**
         *
         * TechLightInitialized İşleyicisi
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static event SubnauticaPluginEventHandler<TechLightInitializedEventArgs> TechLightInitialized;

        /**
         *
         * TechLightInitialized Olayı 
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void OnTechLightInitialized(TechLightInitializedEventArgs ev) => TechLightInitialized.CustomInvoke(ev);

        /**
         *
         * BaseMapRoomScanStopping İşleyicisi
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static event SubnauticaPluginEventHandler<BaseMapRoomScanStoppingEventArgs> BaseMapRoomScanStopping;

        /**
         *
         * BaseMapRoomScanStopping Olayı 
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void OnBaseMapRoomScanStopping(BaseMapRoomScanStoppingEventArgs ev) => BaseMapRoomScanStopping.CustomInvoke(ev);

        /**
         *
         * BaseMapRoomScanStarting İşleyicisi
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static event SubnauticaPluginEventHandler<BaseMapRoomScanStartingEventArgs> BaseMapRoomScanStarting;

        /**
         *
         * BaseMapRoomScanStarting Olayı 
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void OnBaseMapRoomScanStarting(BaseMapRoomScanStartingEventArgs ev) => BaseMapRoomScanStarting.CustomInvoke(ev);

        /**
         *
         * MapRoomCameraChanging İşleyicisi
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static event SubnauticaPluginEventHandler<MapRoomCameraChangingEventArgs> BaseMapRoomCameraChanging;

        /**
         *
         * MapRoomCameraChanging Olayı 
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void OnBaseMapRoomCameraChanging(MapRoomCameraChangingEventArgs ev) => BaseMapRoomCameraChanging.CustomInvoke(ev);

        /**
         *
         * MapRoomResourceDiscovering İşleyicisi
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static event SubnauticaPluginEventHandler<BaseMapRoomResourceDiscoveringEventArgs> BaseMapRoomResourceDiscovering;

        /**
         *
         * MapRoomResourceDiscovering Olayı 
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void OnBaseMapRoomResourceDiscovering(BaseMapRoomResourceDiscoveringEventArgs ev) => BaseMapRoomResourceDiscovering.CustomInvoke(ev);

        /**
         *
         * BaseMapRoomInitialized İşleyicisi
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static event SubnauticaPluginEventHandler<BaseMapRoomInitializedEventArgs> BaseMapRoomInitialized;

        /**
         *
         * BaseMapRoomInitialized Olayı 
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void OnBaseMapRoomInitialized(BaseMapRoomInitializedEventArgs ev) => BaseMapRoomInitialized.CustomInvoke(ev);

        /**
         *
         * BaseMoonpoolExpansionUndockingTimelineCompleting İşleyicisi
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static event SubnauticaPluginEventHandler<BaseMoonpoolExpansionUndockingTimelineCompletingEventArgs> BaseMoonpoolExpansionUndockingTimelineCompleting;

        /**
         *
         * BaseMoonpoolExpansionUndockingTimelineCompleting Olayı 
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void OnBaseMoonpoolExpansionUndockingTimelineCompleting(BaseMoonpoolExpansionUndockingTimelineCompletingEventArgs ev) => BaseMoonpoolExpansionUndockingTimelineCompleting.CustomInvoke(ev);

        /**
         *
         * BaseMoonpoolExpansionDockingTimelineCompleting İşleyicisi
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static event SubnauticaPluginEventHandler<BaseMoonpoolExpansionDockingTimelineCompletingEventArgs> BaseMoonpoolExpansionDockingTimelineCompleting;

        /**
         *
         * BaseMoonpoolExpansionDockingTimelineCompleting Olayı 
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void OnBaseMoonpoolExpansionDockingTimelineCompleting(BaseMoonpoolExpansionDockingTimelineCompletingEventArgs ev) => BaseMoonpoolExpansionDockingTimelineCompleting.CustomInvoke(ev);

        /**
         *
         * BaseMoonpoolExpansionDockTail İşleyicisi
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static event SubnauticaPluginEventHandler<BaseMoonpoolExpansionDockTailEventArgs> BaseMoonpoolExpansionDockTail;

        /**
         *
         * BaseMoonpoolExpansionDockTail Olayı 
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void OnBaseMoonpoolExpansionDockTail(BaseMoonpoolExpansionDockTailEventArgs ev) => BaseMoonpoolExpansionDockTail.CustomInvoke(ev);

        /**
         *
         * BaseMoonpoolExpansionUndockTail İşleyicisi
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static event SubnauticaPluginEventHandler<BaseMoonpoolExpansionUndockTailEventArgs> BaseMoonpoolExpansionUndockTail;

        /**
         *
         * BaseMoonpoolExpansionUndockTail Olayı 
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void OnBaseMoonpoolExpansionUndockTail(BaseMoonpoolExpansionUndockTailEventArgs ev) => BaseMoonpoolExpansionUndockTail.CustomInvoke(ev);
    }
}