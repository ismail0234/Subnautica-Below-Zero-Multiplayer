namespace Subnautica.Events.Handlers
{
    using Subnautica.Events.EventArgs;

    using static Subnautica.API.Extensions.EventExtensions;

    public class Player
    {
        /**
         *
         * Updated İşleyicisi
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static event SubnauticaPluginEventHandler<PlayerUpdatedEventArgs> Updated;

        /**
         *
         * Updated Olayı 
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void OnUpdated(PlayerUpdatedEventArgs ev) => Updated.CustomInvoke(ev);

        /**
         *
         * PlayerStatsUpdated İşleyicisi
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static event SubnauticaPluginEventHandler<PlayerStatsUpdatedEventArgs> StatsUpdated;

        /**
         *
         * PlayerStatsUpdated Olayı 
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void OnStatsUpdated(PlayerStatsUpdatedEventArgs ev) => StatsUpdated.CustomInvoke(ev);

        /**
         *
         * PlayerBaseEntered İşleyicisi
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static event SubnauticaPluginEventHandler<PlayerBaseEnteredEventArgs> PlayerBaseEntered;

        /**
         *
         * PlayerBaseEntered Olayı 
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void OnPlayerBaseEntered(PlayerBaseEnteredEventArgs ev) => PlayerBaseEntered.CustomInvoke(ev);

        /**
         *
         * PlayerBaseExited İşleyicisi
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static event SubnauticaPluginEventHandler<PlayerBaseExitedEventArgs> PlayerBaseExited;

        /**
         *
         * PlayerBaseExited Olayı 
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void OnPlayerBaseExited(PlayerBaseExitedEventArgs ev) => PlayerBaseExited.CustomInvoke(ev);

        /**
         *
         * ItemDrawed İşleyicisi
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static event SubnauticaPluginEventHandler<ItemDrawedEventArgs> ItemDrawed;

        /**
         *
         * ItemDrawed Olayı 
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void OnItemDrawed(ItemDrawedEventArgs ev) => ItemDrawed.CustomInvoke(ev);

        /**
         *
         * ItemActionStarted İşleyicisi
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static event SubnauticaPluginEventHandler<ItemActionStartedEventArgs> ItemActionStarted;

        /**
         *
         * ItemActionStarted Olayı 
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void OnItemActionStarted(ItemActionStartedEventArgs ev) => ItemActionStarted.CustomInvoke(ev);

        /**
         *
         * ItemFirstUseAnimationStoped İşleyicisi
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static event SubnauticaPluginEventHandler<ItemFirstUseAnimationStopedEventArgs> ItemFirstUseAnimationStoped;

        /**
         *
         * ItemFirstUseAnimationStoped Olayı 
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void OnItemFirstUseAnimationStoped(ItemFirstUseAnimationStopedEventArgs ev) => ItemFirstUseAnimationStoped.CustomInvoke(ev);

        /**
         *
         * EntityScannerCompleted İşleyicisi
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static event SubnauticaPluginEventHandler<EntityScannerCompletedEventArgs> EntityScannerCompleted;

        /**
         *
         * EntityScannerCompleted Olayı 
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void OnEntityScannerCompleted(EntityScannerCompletedEventArgs ev) => EntityScannerCompleted.CustomInvoke(ev);

        /**
         *
         * ItemPickedUp İşleyicisi
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static event SubnauticaPluginEventHandler<PlayerItemPickedUpEventArgs> ItemPickedUp;

        /**
         *
         * ItemPickedUp Olayı 
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void OnItemPickedUp(PlayerItemPickedUpEventArgs ev) => ItemPickedUp.CustomInvoke(ev);

        /**
         *
         * PlayerAnimationChanged İşleyicisi
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static event SubnauticaPluginEventHandler<PlayerAnimationChangedEventArgs> AnimationChanged;

        /**
         *
         * PlayerAnimationChanged Olayı 
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void OnAnimationChanged(PlayerAnimationChangedEventArgs ev) => AnimationChanged.CustomInvoke(ev);

        /**
         *
         * PlayerItemDroping İşleyicisi
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static event SubnauticaPluginEventHandler<PlayerItemDropingEventArgs> ItemDroping;

        /**
         *
         * PlayerItemDroping Olayı 
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void OnItemDroping(PlayerItemDropingEventArgs ev) => ItemDroping.CustomInvoke(ev);

        /**
         *
         * SleepScreenStopingStarted İşleyicisi
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static event SubnauticaPluginEventHandler SleepScreenStopingStarted;

        /**
         *
         * SleepScreenStopingStarted Olayı 
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void OnSleepScreenStopingStarted() => SleepScreenStopingStarted.CustomInvoke();

        /**
         *
         * SleepScreenStartingCompleted İşleyicisi
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static event SubnauticaPluginEventHandler SleepScreenStartingCompleted;

        /**
         *
         * SleepScreenStartingCompleted Olayı 
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void OnSleepScreenStartingCompleted() => SleepScreenStartingCompleted.CustomInvoke();

        /**
         *
         * UseableDiveHatchClicking İşleyicisi
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static event SubnauticaPluginEventHandler<UseableDiveHatchClickingEventArgs> UseableDiveHatchClicking;

        /**
         *
         * UseableDiveHatchClicking Olayı 
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void OnUseableDiveHatchClicking(UseableDiveHatchClickingEventArgs ev) => UseableDiveHatchClicking.CustomInvoke(ev);

        /**
         *
         * EnteredInterior İşleyicisi
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static event SubnauticaPluginEventHandler<PlayerEnteredInteriorEventArgs> EnteredInterior;

        /**
         *
         * EnteredInterior Olayı 
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void OnEnteredInterior(PlayerEnteredInteriorEventArgs ev) => EnteredInterior.CustomInvoke(ev);

        /**
         *
         * ExitedInterior İşleyicisi
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static event SubnauticaPluginEventHandler<PlayerExitedInteriorEventArgs> ExitedInterior;

        /**
         *
         * ExitedInterior Olayı 
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void OnExitedInterior(PlayerExitedInteriorEventArgs ev) => ExitedInterior.CustomInvoke(ev);
        
        /**
         *
         * Climbing İşleyicisi
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static event SubnauticaPluginEventHandler<PlayerClimbingEventArgs> Climbing;

        /**
         *
         * Climbing Olayı 
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void OnClimbing(PlayerClimbingEventArgs ev) => Climbing.CustomInvoke(ev);
        
        /**
         *
         * Dead İşleyicisi
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static event SubnauticaPluginEventHandler<PlayerDeadEventArgs> Dead;

        /**
         *
         * Dead Olayı 
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void OnDead(PlayerDeadEventArgs ev) => Dead.CustomInvoke(ev);
        
        /**
         *
         * OnSpawned İşleyicisi
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static event SubnauticaPluginEventHandler Spawned;

        /**
         *
         * OnSpawned Olayı 
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void OnSpawned() => Spawned.CustomInvoke();
        
        /**
         *
         * EnergyMixinClicking İşleyicisi
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static event SubnauticaPluginEventHandler<EnergyMixinClickingEventArgs> EnergyMixinClicking;

        /**
         *
         * EnergyMixinClicking Olayı 
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void OnEnergyMixinClicking(EnergyMixinClickingEventArgs ev) => EnergyMixinClicking.CustomInvoke(ev);
        
        /**
         *
         * EnergyMixinSelecting İşleyicisi
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static event SubnauticaPluginEventHandler<EnergyMixinSelectingEventArgs> EnergyMixinSelecting;

        /**
         *
         * EnergyMixinSelecting Olayı 
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void OnEnergyMixinSelecting(EnergyMixinSelectingEventArgs ev) => EnergyMixinSelecting.CustomInvoke(ev);

        
        /**
         *
         * EnergyMixinClosed İşleyicisi
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static event SubnauticaPluginEventHandler<EnergyMixinClosedEventArgs> EnergyMixinClosed;

        /**
         *
         * EnergyMixinClosed Olayı 
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void OnEnergyMixinClosed(EnergyMixinClosedEventArgs ev) => EnergyMixinClosed.CustomInvoke(ev);

        /**
         *
         * BreakableResourceBreaking İşleyicisi
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static event SubnauticaPluginEventHandler<BreakableResourceBreakingEventArgs> BreakableResourceBreaking;

        /**
         *
         * BreakableResourceBreaking Olayı 
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void OnBreakableResourceBreaking(BreakableResourceBreakingEventArgs ev) => BreakableResourceBreaking.CustomInvoke(ev);

        /**
         *
         * PingVisibilityChanged İşleyicisi
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static event SubnauticaPluginEventHandler<PlayerPingVisibilityChangedEventArgs> PingVisibilityChanged;

        /**
         *
         * PingVisibilityChanged Olayı 
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void OnPingVisibilityChanged(PlayerPingVisibilityChangedEventArgs ev) => PingVisibilityChanged.CustomInvoke(ev);

        /**
         *
         * PingColorChanged İşleyicisi
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static event SubnauticaPluginEventHandler<PlayerPingColorChangedEventArgs> PingColorChanged;

        /**
         *
         * PingColorChanged Olayı 
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void OnPingColorChanged(PlayerPingColorChangedEventArgs ev) => PingColorChanged.CustomInvoke(ev);
        
        /**
         *
         * PrecursorTeleporterUsed İşleyicisi
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static event SubnauticaPluginEventHandler PrecursorTeleporterUsed;

        /**
         *
         * PrecursorTeleporterUsed Olayı 
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void OnPrecursorTeleporterUsed() => PrecursorTeleporterUsed.CustomInvoke();
        
        /**
         *
         * PrecursorTeleportationCompleted İşleyicisi
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static event SubnauticaPluginEventHandler PrecursorTeleportationCompleted;

        /**
         *
         * PrecursorTeleportationCompleted Olayı 
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void OnPrecursorTeleportationCompleted() => PrecursorTeleportationCompleted.CustomInvoke();
        
        /**
         *
         * ToolBatteryEnergyChanged İşleyicisi
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static event SubnauticaPluginEventHandler<ToolBatteryEnergyChangedEventArgs> ToolBatteryEnergyChanged;

        /**
         *
         * ToolBatteryEnergyChanged Olayı 
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void OnToolBatteryEnergyChanged(ToolBatteryEnergyChangedEventArgs ev) => ToolBatteryEnergyChanged.CustomInvoke(ev);
        
        /**
         *
         * PlayerUsingCommand İşleyicisi
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static event SubnauticaPluginEventHandler<PlayerUsingCommandEventArgs> PlayerUsingCommand;

        /**
         *
         * PlayerUsingCommand Olayı 
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void OnPlayerUsingCommand(PlayerUsingCommandEventArgs ev) => PlayerUsingCommand.CustomInvoke(ev);
        
        /**
         *
         * RespawnPointChanged İşleyicisi
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static event SubnauticaPluginEventHandler<PlayerRespawnPointChangedEventArgs> RespawnPointChanged;

        /**
         *
         * RespawnPointChanged Olayı 
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void OnRespawnPointChanged(PlayerRespawnPointChangedEventArgs ev) => RespawnPointChanged.CustomInvoke(ev);
        
        /**
         *
         * Freezed İşleyicisi
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static event SubnauticaPluginEventHandler<PlayerFreezedEventArgs> Freezed;

        /**
         *
         * Freezed Olayı 
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void OnFreezed(PlayerFreezedEventArgs ev) => Freezed.CustomInvoke(ev);
        
        /**
         *
         * Unfreezed İşleyicisi
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static event SubnauticaPluginEventHandler Unfreezed;

        /**
         *
         * Unfreezed Olayı 
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void OnUnfreezed() => Unfreezed.CustomInvoke();
    }
}
