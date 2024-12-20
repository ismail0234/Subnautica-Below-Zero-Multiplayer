namespace Subnautica.Events.Patches.Events.Furnitures
{
    using System;

    using HarmonyLib;

    using Subnautica.API.Extensions;
    using Subnautica.API.Features;
    using Subnautica.Events.EventArgs;
    using Subnautica.Events.Patches.Fixes.Interact;

    using UnityEngine;

    [HarmonyPatch]
    public static class CrafterBegin
    {
        [HarmonyPrefix]
        [HarmonyPatch(typeof(global::GhostCrafter), nameof(global::GhostCrafter.Craft))]
        private static bool GhostCrafter_Craft(global::GhostCrafter __instance, TechType techType, float duration)
        {
            if (!Network.IsMultiplayerActive)
            {
                return true;
            }

            if (__instance.baseComp && !__instance.baseComp.IsPowered(__instance.transform.position))
            {
                return false;
            }

            if (__instance.needsPower)
            {
                if (!CrafterLogic.IsCraftRecipeFulfilled(techType))
                {
                    return false;
                }

                if (GameModeManager.GetOption<bool>(GameOption.TechnologyRequiresPower) && __instance.powerRelay.GetPower() < 5f)
                {
                    return false;
                }
            }

            var uniqueId  = GhostCrafter.GetUniqueId(__instance.gameObject);
            var _techType = GhostCrafter.GetTechType(__instance.gameObject);
            if (uniqueId.IsNull())
            {
                return false;
            }

            if (EventBlocker.IsEventBlocked(_techType))
            {
                return false;
            }

            try
            {
                duration = !TechData.GetCraftTime(techType, out duration) ? __instance.spawnAnimationDelay + __instance.spawnAnimationDuration : Mathf.Max(__instance.spawnAnimationDelay + __instance.spawnAnimationDuration, duration);

                CrafterBeginEventArgs args = new CrafterBeginEventArgs(uniqueId, _techType, techType, duration);

                Handlers.Furnitures.OnCrafterBegin(args);

                return args.IsAllowed;
            }
            catch (Exception e)
            {
                Log.Error($"CrafterBegin.GhostCrafter_Craft: {e}\n{e.StackTrace}");
                return true;
            }
        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(global::GhostCrafter), nameof(global::GhostCrafter.OnCraftingEnd))]
        private static bool GhostCrafter_OnCraftingEnd(global::GhostCrafter __instance)
        {
            if (!Network.IsMultiplayerActive)
            {
                return true;
            }

            if (__instance.logic == null)
            {
                return false;
            }

            var uniqueId = GhostCrafter.GetUniqueId(__instance.gameObject);
            var techType = GhostCrafter.GetTechType(__instance.gameObject);
            if (uniqueId.IsNull())
            {
                return false;
            }

            try
            {
                CrafterEndedEventArgs args = new CrafterEndedEventArgs(uniqueId, techType, __instance.logic.craftingTechType, __instance);

                Handlers.Furnitures.OnCrafterEnded(args);
            }
            catch (Exception e)
            {
                Log.Error($"CrafterBegin.GhostCrafter_OnCraftingEnd: {e}\n{e.StackTrace}");
            }

            return false;
        }
    }
}
