namespace Subnautica.Events.Handlers
{
    using Subnautica.Events.EventArgs;

    using static Subnautica.API.Extensions.EventExtensions;

    public class Story
    {
        /**
         *
         * BridgeFluidClicking İşleyicisi
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static event SubnauticaPluginEventHandler<BridgeFluidClickingEventArgs> BridgeFluidClicking;

        /**
         *
         * BridgeFluidClicking Olayı 
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void OnBridgeFluidClicking(BridgeFluidClickingEventArgs ev) => BridgeFluidClicking.CustomInvoke(ev);

        /**
         *
         * BridgeTerminalClicking İşleyicisi
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static event SubnauticaPluginEventHandler<BridgeTerminalClickingEventArgs> BridgeTerminalClicking;

        /**
         *
         * BridgeTerminalClicking Olayı 
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void OnBridgeTerminalClicking(BridgeTerminalClickingEventArgs ev) => BridgeTerminalClicking.CustomInvoke(ev);

        /**
         *
         * BridgeInitialized İşleyicisi
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static event SubnauticaPluginEventHandler<BridgeInitializedEventArgs> BridgeInitialized;

        /**
         *
         * BridgeInitialized Olayı 
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void OnBridgeInitialized(BridgeInitializedEventArgs ev) => BridgeInitialized.CustomInvoke(ev);

        /**
         *
         * RadioTowerTOMUsing İşleyicisi
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static event SubnauticaPluginEventHandler<RadioTowerTOMUsingEventArgs> RadioTowerTOMUsing;

        /**
         *
         * RadioTowerTOMUsing Olayı 
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void OnRadioTowerTOMUsing(RadioTowerTOMUsingEventArgs ev) => RadioTowerTOMUsing.CustomInvoke(ev);

        /**
         *
         * StorySignalSpawning İşleyicisi
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static event SubnauticaPluginEventHandler<StorySignalSpawningEventArgs> StorySignalSpawning;

        /**
         *
         * StorySignalSpawning Olayı 
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void OnStorySignalSpawning(StorySignalSpawningEventArgs ev) => StorySignalSpawning.CustomInvoke(ev);

        /**
         *
         * StoryGoalTriggering İşleyicisi
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static event SubnauticaPluginEventHandler<StoryGoalTriggeringEventArgs> StoryGoalTriggering;

        /**
         *
         * StoryGoalTriggering Olayı 
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void OnStoryGoalTriggering(StoryGoalTriggeringEventArgs ev) => StoryGoalTriggering.CustomInvoke(ev);

        /**
         *
         * CinematicTriggering İşleyicisi
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static event SubnauticaPluginEventHandler<CinematicTriggeringEventArgs> CinematicTriggering;

        /**
         *
         * CinematicTriggering Olayı 
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void OnCinematicTriggering(CinematicTriggeringEventArgs ev) => CinematicTriggering.CustomInvoke(ev);

        /**
         *
         * StoryCalling İşleyicisi
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static event SubnauticaPluginEventHandler<StoryCallingEventArgs> StoryCalling;

        /**
         *
         * StoryCalling Olayı 
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void OnStoryCalling(StoryCallingEventArgs ev) => StoryCalling.CustomInvoke(ev);

        /**
         *
         * StoryHandClicking İşleyicisi
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static event SubnauticaPluginEventHandler<StoryHandClickingEventArgs> StoryHandClicking;

        /**
         *
         * StoryHandClicking Olayı 
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void OnStoryHandClicking(StoryHandClickingEventArgs ev) => StoryHandClicking.CustomInvoke(ev);

        /**
         *
         * StoryCinematicStarted İşleyicisi
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static event SubnauticaPluginEventHandler<StoryCinematicStartedEventArgs> StoryCinematicStarted;

        /**
         *
         * StoryCinematicStarted Olayı 
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void OnStoryCinematicStarted(StoryCinematicStartedEventArgs ev) => StoryCinematicStarted.CustomInvoke(ev);

        /**
         *
         * StoryCinematicCompleted İşleyicisi
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static event SubnauticaPluginEventHandler<StoryCinematicCompletedEventArgs> StoryCinematicCompleted;

        /**
         *
         * StoryCinematicCompleted Olayı 
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void OnStoryCinematicCompleted(StoryCinematicCompletedEventArgs ev) => StoryCinematicCompleted.CustomInvoke(ev);

        /**
         *
         * MobileExtractorMachineInitialized İşleyicisi
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static event SubnauticaPluginEventHandler MobileExtractorMachineInitialized;

        /**
         *
         * MobileExtractorMachineInitialized Olayı 
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void OnMobileExtractorMachineInitialized() => MobileExtractorMachineInitialized.CustomInvoke();

        /**
         *
         * MobileExtractorMachineSampleAdding İşleyicisi
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static event SubnauticaPluginEventHandler<MobileExtractorMachineSampleAddingEventArgs> MobileExtractorMachineSampleAdding;

        /**
         *
         * MobileExtractorMachineSampleAdding Olayı 
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void OnMobileExtractorMachineSampleAdding(MobileExtractorMachineSampleAddingEventArgs ev) => MobileExtractorMachineSampleAdding.CustomInvoke(ev);

        /**
         *
         * MobileExtractorConsoleUsing İşleyicisi
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static event SubnauticaPluginEventHandler<MobileExtractorConsoleUsingEventArgs> MobileExtractorConsoleUsing;

        /**
         *
         * MobileExtractorConsoleUsing Olayı 
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void OnMobileExtractorConsoleUsing(MobileExtractorConsoleUsingEventArgs ev) => MobileExtractorConsoleUsing.CustomInvoke(ev);

        /**
         *
         * ShieldBaseEnterTriggering İşleyicisi
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static event SubnauticaPluginEventHandler<ShieldBaseEnterTriggeringEventArgs> ShieldBaseEnterTriggering;

        /**
         *
         * ShieldBaseEnterTriggering Olayı 
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void OnShieldBaseEnterTriggering(ShieldBaseEnterTriggeringEventArgs ev) => ShieldBaseEnterTriggering.CustomInvoke(ev);
    }
}