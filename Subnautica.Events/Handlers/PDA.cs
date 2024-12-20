namespace Subnautica.Events.Handlers
{
    using Subnautica.Events.EventArgs;

    using static Subnautica.API.Extensions.EventExtensions;

    public class PDA
    {
        /**
         *
         * EncyclopediaAdded İşleyicisi
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static event SubnauticaPluginEventHandler<EncyclopediaAddedEventArgs> EncyclopediaAdded;

        /**
         *
         * EncyclopediaAdded Olayı 
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void OnEncyclopediaAdded(EncyclopediaAddedEventArgs ev) => EncyclopediaAdded.CustomInvoke(ev);

        /**
         *
         * TechnologyFragmentAdded İşleyicisi
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static event SubnauticaPluginEventHandler<TechnologyFragmentAddedEventArgs> TechnologyFragmentAdded;

        /**
         *
         * TechnologyFragmentAdded Olayı 
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void OnTechnologyFragmentAdded(TechnologyFragmentAddedEventArgs ev) => TechnologyFragmentAdded.CustomInvoke(ev);

        /**
         *
         * TechnologyAdded İşleyicisi
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static event SubnauticaPluginEventHandler<TechnologyAddedEventArgs> TechnologyAdded;

        /**
         *
         * TechnologyAdded Olayı 
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void OnTechnologyAdded(TechnologyAddedEventArgs ev) => TechnologyAdded.CustomInvoke(ev);

        /**
         *
         * ScannerCompleted İşleyicisi
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static event SubnauticaPluginEventHandler<ScannerCompletedEventArgs> ScannerCompleted;

        /**
         *
         * ScannerCompleted Olayı 
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void OnScannerCompleted(ScannerCompletedEventArgs ev) => ScannerCompleted.CustomInvoke(ev);

        /**
         *
         * ItemPinAdded İşleyicisi
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static event SubnauticaPluginEventHandler ItemPinAdded;

        /**
         *
         * ItemPinAdded Olayı 
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void OnItemPinAdded() => ItemPinAdded.CustomInvoke();

        /**
         *
         * ItemPinRemoved İşleyicisi
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static event SubnauticaPluginEventHandler ItemPinRemoved;

        /**
         *
         * ItemPinRemoved Olayı 
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void OnItemPinRemoved() => ItemPinRemoved.CustomInvoke();

        /**
         *
         * ItemPinMoved İşleyicisi
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static event SubnauticaPluginEventHandler ItemPinMoved;

        /**
         *
         * ItemPinMoved Olayı 
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void OnItemPinMoved() => ItemPinMoved.CustomInvoke();

        /**
         *
         * LogAdded İşleyicisi
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static event SubnauticaPluginEventHandler<PDALogAddedEventArgs> LogAdded;

        /**
         *
         * LogAdded Olayı 
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void OnLogAdded(PDALogAddedEventArgs ev) => LogAdded.CustomInvoke(ev);

        /**
         *
         * NotificationToggle İşleyicisi
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static event SubnauticaPluginEventHandler<NotificationToggleEventArgs> NotificationToggle;

        /**
         *
         * NotificationToggle Olayı 
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void OnNotificationToggle(NotificationToggleEventArgs ev) => NotificationToggle.CustomInvoke(ev);

        /**
         *
         * TechAnalyzeAdded İşleyicisi
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static event SubnauticaPluginEventHandler<TechAnalyzeAddedEventArgs> TechAnalyzeAdded;

        /**
         *
         * TechAnalyzeAdded Olayı 
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void OnTechAnalyzeAdded(TechAnalyzeAddedEventArgs ev) => TechAnalyzeAdded.CustomInvoke(ev);

        /**
         *
         * JukeboxDiskAdded İşleyicisi
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static event SubnauticaPluginEventHandler<JukeboxDiskAddedEventArgs> JukeboxDiskAdded;

        /**
         *
         * JukeboxDiskAdded Olayı 
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void OnJukeboxDiskAdded(JukeboxDiskAddedEventArgs ev) => JukeboxDiskAdded.CustomInvoke(ev);

        /**
         *
         * Closing İşleyicisi
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static event SubnauticaPluginEventHandler<PDAClosingEventArgs> Closing;

        /**
         *
         * Closing Olayı 
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void OnClosing(PDAClosingEventArgs ev) => Closing.CustomInvoke(ev);
    }
}