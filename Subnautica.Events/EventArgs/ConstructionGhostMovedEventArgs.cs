namespace Subnautica.Events.EventArgs
{
    using System;

    using Subnautica.API.Extensions;
    using Subnautica.API.Features;

    using UnityEngine;

    public class ConstructionGhostMovedEventArgs : EventArgs
    {
        /**
         *
         * Sınıf ayarlamalarını yapar
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public ConstructionGhostMovedEventArgs(GameObject ghostModel, TechType techType, Transform aimTranform, bool isCanPlace, int lastRotation)
        {
            this.GhostModel      = ghostModel;
            this.UniqueId        = ghostModel.GetIdentityId(true);
            this.TechType        = techType;
            this.Position        = ghostModel.transform.position;
            this.Rotation        = ghostModel.transform.rotation;
            this.AimTransform    = aimTranform;
            this.IsCanPlace      = isCanPlace;
            this.UpdatePlacement = Network.Temporary.GetProperty<bool>(ghostModel.GetIdentityId(), "UpdatePlacementResult");
            this.LastRotation    = lastRotation;
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
         * TechType Değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public TechType TechType { get; private set; }

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
         * Kimlik
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public string UniqueId { get; private set; }

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
         * UpdatePlacement Değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public bool UpdatePlacement { get; private set; }

        /**
         *
         * Son Açı Değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public int LastRotation { get; private set; }
    }
}
