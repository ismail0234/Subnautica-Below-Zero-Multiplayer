namespace Subnautica.Events.Patches.Fixes.World
{
    using Subnautica.API.Features;

    using HarmonyLib;
    using UnityEngine;

    [HarmonyPatch(typeof(global::PrecursorTeleporterCollider), nameof(global::PrecursorTeleporterCollider.OnTriggerEnter))]
    public class PrecursorTeleporter
    {
        private static bool Prefix(global::PrecursorTeleporterCollider __instance, Collider col)
        {
            if (!Network.IsMultiplayerActive)
            {
                return true;
            }

            if (col.isTrigger)
            {
                return false;
            }

            GameObject gameObject = UWE.Utils.GetEntityRoot(col.gameObject);
            if (!gameObject)
            {
                gameObject = col.gameObject;
            }

            if (gameObject == global::Player.main.gameObject)
            {
                __instance.SendMessageUpwards("BeginTeleportPlayer", gameObject, SendMessageOptions.RequireReceiver);
            }

            return false;
        }
    }
}
