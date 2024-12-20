namespace Subnautica.Network.Models.Storage.Player
{
    using MessagePack;

    [MessagePackObject]
    public class NotificationItem
    {
        /**
         *
         * Grubu Barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(0)]
        public NotificationManager.Group Group { get; set; }

        /**
         *
         * Key Barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(1)]
        public string Key { get; set; }

        /**
         *
         * IsViewed Barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(2)]
        public bool IsViewed { get; set; }

        /**
         *
         * IsPing Barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(3)]
        public bool IsPing { get; set; }

        /**
         *
         * IsVisibility Barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(4)]
        public bool IsVisible { get; set; }

        /**
         *
         * ColorIndex Barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(5)]
        public sbyte ColorIndex { get; set; }

        /**
         *
         * Sınıf ayarlamalarını yapar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public NotificationItem()
        {

        }

        /**
         *
         * Sınıf ayarlamalarını yapar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public NotificationItem(NotificationManager.Group group, string key, bool isViewed, bool isPing, bool isVisible, sbyte colorIndex)
        {
            this.Group      = group;
            this.Key        = key;
            this.IsViewed   = isViewed;
            this.IsPing     = isPing;
            this.IsVisible  = isVisible;
            this.ColorIndex = colorIndex;
        }
    }
}

