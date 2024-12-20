namespace Subnautica.Events.Handlers
{
    using Subnautica.Events.EventArgs;

    using static Subnautica.API.Extensions.EventExtensions;

    public class World
    {
        /**
         *
         * ThermalLilyRangeChecking İşleyicisi
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static event SubnauticaPluginEventHandler<ThermalLilyRangeCheckingEventArgs> ThermalLilyRangeChecking;

        /**
         *
         * ThermalLilyRangeChecking Olayı 
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void OnThermalLilyRangeChecking(ThermalLilyRangeCheckingEventArgs ev) => ThermalLilyRangeChecking.CustomInvoke(ev);

        /**
         *
         * ThermalLilyAnimationAnglesChecking İşleyicisi
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static event SubnauticaPluginEventHandler<ThermalLilyAnimationAnglesCheckingEventArgs> ThermalLilyAnimationAnglesChecking;

        /**
         *
         * ThermalLilyAnimationAnglesChecking Olayı 
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void OnThermalLilyAnimationAnglesChecking(ThermalLilyAnimationAnglesCheckingEventArgs ev) => ThermalLilyAnimationAnglesChecking.CustomInvoke(ev);

        /**
         *
         * OxygenPlantClicking İşleyicisi
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static event SubnauticaPluginEventHandler<OxygenPlantClickingEventArgs> OxygenPlantClicking;

        /**
         *
         * OxygenPlantClicking Olayı 
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void OnOxygenPlantClicking(OxygenPlantClickingEventArgs ev) => OxygenPlantClicking.CustomInvoke(ev);

        /**
         *
         * EntitySpawning İşleyicisi
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static event SubnauticaPluginEventHandler<EntitySpawningEventArgs> EntitySpawning;

        /**
         *
         * EntitySpawning Olayı 
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void OnEntitySpawning(EntitySpawningEventArgs ev) => EntitySpawning.CustomInvoke(ev);

        /**
         *
         * AlterraPdaPickedUp İşleyicisi
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static event SubnauticaPluginEventHandler<AlterraPdaPickedUpEventArgs> AlterraPdaPickedUp;

        /**
         *
         * AlterraPdaPickedUp Olayı 
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void OnAlterraPdaPickedUp(AlterraPdaPickedUpEventArgs ev) => AlterraPdaPickedUp.CustomInvoke(ev);

        /**
         *
         * JukeboxDiskPickedUp İşleyicisi
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static event SubnauticaPluginEventHandler<JukeboxDiskPickedUpEventArgs> JukeboxDiskPickedUp;

        /**
         *
         * JukeboxDiskPickedUp Olayı 
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void OnJukeboxDiskPickedUp(JukeboxDiskPickedUpEventArgs ev) => JukeboxDiskPickedUp.CustomInvoke(ev);

        /**
         *
         * EntitySpawned İşleyicisi
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static event SubnauticaPluginEventHandler<EntitySpawnedEventArgs> EntitySpawned;

        /**
         *
         * EntitySpawned Olayı 
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void OnEntitySpawned(EntitySpawnedEventArgs ev) => EntitySpawned.CustomInvoke(ev);

        /**
         *
         * SupplyCrateOpened İşleyicisi
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static event SubnauticaPluginEventHandler<SupplyCrateOpenedEventArgs> SupplyCrateOpened;

        /**
         *
         * SupplyCrateOpened Olayı 
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void OnSupplyCrateOpened(SupplyCrateOpenedEventArgs ev) => SupplyCrateOpened.CustomInvoke(ev);

        /**
         *
         * DataboxItemPickedUp İşleyicisi
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static event SubnauticaPluginEventHandler<DataboxItemPickedUpEventArgs> DataboxItemPickedUp;

        /**
         *
         * DataboxItemPickedUp Olayı 
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void OnDataboxItemPickedUp(DataboxItemPickedUpEventArgs ev) => DataboxItemPickedUp.CustomInvoke(ev);

        /**
         *
         * TakeDamaging İşleyicisi
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static event SubnauticaPluginEventHandler<TakeDamagingEventArgs> TakeDamaging;

        /**
         *
         * TakeDamaging Olayı 
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void OnTakeDamaging(TakeDamagingEventArgs ev) => TakeDamaging.CustomInvoke(ev);

        /**
         *
         * FruitHarvesting İşleyicisi
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static event SubnauticaPluginEventHandler<FruitHarvestingEventArgs> FruitHarvesting;

        /**
         *
         * FruitHarvesting Olayı 
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void OnFruitHarvesting(FruitHarvestingEventArgs ev) => FruitHarvesting.CustomInvoke(ev);

        /**
         *
         * CellLoading İşleyicisi
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static event SubnauticaPluginEventHandler<CellLoadingEventArgs> CellLoading;

        /**
         *
         * CellLoading Olayı 
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void OnCellLoading(CellLoadingEventArgs ev) => CellLoading.CustomInvoke(ev);

        /**
         *
         * CellUnLoading İşleyicisi
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static event SubnauticaPluginEventHandler<CellUnLoadingEventArgs> CellUnLoading;

        /**
         *
         * CellUnLoading Olayı 
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void OnCellUnLoading(CellUnLoadingEventArgs ev) => CellUnLoading.CustomInvoke(ev);

        /**
         *
         * GrownPlantHarvesting İşleyicisi
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static event SubnauticaPluginEventHandler<GrownPlantHarvestingEventArgs> GrownPlantHarvesting;

        /**
         *
         * GrownPlantHarvesting Olayı 
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void OnGrownPlantHarvesting(GrownPlantHarvestingEventArgs ev) => GrownPlantHarvesting.CustomInvoke(ev);

        /**
         *
         * EntitySlotSpawning İşleyicisi
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static event SubnauticaPluginEventHandler<EntitySlotSpawningEventArgs> EntitySlotSpawning;

        /**
         *
         * EntitySlotSpawning Olayı 
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void OnEntitySlotSpawning(EntitySlotSpawningEventArgs ev) => EntitySlotSpawning.CustomInvoke(ev);

        /**
         *
         * LaserCutterUsing İşleyicisi
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static event SubnauticaPluginEventHandler<LaserCutterEventArgs> LaserCutterUsing;

        /**
         *
         * LaserCutterUsing Olayı 
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void OnLaserCutterUsing(LaserCutterEventArgs ev) => LaserCutterUsing.CustomInvoke(ev);

        /**
         *
         * SealedInitialized İşleyicisi
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static event SubnauticaPluginEventHandler<SealedInitializedEventArgs> SealedInitialized;

        /**
         *
         * SealedInitialized Olayı 
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void OnSealedInitialized(SealedInitializedEventArgs ev) => SealedInitialized.CustomInvoke(ev);

        /**
         *
         * ElevatorCalling İşleyicisi
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static event SubnauticaPluginEventHandler<ElevatorCallingEventArgs> ElevatorCalling;

        /**
         *
         * ElevatorCalling Olayı 
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void OnElevatorCalling(ElevatorCallingEventArgs ev) => ElevatorCalling.CustomInvoke(ev);

        /**
         *
         * SpawnOnKilling İşleyicisi
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static event SubnauticaPluginEventHandler<SpawnOnKillingEventArgs> SpawnOnKilling;

        /**
         *
         * SpawnOnKilling Olayı 
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void OnSpawnOnKilling(SpawnOnKillingEventArgs ev) => SpawnOnKilling.CustomInvoke(ev);

        /**
         *
         * WeatherProfileChanged İşleyicisi
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static event SubnauticaPluginEventHandler<WeatherProfileChangedEventArgs> WeatherProfileChanged;

        /**
         *
         * WeatherProfileChanged Olayı 
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void OnWeatherProfileChanged(WeatherProfileChangedEventArgs ev) => WeatherProfileChanged.CustomInvoke(ev);

        /**
         *
         * TeleporterTerminalActivating İşleyicisi
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static event SubnauticaPluginEventHandler<TeleporterTerminalActivatingEventArgs> TeleporterTerminalActivating;

        /**
         *
         * TeleporterTerminalActivating Olayı 
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void OnTeleporterTerminalActivating(TeleporterTerminalActivatingEventArgs ev) => TeleporterTerminalActivating.CustomInvoke(ev);

        /**
         *
         * TeleporterInitialized İşleyicisi
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static event SubnauticaPluginEventHandler<TeleporterInitializedEventArgs> TeleporterInitialized;

        /**
         *
         * TeleporterInitialized Olayı 
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void OnTeleporterInitialized(TeleporterInitializedEventArgs ev) => TeleporterInitialized.CustomInvoke(ev);

        /**
         *
         * ElevatorInitialized İşleyicisi
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static event SubnauticaPluginEventHandler<ElevatorInitializedEventArgs> ElevatorInitialized;

        /**
         *
         * ElevatorInitialized Olayı 
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void OnElevatorInitialized(ElevatorInitializedEventArgs ev) => ElevatorInitialized.CustomInvoke(ev);

        /**
         *
         * CrushDamaging İşleyicisi
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static event SubnauticaPluginEventHandler<CrushDamagingEventArgs> CrushDamaging;

        /**
         *
         * CrushDamaging Olayı 
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void OnCrushDamaging(CrushDamagingEventArgs ev) => CrushDamaging.CustomInvoke(ev);

        /**
         *
         * CosmeticItemPlacing İşleyicisi
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static event SubnauticaPluginEventHandler<CosmeticItemPlacingEventArgs> CosmeticItemPlacing;

        /**
         *
         * CosmeticItemPlacing Olayı 
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void OnCosmeticItemPlacing(CosmeticItemPlacingEventArgs ev) => CosmeticItemPlacing.CustomInvoke(ev);
    }
}