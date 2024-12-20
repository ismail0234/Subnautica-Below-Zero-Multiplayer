namespace Subnautica.Events.EventArgs
{
    using System;

    using UnityEngine;

    public class SubNameInputDeselectedEventArgs : EventArgs
    {
        /**
         *
         * Sınıf ayarlamalarını yapar
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public SubNameInputDeselectedEventArgs(string uniqueId, TechType techType, string name, Color baseColor, Color stripeColor1, Color stripeColor2, Color nameColor)
        {
            this.UniqueId     = uniqueId;
            this.TechType     = techType;
            this.Name         = name;
            this.BaseColor    = baseColor;
            this.StripeColor1 = stripeColor1;
            this.StripeColor2 = stripeColor2;
            this.NameColor    = nameColor;
        }

        /**
         *
         * UniqueId değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public string UniqueId { get; set; }

        /**
         *
         * Name değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public string Name { get; set; }

        /**
         *
         * BaseColor değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public Color BaseColor { get; set; }

        /**
         *
         * StripeColor1 değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public Color StripeColor1 { get; set; }

        /**
         *
         * StripeColor2 değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public Color StripeColor2 { get; set; }

        /**
         *
         * NameColor değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public Color NameColor { get; set; }

        /**
         *
         * TechType değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public TechType TechType { get; set; }
    }
}