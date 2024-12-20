namespace Subnautica.Events.Patches.Fixes.World
{
    using HarmonyLib;

    using Subnautica.API.Features;

    using System;

    [HarmonyPatch]
    public class Brinicle
    {
        [HarmonyPrefix]
        [HarmonyPatch(typeof(global::Brinicle), nameof(global::Brinicle.Update))]
        private static bool Brinicle_Update()
        {
            return !Network.IsMultiplayerActive;
        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(global::Brinicle), nameof(global::Brinicle.SetState), new Type[] { typeof(global::Brinicle.State), typeof(float) })]
        private static bool Brinicle_SetState()
        {
            return !Network.IsMultiplayerActive;
        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(global::Brinicle), nameof(global::Brinicle.RandomizeRotation))]
        private static bool Brinicle_RandomizeRotation()
        {
            return !Network.IsMultiplayerActive;
        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(global::Brinicle), nameof(global::Brinicle.Start))]
        private static bool Brinicle_Start()
        {
            return !Network.IsMultiplayerActive;
        }
    }
}
