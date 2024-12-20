namespace Subnautica.Events.EventArgs
{
    using System;

    public class DeconstructionBeginEventArgs : EventArgs
    {
        /**
         *
         * Sınıf ayarlamalarını yapar
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public DeconstructionBeginEventArgs(string uniqueId, global::BaseDeconstructable baseDeconstructable, TechType techType, bool isAllowed = true)
        {
            this.UniqueId            = uniqueId;
            this.BaseDeconstructable = baseDeconstructable;
            this.TechType            = techType;
            this.IsAllowed           = isAllowed;
        }

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
         * BaseDeconstructable
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public global::BaseDeconstructable BaseDeconstructable { get; private set; }

        /**
         *
         * TechType
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public TechType TechType { get; private set; }

        /**
         *
         * Olayın çalıştırılıp/çalıştırılmayacağı
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public bool IsAllowed { get; set; }
    }
}
