namespace Subnautica.Events.EventArgs
{
    using System;

    public class SupplyCrateOpenedEventArgs : EventArgs
    {
        /**
         *
         * Sınıf ayarlamalarını yapar
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public SupplyCrateOpenedEventArgs(string uniqueId)
        {
            this.UniqueId = uniqueId;
        }

        /**
         *
         * Kimlik
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public string UniqueId { get; private set; }
    }
}
