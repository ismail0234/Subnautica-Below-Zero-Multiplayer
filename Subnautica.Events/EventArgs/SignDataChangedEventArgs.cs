namespace Subnautica.Events.EventArgs
{
    using System;

    public class SignDataChangedEventArgs : EventArgs
    {
        /**
         *
         * Sınıf ayarlamalarını yapar
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public SignDataChangedEventArgs(string uniqueId, TechType techType, string text, int scaleIndex, int colorIndex, bool[] elementsState, bool isBackgroundEnabled)
        {
            this.UniqueId            = uniqueId;
            this.TechType            = techType;
            this.Text                = text;
            this.ScaleIndex          = scaleIndex;
            this.ColorIndex          = colorIndex;
            this.ElementsState       = elementsState;
            this.IsBackgroundEnabled = isBackgroundEnabled;
        }

        /**
         *
         * UniqueId Değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public string UniqueId { get; private set; }

        /**
         *
         * TechType Değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public TechType TechType { get; private set; }

        /**
         *
         * Text Değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public string Text { get; private set; }

        /**
         *
         * ScaleIndex Değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public int ScaleIndex { get; private set; }

        /**
         *
         * ColorIndex Değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public int ColorIndex { get; private set; }

        /**
         *
         * ElementsState Değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public bool[] ElementsState { get; private set; }

        /**
         *
         * IsBackgroundEnabled Değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public bool IsBackgroundEnabled { get; private set; }
    }
}
