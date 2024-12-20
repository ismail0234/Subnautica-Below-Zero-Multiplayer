namespace Subnautica.Events.EventArgs
{
    using System;

    using UnityEngine;

    public class ThermalLilyAnimationAnglesCheckingEventArgs : EventArgs
    {
        /**
         *
         * Sınıf ayarlamalarını yapar
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public ThermalLilyAnimationAnglesCheckingEventArgs(Vector3 position, float range, Vector3 playerPosition = default(Vector3), bool isAllowed = true)
        {
            this.LilyPosition   = position;
            this.PlayerRange    = range;
            this.PlayerPosition = playerPosition;
            this.IsAllowed      = isAllowed;
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
         * PlayerPosition Değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public Vector3 PlayerPosition { get; set; }

        /**
         *
         * PlayerRange Değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public float PlayerRange { get; private set; }

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