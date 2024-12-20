namespace Subnautica.Events.Patches.Events.Building
{
    using HarmonyLib;
    using Subnautica.API.Features;
    using Subnautica.API.Extensions;
    using Subnautica.Events.EventArgs;
    using System;

    using UnityEngine;
    using static RootMotion.FinalIK.RagdollUtility;

    [HarmonyPatch(typeof(Constructable), nameof(Constructable.Construct))]
    public static class FurnitureConstructingCompleted
    {
        /**
         *
         * Fonksiyonu yamalar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private static void Postfix(Constructable __instance)
        {
            if (Network.IsMultiplayerActive)
            {
                try
                {
                    if (__instance.constructedAmount >= 1f && !__instance.GetComponentInParent<ConstructableBase>())
                    {
                        string baseId = null;
                        if(__instance.GetComponentInParent<Base>() != null)
                        {
                            baseId = Network.Identifier.GetIdentityId(__instance.GetComponentInParent<Base>().gameObject);
                        }

                        ConstructionCompletedEventArgs args = new ConstructionCompletedEventArgs(__instance.gameObject.GetIdentityId(), baseId, __instance.techType, new Vector3());

                        Handlers.Building.OnConstructingCompleted(args);
                    }
                }
                catch (Exception e)
                {
                    Log.Error($"Building.FurnitureConstructingCompleted: {e}\n{e.StackTrace}");
                }
            }
        }
    }

    [HarmonyPatch(typeof(BaseGhost), nameof(BaseGhost.Finish))]
    public static class BaseConstructingCompleted
    {
        /**
         *
         * Fonksiyonu yamalar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private static void Postfix(BaseGhost __instance)
        {
            if (Network.IsMultiplayerActive)
            {
                try
                {
                    var constructableBase = __instance.GetComponentInParent<ConstructableBase>();
                    if (constructableBase == null)
                    {
                        Log.Error("BaseGhost.Finish ConstructableBase NULL");
                        return;
                    }

                    var cellTransform = __instance.TargetBase.GetCellObject(__instance.TargetBase.WorldToGrid(__instance.transform.position));
                    if (cellTransform == null)
                    {
                        Log.Error("BaseGhost.Finish offset null");
                        return;
                    }
           
                    GameObject placedPiece = null;
                    if (constructableBase.techType == TechType.BasePartition)
                    {
                        placedPiece = BaseConstructingCompleted.FindPartitionPiece(constructableBase.gameObject.GetIdentityId(), cellTransform);
                    }
                    else
                    {
                        placedPiece = BaseConstructingCompleted.FindBasePiece(constructableBase.gameObject.GetIdentityId(), cellTransform);
                    }


                    if (placedPiece == null)
                    {
                        try
                        {
                            Log.Error("placedPiece is null => " + __instance.transform.position + ", CELL: " + __instance.TargetBase.WorldToGrid(__instance.transform.position) + ", IS GHOST => " + __instance.TargetBase.isGhost + ", Current: " + __instance.GhostBase.isGhost);
                        }
                        catch (Exception ex)
                        {
                            Log.Info("placedPiece exexexe: " + ex);
                        }

                        return;
                    }

                    string constructableId = Network.Identifier.GetIdentityId(constructableBase.gameObject);
                    string baseId          = Network.Identifier.GetIdentityId(__instance.TargetBase.gameObject);

                    Log.Error("BaseId: " + baseId + ", constructableId: " + constructableId);

                    var baseDeconstructable = placedPiece.GetComponent<BaseDeconstructable>();
                    if (baseDeconstructable != null && baseDeconstructable.face.HasValue)
                    {
                        Log.Error("ConstructingCompletedEventArgs YES -> CELL: " + baseDeconstructable.face.Value.cell + ", direction: " + baseDeconstructable.face.Value.direction + ", type => " + baseDeconstructable.faceType + ", CellPosition => " + placedPiece.transform.position + ", localPosition: " + placedPiece.transform.localPosition + ", localRotation: " + placedPiece.transform.localRotation + ", UNIQUE ID: " + constructableId + ", tech: " + constructableBase.techType);
                        ConstructionCompletedEventArgs args = new ConstructionCompletedEventArgs(constructableId, baseId, constructableBase.techType, placedPiece.transform.position, true, placedPiece.transform.localPosition, placedPiece.transform.localRotation, baseDeconstructable.face.Value.direction, baseDeconstructable.faceType);

                        Handlers.Building.OnConstructingCompleted(args);
                    }
                    else
                    {
                        Log.Error("ConstructingCompletedEventArgs NO -> BASE: localPosition: " + placedPiece.transform.localPosition + ", localRotation: " + placedPiece.transform.localRotation + ", UNIQUE ID: " + constructableId + ", tech: " + constructableBase.techType);
                        ConstructionCompletedEventArgs args = new ConstructionCompletedEventArgs(constructableId, baseId, constructableBase.techType, placedPiece.transform.position, false, placedPiece.transform.localPosition, placedPiece.transform.localRotation);

                        Handlers.Building.OnConstructingCompleted(args);
                    }
                }
                catch (Exception e)
                {
                    Log.Error($"Building.ConstructingCompleted: {e}\n{e.StackTrace}");
                }
            }
        }

        /**
         *
         * Parçayı döner.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private static GameObject FindPartitionPiece(string uniqueId, Transform cellTransform)
        {
            foreach (var child in cellTransform.GetComponentsInChildren<BaseDeconstructable>())
            {
                if (child.TryGetComponent<UniqueIdentifier>(out var uniqueIdentifier) && uniqueIdentifier.Id == uniqueId)
                {
                    return child.gameObject;
                }
            }

            return null;
        }

        /**
         *
         * Base parçasını döner.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private static GameObject FindBasePiece(string uniqueId, Transform cellTransform)
        {
            int cellCount = cellTransform.childCount - 1;

            for (int i = cellCount; i >= 0; i--)
            {
                Transform child = cellTransform.GetChild(i);

                if (child.TryGetComponent<UniqueIdentifier>(out var uniqueIdentifier) && uniqueIdentifier.Id == uniqueId)
                {
                    return child.gameObject;
                }
            }

            return null;
        }
    }
}
