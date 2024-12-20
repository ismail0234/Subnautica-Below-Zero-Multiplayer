namespace Subnautica.Events.Handlers
{
    using Subnautica.Events.EventArgs;

    using static Subnautica.API.Extensions.EventExtensions;

    public class Storage
    {
        /**
         *
         * Opening İşleyicisi
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static event SubnauticaPluginEventHandler<StorageOpeningEventArgs> Opening;

        /**
         *
         * Opening Olayı 
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void OnOpening(StorageOpeningEventArgs ev) => Opening.CustomInvoke(ev);

        /**
         *
         * ItemAdded İşleyicisi
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static event SubnauticaPluginEventHandler<StorageItemAddedEventArgs> ItemAdded;

        /**
         *
         * ItemAdded Olayı 
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void OnItemAdded(StorageItemAddedEventArgs ev) => ItemAdded.CustomInvoke(ev);

        /**
         *
         * ItemRemoved İşleyicisi
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static event SubnauticaPluginEventHandler<StorageItemRemovedEventArgs> ItemRemoved;

        /**
         *
         * ItemRemoved Olayı 
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void OnItemRemoved(StorageItemRemovedEventArgs ev) => ItemRemoved.CustomInvoke(ev);

        /**
         *
         * NuclearReactorItemAdded İşleyicisi
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static event SubnauticaPluginEventHandler<NuclearReactorItemAddedEventArgs> NuclearReactorItemAdded;

        /**
         *
         * NuclearReactorItemAdded Olayı 
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void OnNuclearReactorItemAdded(NuclearReactorItemAddedEventArgs ev) => NuclearReactorItemAdded.CustomInvoke(ev);

        /**
         *
         * NuclearReactorItemRemoved İşleyicisi
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static event SubnauticaPluginEventHandler<NuclearReactorItemRemovedEventArgs> NuclearReactorItemRemoved;

        /**
         *
         * NuclearReactorItemRemoved Olayı 
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void OnNuclearReactorItemRemoved(NuclearReactorItemRemovedEventArgs ev) => NuclearReactorItemRemoved.CustomInvoke(ev);

        /**
         *
         * ChargerItemAdded İşleyicisi
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static event SubnauticaPluginEventHandler<ChargerItemAddedEventArgs> ChargerItemAdded;

        /**
         *
         * ChargerItemAdded Olayı 
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void OnChargerItemAdded(ChargerItemAddedEventArgs ev) => ChargerItemAdded.CustomInvoke(ev);

        /**
         *
         * ChargerItemRemoved İşleyicisi
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static event SubnauticaPluginEventHandler<ChargerItemRemovedEventArgs> ChargerItemRemoved;

        /**
         *
         * ChargerItemRemoved Olayı 
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void OnChargerItemRemoved(ChargerItemRemovedEventArgs ev) => ChargerItemRemoved.CustomInvoke(ev);

        /**
         *
         * ItemRemoving İşleyicisi
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static event SubnauticaPluginEventHandler<StorageItemRemovingEventArgs> ItemRemoving;

        /**
         *
         * ItemRemoving Olayı 
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void OnItemRemoving(StorageItemRemovingEventArgs ev) => ItemRemoving.CustomInvoke(ev);

        /**
         *
         * ItemAdding İşleyicisi
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static event SubnauticaPluginEventHandler<StorageItemAddingEventArgs> ItemAdding;

        /**
         *
         * ItemAdding Olayı 
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void OnItemAdding(StorageItemAddingEventArgs ev) => ItemAdding.CustomInvoke(ev);
    }
}