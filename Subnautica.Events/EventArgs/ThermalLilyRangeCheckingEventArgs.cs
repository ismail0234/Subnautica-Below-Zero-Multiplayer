namespace Subnautica.Events.EventArgs
{
    using System;

    using UnityEngine;

    public class ThermalLilyRangeCheckingEventArgs : EventArgs
    {
        /**
         *
         * Sınıf ayarlamalarını yapar
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public ThermalLilyRangeCheckingEventArgs(Vector3 position, float range, bool isPlayerInRange = false, bool isAllowed = true)
        {
            this.LilyPosition    = position;
            this.PlayerRange     = range;
            this.IsPlayerInRange = isPlayerInRange;
            this.IsAllowed       = isAllowed;
        }

        /**
         *
         * Position Değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public Vector3 LilyPosition { get; private set; }

        /**
         *
         * PlayerRange Değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public float PlayerRange { get; set; }

        /**
         *
         * IsPlayerInRange Değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public bool IsPlayerInRange { get; set; }

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