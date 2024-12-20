namespace Subnautica.Events.Patches.Fixes.Construction
{
    using HarmonyLib;

    using Subnautica.API.Extensions;
    using Subnautica.API.Features;
    
    [HarmonyPatch]
    public class BaseDataCache
    {
        [HarmonyPostfix]
        [HarmonyPatch(typeof(global::Base), nameof(global::Base.CanSetFace))]
        private static void CanSetFace(global::Base __instance, ref bool __result, global::Base.Face srcStart)
        {
            if (Network.IsMultiplayerActive && __result)
            {
                Network.Temporary.SetProperty(__instance.gameObject.GetIdentityId(), "FaceStart", srcStart);
            }
        }

        [HarmonyPostfix]
        [HarmonyPatch(typeof(global::Base), nameof(global::Base.CanSetLadder))]
        private static void CanSetLadder(global::Base __instance, ref bool __result, global::Base.Face faceStart, global::Base.Face faceEnd)
        {
            if (Network.IsMultiplayerActive && __result)
            {
                Network.Temporary.SetProperty(__instance.gameObject.GetIdentityId(), "FaceStart", faceStart);
                Network.Temporary.SetProperty(__instance.gameObject.GetIdentityId(), "FaceEnd", faceEnd);
            }
        }

        [HarmonyPostfix]
        [HarmonyPatch(typeof(global::Base), nameof(global::Base.CanSetBulkhead))]
        private static void CanSetBulkhead(global::Base __instance, ref bool __result, global::Base.Face fromCell)
        {
            if (Network.IsMultiplayerActive && __result)
            {
                Network.Temporary.SetProperty(__instance.gameObject.GetIdentityId(), "FaceStart", fromCell);
            }
        }

        [HarmonyPostfix]
        [HarmonyPatch(typeof(global::Base), nameof(global::Base.CanSetPartition))]
        private static void CanSetPartition(global::Base __instance, ref bool __result, Int3 cell, global::Base.Direction partitionDirection)
        {
            if (Network.IsMultiplayerActive && __result)
            {
                Network.Temporary.SetProperty(__instance.gameObject.GetIdentityId(), "FaceCell", cell);
                Network.Temporary.SetProperty(__instance.gameObject.GetIdentityId(), "Direction", partitionDirection);
            }
        }

        [HarmonyPostfix]
        [HarmonyPatch(typeof(global::Base), nameof(global::Base.CanSetPartitionDoor))]
        private static void CanSetPartitionDoor(global::Base __instance, ref bool __result, Int3 cell, global::Base.Direction doorFaceDirection)
        {
            if (Network.IsMultiplayerActive && __result)
            {
                Network.Temporary.SetProperty(__instance.gameObject.GetIdentityId(), "FaceCell", cell);
                Network.Temporary.SetProperty(__instance.gameObject.GetIdentityId(), "Direction", doorFaceDirection);
            }
        }

        [HarmonyPostfix]
        [HarmonyPatch(typeof(global::Base), nameof(global::Base.CanSetModule))]
        private static void CanSetModule(global::Base __instance, ref bool __result, ref global::Base.Face face)
        {
            if (Network.IsMultiplayerActive && __result)
            {
                Network.Temporary.SetProperty(__instance.gameObject.GetIdentityId(), "FaceStart", face);
            }
        }

        [HarmonyPostfix]
        [HarmonyPatch(typeof(global::Base), nameof(global::Base.CanSetConnector))]
        private static void CanSetConnector(global::Base __instance, ref bool __result, global::Base.Face cell)
        {
            if (Network.IsMultiplayerActive && __result)
            {
                Network.Temporary.SetProperty(__instance.gameObject.GetIdentityId(), "FaceCell", cell);
            }
        }

        [HarmonyPostfix]
        [HarmonyPatch(typeof(global::Base), nameof(global::Base.CanSetWaterPark))]
        private static void CanSetWaterPark(global::Base __instance, ref bool __result, Int3 cell)
        {
            if (Network.IsMultiplayerActive && __result)
            {
                Network.Temporary.SetProperty(__instance.gameObject.GetIdentityId(), "FaceCell", cell);
                Network.Temporary.SetProperty(__instance.gameObject.GetIdentityId(), "Direction", Base.Direction.Below);
            }
        }
    }
}
