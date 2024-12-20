namespace Subnautica.Events.Handlers
{
    using Subnautica.Events.EventArgs;

    using static Subnautica.API.Extensions.EventExtensions;

    public class Items
    {
        /**
         *
         * KnifeUsing İşleyicisi
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static event SubnauticaPluginEventHandler<KnifeUsingEventArgs> KnifeUsing;

        /**
         *
         * KnifeUsing Olayı 
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void OnKnifeUsing(KnifeUsingEventArgs ev) => KnifeUsing.CustomInvoke(ev);
        
        /**
         *
         * ScannerUsing İşleyicisi
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static event SubnauticaPluginEventHandler<ScannerUsingEventArgs> ScannerUsing;

        /**
         *
         * ScannerUsing Olayı 
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void OnScannerUsing(ScannerUsingEventArgs ev) => ScannerUsing.CustomInvoke(ev);
        
        /**
         *
         * ConstructorDeploying İşleyicisi
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static event SubnauticaPluginEventHandler<ConstructorDeployingEventArgs> ConstructorDeploying;

        /**
         *
         * ConstructorDeploying Olayı 
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void OnConstructorDeploying(ConstructorDeployingEventArgs ev) => ConstructorDeploying.CustomInvoke(ev);
        
        /**
         *
         * ConstructorEngageToggle İşleyicisi
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static event SubnauticaPluginEventHandler<ConstructorEngageToggleEventArgs> ConstructorEngageToggle;

        /**
         *
         * ConstructorEngageToggle Olayı 
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void OnConstructorEngageToggle(ConstructorEngageToggleEventArgs ev) => ConstructorEngageToggle.CustomInvoke(ev);
        
        /**
         *
         * ConstructorCrafting İşleyicisi
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static event SubnauticaPluginEventHandler<ConstructorCraftingEventArgs> ConstructorCrafting;

        /**
         *
         * ConstructorCrafting Olayı 
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void OnConstructorCrafting(ConstructorCraftingEventArgs ev) => ConstructorCrafting.CustomInvoke(ev);
        
        /**
         *
         * HoverbikeDeploying İşleyicisi
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static event SubnauticaPluginEventHandler<HoverbikeDeployingEventArgs> HoverbikeDeploying;

        /**
         *
         * HoverbikeDeploying Olayı 
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void OnHoverbikeDeploying(HoverbikeDeployingEventArgs ev) => HoverbikeDeploying.CustomInvoke(ev);
        
        /**
         *
         * DeployableStorageDeploying İşleyicisi
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static event SubnauticaPluginEventHandler<DeployableStorageDeployingEventArgs> DeployableStorageDeploying;

        /**
         *
         * DeployableStorageDeploying Olayı 
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void OnDeployableStorageDeploying(DeployableStorageDeployingEventArgs ev) => DeployableStorageDeploying.CustomInvoke(ev);
        
        /**
         *
         * LEDLightDeploying İşleyicisi
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static event SubnauticaPluginEventHandler<LEDLightDeployingEventArgs> LEDLightDeploying;

        /**
         *
         * LEDLightDeploying Olayı 
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void OnLEDLightDeploying(LEDLightDeployingEventArgs ev) => LEDLightDeploying.CustomInvoke(ev);
        
        /**
         *
         * BeaconDeploying İşleyicisi
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static event SubnauticaPluginEventHandler<BeaconDeployingEventArgs> BeaconDeploying;

        /**
         *
         * BeaconDeploying Olayı 
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void OnBeaconDeploying(BeaconDeployingEventArgs ev) => BeaconDeploying.CustomInvoke(ev);
        
        /**
         *
         * FlareDeploying İşleyicisi
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static event SubnauticaPluginEventHandler<FlareDeployingEventArgs> FlareDeploying;

        /**
         *
         * FlareDeploying Olayı 
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void OnFlareDeploying(FlareDeployingEventArgs ev) => FlareDeploying.CustomInvoke(ev);
        
        /**
         *
         * ThumperDeploying İşleyicisi
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static event SubnauticaPluginEventHandler<ThumperDeployingEventArgs> ThumperDeploying;

        /**
         *
         * ThumperDeploying Olayı 
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void OnThumperDeploying(ThumperDeployingEventArgs ev) => ThumperDeploying.CustomInvoke(ev);
        
        /**
         *
         * TeleportationToolUsed İşleyicisi
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static event SubnauticaPluginEventHandler<TeleportationToolUsedEventArgs> TeleportationToolUsed;

        /**
         *
         * TeleportationToolUsed Olayı 
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void OnTeleportationToolUsed(TeleportationToolUsedEventArgs ev) => TeleportationToolUsed.CustomInvoke(ev);
        
        /**
         *
         * BeaconLabelChanged İşleyicisi
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static event SubnauticaPluginEventHandler<BeaconLabelChangedEventArgs> BeaconLabelChanged;

        /**
         *
         * BeaconLabelChanged Olayı 
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void OnBeaconLabelChanged(BeaconLabelChangedEventArgs ev) => BeaconLabelChanged.CustomInvoke(ev);
        
        /**
         *
         * SpyPenguinDeploying İşleyicisi
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static event SubnauticaPluginEventHandler<SpyPenguinDeployingEventArgs> SpyPenguinDeploying;

        /**
         *
         * SpyPenguinDeploying Olayı 
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void OnSpyPenguinDeploying(SpyPenguinDeployingEventArgs ev) => SpyPenguinDeploying.CustomInvoke(ev);
        
        /**
         *
         * SpyPenguinItemPickedUp İşleyicisi
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static event SubnauticaPluginEventHandler<SpyPenguinItemPickedUpEventArgs> SpyPenguinItemPickedUp;

        /**
         *
         * SpyPenguinItemPickedUp Olayı 
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void OnSpyPenguinItemPickedUp(SpyPenguinItemPickedUpEventArgs ev) => SpyPenguinItemPickedUp.CustomInvoke(ev);
        
        /**
         *
         * SpyPenguinSnowStalkerInteracting İşleyicisi
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static event SubnauticaPluginEventHandler<SpyPenguinSnowStalkerInteractingEventArgs> SpyPenguinSnowStalkerInteracting;

        /**
         *
         * SpyPenguinSnowStalkerInteracting Olayı 
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void OnSpyPenguinSnowStalkerInteracting(SpyPenguinSnowStalkerInteractingEventArgs ev) => SpyPenguinSnowStalkerInteracting.CustomInvoke(ev);
        
        /**
         *
         * SpyPenguinItemGrabing İşleyicisi
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static event SubnauticaPluginEventHandler<SpyPenguinItemGrabingEventArgs> SpyPenguinItemGrabing;

        /**
         *
         * SpyPenguinItemGrabing Olayı 
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void OnSpyPenguinItemGrabing(SpyPenguinItemGrabingEventArgs ev) => SpyPenguinItemGrabing.CustomInvoke(ev);
        
        /**
         *
         * Welding İşleyicisi
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static event SubnauticaPluginEventHandler<WeldingEventArgs> Welding;

        /**
         *
         * Welding Olayı 
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void OnWelding(WeldingEventArgs ev) => Welding.CustomInvoke(ev);
        
        /**
         *
         * DroneCameraDeploying İşleyicisi
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static event SubnauticaPluginEventHandler<DroneCameraDeployingEventArgs> DroneCameraDeploying;

        /**
         *
         * DroneCameraDeploying Olayı 
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void OnDroneCameraDeploying(DroneCameraDeployingEventArgs ev) => DroneCameraDeploying.CustomInvoke(ev);
        
        /**
         *
         * PipeSurfaceFloaterDeploying İşleyicisi
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static event SubnauticaPluginEventHandler<PipeSurfaceFloaterDeployingEventArgs> PipeSurfaceFloaterDeploying;

        /**
         *
         * PipeSurfaceFloaterDeploying Olayı 
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void OnPipeSurfaceFloaterDeploying(PipeSurfaceFloaterDeployingEventArgs ev) => PipeSurfaceFloaterDeploying.CustomInvoke(ev);
        
        /**
         *
         * OxygenPipePlacing İşleyicisi
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static event SubnauticaPluginEventHandler<OxygenPipePlacingEventArgs> OxygenPipePlacing;

        /**
         *
         * OxygenPipePlacing Olayı 
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void OnOxygenPipePlacing(OxygenPipePlacingEventArgs ev) => OxygenPipePlacing.CustomInvoke(ev);
    }
}
