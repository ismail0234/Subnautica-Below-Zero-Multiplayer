namespace Subnautica.Network.Models.Server
{
    using System.Collections.Generic;

    using MessagePack;

    using Subnautica.API.Enums;
    using Subnautica.Network.Models.Core;
    using Subnautica.Network.Structures;

    [MessagePackObject]
    public class EnergyMixinTransmissionArgs : NetworkPacket
    {
        /**
         *
         * Ağ Paket Türü
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(0)]
        public override ProcessType Type { get; set; } = ProcessType.EnergyMixinTransmission;

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
        public List<EnergyMixinTransmissionItem> Items { get; set; }
    }

    [MessagePackObject]
    public class EnergyMixinTransmissionItem
    {
        /**
         *
         * VehicleId değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(0)]
        public ushort ItemId { get; set; }

        /**
         *
         * Charge değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(1)]
        public float Charge { get; set; }

        /**
         *
         * Position değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [IgnoreMember]
        public ZeroVector3 Position { get; set; }

        /**
         *
         * Sınıf Ayarlarını yapar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public EnergyMixinTransmissionItem()
        {

        }

        /**
         *
         * Sınıf Ayarlarını yapar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public EnergyMixinTransmissionItem(ushort itemId, float charge, ZeroVector3 position)
        {
            this.ItemId   = itemId;
            this.Charge   = charge;
            this.Position = position;
        }
    }
}