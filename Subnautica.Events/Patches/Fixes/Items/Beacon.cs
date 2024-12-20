namespace Subnautica.Events.Patches.Fixes.Items
{
    using System.Collections.Generic;

    using HarmonyLib;

    using Subnautica.API.Features;

    [HarmonyPatch]
    public static class Beacon
    {
        [HarmonyPrefix]
        [HarmonyPatch(typeof(global::Beacon), nameof(global::Beacon.OnProtoDeserialize))]
        private static bool Beacon_OnProtoDeserialize(global::Beacon __instance)
        {
            return !Network.IsMultiplayerActive;
        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(global::Beacon), nameof(global::Beacon.Start))]
        private static bool Beacon_Start_Prefix(global::Beacon __instance)
        {
            if (!Network.IsMultiplayerActive)
            {
                return true;
            }

            if (__instance.deployedOnLand)
            {
                __instance.SetDeployedOnLand();
            }

            return false;
        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(global::Beacon), nameof(global::Beacon.Awake))]
        private static void Beacon_Awake_Prefix(global::Beacon __instance)
        {
            PrefixPostfixBlock(__instance);
        }

        [HarmonyPostfix]
        [HarmonyPatch(typeof(global::Beacon), nameof(global::Beacon.Awake))]
        private static void Beacon_Awake_Postfix(global::Beacon __instance)
        {
            PrefixPostfixBlock(__instance, false);
        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(global::Beacon), nameof(global::Beacon.OnDestroy))]
        private static void Beacon_OnDestroy_Prefix(global::Beacon __instance)
        {
            PrefixPostfixBlock(__instance, false);
        }

        /**
         *
         * Bloklama işlemini ayarlar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private static bool PrefixPostfixBlock(global::Beacon __instance, bool isPrefix = true)
        {
            if (!Network.IsMultiplayerActive)
            {
                return false;
            }

            var uniqueId = Network.Identifier.GetIdentityId(__instance.gameObject, false);
            if (string.IsNullOrEmpty(uniqueId))
            {
                return false;
            }

            if (isPrefix)
            {
                Blockers[uniqueId] = EventBlocker.Create(TechType.Beacon);
            }
            else
            {
                if (Blockers.TryGetValue(uniqueId, out var blocker))
                {
                    blocker.Dispose();
                }

                Blockers.Remove(uniqueId);
            }

            return true;
        }

        /**
         *
         * Bloklayıcıları barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private static Dictionary<string, EventBlocker> Blockers = new Dictionary<string, EventBlocker>();
    }
}
