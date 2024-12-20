namespace Subnautica.API.MonoBehaviours.Components
{
    using UnityEngine;

    public class BasePieceData
    {
        /**
         *
         * Position Değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public Vector3 Position { get; set; }

        /**
         *
         * LocalPosition Değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public Vector3 LocalPosition { get; set; }

        /**
         *
         * LocalRotation Değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public Quaternion LocalRotation { get; set; }

        /**
         *
         * Transform Değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public Transform CurrentTransform { get; set; }

        /**
         *
         * FaceDirection Değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public Base.Direction FaceDirection { get; set; }

        /**
         *
         * FaceType Değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public Base.FaceType FaceType { get; set; }

        /**
         *
         * TechType Değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public TechType TechType { get; set; } = TechType.None;

        /**
         *
         * Karşılaştırma Yapar
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public override bool Equals(System.Object obj)
        {
            BasePieceData basePieceData = obj as BasePieceData;
            if (basePieceData is null)
            {
                return false;
            }

            return basePieceData.Position == this.Position && basePieceData.LocalPosition == this.LocalPosition && basePieceData.LocalRotation == this.LocalRotation && basePieceData.FaceDirection == this.FaceDirection && basePieceData.FaceType == this.FaceType && basePieceData.TechType == this.TechType;
        }

        /**
         *
         * Karşılaştırma Yapar
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public override int GetHashCode()
        {
            return (this.Position, this.LocalPosition, this.LocalRotation, this.FaceDirection, this.FaceType).GetHashCode();
        }
    }
}
