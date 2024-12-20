namespace Subnautica.Events.EventArgs
{
    using System;

    public class TechnologyFragmentAddedEventArgs : EventArgs
    {
        /**
         *
         * Sınıf ayarlamalarını yapar
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public TechnologyFragmentAddedEventArgs(string uniqueId, TechType type, int unlocked, int totalFragment)
        {
            this.UniqueId      = uniqueId;
            this.TechType      = type;
            this.Unlocked      = unlocked;
            this.TotalFragment = totalFragment;
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
         * Unlocked Değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public int Unlocked { get; private set; }

        /**
         *
         * TotalFragment
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public int TotalFragment { get; private set; }
    }
}
