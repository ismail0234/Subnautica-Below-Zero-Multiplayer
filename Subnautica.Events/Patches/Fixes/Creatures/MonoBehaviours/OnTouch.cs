namespace Subnautica.Events.Patches.Fixes.Creatures.MonoBehaviours
{
    using HarmonyLib;

    using Subnautica.API.Extensions;
    using Subnautica.API.Features;

    using UnityEngine;

    [HarmonyPatch(typeof(global::OnTouch), nameof(global::OnTouch.RespondToCollider))]
    public class OnTouch
    {
        private static void Postfix(global::OnTouch __instance, Collider collider, ref bool __result)
        {
            if (Network.IsMultiplayerActive && __result == false && collider.name.IsMultiplayerPlayer())
            {
                __result = true;
            }
        }
    }
}
