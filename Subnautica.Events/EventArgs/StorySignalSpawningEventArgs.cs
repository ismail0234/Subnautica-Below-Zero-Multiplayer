namespace Subnautica.Events.EventArgs
{
    using System;

    using UnityEngine;

    public class StorySignalSpawningEventArgs : EventArgs
    {
        /**
         *
         * Sınıf ayarlamalarını yapar
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public StorySignalSpawningEventArgs(global::Story.UnlockSignalData.SignalType signalType, Vector3 targetPosition, string targetDescription, bool isAllowed = true)
        {
            this.SignalType         = signalType;
            this.TargetPosition     = targetPosition;
            this.TargetDescription  = targetDescription;
            this.IsAllowed          = isAllowed;
        }

        /**
         *
         * SignalType değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public global::Story.UnlockSignalData.SignalType SignalType { get; set; }

        /**
         *
         * TargetPosition Değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public Vector3 TargetPosition { get; set; }

        /**
         *
         * TargetDescription değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public string TargetDescription { get; set; }

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
