namespace Subnautica.Network.Models.Server
{
    using MessagePack;

    using Subnautica.API.Enums;
    using Subnautica.API.Features;
    using Subnautica.Network.Models.Core;

    [MessagePackObject]
    public class SeaTruckSleeperModuleArgs : NetworkPacket
    {
        /**
         *
         * Ağ Paket Türü
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(0)]
        public override ProcessType Type { get; set; } = ProcessType.SeaTruckSleeperModule;

        /**
         *
         * UniqueId Değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(5)]
        public string UniqueId { get; set; }

        /**
         *
         * IsOpeningPictureFrame Değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(6)]
        public bool IsOpeningPictureFrame { get; set; }

        /**
         *
         * IsSelectingPictureFrame Değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(7)]
        public bool IsSelectingPictureFrame { get; set; }

        /**
         *
         * PictureFrameData Değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(8)]
        public byte[] PictureFrameData { get; set; }

        /**
         *
         * PictureFrameName Değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(9)]
        public string PictureFrameName { get; set; }

        /**
         *
         * JukeboxData Değerini barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(10)]
        public CustomProperty JukeboxData { get; set; }

        /**
         *
         * SleepingSide Değerini barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(11)]
        public Bed.BedSide SleepingSide { get; set; }

        /**
         *
         * IsSleeping Değerini barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(12)]
        public bool IsSleeping { get; set; }
    }
}