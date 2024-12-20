namespace Subnautica.API.MonoBehaviours
{
    using System.Collections.Generic;
    using System.Linq;

    using Subnautica.API.Extensions;
    using Subnautica.API.Features;
    using Subnautica.API.MonoBehaviours.Components;

    using UnityEngine;

    public class BasePieceComponent : MonoBehaviour
    {
        /**
         *
         * UniqueId değerini barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public string UniqueId { get; set; }

        /**
         *
         * PlacePosition değerini barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public Vector3 PlacePosition { get; set; }

        /**
         *
         * BasePieces değerini barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public List<BasePieceData> BasePieces { get; set; } = new List<BasePieceData>();

        /**
         *
         * UniqueId veriyi günceller.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void SetUniqueId(string uniqueId)
        {
            this.UniqueId = uniqueId;
        }

        /**
         *
         * baseTransform veriyi günceller.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void SetBasePieces(Transform baseTransform)
        {
            this.BasePieces = ParseBasePieces(baseTransform);
        }

        /**
         *
         * PlacePosition veriyi günceller.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void SetPlacePosition(Vector3 placePosition)
        {
            this.PlacePosition = placePosition;
        }

        /**
         *
         * En son eklenen base parçasını döner.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public BasePieceData GetAddedPiece(Transform baseTransform)
        {
            var lastBasePieces = ParseBasePieces(baseTransform, true);
            if (lastBasePieces.Count <= 0)
            {
                Log.Error(string.Format("GetAddedPiece First - lastBasePieces: 0"));
                return null;
            }




            /*
            int differentChildCount = lastBasePieces.Count - this.BasePieces.Count;
            if (differentChildCount != 1)
            {
                Log.Error(string.Format("GetAddedPiece Two - lastBasePieces: {0}, BasePieces: {1}, Different: {2}", lastBasePieces.Count, this.BasePieces.Count, differentChildCount));
                return null;
            }*/

            var different = lastBasePieces.Except(this.BasePieces).ToList();
            if (different.Count <= 0)
            {
                return null;
            }
            
            if (different.Count > 1)
            {
                Log.Error(string.Format("GetAddedPiece Three - lastBasePieces: {0}, BasePieces: {1}, Different: {2}", lastBasePieces.Count, this.BasePieces.Count, different.Count));
             //   return null;
            }

            return different.GetLast();
        }

        /**
         *
         * ParseBasePieces parçala işlemini yapar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static List<BasePieceData> ParseBasePieces(Transform baseTransform, bool isTransformStorage = false)
        {
            if (baseTransform == null)
            {
                return new List<BasePieceData>();
            }

            List<BasePieceData> pieces = new List<BasePieceData>();

            if (isTransformStorage)
            {
                foreach (var deconstructable in baseTransform.GetComponentsInChildren<BaseDeconstructable>())
                {
                    if (deconstructable.recipe == TechType.BasePartition)
                    {
                        BasePieceData basePieceData = new BasePieceData()
                        {
                            Position      = deconstructable.transform.position,
                            LocalPosition = deconstructable.transform.localPosition,
                            LocalRotation = deconstructable.transform.localRotation,
                            TechType      = deconstructable.recipe,
                        };

                        if (deconstructable.face != null && deconstructable.face.HasValue)
                        {
                            basePieceData.FaceDirection = deconstructable.face.Value.direction;
                            basePieceData.FaceType      = deconstructable.faceType;
                        }

                        if (isTransformStorage)
                        {
                            basePieceData.CurrentTransform = deconstructable.transform;
                        }

                        pieces.Add(basePieceData);
                    }
                }

                var query = pieces.GroupBy(x => x).Where(g => g.Count() == 1).Select(y => y.Key).ToList();
                if (query != null && query.Count == 1)
                {
                    return query;
                }

                pieces.Clear();
            }

            foreach (Transform child in baseTransform)
            {
                if (child.name.Contains("CorridorConnector"))
                {
                    continue;
                }

                var baseDeconstructable = child.GetComponent<BaseDeconstructable>();
                if (baseDeconstructable == null)
                {
                    continue;
                }

                BasePieceData basePieceData = new BasePieceData()
                {
                    Position      = child.position,
                    LocalPosition = child.transform.localPosition,
                    LocalRotation = child.transform.localRotation,
                    TechType      = baseDeconstructable.recipe,
                };

                if (baseDeconstructable.face != null && baseDeconstructable.face.HasValue)
                {
                    basePieceData.FaceDirection = baseDeconstructable.face.Value.direction;
                    basePieceData.FaceType      = baseDeconstructable.faceType;
                }

                if (isTransformStorage)
                {
                    basePieceData.CurrentTransform = child;
                }

                pieces.Add(basePieceData);
            }

            return pieces;
        }

        /**
         *
         * Base parçasını arar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static GameObject FindBasePiece(Transform cellTransform, Vector3 cellPosition, bool isFaceHasValue, Vector3 localPosition, Quaternion localRotation, Base.FaceType faceType, Base.Direction faceDirection)
        {
            if (faceType == Base.FaceType.Partition)
            {
                return null;

                /*
                Log.Info("FIND ITEM CellPos: " + cellPosition + ", LocalPos: " + localPosition + ", localRot: " + localRotation + ", faceType: " + faceType + ", direction: " + faceDirection);

                GameObject gameObject = null;
                foreach (var child in cellTransform.GetComponentsInChildren<BaseDeconstructable>())
                {
                    if (child.recipe != TechType.BasePartition)
                    {
                        continue;
                    }

                    // child.transform.position == cellPosition && 
                    if (gameObject == null && child.transform.localPosition == localPosition && child.transform.localRotation == localRotation && child.face.Value.direction == faceDirection && child.faceType == faceType)
                    {
                        gameObject = child.gameObject;

                        // Log.Info("[+] MODULE: CellPos: " + child.transform.position + ", LocalPos: " + child.transform.localPosition + ", localrotation: " + localRotation + ", faceType: " + child.faceType + ", direction: " + child.face.Value.direction + ", faceCell: " + child.face.Value.cell);
                    }
                    else
                    {
                        // Log.Info("[-] MODULE: CellPos: " + child.transform.position + ", LocalPos: " + child.transform.localPosition + ", localrotation: " + localRotation  + ", faceType: " + child.faceType + ", direction: " + child.face.Value.direction + ", faceCell: " + child.face.Value.cell);
                    }
                }

                if (gameObject)
                {
                    return gameObject;
                }

                return null;
                */
            }

            var isBulkhead = faceType == Base.FaceType.BulkheadOpened || faceType == Base.FaceType.BulkheadClosed;

            foreach (Transform child in cellTransform)
            {
                if (child.name.Contains("CorridorConnector"))
                {
                    continue;
                }

                if (child.transform.position != cellPosition)
                {
                    continue;
                }

                if (!child.TryGetComponent<BaseDeconstructable>(out var baseDeconstructable))
                {
                    continue;
                }

                if (!isFaceHasValue)
                {
                    if (!baseDeconstructable.face.HasValue)
                    {
                        return child.gameObject;
                    }

                    return null;
                }

                if (baseDeconstructable.face == null || !baseDeconstructable.face.HasValue)
                {
                    continue;
                }


                if (child.transform.localPosition == localPosition && child.transform.localRotation == localRotation && baseDeconstructable.face.Value.direction == faceDirection)
                {
                    if (isBulkhead)
                    {
                        if ((baseDeconstructable.faceType == Base.FaceType.BulkheadClosed || baseDeconstructable.faceType == Base.FaceType.BulkheadOpened))
                        {
                            return child.gameObject;
                        }
                    }
                    else if (baseDeconstructable.faceType == faceType)
                    {
                        return child.gameObject;

                    }
                }
            }

            return null;
        }
    }
}