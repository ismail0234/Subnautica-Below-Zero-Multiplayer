namespace Subnautica.Server.Abstracts
{
    using System.Collections.Generic;

    using Subnautica.API.Enums;
    using Subnautica.Server.Processors;
    using Subnautica.Server.Abstracts.Processors;

    using Building      = Subnautica.Server.Processors.Building;
    using Encyclopedia  = Subnautica.Server.Processors.Encyclopedia;
    using Inventory     = Subnautica.Server.Processors.Inventory;
    using PDA           = Subnautica.Server.Processors.PDA;
    using Player        = Subnautica.Server.Processors.Player;
    using Startup       = Subnautica.Server.Processors.Startup;
    using Technology    = Subnautica.Server.Processors.Technology;
    using Metadata      = Subnautica.Server.Processors.Metadata;
    using General       = Subnautica.Server.Processors.General;
    using WorldEntities = Subnautica.Server.Processors.WorldEntities;
    using Items         = Subnautica.Server.Processors.Items;
    using Vehicle       = Subnautica.Server.Processors.Vehicle;
    using Creatures     = Subnautica.Server.Processors.Creatures;
    using Story         = Subnautica.Server.Processors.Story;
    using World         = Subnautica.Server.Processors.World;

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
            { ProcessType.None                          , new NoneProcessor() },
            { ProcessType.JoiningServer                 , new Player.JoiningProcessor() },
            { ProcessType.PlayerUpdated                 , new Player.UpdatedProcessor() },
            { ProcessType.PlayerStats                   , new Player.StatsProcessor() },
            { ProcessType.SubrootToggle                 , new Player.SubrootToggleProcessor() },
            { ProcessType.InteriorToggle                , new Player.InteriorToggleProcessor() },
            { ProcessType.UseableDiveHatch              , new Player.UseableDiveHatchProcessor()},
            { ProcessType.PlayerClimb                   , new Player.ClimbProcessor()},
            { ProcessType.PlayerDead                    , new Player.DeadProcessor()},
            { ProcessType.PlayerSpawn                   , new Player.SpawnProcessor()},
            { ProcessType.PlayerToolEnergy              , new Player.ToolEnergyProcessor()},
            { ProcessType.PlayerConsoleCommand          , new Player.ConsoleCommandProcessor()},
            { ProcessType.PlayerRespawnPointChanged     , new Player.RespawnPointProcessor()},
            { ProcessType.PlayerInitialEquipment        , new Player.FirstInitialEquipmentProcessor()},
            { ProcessType.Ping                          , new Player.PingProcessor()},
            { ProcessType.PlayerFreeze                  , new Player.FreezeProcessor()},
            { ProcessType.PlayerAnimationChanged        , new Player.AnimationChangedProcessor()},
            { ProcessType.ItemDrop                      , new Player.ItemDropProcessor()},
            { ProcessType.ItemPickup                    , new Player.ItemPickupProcessor()},
            { ProcessType.EncyclopediaAdded             , new Encyclopedia.AddedProcessor() },
            { ProcessType.TechnologyAdded               , new Technology.AddedProcessor() },
            { ProcessType.TechnologyFragmentAdded       , new Technology.FragmentAddedProcessor() },
            { ProcessType.ScannerCompleted              , new Technology.ScannerCompletedProcessor() },
            { ProcessType.InventoryItem                 , new Inventory.ItemProcessor() },
            { ProcessType.InventoryEquipment            , new Inventory.EquipmentProcessor() },
            { ProcessType.InventoryQuickSlot            , new Inventory.QuickSlotProcessor() },
            { ProcessType.ItemPin                       , new Inventory.ItemPinProcessor() },
            { ProcessType.ConstructingGhostMoving       , new Building.GhostMovedProcessor() },
            { ProcessType.ConstructingRemoved           , new Building.RemovedProcessor() },
            { ProcessType.ConstructingAmountChanged     , new Building.AmountChangedProcessor() },
            { ProcessType.ConstructingGhostTryPlacing   , new Building.GhostTryPlacingProcessor() },
            { ProcessType.ConstructingCompleted         , new Building.CompletedProcessor() },
            { ProcessType.DeconstructionBegin           , new Building.DeconstructionBeginProcessor() },
            { ProcessType.FurnitureDeconstructionBegin  , new Building.FurnitureDeconstructionBeginProcessor() },
            { ProcessType.MetadataRequest               , new Building.MetadataRequestProcessor() },
            { ProcessType.BaseHullStrength              , new Building.BaseHullStrengthProcessor() },
            { ProcessType.ConstructionHealth            , new Building.HealthProcessor() },
            { ProcessType.NotificationAdded             , new PDA.NotificationProcessor() },
            { ProcessType.TechAnalyzeAdded              , new PDA.TechAnalyzeAddedProcessor() },
            { ProcessType.JukeboxDiskAdded              , new PDA.JukeboxDiskAddedProcessor() },
            { ProcessType.WorldLoaded                   , new Startup.WorlLoadedProcessor() },
            { ProcessType.Interact                      , new General.InteractProcessor() },
            { ProcessType.FirstTimeStartServer          , new General.IntroProcessor()},
            { ProcessType.LifepodProcess                , new General.LifepodProcessor()},
            { ProcessType.StorageOpen                   , new General.StorageOpenProcessor()},
            { ProcessType.ResourceDiscover              , new General.ResourceDiscoverProcessor()},
            { ProcessType.EntityScannerCompleted        , new World.EntityScannerProcessor() },
            { ProcessType.StaticEntityPickedUp          , new World.StaticEntityProcessor()},
            { ProcessType.WorldEntityAction             , new World.WorldEntityActionProcessor()},
            { ProcessType.WorldDynamicEntityPosition    , new World.WorldDynamicEntityPositionProcessor()},
            { ProcessType.WorldCreaturePosition         , new World.WorldCreaturePositionProcessor()},
            { ProcessType.EntitySlotProcess             , new World.EntitySlotSpawnProcessor()},
            { ProcessType.SpawnOnKill                   , new World.SpawnOnKillProcessor()},
            { ProcessType.WeatherChanged                , new World.WeatherProcessor()},        
            { ProcessType.PrecursorTeleporter           , new World.PrecursorTeleporterProcessor()},        
            { ProcessType.WelderRepair                  , new World.WelderProcessor()},
            { ProcessType.Brinicle                      , new World.BrinicleProcessor()},
            { ProcessType.CosmeticItem                  , new World.CosmeticItemProcessor()},
            { ProcessType.CreatureProcess               , new World.WorldCreatureActionProcessor()},
            { ProcessType.PlayerItemAction              , new Items.PlayerItemActionProcessor()},
            { ProcessType.VehicleUpgradeConsole         , new Vehicle.UpgradeConsoleProcessor()},
            { ProcessType.VehicleEnter                  , new Vehicle.EnterProcessor()},
            { ProcessType.VehicleExit                   , new Vehicle.ExitProcessor()},
            { ProcessType.VehicleUpdated                , new Vehicle.UpdatedProcessor()},
            { ProcessType.VehicleBattery                , new Vehicle.BatteryProcessor()},
            { ProcessType.ExosuitStorage                , new Vehicle.ExosuitStorageProcessor()},
            { ProcessType.ExosuitDrill                  , new Vehicle.ExosuitDrillProcessor()},        
            { ProcessType.VehicleLight                  , new Vehicle.LightProcessor()},
            { ProcessType.VehicleInterior               , new Vehicle.InteriorProcessor()},
            { ProcessType.VehicleHealth                 , new Vehicle.HealthProcessor()},
            { ProcessType.SeaTruckConnection            , new Vehicle.SeaTruckConnectionProcessor()},
            { ProcessType.SeaTruckStorageModule         , new Vehicle.SeaTruckStorageModuleProcessor()},
            { ProcessType.SeaTruckAquariumModule        , new Vehicle.SeaTruckAquariumModuleProcessor()},
            { ProcessType.SeaTruckFabricatorModule      , new Vehicle.SeaTruckFabricatorModuleProcessor()},
            { ProcessType.SeaTruckSleeperModule         , new Vehicle.SeaTruckSleeperModuleProcessor()},
            { ProcessType.SeaTruckDockingModule         , new Vehicle.SeaTruckDockingModuleProcessor()},
            { ProcessType.ExosuitJump                   , new Vehicle.ExosuitJumpProcessor()},            
            { ProcessType.CreatureHealth                , new Creatures.HealthProcessor()},
            { ProcessType.CreatureAnimation             , new Creatures.AnimationChangedProcessor()},  
            { ProcessType.CreatureFreeze                , new Creatures.FreezeProcessor()},   
            { ProcessType.CreatureCallSound             , new Creatures.CallSoundProcessor()},    
            { ProcessType.CreatureAttackLastTarget      , new Creatures.AttackLastTargetProcessor()},
            { ProcessType.CreatureLeviathanMeleeAttack  , new Creatures.LeviathanMeleeAttackProcessor()},
            { ProcessType.CreatureMeleeAttack           , new Creatures.MeleeAttackProcessor()},
            { ProcessType.StoryBridge                   , new Story.BridgeProcessor()},
            { ProcessType.StoryRadioTower               , new Story.RadioTowerProcessor()},
            { ProcessType.StorySignal                   , new Story.SignalProcessor()},
            { ProcessType.StoryTrigger                  , new Story.TriggerProcessor()},
            { ProcessType.StoryCinematicTrigger         , new Story.CinematicProcessor()},
            { ProcessType.StoryCall                     , new Story.CallProcessor()},
            { ProcessType.StoryInteract                 , new Story.InteractProcessor()},
            { ProcessType.StoryPlayerVisibility         , new Story.PlayerVisibilityProcessor()},
            { ProcessType.StoryFrozenCreature           , new Story.FrozenCreatureProcessor()},
            { ProcessType.StoryShieldBase               , new Story.ShieldBaseProcessor()},
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
            { TechType.AromatherapyLamp      , new Metadata.AromatherapyLampProcessor() },
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
            { TechType.Locker                , new Metadata.StorageProcessor() },
            { TechType.SmallLocker           , new Metadata.StorageProcessor() },
            { TechType.BaseBioReactor        , new Metadata.BioReactorProcessor() },
            { TechType.BaseNuclearReactor    , new Metadata.NuclearReactorProcessor() },
            { TechType.Trashcans             , new Metadata.TrashcansProcessor() },
            { TechType.LabTrashcan           , new Metadata.LabTrashcanProcessor() },
            { TechType.BatteryCharger        , new Metadata.ChargerProcessor() },
            { TechType.PowerCellCharger      , new Metadata.ChargerProcessor() },
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
            { TechType.GlowWhale  , new Creatures.GlowWhaleProcessor() },
            { TechType.Crash      , new Creatures.CrashFishProcessor() },
            { TechType.LilyPaddler, new Creatures.LilyPaddlerProcessor() },
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
            { TechType.Hoverbike          , new Items.HoverbikeProcessor() },
            { TechType.SmallStorage       , new Items.DeployableStorageProcessor() },
            { TechType.QuantumLocker      , new Items.DeployableStorageProcessor() },
            { TechType.LEDLight           , new Items.LEDLightProcessor() },
            { TechType.Thumper            , new Items.ThumperProcessor() },
            { TechType.Flare              , new Items.FlareProcessor() },
            { TechType.DiveReel           , new Items.DiveReelProcessor() },
            { TechType.Beacon             , new Items.BeaconProcessor() },
            { TechType.TeleportationTool  , new Items.TeleportationToolProcessor() },
            { TechType.SpyPenguin         , new Items.SpyPenguinProcessor() },
            { TechType.MapRoomCamera      , new Items.MapRoomCameraProcessor() },
            { TechType.PipeSurfaceFloater , new Items.PipeSurfaceFloaterProcessor() },
        };
    }
}