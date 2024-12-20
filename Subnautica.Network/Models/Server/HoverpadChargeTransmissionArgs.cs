namespace Subnautica.Network.Models.Server
{
    using System.Collections.Generic;

    using LiteNetLib;

    using MessagePack;

    using Subnautica.API.Enums;
    using Subnautica.Network.Models.Core;

    [MessagePackObject]
    public class HoverpadChargeTransmissionArgs : NetworkPacket
    {
        /**
         *
         * Ağ Paket Türü
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(0)]
        public override ProcessType Type { get; set; } = ProcessType.HoverpadChargeTransmission;

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
         * Packet Teslim Türü
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(2)]
        public override DeliveryMethod DeliveryMethod { get; set; } = DeliveryMethod.ReliableSequenced;

        /**
         *
         * Energies Değerini Barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(5)]
        public Dictionary<uint, HoverpadEnergyTransmissionItem> Items { get; set; }
    }

    [MessagePackObject]
    public class HoverpadEnergyTransmissionItem
    {
        /**
         *
         * Health Değerini Barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(0)]
        public byte Health { get; set; }

        /**
         *
         * Charge Değerini Barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(1)]
        public byte Charge { get; set; }

        /**
         *
         * Sınıf ayarlarını yapar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public HoverpadEnergyTransmissionItem()
        {

        }

        /**
         *
         * Sınıf ayarlarını yapar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public HoverpadEnergyTransmissionItem(byte health, byte charge)
        {
            this.Health = health;
            this.Charge = charge;
        }
    }
}
