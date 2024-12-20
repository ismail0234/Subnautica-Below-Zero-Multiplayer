namespace Subnautica.Events.Handlers
{
    using Subnautica.Events.EventArgs;

    using static Subnautica.API.Extensions.EventExtensions;

    public class Game
    {
        /**
         *
         * GameQuitting İşleyicisi
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static event SubnauticaPluginEventHandler Quitting;

        /**
         *
         * GameQuitting Olayı 
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void OnQuitting() => Quitting.CustomInvoke();

        /**
         *
         * QuittingToMainMenu İşleyicisi
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static event SubnauticaPluginEventHandler<QuittingToMainMenuEventArgs> QuittingToMainMenu;

        /**
         *
         * QuittingToMainMenu Olayı 
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void OnQuittingToMainMenu(QuittingToMainMenuEventArgs ev) => QuittingToMainMenu.CustomInvoke(ev);

        /**
         *
         * SceneLoaded İşleyicisi
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static event SubnauticaPluginEventHandler<SceneLoadedEventArgs> SceneLoaded;

        /**
         *
         * SceneLoaded Olayı 
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void OnSceneLoaded(SceneLoadedEventArgs ev) => SceneLoaded.CustomInvoke(ev);

        /**
         *
         * MenuSaveCancelDeleteButtonClicking İşleyicisi
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static event SubnauticaPluginEventHandler<MenuSaveCancelDeleteButtonClickingEventArgs> MenuSaveCancelDeleteButtonClicking;

        /**
         *
         * MenuSaveCancelDeleteButtonClicking Olayı 
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void OnMenuSaveCancelDeleteButtonClicking(MenuSaveCancelDeleteButtonClickingEventArgs ev) => MenuSaveCancelDeleteButtonClicking.CustomInvoke(ev);

        /**
         *
         * MenuSaveDeleteButtonClicking İşleyicisi
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static event SubnauticaPluginEventHandler<MenuSaveDeleteButtonClickingEventArgs> MenuSaveDeleteButtonClicking;

        /**
         *
         * MenuSaveDeleteButtonClicking Olayı 
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void OnMenuSaveDeleteButtonClicking(MenuSaveDeleteButtonClickingEventArgs ev) => MenuSaveDeleteButtonClicking.CustomInvoke(ev);

        /**
         *
         * MenuSaveLoadButtonClicking İşleyicisi
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static event SubnauticaPluginEventHandler<MenuSaveLoadButtonClickingEventArgs> MenuSaveLoadButtonClicking;

        /**
         *
         * MenuSaveLoadButtonClicking Olayı 
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void OnMenuSaveLoadButtonClicking(MenuSaveLoadButtonClickingEventArgs ev) => MenuSaveLoadButtonClicking.CustomInvoke(ev);

        /**
         *
         * MenuSaveUpdateLoadedButtonState İşleyicisi
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static event SubnauticaPluginEventHandler<MenuSaveUpdateLoadedButtonStateEventArgs> MenuSaveUpdateLoadedButtonState;

        /**
         *
         * MenuSaveUpdateLoadedButtonState Olayı 
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void OnMenuSaveUpdateLoadedButtonState(MenuSaveUpdateLoadedButtonStateEventArgs ev) => MenuSaveUpdateLoadedButtonState.CustomInvoke(ev);

        /**
         *
         * InGameMenuClosed İşleyicisi
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static event SubnauticaPluginEventHandler<InGameMenuClosedEventArgs> InGameMenuClosed;

        /**
         *
         * InGameMenuClosed Olayı 
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void OnInGameMenuClosed(InGameMenuClosedEventArgs ev) => InGameMenuClosed.CustomInvoke(ev);
        
        /**
         *
         * InGameMenuClosing İşleyicisi
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static event SubnauticaPluginEventHandler<InGameMenuClosingEventArgs> InGameMenuClosing;

        /**
         *
         * InGameMenuClosing Olayı 
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void OnInGameMenuClosing(InGameMenuClosingEventArgs ev) => InGameMenuClosing.CustomInvoke(ev);

        /**
         *
         * InGameMenuOpened İşleyicisi
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static event SubnauticaPluginEventHandler<InGameMenuOpenedEventArgs> InGameMenuOpened;

        /**
         *
         * InGameMenuOpened Olayı 
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void OnInGameMenuOpened(InGameMenuOpenedEventArgs ev) => InGameMenuOpened.CustomInvoke(ev);

        /**
         *
         * InGameMenuOpening İşleyicisi
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static event SubnauticaPluginEventHandler<InGameMenuOpeningEventArgs> InGameMenuOpening;

        /**
         *
         * InGameMenuOpening Olayı 
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void OnInGameMenuOpening(InGameMenuOpeningEventArgs ev) => InGameMenuOpening.CustomInvoke(ev);

        /**
         *
         * SettingsRunInBackgroundChanging İşleyicisi
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static event SubnauticaPluginEventHandler<SettingsRunInBackgroundChangingEventArgs> SettingsRunInBackgroundChanging;

        /**
         *
         * SettingsRunInBackgroundChanging Olayı 
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void OnSettingsRunInBackgroundChanging(SettingsRunInBackgroundChangingEventArgs ev) => SettingsRunInBackgroundChanging.CustomInvoke(ev);

        /**
         *
         * SettingsPdaGamePauseChanging İşleyicisi
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static event SubnauticaPluginEventHandler<SettingsPdaGamePauseChangingEventArgs> SettingsPdaGamePauseChanging;

        /**
         *
         * SettingsPdaGamePauseChanging Olayı 
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void OnSettingsPdaGamePauseChanging(SettingsPdaGamePauseChangingEventArgs ev) => SettingsPdaGamePauseChanging.CustomInvoke(ev);

        /**
         *
         * WorldLoading İşleyicisi
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static event SubnauticaPluginEventHandler<WorldLoadingEventArgs> WorldLoading;

        /**
         *
         * WorldLoading Olayı 
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void OnWorldLoading(WorldLoadingEventArgs ev) => WorldLoading.CustomInvoke(ev);


        /**
         *
         * WorldLoaded İşleyicisi
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static event SubnauticaPluginEventHandler<WorldLoadedEventArgs> WorldLoaded;

        /**
         *
         * WorldLoaded Olayı 
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void OnWorldLoaded(WorldLoadedEventArgs ev) => WorldLoaded.CustomInvoke(ev);

        /**
         *
         * ScreenshotsRemoved İşleyicisi
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static event SubnauticaPluginEventHandler<ScreenshotsRemovedEventArgs> ScreenshotsRemoved;

        /**
         *
         * ScreenshotsRemoved Olayı 
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void OnScreenshotsRemoved(ScreenshotsRemovedEventArgs ev) => ScreenshotsRemoved.CustomInvoke(ev);

        /**
         *
         * PowerSourceRemoving İşleyicisi
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static event SubnauticaPluginEventHandler<PowerSourceRemovingEventArgs> PowerSourceRemoving;

        /**
         *
         * PowerSourceRemoving Olayı 
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void OnPowerSourceRemoving(PowerSourceRemovingEventArgs ev) => PowerSourceRemoving.CustomInvoke(ev);

        /**
         *
         * PowerSourceAdding İşleyicisi
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static event SubnauticaPluginEventHandler<PowerSourceAddingEventArgs> PowerSourceAdding;

        /**
         *
         * PowerSourceAdding Olayı 
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void OnPowerSourceAdding(PowerSourceAddingEventArgs ev) => PowerSourceAdding.CustomInvoke(ev);

        /**
         *
         * IntroChecking İşleyicisi
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static event SubnauticaPluginEventHandler<IntroCheckingEventArgs> IntroChecking;

        /**
         *
         * IntroChecking Olayı 
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void OnIntroChecking(IntroCheckingEventArgs ev) => IntroChecking.CustomInvoke(ev);

        /**
         *
         * LifepodInterpolation İşleyicisi
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static event SubnauticaPluginEventHandler<LifepodInterpolationEventArgs> LifepodInterpolation;

        /**
         *
         * LifepodInterpolation Olayı 
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void OnLifepodInterpolation(LifepodInterpolationEventArgs ev) => LifepodInterpolation.CustomInvoke(ev);

        /**
         *
         * LifepodZoneCheck İşleyicisi
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static event SubnauticaPluginEventHandler<LifepodZoneCheckEventArgs> LifepodZoneCheck;

        /**
         *
         * LifepodZoneCheck Olayı 
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void OnLifepodZoneCheck(LifepodZoneCheckEventArgs ev) => LifepodZoneCheck.CustomInvoke(ev);

        /**
         *
         * LifepodZoneSelecting İşleyicisi
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static event SubnauticaPluginEventHandler<LifepodZoneSelectingEventArgs> LifepodZoneSelecting;

        /**
         *
         * LifepodZoneSelecting Olayı 
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void OnLifepodZoneSelecting(LifepodZoneSelectingEventArgs ev) => LifepodZoneSelecting.CustomInvoke(ev);

        /**
         *
         * SubNameInputSelecting İşleyicisi
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static event SubnauticaPluginEventHandler<SubNameInputSelectingEventArgs> SubNameInputSelecting;

        /**
         *
         * SubNameInputSelecting Olayı 
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void OnSubNameInputSelecting(SubNameInputSelectingEventArgs ev) => SubNameInputSelecting.CustomInvoke(ev);

        /**
         *
         * SubNameInputDeselected İşleyicisi
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static event SubnauticaPluginEventHandler<SubNameInputDeselectedEventArgs> SubNameInputDeselected;

        /**
         *
         * SubNameInputDeselected Olayı 
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void OnSubNameInputDeselected(SubNameInputDeselectedEventArgs ev) => SubNameInputDeselected.CustomInvoke(ev);

        /**
         *
         * EntityDistributionLoaded İşleyicisi
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static event SubnauticaPluginEventHandler EntityDistributionLoaded;

        /**
         *
         * EntityDistributionLoaded Olayı 
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void OnEntityDistributionLoaded() => EntityDistributionLoaded.CustomInvoke();
    }
}
