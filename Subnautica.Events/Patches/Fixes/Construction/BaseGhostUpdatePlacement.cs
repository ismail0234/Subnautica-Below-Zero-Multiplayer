namespace Subnautica.Events.Patches.Fixes.Construction
{
    using HarmonyLib;

    using Subnautica.API.Extensions;
    using Subnautica.API.Features;

    [HarmonyPatch]
    public class BaseGhostUpdatePlacement
    {
        [HarmonyPostfix]
        [HarmonyPatch(typeof(global::BaseAddFaceGhost), nameof(global::BaseAddFaceGhost.UpdatePlacement))]
        private static void BaseAddFaceGhost(global::BaseAddFaceGhost __instance, ref bool __result)
        {
            if (Network.IsMultiplayerActive)
            {
                Network.Temporary.SetProperty(__instance.gameObject.GetIdentityId(), "UpdatePlacementResult", __result);
            }
        }

        [HarmonyPostfix]
        [HarmonyPatch(typeof(global::BaseAddLadderGhost), nameof(global::BaseAddLadderGhost.UpdatePlacement))]
        private static void BaseAddLadderGhost(global::BaseAddLadderGhost __instance, ref bool __result)
        {
            if (Network.IsMultiplayerActive)
            {
                Network.Temporary.SetProperty(__instance.gameObject.GetIdentityId(), "UpdatePlacementResult", __result);
            }
        }

        [HarmonyPostfix]
        [HarmonyPatch(typeof(global::BaseAddBulkheadGhost), nameof(global::BaseAddBulkheadGhost.UpdatePlacement))]
        private static void BaseAddBulkheadGhost(global::BaseAddBulkheadGhost __instance, ref bool __result)
        {
            if (Network.IsMultiplayerActive)
            {
                Network.Temporary.SetProperty(__instance.gameObject.GetIdentityId(), "UpdatePlacementResult", __result);
            }
        }

        [HarmonyPostfix]
        [HarmonyPatch(typeof(global::BaseAddPartitionGhost), nameof(global::BaseAddPartitionGhost.UpdatePlacement))]
        private static void BaseAddPartitionGhost(global::BaseAddPartitionGhost __instance, ref bool __result)
        {
            if (Network.IsMultiplayerActive)
            {
                Network.Temporary.SetProperty(__instance.gameObject.GetIdentityId(), "UpdatePlacementResult", __result);
            }
        }

        [HarmonyPostfix]
        [HarmonyPatch(typeof(global::BaseAddPartitionDoorGhost), nameof(global::BaseAddPartitionDoorGhost.UpdatePlacement))]
        private static void BaseAddPartitionDoorGhost(global::BaseAddPartitionDoorGhost __instance, ref bool __result)
        {
            if (Network.IsMultiplayerActive)
            {
                Network.Temporary.SetProperty(__instance.gameObject.GetIdentityId(), "UpdatePlacementResult", __result);
            }
        }

        [HarmonyPostfix]
        [HarmonyPatch(typeof(global::BaseAddModuleGhost), nameof(global::BaseAddModuleGhost.UpdatePlacement))]
        private static void BaseAddModuleGhost(global::BaseAddModuleGhost __instance, ref bool __result)
        {
            if (Network.IsMultiplayerActive)
            {
                Network.Temporary.SetProperty(__instance.gameObject.GetIdentityId(), "UpdatePlacementResult", __result);
            }
        }

        [HarmonyPostfix]
        [HarmonyPatch(typeof(global::BaseAddConnectorGhost), nameof(global::BaseAddConnectorGhost.UpdatePlacement))]
        private static void BaseAddConnectorGhost(global::BaseAddConnectorGhost __instance, ref bool __result)
        {
            if (Network.IsMultiplayerActive)
            {
                Network.Temporary.SetProperty(__instance.gameObject.GetIdentityId(), "UpdatePlacementResult", __result);
            }
        }

        [HarmonyPostfix]
        [HarmonyPatch(typeof(global::BaseAddCellGhost), nameof(global::BaseAddCellGhost.UpdatePlacement))]
        private static void BaseAddCellGhost(global::BaseAddCellGhost __instance, ref bool __result)
        {
            if (Network.IsMultiplayerActive)
            {
                Network.Temporary.SetProperty(__instance.gameObject.GetIdentityId(), "TargetOffset", __instance.targetOffset);
                Network.Temporary.SetProperty(__instance.gameObject.GetIdentityId(), "UpdatePlacementResult", __result);
            }
        }

        [HarmonyPostfix]
        [HarmonyPatch(typeof(global::BaseAddCorridorGhost), nameof(global::BaseAddCorridorGhost.UpdatePlacement))]
        private static void BaseAddCorridorGhost(global::BaseAddCorridorGhost __instance, ref bool __result)
        {
            if (Network.IsMultiplayerActive)
            {
                Network.Temporary.SetProperty(__instance.gameObject.GetIdentityId(), "TargetOffset", __instance.targetOffset);
                Network.Temporary.SetProperty(__instance.gameObject.GetIdentityId(), "UpdatePlacementResult", __result);
            }
        }

        [HarmonyPostfix]
        [HarmonyPatch(typeof(global::BaseAddMapRoomGhost), nameof(global::BaseAddMapRoomGhost.UpdatePlacement))]
        private static void BaseAddMapRoomGhost(global::BaseAddMapRoomGhost __instance, ref bool __result)
        {
            if (Network.IsMultiplayerActive)
            {
                Network.Temporary.SetProperty(__instance.gameObject.GetIdentityId(), "TargetOffset", __instance.targetOffset);
                Network.Temporary.SetProperty(__instance.gameObject.GetIdentityId(), "UpdatePlacementResult", __result);
            }
        }

        [HarmonyPostfix]
        [HarmonyPatch(typeof(global::BaseAddWaterPark), nameof(global::BaseAddWaterPark.UpdatePlacement))]
        private static void BaseAddWaterPark(global::BaseAddWaterPark __instance, ref bool __result)
        {
            if (Network.IsMultiplayerActive)
            {
                Network.Temporary.SetProperty(__instance.gameObject.GetIdentityId(), "UpdatePlacementResult", __result);
            }
        }
    }
}
