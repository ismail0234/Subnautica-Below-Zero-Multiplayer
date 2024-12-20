namespace Subnautica.Events.EventArgs
{
    using System;

    public class LilyPaddlerHypnotizeStartingEventArgs : EventArgs
    {
        /**
         *
         * Sınıf ayarlamalarını yapar
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public LilyPaddlerHypnotizeStartingEventArgs(string creatureId, byte targetId, bool isAllowed = true)
        {
            this.CreatureId = creatureId;
            this.TargetId   = targetId;
            this.IsAllowed  = isAllowed;
        }

        /**
         *
         * CreatureId Değerini barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public string CreatureId { get; set; }

        /**
         *
         * TargetId Değerini barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public byte TargetId { get; set; }

        /**
         *
         * IsAllowed Değerini barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public bool IsAllowed { get; set; }
    }
}