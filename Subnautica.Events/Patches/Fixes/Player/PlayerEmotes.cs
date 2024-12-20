namespace Subnautica.Events.Patches.Fixes.Player
{
    using HarmonyLib;

    using UnityEngine;

    using Subnautica.API.Features;

    [HarmonyPatch(typeof(global::OnDisplayHeldTool), nameof(global::OnDisplayHeldTool.OnStateEnter))]
    public static class PlayerEmotes
    {
        private static bool Prefix(Animator animator)
        {
            return !Network.IsMultiplayerActive || global::Player.main.playerAnimator == animator;
        }
    }
}