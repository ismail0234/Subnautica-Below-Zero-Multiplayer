namespace Subnautica.Client
{
    using HarmonyLib;

    using Subnautica.API.Extensions;
    using Subnautica.API.Features;

    using System;
    using System.Diagnostics;

    using Handlers = Subnautica.Events.Handlers;
    
    public class Main : SubnauticaPlugin
    {

        /**
         *
         * Eklenti Adı
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public override string Name { get; } = "BOT Benson Client";

        /**
         *
         * Router
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public Router Router { get; set; }

        /**
         *
         * Eklenti Aktifleştiğinde tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public override void OnEnabled()
        {
            base.OnEnabled();

            UnityEngine.Debug.Log("Subnautica.Client -> Loading");

            this.CheckBepinex();

            this.Router = new Router();
            this.Router.OnPluginEnabled();
         
            Handlers.Game.InGameMenuClosed                   += this.Router.OnInGameMenuClosed;
            Handlers.Game.InGameMenuOpened                   += this.Router.OnInGameMenuOpened;
            Handlers.Game.SceneLoaded                        += this.Router.OnSceneLoaded;
            Handlers.Game.MenuSaveCancelDeleteButtonClicking += this.Router.OnMenuSaveCancelDeleteButtonClicking;
            Handlers.Game.MenuSaveLoadButtonClicking         += this.Router.OnMenuSaveLoadButtonClicking;
            Handlers.Game.MenuSaveDeleteButtonClicking       += this.Router.OnMenuSaveDeleteButtonClicking;
            Handlers.Game.MenuSaveUpdateLoadedButtonState    += this.Router.OnMenuSaveUpdateLoadedButtonState;
            Handlers.Game.SettingsRunInBackgroundChanging    += this.Router.OnSettingsRunInBackgroundChanging;
            Handlers.Game.SettingsPdaGamePauseChanging       += this.Router.OnSettingsPdaGamePauseChanging;
            Handlers.Game.Quitting                           += this.Router.OnQuitting;
            Handlers.Game.QuittingToMainMenu                 += this.Router.OnQuittingToMainMenu;
            Handlers.Game.WorldLoaded                        += this.Router.OnWorldLoaded;
            Handlers.Game.WorldLoading                       += this.Router.OnWorldLoading;
            Handlers.Game.IntroChecking                      += this.Router.OnIntroChecking;
            Handlers.Game.LifepodZoneSelecting               += this.Router.OnLifepodZoneSelecting;
            Handlers.Game.LifepodZoneCheck                   += this.Router.OnLifepodZoneCheck;
            Handlers.Game.LifepodInterpolation               += this.Router.OnLifepodInterpolation;
            Handlers.Game.SubNameInputDeselected             += this.Router.OnSubNameInputDeselected;
            Handlers.Game.SubNameInputSelecting              += this.Router.OnSubNameInputSelecting;

            Handlers.PDA.EncyclopediaAdded                   += this.Router.OnEncyclopediaAdded;
            Handlers.PDA.TechnologyAdded                     += this.Router.OnTechnologyAdded;
            Handlers.PDA.TechnologyFragmentAdded             += this.Router.OnTechnologyFragmentAdded;
            Handlers.PDA.ScannerCompleted                    += this.Router.OnScannerCompleted;
            Handlers.PDA.ItemPinAdded                        += this.Router.OnItemPinAdded;
            Handlers.PDA.ItemPinRemoved                      += this.Router.OnItemPinRemoved;
            Handlers.PDA.ItemPinMoved                        += this.Router.OnItemPinMoved;
            Handlers.PDA.LogAdded                            += this.Router.OnPDALogAdded;
            Handlers.PDA.NotificationToggle                  += this.Router.OnNotificationToggle;
            Handlers.PDA.TechAnalyzeAdded                    += this.Router.OnTechAnalyzeAdded;
            Handlers.PDA.JukeboxDiskAdded                    += this.Router.OnJukeboxDiskAdded;
            Handlers.PDA.Closing                             += this.Router.OnClosing;

            Handlers.Storage.Opening                         += this.Router.OnStorageOpening;
            Handlers.Storage.ItemAdding                      += this.Router.OnStorageItemAdding;
            Handlers.Storage.ItemRemoving                    += this.Router.OnStorageItemRemoving;
            Handlers.Storage.NuclearReactorItemAdded         += this.Router.OnNuclearReactorItemAdded;
            Handlers.Storage.NuclearReactorItemRemoved       += this.Router.OnNuclearReactorItemRemoved;
            Handlers.Storage.ChargerItemAdded                += this.Router.OnChargerItemAdded;
            Handlers.Storage.ChargerItemRemoved              += this.Router.OnChargerItemRemoved;

            Handlers.Building.ConstructingGhostMoved       += this.Router.OnConstructingGhostMoved;
            Handlers.Building.ConstructingGhostTryPlacing  += this.Router.OnConstructingGhostTryPlacing;
            Handlers.Building.ConstructingAmountChanged    += this.Router.OnConstructingAmountChanged;
            Handlers.Building.ConstructingCompleted        += this.Router.OnConstructingCompleted;
            Handlers.Building.ConstructingRemoved          += this.Router.OnConstructingRemoved;
            Handlers.Building.DeconstructionBegin          += this.Router.OnDeconstructionBegin;
            Handlers.Building.FurnitureDeconstructionBegin += this.Router.OnFurnitureDeconstructionBegin;
            Handlers.Building.BaseHullStrengthCrushing     += this.Router.OnBaseHullStrengthCrushing;

            Handlers.Furnitures.ToiletSwitchToggle               += this.Router.OnToiletSwitchToggle;
            Handlers.Furnitures.EmmanuelPendulumSwitchToggle     += this.Router.OnEmmanuelPendulumSwitchToggle;
            Handlers.Furnitures.AromatherapyLampSwitchToggle     += this.Router.OnAromatherapyLampSwitchToggle;
            Handlers.Furnitures.SmallStoveSwitchToggle           += this.Router.OnSmallStoveSwitchToggle;
            Handlers.Furnitures.SinkSwitchToggle                 += this.Router.OnSinkSwitchToggle;
            Handlers.Furnitures.ShowerSwitchToggle               += this.Router.OnShowerSwitchToggle;
            Handlers.Furnitures.SnowmanDestroying                += this.Router.OnSnowmanDestroying;
            Handlers.Furnitures.SignDataChanged                  += this.Router.OnSignDataChanged;
            Handlers.Furnitures.PictureFrameImageSelecting       += this.Router.OnPictureFrameImageSelecting;
            Handlers.Furnitures.BedIsCanSleepChecking            += this.Router.OnBedIsCanSleepChecking;
            Handlers.Furnitures.BedEnterInUseMode                += this.Router.OnBedEnterInUseMode;
            Handlers.Furnitures.BedExitInUseMode                 += this.Router.OnBedExitInUseMode;
            Handlers.Furnitures.JukeboxUsed                      += this.Router.OnJukeboxUsed;
            Handlers.Furnitures.CrafterItemPickup                += this.Router.OnCrafterItemPickup;
            Handlers.Furnitures.CrafterOpening                   += this.Router.OnCrafterOpening;
            Handlers.Furnitures.CrafterClosed                    += this.Router.OnCrafterClosed;
            Handlers.Furnitures.CrafterBegin                     += this.Router.OnCrafterBegin;
            Handlers.Furnitures.CrafterEnded                     += this.Router.OnCrafterEnded;
            Handlers.Furnitures.BenchSitdown                     += this.Router.OnBenchSitdown;
            Handlers.Furnitures.BenchStandup                     += this.Router.OnBenchStandup;
            Handlers.Furnitures.SignSelect                       += this.Router.OnSignSelect;
            Handlers.Furnitures.SignDeselect                     += this.Router.OnSignDeselect;
            Handlers.Furnitures.PictureFrameOpening              += this.Router.OnPictureFrameOpening;
            Handlers.Furnitures.ChargerOpening                   += this.Router.OnChargerOpening;
            Handlers.Furnitures.RecyclotronRecycle               += this.Router.OnRecyclotronRecycle;
            Handlers.Furnitures.HoverpadHoverbikeSpawning        += this.Router.OnHoverpadHoverbikeSpawning;
            Handlers.Furnitures.PlanterItemAdded                 += this.Router.OnPlanterItemAdded;
            Handlers.Furnitures.PlanterProgressCompleted         += this.Router.OnPlanterProgressCompleted;
            Handlers.Furnitures.PlanterGrowned                   += this.Router.OnPlanterGrowned;
            Handlers.Furnitures.BulkheadOpening                  += this.Router.OnBulkheadOpening;
            Handlers.Furnitures.BulkheadClosing                  += this.Router.OnBulkheadClosing;
            Handlers.Furnitures.BaseControlRoomMinimapUsing      += this.Router.OnBaseControlRoomMinimapUsing;
            Handlers.Furnitures.BaseControlRoomMinimapExiting    += this.Router.OnBaseControlRoomMinimapExiting;
            Handlers.Furnitures.BaseControlRoomCellPowerChanging += this.Router.OnBaseControlRoomCellPowerChanging;
            Handlers.Furnitures.BaseControlRoomMinimapMoving     += this.Router.OnBaseControlRoomMinimapMoving;
            Handlers.Furnitures.HoverpadDocking                  += this.Router.OnHoverpadDocking;
            Handlers.Furnitures.HoverpadUnDocking                += this.Router.OnHoverpadUnDocking;
            Handlers.Furnitures.HoverpadShowroomTriggering       += this.Router.OnHoverpadShowroomTriggering;
            Handlers.Furnitures.SpotLightInitialized             += this.Router.OnSpotLightInitialized;
            Handlers.Furnitures.TechLightInitialized             += this.Router.OnTechLightInitialized;
            Handlers.Furnitures.BaseMapRoomScanStarting          += this.Router.OnBaseMapRoomScanStarting;
            Handlers.Furnitures.BaseMapRoomScanStopping          += this.Router.OnBaseMapRoomScanStopping;
            Handlers.Furnitures.BaseMapRoomCameraChanging        += this.Router.OnBaseMapRoomCameraChanging;
            Handlers.Furnitures.BaseMapRoomResourceDiscovering   += this.Router.OnBaseMapRoomResourceDiscovering;
            Handlers.Furnitures.BaseMapRoomInitialized           += this.Router.OnBaseMapRoomInitialized;
            Handlers.Furnitures.BaseMoonpoolExpansionUndockingTimelineCompleting += this.Router.OnBaseMoonpoolExpansionUndockingTimelineCompleting;
            Handlers.Furnitures.BaseMoonpoolExpansionDockingTimelineCompleting   += this.Router.OnBaseMoonpoolExpansionDockingTimelineCompleting;
            Handlers.Furnitures.BaseMoonpoolExpansionDockTail                    += this.Router.OnBaseMoonpoolExpansionDockTail;
            Handlers.Furnitures.BaseMoonpoolExpansionUndockTail                  += this.Router.OnBaseMoonpoolExpansionUndockTail;

            Handlers.Player.Updated                         += this.Router.OnPlayerUpdated;
            Handlers.Player.StatsUpdated                    += this.Router.OnPlayerStatsUpdated;
            Handlers.Player.PlayerBaseEntered               += this.Router.OnPlayerBaseEntered;
            Handlers.Player.PlayerBaseExited                += this.Router.OnPlayerBaseExited;
            Handlers.Player.EntityScannerCompleted          += this.Router.OnEntityScannerCompleted;
            Handlers.Player.ItemPickedUp                    += this.Router.OnPlayerItemPickedUp;
            Handlers.Player.AnimationChanged                += this.Router.OnPlayerAnimationChanged;
            Handlers.Player.ItemDroping                     += this.Router.OnPlayerItemDroping;
            Handlers.Player.SleepScreenStartingCompleted    += this.Router.OnSleepScreenStartingCompleted;
            Handlers.Player.SleepScreenStopingStarted       += this.Router.OnSleepScreenStopingStarted;
            Handlers.Player.UseableDiveHatchClicking        += this.Router.OnUseableDiveHatchClicking;
            Handlers.Player.EnteredInterior                 += this.Router.OnEnteredInterior;
            Handlers.Player.ExitedInterior                  += this.Router.OnExitedInterior;
            Handlers.Player.Climbing                        += this.Router.OnPlayerClimbing;
            Handlers.Player.Dead                            += this.Router.OnPlayerDead;
            Handlers.Player.Spawned                         += this.Router.OnPlayerSpawned;
            Handlers.Player.EnergyMixinSelecting            += this.Router.OnEnergyMixinSelecting;
            Handlers.Player.EnergyMixinClicking             += this.Router.OnEnergyMixinClicking;
            Handlers.Player.EnergyMixinClosed               += this.Router.OnEnergyMixinClosed;
            Handlers.Player.BreakableResourceBreaking       += this.Router.OnBreakableResourceBreaking;
            Handlers.Player.PingVisibilityChanged           += this.Router.OnPingVisibilityChanged;
            Handlers.Player.PingColorChanged                += this.Router.OnPingColorChanged;
            Handlers.Player.PrecursorTeleporterUsed         += this.Router.OnPrecursorTeleporterUsed;
            Handlers.Player.PrecursorTeleportationCompleted += this.Router.OnPrecursorTeleportationCompleted;
            Handlers.Player.ToolBatteryEnergyChanged        += this.Router.OnToolBatteryEnergyChanged;
            Handlers.Player.PlayerUsingCommand              += this.Router.OnUsingCommand;
            Handlers.Player.RespawnPointChanged             += this.Router.OnPlayerRespawnPointChanged;
            Handlers.Player.Freezed                         += this.Router.OnPlayerFreezed;
            Handlers.Player.Unfreezed                       += this.Router.OnPlayerUnfreezed;

            Handlers.Inventory.ItemRemoved              += this.Router.OnInventoryItemRemoved;
            Handlers.Inventory.ItemAdded                += this.Router.OnInventoryItemAdded;
            Handlers.Inventory.EquipmentEquiped         += this.Router.OnEquipmentEquiped;
            Handlers.Inventory.EquipmentUnequiped       += this.Router.OnEquipmentUnequiped;
            Handlers.Inventory.QuickSlotBinded          += this.Router.OnQuickSlotBinded;
            Handlers.Inventory.QuickSlotUnbinded        += this.Router.OnQuickSlotUnbinded;
            Handlers.Inventory.QuickSlotActiveChanged   += this.Router.OnQuickSlotActiveChanged;

            Handlers.World.ThermalLilyRangeChecking           += this.Router.OnThermalLilyRangeChecking;
            Handlers.World.ThermalLilyAnimationAnglesChecking += this.Router.OnThermalLilyAnimationAnglesChecking;
            Handlers.World.OxygenPlantClicking                += this.Router.OnOxygenPlantClicking;
            Handlers.World.EntitySpawning                     += this.Router.OnEntitySpawning;
            Handlers.World.EntitySlotSpawning                 += this.Router.OnEntitySlotSpawning;
            Handlers.World.EntitySpawned                      += this.Router.OnEntitySpawned;
            Handlers.World.AlterraPdaPickedUp                 += this.Router.OnAlterraPdaPickedUp;
            Handlers.World.JukeboxDiskPickedUp                += this.Router.OnJukeboxDiskPickedUp;
            Handlers.World.SupplyCrateOpened                  += this.Router.OnSupplyCrateOpened;
            Handlers.World.DataboxItemPickedUp                += this.Router.OnDataboxItemPickedUp;
            Handlers.World.TakeDamaging                       += this.Router.OnTakeDamaging;
            Handlers.World.FruitHarvesting                    += this.Router.OnFruitHarvesting;
            Handlers.World.GrownPlantHarvesting               += this.Router.OnGrownPlantHarvesting;
            Handlers.World.CellLoading                        += this.Router.OnCellLoading;
            Handlers.World.CellUnLoading                      += this.Router.OnCellUnLoading;
            Handlers.World.LaserCutterUsing                   += this.Router.OnLaserCutterUsing;
            Handlers.World.SealedInitialized                  += this.Router.OnSealedInitialized;
            Handlers.World.ElevatorCalling                    += this.Router.OnElevatorCalling;
            Handlers.World.SpawnOnKilling                     += this.Router.OnSpawnOnKilling;
            Handlers.World.WeatherProfileChanged              += this.Router.OnWeatherProfileChanged;
            Handlers.World.TeleporterTerminalActivating       += this.Router.OnTeleporterTerminalActivating;
            Handlers.World.TeleporterInitialized              += this.Router.OnTeleporterInitialized;
            Handlers.World.ElevatorInitialized                += this.Router.OnElevatorInitialized;
            Handlers.World.CrushDamaging                      += this.Router.OnCrushDamaging;
            Handlers.World.CosmeticItemPlacing                += this.Router.OnCosmeticItemPlacing;

            Handlers.Items.KnifeUsing                       += this.Router.OnKnifeUsing;
            Handlers.Items.ScannerUsing                     += this.Router.OnScannerUsing;
            Handlers.Items.ConstructorDeploying             += this.Router.OnConstructorDeploying;
            Handlers.Items.ConstructorEngageToggle          += this.Router.OnConstructorEngageToggle;
            Handlers.Items.ConstructorCrafting              += this.Router.OnConstructorCrafting;
            Handlers.Items.HoverbikeDeploying               += this.Router.OnHoverbikeDeploying;
            Handlers.Items.DeployableStorageDeploying       += this.Router.OnDeployableStorageDeploying;
            Handlers.Items.LEDLightDeploying                += this.Router.OnLEDLightDeploying;
            Handlers.Items.BeaconDeploying                  += this.Router.OnBeaconDeploying;
            Handlers.Items.FlareDeploying                   += this.Router.OnFlareDeploying;
            Handlers.Items.ThumperDeploying                 += this.Router.OnThumperDeploying;
            Handlers.Items.TeleportationToolUsed            += this.Router.OnTeleportationToolUsed;
            Handlers.Items.BeaconLabelChanged               += this.Router.OnBeaconLabelChanged;
            Handlers.Items.SpyPenguinDeploying              += this.Router.OnSpyPenguinDeploying;
            Handlers.Items.SpyPenguinItemPickedUp           += this.Router.OnSpyPenguinItemPickedUp;
            Handlers.Items.SpyPenguinSnowStalkerInteracting += this.Router.OnSpyPenguinSnowStalkerInteracting;
            Handlers.Items.SpyPenguinItemGrabing            += this.Router.OnSpyPenguinItemGrabing;
            Handlers.Items.Welding                          += this.Router.OnWelding;
            Handlers.Items.DroneCameraDeploying             += this.Router.OnDroneCameraDeploying;
            Handlers.Items.PipeSurfaceFloaterDeploying      += this.Router.OnPipeSurfaceFloaterDeploying;
            Handlers.Items.OxygenPipePlacing                += this.Router.OnOxygenPipePlacing;

            Handlers.Vehicle.UpgradeConsoleOpening              += this.Router.OnUpgradeConsoleOpening;
            Handlers.Vehicle.UpgradeConsoleModuleAdded          += this.Router.OnUpgradeConsoleModuleAdded;
            Handlers.Vehicle.UpgradeConsoleModuleRemoved        += this.Router.OnUpgradeConsoleModuleRemoved;
            Handlers.Vehicle.Entering                           += this.Router.OnVehicleEntering;
            Handlers.Vehicle.Exited                             += this.Router.OnVehicleExited;
            Handlers.Vehicle.Updated                            += this.Router.OnVehicleUpdated;
            Handlers.Vehicle.LightChanged                       += this.Router.OnVehicleLightChanged;
            Handlers.Vehicle.InteriorToggle                     += this.Router.OnVehicleInteriorToggle;
            Handlers.Vehicle.SeaTruckConnecting                 += this.Router.OnSeaTruckConnecting;
            Handlers.Vehicle.ExosuitJumping                     += this.Router.OnExosuitJumping;
            Handlers.Vehicle.SeaTruckDetaching                  += this.Router.OnSeaTruckDetaching;
            Handlers.Vehicle.ExosuitItemPickedUp                += this.Router.OnExosuitItemPickedUp;
            Handlers.Vehicle.ExosuitDrilling                    += this.Router.OnExosuitDrilling;
            Handlers.Vehicle.Docking                            += this.Router.OnVehicleDocking;
            Handlers.Vehicle.Undocking                          += this.Router.OnVehicleUndocking;
            Handlers.Vehicle.SeaTruckPictureFrameOpening        += this.Router.OnSeaTruckPictureFrameOpening;
            Handlers.Vehicle.SeaTruckPictureFrameImageSelecting += this.Router.OnSeaTruckPictureFrameImageSelecting;
            Handlers.Vehicle.MapRoomCameraDocking               += this.Router.OnMapRoomCameraDocking;
            Handlers.Vehicle.SeaTruckModuleInitialized          += this.Router.OnSeaTruckModuleInitialized;

            Handlers.Story.BridgeFluidClicking                += this.Router.OnBridgeFluidClicking;
            Handlers.Story.BridgeTerminalClicking             += this.Router.OnBridgeTerminalClicking;
            Handlers.Story.BridgeInitialized                  += this.Router.OnBridgeInitialized;
            Handlers.Story.StoryGoalTriggering                += this.Router.OnStoryGoalTriggering;
            Handlers.Story.StorySignalSpawning                += this.Router.OnStorySignalSpawning;
            Handlers.Story.CinematicTriggering                += this.Router.OnCinematicTriggering;
            Handlers.Story.RadioTowerTOMUsing                 += this.Router.OnRadioTowerTOMUsing;
            Handlers.Story.StoryCalling                       += this.Router.OnStoryCalling;
            Handlers.Story.StoryHandClicking                  += this.Router.OnStoryHandClicking;
            Handlers.Story.StoryCinematicStarted              += this.Router.OnStoryCinematicStarted;
            Handlers.Story.StoryCinematicCompleted            += this.Router.OnStoryCinematicCompleted;
            Handlers.Story.MobileExtractorMachineInitialized  += this.Router.OnMobileExtractorMachineInitialized;
            Handlers.Story.MobileExtractorMachineSampleAdding += this.Router.OnMobileExtractorMachineSampleAdding;
            Handlers.Story.MobileExtractorConsoleUsing        += this.Router.OnMobileExtractorConsoleUsing;
            Handlers.Story.ShieldBaseEnterTriggering          += this.Router.OnShieldBaseEnterTriggering;

            Handlers.Creatures.AnimationChanged                 += this.Router.OnCreatureAnimationChanged;
            Handlers.Creatures.GlowWhaleRideStarting            += this.Router.OnGlowWhaleRideStarting;
            Handlers.Creatures.GlowWhaleRideStoped              += this.Router.OnGlowWhaleRideStoped;
            Handlers.Creatures.GlowWhaleEyeCinematicStarting    += this.Router.OnGlowWhaleEyeCinematicStarting;
            Handlers.Creatures.GlowWhaleSFXTriggered            += this.Router.OnGlowWhaleSFXTriggered;
            Handlers.Creatures.CrashFishInflating               += this.Router.OnCrashFishInflating;
            Handlers.Creatures.LilyPaddlerHypnotizeStarting     += this.Router.OnLilyPaddlerHypnotizeStarting;
            Handlers.Creatures.Freezing                         += this.Router.OnFreezing;
            Handlers.Creatures.CallSoundTriggering              += this.Router.OnCallSoundTriggering;
            Handlers.Creatures.CreatureAttackLastTargetStarting += this.Router.OnCreatureAttackLastTargetStarting;
            Handlers.Creatures.CreatureAttackLastTargetStopped  += this.Router.OnCreatureAttackLastTargetStopped;
            Handlers.Creatures.LeviathanMeleeAttacking          += this.Router.OnLeviathanMeleeAttacking;
            Handlers.Creatures.MeleeAttacking                   += this.Router.OnMeleeAttacking;
            Handlers.Creatures.Enabled                          += this.Router.OnCreatureEnabled;
            Handlers.Creatures.Disabled                         += this.Router.OnCreatureDisabled;

            try
            {
                var harmony = new Harmony("Subnautica.Client.Main");
                harmony.PatchAll();
            }
            catch (Exception e)
            {
                Log.Error($"Harmony - Patching failed! {e}");
            }
        }

        /**
         *
         * Bepinex kontrolü yapar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private void CheckBepinex()
        {
            Settings.IsBepinexInstalled = Tools.IsBepinexInstalled();
        }

        /**
         *
         * Ana Menüye Döner.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void ReturnToMainMenu()
        {
            ZeroGame.QuitToMainMenu();
        }
    }
}