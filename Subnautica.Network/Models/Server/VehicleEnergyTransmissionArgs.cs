namespace Subnautica.Network.Models.Server
{
    using System.Collections.Generic;

    using MessagePack;

    using Subnautica.API.Enums;
    using Subnautica.Network.Models.Core;

    [MessagePackObject]
    public class VehicleEnergyTransmissionArgs : NetworkPacket
    {
        /**
         *
         * Ağ Paket Türü
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(0)]
        public override ProcessType Type { get; set; } = ProcessType.VehicleEnergyTransmission;

        /**
         *
         * Packet Kanal Türü
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(1)]
        public override NetworkChannel ChannelType { get; set; } = NetworkChannel.EnergyTransmission;

        /**
         *
         * PowerCells değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(5)]
        public List<VehicleEnergyTransmissionItem> PowerCells { get; set; }
    }

    [MessagePackObject]
    public class VehicleEnergyTransmissionItem
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
        public float PowerCell1 { get; set; }

        /**
         *
         * VehicleId değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(2)]
        public float PowerCell2 { get; set; }

        /**
         *
         * Sınıf Ayarlarını yapar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public VehicleEnergyTransmissionItem()
        {

        }

        /**
         *
         * Sınıf Ayarlarını yapar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public VehicleEnergyTransmissionItem(string vehicleId, float powerCell1, float powerCell2)
        {
            this.VehicleId  = vehicleId;
            this.PowerCell1 = powerCell1;
            this.PowerCell2 = powerCell2;
        }
    }
}