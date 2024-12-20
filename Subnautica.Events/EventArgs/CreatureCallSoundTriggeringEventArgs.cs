namespace Subnautica.Events.EventArgs
{
    using System;

    public class CreatureCallSoundTriggeringEventArgs : EventArgs
    {
        /**
         *
         * Sınıf ayarlamalarını yapar
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public CreatureCallSoundTriggeringEventArgs(string uniqueId, byte callId, string animation = null, bool isAllowed = true)
        {
            this.UniqueId  = uniqueId;
            this.CallId    = callId;
            this.Animation = animation;
            this.IsAllowed = isAllowed;
        }

        /**
         *
         * Yaratık benzersiz ID değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public string UniqueId { get; set; }

        /**
         *
         * CallId değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public byte CallId { get; set; }

        /**
         *
         * Animation değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public string Animation { get; set; }

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
