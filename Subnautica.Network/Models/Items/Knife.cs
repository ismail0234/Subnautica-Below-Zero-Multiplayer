namespace Subnautica.Network.Models.Items
{
    using MessagePack;

    using Subnautica.Network.Core.Components;
    using Subnautica.Network.Structures;

    [MessagePackObject]
    public class Knife : NetworkPlayerItemComponent
    {
        /**
         *
         * TechType değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(1)]
        public override TechType TechType { get; set; } = TechType.Knife;

        /**
         *
         * VFXEventType Değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(4)]
        public VFXEventTypes VFXEventType { get; set; }

        /**
         *
         * TargetPosition Değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(5)]
        public ZeroVector3 TargetPosition { get; set; }

        /**
         *
         * Orientation Değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(6)]
        public ZeroVector3 Orientation { get; set; }

        /**
         *
         * SurfaceType Değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(7)]
        public VFXSurfaceTypes SurfaceType { get; set; }

        /**
         *
         * SoundSurfaceType Değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(8)]
        public VFXSurfaceTypes SoundSurfaceType { get; set; }

        /**
         *
         * IsUnderwater Değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(9)]
        public bool IsUnderwater { get; set; }
    }
}