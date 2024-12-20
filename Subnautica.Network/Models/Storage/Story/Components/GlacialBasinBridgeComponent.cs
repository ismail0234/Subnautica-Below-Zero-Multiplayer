namespace Subnautica.Network.Models.Storage.Story.Components
{
    using MessagePack;

    [MessagePackObject]
    public class GlacialBasinBridgeComponent
    {
        /**
         *
         * IsExtended Değerini barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(0)]
        public bool IsExtended { get; set; }

        /**
         *
         * IsFirstExtension Değerini barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(1)]
        public bool IsFirstExtension { get; set; }

        /**
         *
         * IsExtended Değerini barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(2)]
        public float Time { get; set; }

        /**
         *
         * Köprüyü uzatır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public bool Extend(float serverTime, float animationTime)
        {
            if (serverTime < this.Time)
            {
                return false;
            }

            this.IsExtended = true;
            this.Time       = serverTime + animationTime;

            this.IsFirstExtension = true;
            return true;
        }

        /**
         *
         * Köprüyü uzatır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public bool Retract(float serverTime, float animationTime)
        {
            if (serverTime < this.Time)
            {
                return false;
            }

            this.IsExtended = false;
            this.Time       = serverTime + animationTime;
            return true;
        }
    }
}