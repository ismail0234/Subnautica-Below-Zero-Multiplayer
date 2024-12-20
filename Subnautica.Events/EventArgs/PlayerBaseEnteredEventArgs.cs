namespace Subnautica.Events.EventArgs
{
    using System;

    public class PlayerBaseEnteredEventArgs : EventArgs
    {
        /**
         *
         * Sınıf ayarlamalarını yapar
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public PlayerBaseEnteredEventArgs(string uniqueId)
        {
            this.UniqueId = uniqueId;
        }

        /**
         *
         * Yapı Kimliği değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public string UniqueId { get; }
    }
}
