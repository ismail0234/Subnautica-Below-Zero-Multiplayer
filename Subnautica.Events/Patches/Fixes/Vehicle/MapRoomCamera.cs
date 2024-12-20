namespace Subnautica.Events.Patches.Fixes.Vehicle
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection.Emit;

    using HarmonyLib;

    using Subnautica.API.Features;
    using UnityEngine;

    using UWE;

    [HarmonyPatch]
    public class MapRoomCamera
    {
        [HarmonyPrefix]
        [HarmonyPatch(typeof(global::MapRoomCamera), nameof(global::MapRoomCamera.UpdateEnergyRecharge))]
        private static bool UpdateEnergyRecharge()
        {
            return !Network.IsMultiplayerActive;
        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(global::MapRoomCameraDocking), nameof(global::MapRoomCameraDocking.UndockCamera))]
        private static bool UndockCamera()
        {
            if (!Network.IsMultiplayerActive || EventBlocker.IsEventBlocked(TechType.MapRoomCamera))
            {
                return true;
            }

            return false;
        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(global::CollectShiny), nameof(global::CollectShiny.TryPickupShinyTarget))]
        private static bool CollectShiny_TryPickupShinyTarget(global::CollectShiny __instance)
        {
            if (!Network.IsMultiplayerActive)
            {
                return true;
            }

            if (__instance.shinyTarget == null || !__instance.shinyTarget.activeInHierarchy)
            {
                return true;
            }

            return CraftData.GetTechType(__instance.shinyTarget) != TechType.MapRoomCamera;
        }

        [HarmonyPostfix]
        [HarmonyPatch(typeof(global::MapRoomCamera), nameof(global::MapRoomCamera.Start))]
        private static void Start(global::MapRoomCamera __instance)
        {
            if (Network.IsMultiplayerActive)
            {
                CoroutineHost.StartCoroutine(UpdateBaseMapRoomScreen());
            }
        }

        [HarmonyPostfix]
        [HarmonyPatch(typeof(global::MapRoomCamera), nameof(global::MapRoomCamera.OnDestroy))]
        private static void OnDestroy(global::MapRoomCamera __instance)
        {
            if (Network.IsMultiplayerActive)
            {
                CoroutineHost.StartCoroutine(UpdateBaseMapRoomScreen());
            }
        }

        [HarmonyTranspiler]
        [HarmonyPatch(typeof(global::MapRoomCamera), nameof(global::MapRoomCamera.Update))]
        private static IEnumerable<CodeInstruction> Update(IEnumerable<CodeInstruction> instructions)
        {
            var codes = instructions.ToList();
            var index = codes.FindIndex(q => q.opcode == OpCodes.Callvirt && q.operand.ToString().Contains("ConsumeEnergy"));

            if (index > -1)
            {
                index -= 4;

                codes.RemoveRange(index, 6);
                codes.InsertRange(index, new CodeInstruction[] {
                    new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(MapRoomCamera), nameof(MapRoomCamera.ConsumeEnergy), new Type[] { typeof(global::MapRoomCamera) }))
                });
            }

            return codes.AsEnumerable();
        }

        public static void ConsumeEnergy(global::MapRoomCamera __instance)
        {
            if (!Network.IsMultiplayerActive)
            {
                __instance.energyMixin.ConsumeEnergy(Time.deltaTime * 0.06666f);
            }
        }

        private static IEnumerator UpdateBaseMapRoomScreen()
        {
            yield return new WaitForSecondsRealtime(0.5f);

            foreach (var mapRoom in MapRoomFunctionality.mapRooms)
            {
                var screen = mapRoom.GetComponentInChildren<global::MapRoomScreen>();
                if (screen)
                {
                    screen.OnCurrentSubChanged(screen.baseRoot);
                }
            }
        }
    }
}
