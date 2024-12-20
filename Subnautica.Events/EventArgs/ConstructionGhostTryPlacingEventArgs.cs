namespace Subnautica.Events.EventArgs
{
    using System;

    using UnityEngine;

    public class ConstructionGhostTryPlacingEventArgs : EventArgs
    {
        /**
         *
         * Sınıf ayarlamalarını yapar
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public ConstructionGhostTryPlacingEventArgs(GameObject ghostModel,string uniqueId, string subrootId, TechType techType, int lastRotation, Vector3 position, Quaternion rotation, Transform aimTranform, bool isCanPlace, bool isBasePiece, bool isError, bool isAllowed = true)
        {
            this.GhostModel   = ghostModel;
            this.UniqueId     = uniqueId;
            this.SubrootId    = subrootId;
            this.TechType     = techType;
            this.LastRotation = lastRotation;
            this.Position     = position;
            this.Rotation     = rotation;
            this.AimTransform = aimTranform;
            this.IsCanPlace   = isCanPlace;
            this.IsBasePiece  = isBasePiece;
            this.IsError      = isError;
            this.IsAllowed    = isAllowed;
        }

        /**
         *
         * GhostModel
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public GameObject GhostModel { get; private set; }

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
         * SubrootId Değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public string SubrootId { get; private set; }

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
         * LastRotation Değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public int LastRotation { get; private set; }

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
         * Rotation Değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public Quaternion Rotation { get; private set; }

        /**
         *
         * AimTransform Değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public Transform AimTransform { get; private set; }

        /**
         *
         * IsCanPlace Değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public bool IsCanPlace { get; private set; }

        /**
         *
         * IsBasePiece Değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public bool IsBasePiece { get; private set; }

        /**
         *
         * IsError Değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public bool IsError { get; private set; }

        /**
         *
         * IsAllowed Değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public bool IsAllowed { get; set; }
    }
}
