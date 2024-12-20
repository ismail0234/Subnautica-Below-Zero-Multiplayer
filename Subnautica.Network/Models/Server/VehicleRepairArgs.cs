namespace Subnautica.Network.Models.Server
{
    using System.Collections.Generic;

    using MessagePack;

    using Subnautica.API.Enums;
    using Subnautica.Network.Models.Core;

    [MessagePackObject]
    public class VehicleRepairArgs : NetworkPacket
    {
        /**
         *
         * Ağ Paket Türü
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(0)]
        public override ProcessType Type { get; set; } = ProcessType.VehicleRepair;

        /**
         *
         * UniqueId Değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(5)]
        public List<VehicleRepairItem> Repairs { get; set; } = new List<VehicleRepairItem>();
    }

    [MessagePackObject]
    public class VehicleRepairItem
    {
        /**
         *
         * VehicleId değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(0)]
        public string VehicleId { get; set; }

        /**
         *
         * VehicleId değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(1)]
        public float Health { get; set; }

        /**
         *
         * Sınıf Ayarlarını yapar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public VehicleRepairItem()
        {

        }

        /**
         *
         * Sınıf Ayarlarını yapar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public VehicleRepairItem(string vehicleId, float health)
        {
            this.VehicleId = vehicleId;
            this.Health    = health;
        }
    }
}