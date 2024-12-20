namespace Subnautica.Events.EventArgs
{
    using System;

    using UnityEngine;

    public class ConstructionCompletedEventArgs : EventArgs
    {
        /**
         *
         * Sınıf ayarlamalarını yapar
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public ConstructionCompletedEventArgs(string uniqueId, string baseId, TechType techType, Vector3 cellPosition, bool isFaceHasValue = false, Vector3 localPosition = new Vector3(), Quaternion localRotation = new Quaternion(), Base.Direction faceDirection = Base.Direction.North, Base.FaceType faceType = Base.FaceType.None)
        {
            this.UniqueId       = uniqueId;
            this.BaseId         = baseId;
            this.TechType       = techType;
            this.CellPosition   = cellPosition;
            this.IsFaceHasValue = isFaceHasValue;
            this.LocalPosition  = localPosition;
            this.LocalRotation  = localRotation;
            this.FaceDirection  = faceDirection;
            this.FaceType       = faceType;
        }

        /**
         *
         * Kimlik
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public string UniqueId { get; private set; }

        /**
         *
         * Base Kimliği
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public string BaseId { get; private set; }
        
        /**
         *
         * TechType Değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public TechType TechType { get; private set; }

        /**
         *
         * CellPosition Değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public Vector3 CellPosition { get; private set; }

        /**
         *
         * IsFaceHasValue Değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public bool IsFaceHasValue { get; private set; }

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
         * LocalRotation Değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public Quaternion LocalRotation { get; private set; }

        /**
         *
         * FaceDirection Değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public Base.Direction FaceDirection { get; private set; }

        /**
         *
         * FaceType Değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public Base.FaceType FaceType { get; private set; }
    }
}
