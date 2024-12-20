namespace Subnautica.Network.Models.Server
{
    using MessagePack;
    
    using Subnautica.API.Enums;
    using Subnautica.Network.Models.Core;

    [MessagePackObject]
    public class NotificationAddedArgs : NetworkPacket
    {
        /**
         *
         * Ağ Paket Türü
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(0)]
        public override ProcessType Type { get; set; } = ProcessType.NotificationAdded;

        /**
         *
         * Grubu Barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(5)]
        public NotificationManager.Group Group { get; set; }

        /**
         *
         * Key Barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(6)]
        public string Key { get; set; }

        /**
         *
         * IsNotification Barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(7)]
        public bool IsNotification { get; set; }

        /**
         *
         * IsAdded Barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(8)]
        public bool IsAdded { get; set; }
        
        /**
         *
         * IsVisible Barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(9)]
        public bool IsVisible { get; set; } = true;

        /**
         *
         * ColorIndex Barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(10)]
        public sbyte ColorIndex { get; set; }
    }
}
