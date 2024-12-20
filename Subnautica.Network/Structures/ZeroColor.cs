namespace Subnautica.Network.Structures
{
    using MessagePack;

    [MessagePackObject]
    public class ZeroColor
    {
        /**
         *
         * Renk (R)
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(0)]
        public float R { get; set; }

        /**
         *
         * Renk (G)
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(1)]
        public float G { get; set; }

        /**
         *
         * Renk (B)
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(2)]
        public float B { get; set; }

        /**
         *
         * Renk (Alpha)
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(3)]
        public float A { get; set; }

        /**
         *
         * Sınıf ayarlamalarını yapar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public ZeroColor()
        {
        }

        /**
         *
         * Sınıf ayarlamalarını yapar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public ZeroColor(float r, float g, float b, float a = 1)
        {
            this.R = r;
            this.G = g;
            this.B = b;
            this.A = a;
        }

        /**
         *
         * Metin olarak bastırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public override string ToString()
        {
            return $"[ZeroColor: {this.R}, {this.G}, {this.B}, {this.A}]";
        }
    }
}
