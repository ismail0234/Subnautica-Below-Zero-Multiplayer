namespace Subnautica.Events.Patches.Identity.Building
{
    using System;

    using HarmonyLib;

    using Subnautica.API.Extensions;
    using Subnautica.API.Features;
    using Subnautica.API.MonoBehaviours;
    using Subnautica.API.MonoBehaviours.Components;

    using UnityEngine;

    [HarmonyPatch]
    public class BaseGhostFinished
    {
        [HarmonyPrefix]
        [HarmonyPriority(Priority.First)]
        [HarmonyPatch(typeof(global::BaseGhost), nameof(global::BaseGhost.Finish))]
        public static void BaseGhost_Finish(BaseGhost __instance)
        {
            if (Network.IsMultiplayerActive)
            {
                try
                {

                    if (__instance.GhostBase.gameObject.GetComponent<BasePieceComponent>())
                    {
                        ErrorMessage.AddMessage("EXISTS ALERT. TemporaryBehaviour -> PLEASE REPORT");
                        Log.Error("EXISTS TemporaryBehaviour -> PLEASE REPORT");
                    }
                    else
                    {
                        var constructableBase = __instance.GetComponentInParent<ConstructableBase>();
                        if (constructableBase)
                        {
                            var basePieceComponent = __instance.GhostBase.gameObject.EnsureComponent<BasePieceComponent>();
                            basePieceComponent.SetUniqueId(constructableBase.gameObject.GetIdentityId());
                            basePieceComponent.SetPlacePosition(__instance.transform.position);
                        }
                    }
                }
                catch (Exception e)
                {
                    Log.Error($"BaseGhostFinished.BaseGhost_Finish: {e}\n{e.StackTrace}");
                }
            }
        }

        [HarmonyPrefix]
        [HarmonyPriority(Priority.First)]
        [HarmonyPatch(typeof(global::Base), nameof(global::Base.CopyFrom))]
        public static void Base_CopyFrom_Prefix(global::Base __instance, Base sourceBase, Int3.Bounds sourceRange, Int3 offset)
        {
            if (Network.IsMultiplayerActive)
            {
                try
                {
                    if (sourceBase.TryGetComponent<BasePieceComponent>(out var basePieceComponent))
                    {
                        var cellTransform = __instance.GetCellObject(__instance.WorldToGrid(basePieceComponent.PlacePosition));
                        if (cellTransform)
                        {
                            basePieceComponent.SetBasePieces(cellTransform);
                        }
                    }
                }
                catch (Exception e)
                {
                    Log.Error($"BaseGhostFinished.Base_CopyFrom_Prefix: {e}\n{e.StackTrace}");
                }
            }
        }

        [HarmonyPostfix]
        [HarmonyPriority(Priority.First)]
        [HarmonyPatch(typeof(global::Base), nameof(global::Base.CopyFrom))]
        public static void Base_CopyFrom_Postfix(global::Base __instance, Base sourceBase, Int3.Bounds sourceRange, Int3 offset)
        {
            if (Network.IsMultiplayerActive)
            {
                try
                {
                    if (sourceBase.TryGetComponent<BasePieceComponent>(out var basePieceComponent))
                    {
                        var cellTransform = __instance.GetCellObject(__instance.WorldToGrid(basePieceComponent.PlacePosition));
                        if (cellTransform == null)
                        {
                            Log.Info(string.Format("Base_CopyFrom_Postfix.Postfix - cellTransform is null: {0}, Pos: {1}", __instance.WorldToGrid(basePieceComponent.PlacePosition), basePieceComponent.PlacePosition));
                        }
                        else
                        {
                            var basePiece = basePieceComponent.GetAddedPiece(cellTransform);
                            if (basePiece != null)
                            {
                                if (basePiece.CurrentTransform)
                                {
                                    Network.Identifier.SetIdentityId(basePiece.CurrentTransform.gameObject, basePieceComponent.UniqueId);
                                }

                                Network.BaseFacePiece.Add(basePieceComponent.UniqueId, basePiece.Position, basePiece.LocalPosition, basePiece.LocalRotation, basePiece.FaceDirection, basePiece.FaceType, basePiece.TechType);
                            }
                        }

                        UnityEngine.GameObject.Destroy(basePieceComponent);
                    }
                }
                catch (Exception e)
                {
                    Log.Error($"BaseGhostFinished.Base_CopyFrom_Postfix: {e}\n{e.StackTrace}");
                }
            }
        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(global::BaseDeconstructable), nameof(global::BaseDeconstructable.Init))]
        private static bool BaseDeconstructable_Init_Prefix(global::BaseDeconstructable __instance, Int3.Bounds bounds, Base.Face face, Base.FaceType faceType, TechType recipe)
        {
            if (!Network.IsMultiplayerActive)
            {
                return true;
            }

            if (recipe != TechType.BasePartition)
            {
                return true;
            }

            BasePieceData basePiece = new BasePieceData()
            {
                Position      = __instance.transform.position,
                LocalPosition = __instance.transform.localPosition,
                LocalRotation = __instance.transform.localRotation,
                FaceDirection = face.direction,
                FaceType      = faceType,
                TechType      = recipe,
            };

            string uniqueId = Network.BaseFacePiece.Get(basePiece);
            if (uniqueId.IsNull())
            {
                return true;
            }

            Network.Identifier.SetIdentityId(__instance.gameObject, uniqueId);
            return true;
        }

        [HarmonyPostfix]
        [HarmonyPatch(typeof(global::BaseDeconstructable), nameof(global::BaseDeconstructable.MakeCellDeconstructable))]
        private static void BaseDeconstructable_MakeCellDeconstructable_Prefix(ref BaseDeconstructable __result, Transform geometry, TechType recipe)
        {
            if (Network.IsMultiplayerActive && __result.deconstructedBase && __result.deconstructedBase.isGhost == false)
            {
                BasePieceData basePiece = new BasePieceData()
                {
                    Position      = __result.transform.position,
                    LocalPosition = geometry.transform.localPosition,
                    LocalRotation = geometry.transform.localRotation,
                    FaceDirection = (Base.Direction) 0,
                    FaceType      = __result.faceType,
                    TechType      = recipe,
                };

                var uniqueId = Network.BaseFacePiece.Get(basePiece);
                if (uniqueId.IsNotNull() && __result.gameObject.GetIdentityId() != uniqueId)
                {
                    Network.Identifier.SetIdentityId(__result.gameObject, uniqueId);
                }
            }
        }
    }
}