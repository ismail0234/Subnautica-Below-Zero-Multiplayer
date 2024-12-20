namespace Subnautica.Client.Abstracts
{
    using System.Collections.Generic;

    using Subnautica.API.Enums;
    using Subnautica.Client.Synchronizations.Processors;
    using Subnautica.Client.Abstracts.Processors;

    using Initial            = Subnautica.Client.Synchronizations.InitialSync;
    using Building           = Subnautica.Client.Synchronizations.Processors.Building;
    using Encyclopedia       = Subnautica.Client.Synchronizations.Processors.Encyclopedia;
    using PDA                = Subnautica.Client.Synchronizations.Processors.PDA;
    using Player             = Subnautica.Client.Synchronizations.Processors.Player;
    using Technology         = Subnautica.Client.Synchronizations.Processors.Technology;
    using World              = Subnautica.Client.Synchronizations.Processors.World;
    using Metadata           = Subnautica.Client.Synchronizations.Processors.Metadata;
    using General            = Subnautica.Client.Synchronizations.Processors.General;
    using Items              = Subnautica.Client.Synchronizations.Processors.Items;
    using WorldEntities      = Subnautica.Client.Synchronizations.Processors.WorldEntities;
    using DynamicEntities    = Subnautica.Client.Synchronizations.Processors.WorldEntities.DynamicEntities;
    using Vehicle            = Subnautica.Client.Synchronizations.Processors.Vehicle;
    using Story              = Subnautica.Client.Synchronizations.Processors.Story;
    using Creatures          = Subnautica.Client.Synchronizations.Processors.Creatures;
    using EnergyTransmission = Subnautica.Client.Synchronizations.Processors.World.EnergyTransmission;

    public class ProcessorShared
    {

        /**
         *
         * İşlemleri barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static Dictionary<ProcessType, NormalProcessor> Processors { get; set; } = new Dictionary<ProcessType, NormalProcessor>()
        {
            /**
             *
             * Normal Senkronizasyonlar
             *
             * @author Ismail <ismaiil_0234@hotmail.com>
             *
             */
            { ProcessType.None                               , new NoneProcessor() },
            { ProcessType.JoiningServer                      , new Player.JoiningProcessor() },
            { ProcessType.PlayerUpdated                      , new Player.UpdatedProcessor() },
            { ProcessType.PlayerDisconnected                 , new Player.DisconnectedProcessor() },
            { ProcessType.PlayerStats                        , new Player.StatsProcessor() },
            { ProcessType.SubrootToggle                      , new Player.SubrootToggleProcessor() },
            { ProcessType.InteriorToggle                     , new Player.InteriorToggleProcessor() },
            { ProcessType.AnotherPlayerConnected             , new Player.AnotherPlayerConnectedProcessor() },
            { ProcessType.PlayerAnimationChanged             , new Player.AnimationChangedProcessor()},
            { ProcessType.ItemDrop                           , new Player.ItemDropProcessor()},
            { ProcessType.ItemPickup                         , new Player.ItemPickupProcessor()},
            { ProcessType.UseableDiveHatch                   , new Player.UseableDiveHatchProcessor()},
            { ProcessType.PlayerClimb                        , new Player.ClimbProcessor()},
            { ProcessType.PlayerDead                         , new Player.DeadProcessor()},
            { ProcessType.PlayerSpawn                        , new Player.SpawnProcessor()},
            { ProcessType.PlayerToolEnergy                   , new Player.ToolEnergyProcessor()},
            { ProcessType.PlayerConsoleCommand               , new Player.ConsoleCommandProcessor()},
            { ProcessType.PlayerRespawnPointChanged          , new Player.RespawnPointProcessor()},
            { ProcessType.PlayerInitialEquipment             , new Player.FirstInitialEquipmentProcessor()},
            { ProcessType.Ping                               , new Player.PingProcessor()},
            { ProcessType.PlayerFreeze                       , new Player.FreezeProcessor()},
            { ProcessType.EncyclopediaAdded                  , new Encyclopedia.AddedProcessor() },
            { ProcessType.TechnologyAdded                    , new Technology.AddedProcessor() },
            { ProcessType.TechnologyFragmentAdded            , new Technology.FragmentAddedProcessor() },
            { ProcessType.ScannerCompleted                   , new Technology.ScannerCompletedProcessor() },
            { ProcessType.TechAnalyzeAdded                   , new PDA.TechAnalyzeAddedProcessor() },
            { ProcessType.JukeboxDiskAdded                   , new PDA.JukeboxDiskAddedProcessor() },
            { ProcessType.ConstructingRemoved                , new Building.RemovedProcessor() },
            { ProcessType.ConstructingGhostMoving            , new Building.GhostMovedProcessor() },
            { ProcessType.ConstructingAmountChanged          , new Building.AmountChangedProcessor() },
            { ProcessType.ConstructingGhostTryPlacing        , new Building.GhostTryPlacingProcessor() },
            { ProcessType.ConstructingCompleted              , new Building.CompletedProcessor() },
            { ProcessType.DeconstructionBegin                , new Building.DeconstructionBeginProcessor() },
            { ProcessType.FurnitureDeconstructionBegin       , new Building.FurnitureDeconstructionBeginProcessor() },
            { ProcessType.MetadataRequest                    , new Building.MetadataRequestProcessor() },
            { ProcessType.BaseHullStrength                   , new Building.BaseHullStrengthProcessor() },
            { ProcessType.ConstructionHealth                 , new Building.HealthProcessor() },
            { ProcessType.Interact                           , new General.InteractProcessor() },
            { ProcessType.FirstTimeStartServer               , new General.IntroProcessor()},
            { ProcessType.LifepodProcess                     , new General.LifepodProcessor()},
            { ProcessType.StorageOpen                        , new General.StorageOpenProcessor()},
            { ProcessType.ResourceDiscover                   , new General.ResourceDiscoverProcessor()},
            { ProcessType.EnergyTransmission                 , new World.EnergyProductionProcessor() },
            { ProcessType.HoverpadChargeTransmission         , new World.HoverpadChargeProcessor() },
            { ProcessType.BatteryCharging                    , new World.BatteryChargingProcessor() },
            { ProcessType.FiltrationMachineTransmission      , new World.FiltrationMachineTransmissionProcessor() },
            { ProcessType.EntityScannerCompleted             , new World.EntityScannerProcessor() },
            { ProcessType.StaticEntityPickedUp               , new World.StaticEntityProcessor() },
            { ProcessType.SleepTimeSkip                      , new World.SleepTimeSkipProcessor()},
            { ProcessType.WorldDynamicEntityPosition         , new World.WorldDynamicEntityPositionProcessor()},
            { ProcessType.WorldDynamicEntityOwnershipChanged , new World.WorldDynamicEntityOwnershipChangedProcessor()},
            { ProcessType.CreatureProcess                    , new World.WorldCreatureActionProcessor()},
            { ProcessType.EnergyMixinTransmission            , new World.EnergyMixinTransmissionProcessor()},
            { ProcessType.SpawnOnKill                        , new World.SpawnOnKillProcessor()},
            { ProcessType.EntitySlotProcess                  , new World.EntitySlotSpawnProcessor()},
            { ProcessType.WeatherChanged                     , new World.WeatherProcessor()},
            { ProcessType.PrecursorTeleporter                , new World.PrecursorTeleporterProcessor()},
            { ProcessType.WelderRepair                       , new World.WelderProcessor()},
            { ProcessType.BaseCellWaterLevel                 , new World.BaseCellWaterLevelProcessor()},
            { ProcessType.Brinicle                           , new World.BrinicleProcessor()},
            { ProcessType.CosmeticItem                       , new World.CosmeticItemProcessor()},
            { ProcessType.BaseMapRoomTransmission            , new World.BaseMapRoomTransmissionProcessor()},
            { ProcessType.VehicleRepair                      , new World.VehicleRepairProcessor()},
            { ProcessType.WorldCreaturePosition              , new Creatures.PositionProcessor()},
            { ProcessType.WorldCreatureOwnershipChanged      , new Creatures.OwnershipProcessor()},
            { ProcessType.CreatureHealth                     , new Creatures.HealthProcessor()},
            { ProcessType.CreatureAnimation                  , new Creatures.AnimationChangedProcessor()},
            { ProcessType.CreatureFreeze                     , new Creatures.FreezeProcessor()},
            { ProcessType.CreatureCallSound                  , new Creatures.CallSoundProcessor()},
            { ProcessType.CreatureAttackLastTarget           , new Creatures.AttackLastTargetProcessor()},
            { ProcessType.CreatureLeviathanMeleeAttack       , new Creatures.LeviathanMeleeAttackProcessor()},
            { ProcessType.CreatureMeleeAttack                , new Creatures.MeleeAttackProcessor()},        
            { ProcessType.WorldEntityAction                  , new WorldEntities.WorldEntityActionProcessor()},
            { ProcessType.VehicleEnergyTransmission          , new EnergyTransmission.VehicleEnergyTransmission()},
            { ProcessType.PlayerItemAction                   , new Items.PlayerItemActionProcessor()},
            { ProcessType.WorldLoaded                        , new Initial.WorldProcessor() },
            { ProcessType.VehicleUpgradeConsole              , new Vehicle.UpgradeConsoleProcessor()},
            { ProcessType.VehicleEnter                       , new Vehicle.EnterProcessor()},
            { ProcessType.VehicleExit                        , new Vehicle.ExitProcessor()},
            { ProcessType.VehicleUpdated                     , new Vehicle.UpdatedProcessor()},
            { ProcessType.VehicleHealth                      , new Vehicle.HealthProcessor()},
            { ProcessType.VehicleBattery                     , new Vehicle.BatteryProcessor()},
            { ProcessType.ExosuitStorage                     , new Vehicle.ExosuitStorageProcessor()},
            { ProcessType.ExosuitDrill                       , new Vehicle.ExosuitDrillProcessor()},
            { ProcessType.VehicleLight                       , new Vehicle.LightProcessor()},
            { ProcessType.VehicleInterior                    , new Vehicle.InteriorProcessor()},
            { ProcessType.SeaTruckConnection                 , new Vehicle.SeaTruckConnectionProcessor()},
            { ProcessType.SeaTruckStorageModule              , new Vehicle.SeaTruckStorageModuleProcessor()},
            { ProcessType.SeaTruckAquariumModule             , new Vehicle.SeaTruckAquariumModuleProcessor()},
            { ProcessType.SeaTruckFabricatorModule           , new Vehicle.SeaTruckFabricatorModuleProcessor()},
            { ProcessType.SeaTruckSleeperModule              , new Vehicle.SeaTruckSleeperModuleProcessor()},
            { ProcessType.SeaTruckDockingModule              , new Vehicle.SeaTruckDockingModuleProcessor()},
            { ProcessType.ExosuitJump                        , new Vehicle.ExosuitJumpProcessor()},
            { ProcessType.StoryBridge                        , new Story.BridgeProcessor()},
            { ProcessType.StoryRadioTower                    , new Story.RadioTowerProcessor()},
            { ProcessType.StorySignal                        , new Story.SignalProcessor()},
            { ProcessType.StoryTrigger                       , new Story.TriggerProcessor()},
            { ProcessType.StoryCinematicTrigger              , new Story.CinematicProcessor()},
            { ProcessType.StoryCall                          , new Story.CallProcessor()},
            { ProcessType.StoryInteract                      , new Story.InteractProcessor()},
            { ProcessType.StoryPlayerVisibility              , new Story.PlayerVisibilityProcessor()},
            { ProcessType.StoryFrozenCreature                , new Story.FrozenCreatureProcessor()},
            { ProcessType.StoryShieldBase                    , new Story.ShieldBaseProcessor()},
            { ProcessType.StoryEndGame                       , new Story.EndGameProcessor()},
        };

        /**
         *
         * Metadata İşlemlerini barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static Dictionary<TechType, MetadataProcessor> MetadataProcessors { get; set; } = new Dictionary<TechType, MetadataProcessor>()
        {
            /**
             *
             * Normal Senkronizasyonlar
             *
             * @author Ismail <ismaiil_0234@hotmail.com>
             *
             */
            { TechType.AromatherapyLamp      , new Metadata.AromatherapyProcessor() },
            { TechType.EmmanuelPendulum      , new Metadata.EmmanuelPendulumProcessor() },
            { TechType.Shower                , new Metadata.ShowerProcessor() },
            { TechType.Sink                  , new Metadata.SinkProcessor() },
            { TechType.SmallStove            , new Metadata.SmallStoveProcessor() },
            { TechType.Toilet                , new Metadata.ToiletProcessor() },
            { TechType.Sign                  , new Metadata.SignProcessor() },
            { TechType.PictureFrame          , new Metadata.PictureFrameProcessor() },
            { TechType.Jukebox               , new Metadata.JukeboxProcessor() },
            { TechType.Aquarium              , new Metadata.AquariumProcessor() },
            { TechType.Fabricator            , new Metadata.CrafterProcessor() },
            { TechType.Workbench             , new Metadata.CrafterProcessor() },
            { TechType.BaseUpgradeConsole    , new Metadata.CrafterProcessor() },
            { TechType.StarshipChair         , new Metadata.BenchProcessor() },
            { TechType.StarshipChair2        , new Metadata.BenchProcessor() },
            { TechType.StarshipChair3        , new Metadata.BenchProcessor() },
            { TechType.Bench                 , new Metadata.BenchProcessor() },
            { TechType.Snowman               , new Metadata.SnowmanProcessor() },
            { TechType.BaseBioReactor        , new Metadata.BioReactorProcessor() },
            { TechType.BaseNuclearReactor    , new Metadata.NuclearReactorProcessor() },
            { TechType.Trashcans             , new Metadata.TrashcansProcessor() },
            { TechType.LabTrashcan           , new Metadata.LabTrashcanProcessor() },
            { TechType.BatteryCharger        , new Metadata.ChargerProcessor() },  
            { TechType.PowerCellCharger      , new Metadata.ChargerProcessor() },
            { TechType.Locker                , new Metadata.StorageProcessor() },
            { TechType.SmallLocker           , new Metadata.StorageProcessor() },
            { TechType.Hoverpad              , new Metadata.HoverpadProcessor() },
            { TechType.Recyclotron           , new Metadata.RecyclotronProcessor() },
            { TechType.CoffeeVendingMachine  , new Metadata.CoffeeVendingMachineProcessor() },
            { TechType.Fridge                , new Metadata.FridgeProcessor() },
            { TechType.BaseFiltrationMachine , new Metadata.FiltrationMachineProcessor() },
            { TechType.PlanterBox            , new Metadata.PlanterProcessor() },
            { TechType.PlanterPot            , new Metadata.PlanterProcessor() },
            { TechType.PlanterPot2           , new Metadata.PlanterProcessor() },
            { TechType.PlanterPot3           , new Metadata.PlanterProcessor() },
            { TechType.PlanterShelf          , new Metadata.PlanterProcessor() },
            { TechType.FarmingTray           , new Metadata.PlanterProcessor() },
            { TechType.Bed1                  , new Metadata.BedProcessor() },
            { TechType.Bed2                  , new Metadata.BedProcessor() },
            { TechType.NarrowBed             , new Metadata.BedProcessor() },
            { TechType.BedJeremiah           , new Metadata.BedProcessor() },
            { TechType.BedSam                , new Metadata.BedProcessor() },
            { TechType.BedZeta               , new Metadata.BedProcessor() },
            { TechType.BedDanielle           , new Metadata.BedProcessor() },
            { TechType.BedEmmanuel           , new Metadata.BedProcessor() },
            { TechType.BedFred               , new Metadata.BedProcessor() },
            { TechType.BedParvan             , new Metadata.BedProcessor() },
            { TechType.BaseBulkhead          , new Metadata.BulkheadProcessor() },
            { TechType.BaseControlRoom       , new Metadata.BaseControlRoomProcessor() },
            { TechType.Spotlight             , new Metadata.SpotLightProcessor() },
            { TechType.Techlight             , new Metadata.TechlightProcessor() },
            { TechType.BaseMoonpool          , new Metadata.MoonpoolProcessor() },
            { TechType.BaseMoonpoolExpansion , new Metadata.MoonpoolProcessor() },
            { TechType.BaseMapRoom           , new Metadata.BaseMapRoomProcessor() },
            { TechType.BaseWaterPark         , new Metadata.BaseWaterParkProcessor() },            
        };

        /**
         *
         * Yaratık Senkronizasyon İşlemlerini barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static Dictionary<TechType, WorldCreatureProcessor> WorldCreatureProcessors { get; set; } = new Dictionary<TechType, WorldCreatureProcessor>()
        {
            /**
             *
             * Yaratık Senkronizasyonlar
             *
             * @author Ismail <ismaiil_0234@hotmail.com>
             *
             */
            { TechType.GlowWhale      , new Creatures.GlowWhaleProcessor() },
            { TechType.Crash          , new Creatures.CrashFishProcessor() },
            { TechType.LilyPaddler    , new Creatures.LilyPaddlerProcessor() },
            { TechType.GhostLeviathan , new Creatures.VoidLeviathanProcessor() },
        };

        /**
         *
         * World Entity İşlemlerini barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static Dictionary<EntityProcessType, WorldEntityProcessor> WorldEntityProcessors { get; set; } = new Dictionary<EntityProcessType, WorldEntityProcessor>()
        {
            /**
             *
             * Normal Senkronizasyonlar
             *
             * @author Ismail <ismaiil_0234@hotmail.com>
             *
             */
            { EntityProcessType.OxygenPlant         , new WorldEntities.OxygenPlantProcessor() },
            { EntityProcessType.SupplyCrate         , new WorldEntities.SupplyCrateProcessor() },
            { EntityProcessType.Databox             , new WorldEntities.DataboxProcessor() },
            { EntityProcessType.Destroyable         , new WorldEntities.DestroyableEntityProcessor() },
            { EntityProcessType.DestroyableDynamic  , new WorldEntities.DestroyableDynamicEntityProcessor() },
            { EntityProcessType.Plant               , new WorldEntities.FruitHarvestProcessor() },
            { EntityProcessType.BulkheadDoor        , new WorldEntities.BulkheadDoorProcessor() },
            { EntityProcessType.SealedObject        , new WorldEntities.LaserCutterProcessor() },
            { EntityProcessType.Elevator            , new WorldEntities.ElevatorProcessor() },
            { EntityProcessType.Drillable           , new WorldEntities.DrillableProcessor() },
        };

        /**
         *
         * World Dynamic Entity İşlemlerini barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static Dictionary<TechType, WorldDynamicEntityProcessor> WorldDynamicEntityProcessors { get; set; } = new Dictionary<TechType, WorldDynamicEntityProcessor>()
        {
            /**
             *
             * Normal Senkronizasyonlar
             *
             * @author Ismail <ismaiil_0234@hotmail.com>
             *
             */
            { TechType.Constructor                  , new DynamicEntities.ConstructorProcessor() },
            { TechType.SeaTruck                     , new DynamicEntities.SeaTruckProcessor() },
            { TechType.Exosuit                      , new DynamicEntities.ExosuitProcessor() },
            { TechType.Hoverbike                    , new DynamicEntities.HoverbikeProcessor() },
            { TechType.SmallStorage                 , new DynamicEntities.SmallStorageProcessor() },
            { TechType.QuantumLocker                , new DynamicEntities.QuantumLockerProcessor() },
            { TechType.LEDLight                     , new DynamicEntities.LEDLightProcessor() },
            { TechType.Thumper                      , new DynamicEntities.ThumperProcessor() },
            { TechType.Flare                        , new DynamicEntities.FlareProcessor() },
            { TechType.Beacon                       , new DynamicEntities.BeaconProcessor() },
            { TechType.SpyPenguin                   , new DynamicEntities.SpyPenguinProcessor() },
            { TechType.MapRoomCamera                , new DynamicEntities.MapRoomCameraProcessor() },
            { TechType.PipeSurfaceFloater           , new DynamicEntities.PipeSurfaceFloaterProcessor() },
            { TechType.SeaTruckStorageModule        , new DynamicEntities.SeaTruckStorageModuleProcessor() },
            { TechType.SeaTruckFabricatorModule     , new DynamicEntities.SeaTruckFabricatorModuleProcessor() },
            { TechType.SeaTruckTeleportationModule  , new DynamicEntities.SeaTruckTeleportationModuleProcessor() },
            { TechType.SeaTruckAquariumModule       , new DynamicEntities.SeaTruckAquariumModuleProcessor() },
            { TechType.SeaTruckSleeperModule        , new DynamicEntities.SeaTruckSleeperModuleProcessor() },
            { TechType.SeaTruckDockingModule        , new DynamicEntities.SeaTruckDockingModuleProcessor() },

            /**
             *
             * Yumurta Senkronizasyonları
             *
             * @author Ismail <ismaiil_0234@hotmail.com>
             *
             */
            { TechType.GlowWhaleEgg, new DynamicEntities.BaseWaterParkProcessor() },
        };

        /**
         *
         * Oyuncu Eşya İşlemlerini barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static Dictionary<TechType, PlayerItemProcessor> PlayerItemProcessors { get; set; } = new Dictionary<TechType, PlayerItemProcessor>()
        {
            /**
             *
             * Normal Senkronizasyonlar
             *
             * @author Ismail <ismaiil_0234@hotmail.com>
             *
             */
            { TechType.Scanner            , new Items.ScannerProcessor() },
            { TechType.Knife              , new Items.KnifeProcessor() },
            { TechType.Constructor        , new Items.ConstructorProcessor() },
            { TechType.Flashlight         , new Items.FlashLightProcessor() },
            { TechType.AirBladder         , new Items.AirBladderProcessor() },
            { TechType.Hoverbike          , new Items.HoverbikeProcessor() },
            { TechType.SmallStorage       , new Items.DeployableStorageProcessor() },
            { TechType.QuantumLocker      , new Items.DeployableStorageProcessor() },
            { TechType.LEDLight           , new Items.LEDLightProcessor() },
            { TechType.Seaglide           , new Items.SeaglideProcessor() },
            { TechType.Thumper            , new Items.ThumperProcessor() },
            { TechType.Welder             , new Items.WelderProcessor() },
            { TechType.Flare              , new Items.FlareProcessor() },
            { TechType.DiveReel           , new Items.DiveReelProcessor() },
            { TechType.Beacon             , new Items.BeaconProcessor() },
            { TechType.TeleportationTool  , new Items.TeleportationToolProcessor() },
            { TechType.LaserCutter        , new Items.LaserCutterProcessor() },
            { TechType.SpyPenguin         , new Items.SpyPenguinProcessor() },
            { TechType.MetalDetector      , new Items.MetalDetectorProcessor() },
            { TechType.MapRoomCamera      , new Items.MapRoomCameraProcessor() },
            { TechType.PipeSurfaceFloater , new Items.PipeSurfaceFloaterProcessor() },
        };

        /**
         *
         * Normal işlemci döner.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static NormalProcessor GetNormalProcessor(ProcessType type)
        {
            ProcessorShared.Processors.TryGetValue(type, out var processor);
            return processor;
        }
    }
}
