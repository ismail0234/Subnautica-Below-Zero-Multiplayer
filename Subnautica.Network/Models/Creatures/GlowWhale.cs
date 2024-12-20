namespace Subnautica.Network.Models.Creatures
{
    using MessagePack;

    using Subnautica.API.Enums.Creatures;
    using Subnautica.Network.Core.Components;

    [MessagePackObject]
    public class GlowWhale : NetworkCreatureComponent
    {
        /**
         *
         * IsRideStart değerini barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(0)]
        public bool IsRideStart { get; set; }

        /**
         *
         * IsRideEnd değerini barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(1)]
        public bool IsRideEnd { get; set; }

        /**
         *
         * IsEyeInteract değerini barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(2)]
        public bool IsEyeInteract { get; set; }

        /**
         *
         * SFXType değerini barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(3)]
        public GlowWhaleSFXType SFXType { get; set; }

        /**
         *
         * Sınıf ayarlamalarını yapar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public GlowWhale()
        {

        }

        /**
         *
         * Sınıf ayarlamalarını yapar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public GlowWhale(bool isRideStart, bool isRideEnd, bool isEyeInteract, GlowWhaleSFXType sfxType)
        {
            this.IsRideStart   = isRideStart;
            this.IsRideEnd     = isRideEnd;
            this.IsEyeInteract = isEyeInteract;
            this.SFXType       = sfxType;
        }
    }
}