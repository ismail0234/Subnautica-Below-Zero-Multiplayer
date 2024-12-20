namespace Subnautica.Network.Models.Creatures
{
    using MessagePack;

    using Subnautica.Network.Core.Components;

    [MessagePackObject]
    public class LilyPaddler : NetworkCreatureComponent
    {
        /**
         *
         * TargetId değerini barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(0)]
        public byte TargetId { get; set; }

        /**
         *
         * LastHypnotizeTime değerini barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(1)]
        public float LastHypnotizeTime { get; set; }

        /**
         *
         * Sınıf ayarlamalarını yapar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public LilyPaddler()
        {

        }

        /**
         *
         * Sınıf ayarlamalarını yapar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public LilyPaddler(byte targetId)
        {
            this.TargetId = targetId;
        }
    }
}