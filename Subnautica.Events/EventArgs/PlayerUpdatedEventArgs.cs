namespace Subnautica.Events.EventArgs
{
    using System;
    using System.Collections.Generic;

    using UnityEngine;

    public class PlayerUpdatedEventArgs : EventArgs
    {
        /**
         *
         * Sınıf ayarlamalarını yapar
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public PlayerUpdatedEventArgs(Vector3 position, Vector3 localPosition, Quaternion rotation, TechType techTypeInHand, List<TechType> equipments, float cameraPitch, Vector3 cameraForward, float emoteIndex, bool isPrecursorArm, VFXSurfaceTypes surfaceType)
        {
            this.Position       = position;
            this.LocalPosition  = localPosition;
            this.Rotation       = rotation;
            this.TechTypeInHand = techTypeInHand;
            this.Equipments     = equipments;
            this.CameraPitch    = cameraPitch;
            this.CameraForward  = cameraForward;
            this.EmoteIndex     = emoteIndex;
            this.IsPrecursorArm = isPrecursorArm;
            this.SurfaceType    = surfaceType;
        }

        /**
         *
         * Position Değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public Vector3 Position { get; private set; }

        /**
         *
         * LocalPosition Değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public Vector3 LocalPosition { get; private set; }

        /**
         *
         * Rotation Değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public Quaternion Rotation { get; private set; }

        /**
         *
         * Oyuncu elindeki teknoloji türü
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public TechType TechTypeInHand { get; private set; }

        /**
         *
         * Ekipmanları barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public List<TechType> Equipments { get; private set; }

        /**
         *
         * CameraPitch değerini barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public float CameraPitch { get; private set; }

        /**
         *
         * CameraForward değerini barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public Vector3 CameraForward { get; private set; }

        /**
         *
         * EmoteIndex değerini barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public float EmoteIndex { get; private set; }

        /**
         *
         * IsPrecursorArm değerini barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public bool IsPrecursorArm { get; private set; }

        /**
         *
         * SurfaceType değerini barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public VFXSurfaceTypes SurfaceType { get; private set; }
    }
}
