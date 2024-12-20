namespace Subnautica.Network.Models.Server
{
    using MessagePack;

    using Subnautica.API.Enums;
    using Subnautica.Network.Models.Core;
    using Subnautica.Network.Models.Metadata;

    [MessagePackObject]
    public class InventoryItemArgs : NetworkPacket
    {
        /**
         *
         * Ağ Paket Türü
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(0)]
        public override ProcessType Type { get; set; } = ProcessType.InventoryItem;

        /**
         *
         * ItemId Nesnesi
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(5)]
        public string ItemId { get; set; }

        /**
         *
         * Item Nesnesi
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(6)]
        public StorageItem Item { get; set; }

        /**
         *
         * IsAdded Nesnesi
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(7)]
        public bool IsAdded { get; set; }
    }
}
