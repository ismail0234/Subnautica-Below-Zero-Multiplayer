namespace Subnautica.API.Features.NetworkUtility
{
    using System.Collections.Generic;
    using System.Linq;

    using Subnautica.API.MonoBehaviours.Components;

    using UnityEngine;

    public class BaseFacePiece
    {
        /**
         *
         * BaseFacePieces değerini barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public Dictionary<BasePieceData, string> BaseFacePieces { get; set; } = new Dictionary<BasePieceData, string>();

        /**
         *
         * BaseFacePiece değerini önbelleğe ekler.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public string Get(BasePieceData basePieceData)
        {
            if (this.BaseFacePieces.TryGetValue(basePieceData, out var uniqueId))
            {
                return uniqueId;
            }

            return null;
        }

        /**
         *
         * BaseFacePiece değerini önbelleğe ekler.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void Add(string uniqueId, Vector3 position, Vector3 localPosition, Quaternion localRotation, Base.Direction faceDirection, Base.FaceType faceType, TechType techType)
        {
            BasePieceData basePieceData = new BasePieceData()
            {
                Position      = position,
                LocalPosition = localPosition,
                LocalRotation = localRotation,
                FaceDirection = faceDirection,
                FaceType      = faceType,
                TechType      = techType
            };

            this.BaseFacePieces[basePieceData] = uniqueId;
        }

        /**
         *
         * BaseFacePiece değerini önbellekten siler.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void Remove(string uniqueId)
        {
            foreach (var item in this.BaseFacePieces.Where(q => q.Value == uniqueId).ToList())
            {
                this.BaseFacePieces.Remove(item.Key);
            }
        }

        /**
         *
         * Verileri temizler.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void Dispose()
        {
            this.BaseFacePieces.Clear();
        }
    }
}