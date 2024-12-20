namespace Subnautica.Events.EventArgs
{
    using System;

    public class BeaconLabelChangedEventArgs : EventArgs
    {
        /**
         *
         * Sınıf ayarlamalarını yapar
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public BeaconLabelChangedEventArgs(string uniqueId, string text)
        {
            this.UniqueId = uniqueId;
            this.Text     = text;
        }

        /**
         *
         * UniqueId Değerini barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public string UniqueId { get; set; }

        /**
         *
         * Text Değerini barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public string Text { get; set; }
    }
}
