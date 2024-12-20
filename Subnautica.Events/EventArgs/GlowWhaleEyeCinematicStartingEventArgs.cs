namespace Subnautica.Events.EventArgs
{
    using System;

    public class GlowWhaleEyeCinematicStartingEventArgs : EventArgs
    {
        /**
         *
         * Sınıf ayarlamalarını yapar
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public GlowWhaleEyeCinematicStartingEventArgs(string uniqueId, bool isAllowed = true)
        {
            this.UniqueId  = uniqueId;
            this.IsAllowed = isAllowed;
        }

        /**
         *
         * CreatureId Değerini barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public string UniqueId { get; set; }

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
