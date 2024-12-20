namespace Subnautica.Network.Models.Server
{
    using MessagePack;
    using Subnautica.API.Enums;
    using Subnautica.Network.Models.Core;

    [MessagePackObject]
    public class InventoryQuickSlotItemArgs : NetworkPacket
    {
        /**
         *
         * Ağ Paket Türü
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(0)]
        public override ProcessType Type { get; set; } = ProcessType.InventoryQuickSlot;

        /**
         *
         * Slot Eşya Id'leri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(5)]
        public string[] Slots { get; set; }
        
        /**
         *
         * Aktif olan slot Id
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(6)]
        public int ActiveSlot { get; set; }
    }
}
