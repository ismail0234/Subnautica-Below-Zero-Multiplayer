namespace Subnautica.Events.Patches.Fixes.Furnitures
{
    using System;

    using HarmonyLib;

    using Subnautica.API.Features;

    using UnityEngine;

    [HarmonyPatch(typeof(global::BasePartitionDoor), nameof(global::BasePartitionDoor.GetState))]
    public class BasePartitionDoor
    {
        private static void Postfix(global::BasePartitionDoor __instance, ref bool __result)
        {
            if (!Network.IsMultiplayerActive)
            {
                return;
            }

            if (__result)
            {
                return;
            }

            try
            {

                foreach (var player in ZeroPlayer.GetPlayers())
                {
                    if (string.IsNullOrEmpty(player.CurrentSubRootId))
                    {
                        continue;
                    }

                    Vector3 difference = player.Position - __instance.transform.position;
                    if (Mathf.Abs(difference.y) < 1.0)
                    {
                        float num = difference.x * difference.x + difference.z * difference.z;
                        if (num < 2.25f)
                        {
                            __result = true;
                            return;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Log.Error($"BasePartitionDoor.Postfix: {e}\n{e.StackTrace}");
            }
        }
    }
}
