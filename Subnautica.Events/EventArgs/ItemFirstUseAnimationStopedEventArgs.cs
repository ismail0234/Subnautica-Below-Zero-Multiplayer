namespace Subnautica.Events.EventArgs
{
    using System;

    public class ItemFirstUseAnimationStopedEventArgs : EventArgs
    {
        /**
         *
         * Sınıf ayarlamalarını yapar
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public ItemFirstUseAnimationStopedEventArgs(TechType techType)
        {
            this.TechType = techType;
        }

        /**
         *
         * TechType Değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public TechType TechType { get; private set; }
    }
}
