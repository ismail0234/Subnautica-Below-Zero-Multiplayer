namespace Subnautica.Events.Patches.Fixes.Items
{
    using System.Collections.Generic;
    using System.Linq;

    using HarmonyLib;

    using Subnautica.API.Features;

    using UnityEngine;

    [HarmonyPatch(typeof(global::TeleportationTool), nameof(global::TeleportationTool.ChooseTeleporter))]
    public static class TeleportationTool
    {
        private static bool Prefix(global::TeleportationTool __instance, ref SeaTruckTeleporter __result)
        {
            if (!Network.IsMultiplayerActive)
            {
                return true;
            }

            var distances = new Dictionary<float, SeaTruckTeleporter>();

            foreach (var item in SeaTruckTeleporter.teleporters)
            {
                if (item.isPowered && item.isConstructed)
                {
                    var angle = Vector3.Dot(MainCameraControl.main.viewModel.transform.forward, item.truckSegment.transform.position);

                    distances.Add(angle, item);
                }
            }

            __result = distances.OrderBy(q => q.Key).FirstOrDefault().Value;
            return false;
        }
    }
}