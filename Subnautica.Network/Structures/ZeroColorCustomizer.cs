namespace Subnautica.Network.Structures
{
    using MessagePack;

    [MessagePackObject]
    public class ZeroColorCustomizer
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
         * Sınıf ayarlamalarını yapar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public ZeroColorCustomizer()
        {
        }

        /**
         *
         * Sınıf ayarlamalarını yapar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public ZeroColorCustomizer(string name, ZeroColor baseColor, ZeroColor stripColor1, ZeroColor stripColor2, ZeroColor nameColor)
        {
            this.Name         = name;
            this.BaseColor    = baseColor;
            this.StripeColor1 = stripColor1;
            this.StripeColor2 = stripColor2;
            this.NameColor    = nameColor;
        }

        /**
         *
         * Başka sınıftan verileri kopyalar
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void CopyFrom(ZeroColorCustomizer colorCustomizer)
        {
            this.Name         = colorCustomizer.Name;
            this.BaseColor    = colorCustomizer.BaseColor;
            this.StripeColor1 = colorCustomizer.StripeColor1;
            this.StripeColor2 = colorCustomizer.StripeColor2;
            this.NameColor    = colorCustomizer.NameColor;
        }
    }
}