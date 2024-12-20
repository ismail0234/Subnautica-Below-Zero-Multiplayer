namespace Subnautica.Events.Handlers
{
    using Subnautica.Events.EventArgs;

    using static Subnautica.API.Extensions.EventExtensions;

    public class Inventory
    {
        /**
         *
         * ItemRemoved İşleyicisi
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static event SubnauticaPluginEventHandler<InventoryItemRemovedEventArgs> ItemRemoved;

        /**
         *
         * ItemRemoved Olayı 
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void OnItemRemoved(InventoryItemRemovedEventArgs ev) => ItemRemoved.CustomInvoke(ev);

        /**
         *
         * ItemAdded İşleyicisi
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static event SubnauticaPluginEventHandler<InventoryItemAddedEventArgs> ItemAdded;

        /**
         *
         * ItemAdded Olayı 
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void OnItemAdded(InventoryItemAddedEventArgs ev) => ItemAdded.CustomInvoke(ev);

        /**
         *
         * QuickSlotBinded İşleyicisi
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static event SubnauticaPluginEventHandler QuickSlotBinded;

        /**
         *
         * QuickSlotBinded Olayı 
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void OnQuickSlotBinded() => QuickSlotBinded.CustomInvoke();

        /**
         *
         * QuickSlotUnbinded İşleyicisi
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static event SubnauticaPluginEventHandler QuickSlotUnbinded;

        /**
         *
         * QuickSlotUnbinded Olayı 
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void OnQuickSlotUnbinded() => QuickSlotUnbinded.CustomInvoke();

        /**
         *
         * EquipmentEquiped İşleyicisi
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static event SubnauticaPluginEventHandler EquipmentEquiped;

        /**
         *
         * EquipmentEquiped Olayı 
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void OnEquipmentEquiped() => EquipmentEquiped.CustomInvoke();

        /**
         *
         * EquipmentUnequiped İşleyicisi
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static event SubnauticaPluginEventHandler EquipmentUnequiped;

        /**
         *
         * EquipmentUnequiped Olayı 
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void OnEquipmentUnequiped() => EquipmentUnequiped.CustomInvoke();

        /**
         *
         * QuickSlotActiveChanged İşleyicisi
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static event SubnauticaPluginEventHandler<QuickSlotActiveChangedEventArgs> QuickSlotActiveChanged;

        /**
         *
         * QuickSlotActiveChanged Olayı 
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void OnQuickSlotActiveChanged(QuickSlotActiveChangedEventArgs ev) => QuickSlotActiveChanged.CustomInvoke(ev);
    }
}