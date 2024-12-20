namespace Subnautica.Events.Handlers
{
    using Subnautica.Events.EventArgs;

    using static Subnautica.API.Extensions.EventExtensions;

    public class Vehicle
    {
        /**
         *
         * UpgradeConsoleOpening İşleyicisi
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static event SubnauticaPluginEventHandler<UpgradeConsoleOpeningEventArgs> UpgradeConsoleOpening;

        /**
         *
         * UpgradeConsoleOpening Olayı 
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void OnUpgradeConsoleOpening(UpgradeConsoleOpeningEventArgs ev) => UpgradeConsoleOpening.CustomInvoke(ev);

        /**
         *
         * UpgradeConsoleModuleAdded İşleyicisi
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static event SubnauticaPluginEventHandler<UpgradeConsoleModuleAddedEventArgs> UpgradeConsoleModuleAdded;

        /**
         *
         * UpgradeConsoleModuleAdded Olayı 
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void OnUpgradeConsoleModuleAdded(UpgradeConsoleModuleAddedEventArgs ev) => UpgradeConsoleModuleAdded.CustomInvoke(ev);

        /**
         *
         * UpgradeConsoleModuleRemoved İşleyicisi
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static event SubnauticaPluginEventHandler<UpgradeConsoleModuleRemovedEventArgs> UpgradeConsoleModuleRemoved;

        /**
         *
         * UpgradeConsoleModuleRemoved Olayı 
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void OnUpgradeConsoleModuleRemoved(UpgradeConsoleModuleRemovedEventArgs ev) => UpgradeConsoleModuleRemoved.CustomInvoke(ev);

        /**
         *
         * Entering İşleyicisi
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static event SubnauticaPluginEventHandler<VehicleEnteringEventArgs> Entering;

        /**
         *
         * Entering Olayı 
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void OnEntering(VehicleEnteringEventArgs ev) => Entering.CustomInvoke(ev);

        /**
         *
         * InteriorToggle İşleyicisi
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static event SubnauticaPluginEventHandler<VehicleInteriorToggleEventArgs> InteriorToggle;

        /**
         *
         * InteriorToggle Olayı 
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void OnInteriorToggle(VehicleInteriorToggleEventArgs ev) => InteriorToggle.CustomInvoke(ev);

        /**
         *
         * Exited İşleyicisi
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static event SubnauticaPluginEventHandler<VehicleExitedEventArgs> Exited;

        /**
         *
         * Exited Olayı 
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void OnExited(VehicleExitedEventArgs ev) => Exited.CustomInvoke(ev);

        /**
         *
         * Updated İşleyicisi
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static event SubnauticaPluginEventHandler<VehicleUpdatedEventArgs> Updated;

        /**
         *
         * Updated Olayı 
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void OnUpdated(VehicleUpdatedEventArgs ev) => Updated.CustomInvoke(ev);

        /**
         *
         * LightChanged İşleyicisi
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static event SubnauticaPluginEventHandler<LightChangedEventArgs> LightChanged;

        /**
         *
         * LightChanged Olayı 
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void OnLightChanged(LightChangedEventArgs ev) => LightChanged.CustomInvoke(ev);

        /**
         *
         * SeaTruckConnecting İşleyicisi
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static event SubnauticaPluginEventHandler<SeaTruckConnectingEventArgs> SeaTruckConnecting;

        /**
         *
         * SeaTruckConnecting Olayı 
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void OnSeaTruckConnecting(SeaTruckConnectingEventArgs ev) => SeaTruckConnecting.CustomInvoke(ev);

        /**
         *
         * ExosuitJumping İşleyicisi
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static event SubnauticaPluginEventHandler<ExosuitJumpingEventArgs> ExosuitJumping;

        /**
         *
         * ExosuitJumping Olayı 
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void OnExosuitJumping(ExosuitJumpingEventArgs ev) => ExosuitJumping.CustomInvoke(ev);

        /**
         *
         * SeaTruckDetaching İşleyicisi
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static event SubnauticaPluginEventHandler<SeaTruckDetachingEventArgs> SeaTruckDetaching;

        /**
         *
         * SeaTruckDetaching Olayı 
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void OnSeaTruckDetaching(SeaTruckDetachingEventArgs ev) => SeaTruckDetaching.CustomInvoke(ev);

        /**
         *
         * ExosuitItemPickedUp İşleyicisi
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static event SubnauticaPluginEventHandler<ExosuitItemPickedUpEventArgs> ExosuitItemPickedUp;

        /**
         *
         * ExosuitItemPickedUp Olayı 
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void OnExosuitItemPickedUp(ExosuitItemPickedUpEventArgs ev) => ExosuitItemPickedUp.CustomInvoke(ev);

        /**
         *
         * ExosuitDrilling İşleyicisi
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static event SubnauticaPluginEventHandler<ExosuitDrillingEventArgs> ExosuitDrilling;

        /**
         *
         * ExosuitDrilling Olayı 
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void OnExosuitDrilling(ExosuitDrillingEventArgs ev) => ExosuitDrilling.CustomInvoke(ev);

        /**
         *
         * Docking İşleyicisi
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static event SubnauticaPluginEventHandler<VehicleDockingEventArgs> Docking;

        /**
         *
         * Docking Olayı 
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void OnDocking(VehicleDockingEventArgs ev) => Docking.CustomInvoke(ev);

        /**
         *
         * Undocking İşleyicisi
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static event SubnauticaPluginEventHandler<VehicleUndockingEventArgs> Undocking;

        /**
         *
         * Undocking Olayı 
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void OnUndocking(VehicleUndockingEventArgs ev) => Undocking.CustomInvoke(ev);

        /**
         *
         * SeaTruckPictureFrameOpening İşleyicisi
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static event SubnauticaPluginEventHandler<SeaTruckPictureFrameOpeningEventArgs> SeaTruckPictureFrameOpening;

        /**
         *
         * SeaTruckPictureFrameOpening Olayı 
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void OnSeaTruckPictureFrameOpening(SeaTruckPictureFrameOpeningEventArgs ev) => SeaTruckPictureFrameOpening.CustomInvoke(ev);

        /**
         *
         * SeaTruckPictureFrameImageSelecting İşleyicisi
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static event SubnauticaPluginEventHandler<SeaTruckPictureFrameImageSelectingEventArgs> SeaTruckPictureFrameImageSelecting;

        /**
         *
         * SeaTruckPictureFrameImageSelecting Olayı 
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void OnSeaTruckPictureFrameImageSelecting(SeaTruckPictureFrameImageSelectingEventArgs ev) => SeaTruckPictureFrameImageSelecting.CustomInvoke(ev);

        /**
         *
         * MapRoomCameraDocking İşleyicisi
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static event SubnauticaPluginEventHandler<MapRoomCameraDockingEventArgs> MapRoomCameraDocking;

        /**
         *
         * MapRoomCameraDocking Olayı 
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void OnMapRoomCameraDocking(MapRoomCameraDockingEventArgs ev) => MapRoomCameraDocking.CustomInvoke(ev);

        /**
         *
         * SeaTruckModuleInitialized İşleyicisi
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static event SubnauticaPluginEventHandler<SeaTruckModuleInitializedEventArgs> SeaTruckModuleInitialized;

        /**
         *
         * SeaTruckModuleInitialized Olayı 
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void OnSeaTruckModuleInitialized(SeaTruckModuleInitializedEventArgs ev) => SeaTruckModuleInitialized.CustomInvoke(ev);
    }
}