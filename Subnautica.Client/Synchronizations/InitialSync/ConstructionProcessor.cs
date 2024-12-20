namespace Subnautica.Client.Synchronizations.InitialSync
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;

    using Subnautica.API.Extensions;
    using Subnautica.API.Features;
    using Subnautica.API.MonoBehaviours;
    using Subnautica.Client.Abstracts.Processors;
    using Subnautica.Network.Models.Storage.Construction;
    using Subnautica.Network.Structures;

    using UWE;

    public class ConstructionProcessor
    {
        /**
         *
         * Dünya yapılarını ayarlar
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static IEnumerator InitialConstructions()
        {
            if (Network.Session.Current.Constructions != null)
            {
                foreach (var construction in Network.Session.Current.Constructions.Where(q => q.IsBasePiece && !q.IsStatic))
                {
                    Network.BaseFacePiece.Add(construction.UniqueId, construction.CellPosition.ToVector3(true), construction.LocalPosition.ToVector3(true), construction.LocalRotation.ToQuaternion(true), construction.FaceDirection, construction.FaceType, construction.TechType);
                }
            }

            yield return Multiplayer.Constructing.Builder.LoadConstructions(Network.Session.Current.ConstructionRoot);

            if (Network.Session.Current.Constructions != null)
            {
                foreach (ConstructionItem construction in Network.Session.Current.Constructions.Where(q => !q.IsStatic))
                {
                    Multiplayer.Constructing.Builder.AddBuilding(construction.Id, construction.UniqueId);
                }
            }
        }

        /**
         *
         * Tamamlanmış yapı parçalarının id'lerini tanımlar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static IEnumerator InitialBasePieceIds()
        {
            yield return CoroutineUtils.waitForNextFrame;

            if (Network.Session.Current.Constructions != null)
            {
                try
                {
                    var constructionBases = GetConstructionBases();

                    foreach (var construction in Network.Session.Current.Constructions.Where(q => q.ConstructedAmount == 1f && q.IsBasePiece && !q.IsStatic))
                    {
                        if (construction.BaseId != null && constructionBases.TryGetValue(construction.BaseId, out var baseTransform))
                        {
                            if (construction.TechType == TechType.BasePartition)
                            {
                                continue;
                            }

                            var cellTransform = baseTransform.GetCellObject(baseTransform.WorldToGrid(construction.PlacePosition.ToVector3()));
                            if (cellTransform == null)
                            {
                                Log.Error(string.Format("[InitialBasePieceIds], Not Found: \"cellTransform\", Type: {0}, Coordinate: {1}, CellPosition: {2}, UniqueId: {3}", construction.TechType, construction.PlacePosition, construction.CellPosition, construction.UniqueId));
                                continue;
                            }

                            var placedPiece = BasePieceComponent.FindBasePiece(cellTransform, construction.CellPosition.ToVector3(), construction.IsFaceHasValue, construction.LocalPosition.ToVector3(), construction.LocalRotation.ToQuaternion(), construction.FaceType, construction.FaceDirection);
                            if (placedPiece == null)
                            {
                                Log.Error(string.Format("[InitialBasePieceIds], Not Found: \"placedPiece\", UniqueId: {0}, Type: {1}, Coordinate: {2}, CellPosition: {3}, CellPosition: {4}, CellPosition: {5}", construction.UniqueId, construction.TechType, construction.PlacePosition, construction.CellPosition, construction.LocalPosition, construction.LocalRotation));
                                continue;
                            }

                            placedPiece.gameObject.SetIdentityId(construction.UniqueId);
                        }
                        else
                        {
                            Log.Error(string.Format("[InitialBasePieceIds], Not Found: \"BaseId\", Type: {0}, Coordinate: {1}, CellPosition: {2}, UniqueId: {3}, BaseId: {4}", construction.TechType, construction.PlacePosition, construction.CellPosition, construction.UniqueId, construction.BaseId));
                        }
                    }
                }
                catch (System.Exception e)
                {
                    Log.Error(string.Format("[InitialBasePieceIds] Exception: {0}", e));
                }
            }
        }

        /**
         *
         * Tamamlanmamış yapıların tamamlanma yüzdesini ayarlar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static IEnumerator InitialInCompleteConstructions()
        {
            yield return CoroutineUtils.waitForNextFrame;

            if (Network.Session.Current.Constructions != null)
            {
                try
                {
                    foreach (var construction in Network.Session.Current.Constructions.Where(q => q.ConstructedAmount != 1f && !q.IsStatic))
                    {
                        if (UniqueIdentifier.TryGetIdentifier(construction.UniqueId, out var uid))
                        {
                            var constructableBase = uid.GetComponentInParent<ConstructableBase>();
                            var constructable     = uid.GetComponentInParent<Constructable>();
                            if (constructable && constructable.model == null)
                            {
                                Log.Error(string.Format("[InitialInCompleteConstructions], Destroyed Piece: {0}, UniqueId: {1}, BaseId: {2}", construction.TechType, construction.UniqueId, construction.BaseId));
                                constructable.gameObject.Destroy();
                                continue;
                            }

                            if (constructableBase != null)
                            {
                                constructableBase.constructedAmount = construction.ConstructedAmount;
                                constructableBase.UpdateMaterial();
                            }
                            else if (constructable != null)
                            {
                                constructable.constructedAmount = construction.ConstructedAmount;
                                constructable.UpdateMaterial();
                            }

                            var builder = Multiplayer.Constructing.Builder.CreateBuilder(construction.UniqueId, construction.TechType);
                            builder.InitializeConstructionMode(constructableBase, constructable);
                        }
                        else
                        {
                            Log.Error(string.Format("[InitialInCompleteConstructions], Not Found: {0}, UniqueId: {1}, BaseId: {2}", construction.TechType, construction.UniqueId, construction.BaseId));
                        }
                    }
                }
                catch (System.Exception e)
                {
                    Log.Error(string.Format("[InitialInCompleteConstructions] Exception: {0}", e));
                }
            }
        }

        /**
         *
         * Metadata verilerini ayarlar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static IEnumerator InitialMetadatas()
        {
            yield return CoroutineUtils.waitForNextFrame;

            try
            {
                if (Network.Session.Current.Constructions != null)
                {
                    foreach (var construction in Network.Session.Current.Constructions)
                    {
                        if (construction.Component == null || construction.IsStatic)
                        {
                            continue;
                        }

                        if (construction.ConstructedAmount == 1f || TechGroup.Planters.Contains(construction.TechType))
                        {
                            MetadataProcessor.ExecuteProcessor(construction.TechType, construction.UniqueId, construction.Component, true);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Log.Error(string.Format("[InitialMetadatas] Exception: {0}", e));
            }
        }

        /**
         *
         * Yapı sağlıklarını senkronlar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static IEnumerator InitialHealths()
        {
            yield return CoroutineUtils.waitForNextFrame;

            if (Network.Session.Current.Constructions != null)
            {
                foreach (var construction in Network.Session.Current.Constructions.Where(q => q.LiveMixin != null))
                {
                    if (construction.LiveMixin.IsHealthFull)
                    {
                        continue;
                    }

                    if (UniqueIdentifier.TryGetIdentifier(construction.UniqueId, out var uid))
                    {
                        var liveMixin = uid.GetComponentInParent<global::LiveMixin>();
                        if (liveMixin)
                        {
                            liveMixin.health = construction.LiveMixin.Health;
                        }
                    }
                    else
                    {
                        Log.Error(string.Format("[InitialHealths], Not Found: {0}, UniqueId: {1}, BaseId: {2}", construction.TechType, construction.UniqueId, construction.BaseId));
                    }
                }
            }
        }

        /**
         *
         * İnşaa edilmiş base listesini döner.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private static Dictionary<string, Base> GetConstructionBases()
        {
            var results = new Dictionary<string, Base>();

            foreach (var item in LargeWorldStreamer.main.globalRoot.GetComponentsInChildren<Base>())
            {
                Log.Info("BASE ID: " + Network.Identifier.GetIdentityId(item.gameObject));
                results.Add(Network.Identifier.GetIdentityId(item.gameObject), item);
            }

            return results;
        }
    }
}