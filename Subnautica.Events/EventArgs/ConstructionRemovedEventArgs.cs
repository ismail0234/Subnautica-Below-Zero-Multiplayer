namespace Subnautica.Events.EventArgs
{
    using System;

    public class ConstructionRemovedEventArgs : EventArgs
    {
        /**
         *
         * Sınıf ayarlamalarını yapar
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public ConstructionRemovedEventArgs(TechType techType, string uniqueId, Int3? cell = null)
        {
            this.TechType = techType;
            this.UniqueId = uniqueId;
            this.Cell     = cell;
        }

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
         * Kimlik
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public string UniqueId { get; private set; }

        /**
         *
         * Cell değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public Int3? Cell { get; private set; }
    }
}
