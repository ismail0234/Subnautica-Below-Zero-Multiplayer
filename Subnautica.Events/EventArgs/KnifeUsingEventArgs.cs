namespace Subnautica.Events.EventArgs
{
    using System;

    using UnityEngine;

    public class KnifeUsingEventArgs : EventArgs
    {
        /**
         *
         * Sınıf ayarlamalarını yapar
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public KnifeUsingEventArgs(VFXEventTypes vFXEventType, Vector3 targetPosition, Vector3 orientation, VFXSurfaceTypes surfaceType, VFXSurfaceTypes soundSurfaceType, bool isUnderwater)
        {
            this.VFXEventType     = vFXEventType;
            this.TargetPosition   = targetPosition;
            this.Orientation      = orientation;
            this.SurfaceType      = surfaceType;
            this.SoundSurfaceType = soundSurfaceType;
            this.IsUnderwater     = isUnderwater;
        }
        
        /**
         *
         * VFXEventType Değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public VFXEventTypes VFXEventType { get; set; }
        
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
         * Orientation Değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public Vector3 Orientation { get; set; }
                
        /**
         *
         * SurfaceType Değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public VFXSurfaceTypes SurfaceType { get; set; }
                
        /**
         *
         * SoundSurfaceType Değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public VFXSurfaceTypes SoundSurfaceType { get; set; }

        /**
         *
         * IsUnderwater Değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public bool IsUnderwater { get; set; }
    }
}
