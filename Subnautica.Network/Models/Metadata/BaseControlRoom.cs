namespace Subnautica.Network.Models.Metadata
{
    using MessagePack;

    using Subnautica.Network.Core.Components;
    using Subnautica.Network.Structures;

    [MessagePackObject]
    public class BaseControlRoom : MetadataComponent
    {
        /**
         *
         * Name değerini barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(0)]
        public string Name { get; set; }

        /**
         *
         * BaseColor değerini barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(1)]
        public ZeroColor BaseColor { get; set; }

        /**
         *
         * StripeColor1 değerini barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(2)]
        public ZeroColor StripeColor1 { get; set; }

        /**
         *
         * StripeColor2 değerini barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(3)]
        public ZeroColor StripeColor2 { get; set; }

        /**
         *
         * NameColor değerini barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(4)]
        public ZeroColor NameColor { get; set; }

        /**
         *
         * Minimap değerini barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(5)]
        public BaseControlRoomMinimap Minimap { get; set; }

        /**
         *
         * IsNavigateOpening değerini barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(6)]
        public bool IsNavigateOpening { get; set; }

        /**
         *
         * IsColorCustomizerOpening değerini barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(7)]
        public bool IsColorCustomizerOpening { get; set; }

        /**
         *
         * IsColorCustomizerSave değerini barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(8)]
        public bool IsColorCustomizerSave { get; set; }

        /**
         *
         * IsNavigationExiting değerini barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(9)]
        public bool IsNavigationExiting { get; set; }
    }

    [MessagePackObject]
    public class BaseControlRoomMinimap
    {
        /**
         *
         * Position değerini barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(0)]
        public ZeroVector3 Position { get; set; }

        /**
         *
         * Cell değerini barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(1)]
        public ZeroInt3 Cell { get; set; }

        /**
         *
         * IsPowered değerini barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(2)]
        public bool IsPowered { get; set; }

        /**
         *
         * Sınıf ayarlamalarını yapar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public BaseControlRoomMinimap()
        {

        }

        /**
         *
         * Sınıf ayarlamalarını yapar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public BaseControlRoomMinimap(ZeroVector3 position, ZeroInt3 cell, bool isPowered = false)
        {
            this.Position  = position;
            this.Cell      = cell;
            this.IsPowered = isPowered;
        }
    }
}
