namespace Subnautica.Events.Patches.Fixes.Furnitures
{
    using HarmonyLib;

    using Subnautica.API.Features;

    using UnityEngine;

    [HarmonyPatch]
    public class Hoverpad
    {
        /**
         *
         * Fonksiyonu yamalar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [HarmonyPrefix]
        [HarmonyPatch(typeof(global::HoverpadUndockTrigger), nameof(global::HoverpadUndockTrigger.OnHandHover))]
        private static bool HoverpadUndockTriggerOnHandHover(global::HoverpadUndockTrigger __instance)
        {
            if (!Network.IsMultiplayerActive)
            {
                return true;
            }

            if (!__instance.hoverpad.isBikeDocked)
            {
                return false;
            }

            if (Interact.IsBlocked(Network.Identifier.GetIdentityId(__instance.hoverpad.gameObject)) || Interact.IsBlocked(Network.Identifier.GetIdentityId(__instance.hoverpad.dockedBike.gameObject)))
            {
                Interact.ShowUseDenyMessage();
                return false;
            }

            return true;
        }

        /**
         *
         * Fonksiyonu yamalar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [HarmonyPrefix]
        [HarmonyPatch(typeof(global::HoverbikeEnterTrigger), nameof(global::HoverbikeEnterTrigger.OnHandHover))]
        private static bool HoverbikeEnterTriggerOnHandHover(global::HoverbikeEnterTrigger __instance)
        {
            if (!Network.IsMultiplayerActive)
            {
                return true;
            }

            if (__instance.hoverbike.GetPilotingCraft() || !__instance.hoverbike.GetEnabled() || !__instance.hoverbike.AllowedToPilot())
            {
                return false;
            }

            if (Interact.IsBlocked(Network.Identifier.GetIdentityId(__instance.hoverbike.gameObject)))
            {
                Interact.ShowUseDenyMessage();
                return false;
            }

            return true;
        }

        /**
         *
         * Fonksiyonu yamalar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [HarmonyPrefix]
        [HarmonyPatch(typeof(global::HoverpadConstructor), nameof(global::HoverpadConstructor.OnConstructionDone))]
        private static bool ConstructorOnDone(global::HoverpadConstructor __instance)
        {
            if (!Network.IsMultiplayerActive)
            {
                return true;
            }

            __instance.hoverpad.animator.SetBool("fabricate", false);
            __instance.isConstructing = false;

            global::Story.ItemGoalTracker.OnConstruct(TechType.Hoverbike);

            if (__instance.hoverbike)
            {
                Vehicle.DockHoverbike(__instance.hoverpad, __instance.hoverbike, false);
                __instance.hoverbike.OnConstructionEnd();
            }

            return false;
        }

        /**
         *
         * Fonksiyonu yamalar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [HarmonyPostfix]
        [HarmonyPatch(typeof(global::GUI_HoverpadTerminal), nameof(global::GUI_HoverpadTerminal.StartBuild))]
        private static void TerminalStartBuild(global::GUI_HoverpadTerminal __instance, float buildDuration)
        {
            if (Network.IsMultiplayerActive)
            {
                __instance.buildStartTime = DayNightCycle.main.timePassedAsFloat;
                __instance.buildDuration = buildDuration + __instance.paddingTime;
            }
        }

        /**
         *
         * Fonksiyonu yamalar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [HarmonyPrefix]
        [HarmonyPatch(typeof(global::GUI_HoverpadTerminal), nameof(global::GUI_HoverpadTerminal.Update))]
        private static bool TerminalUpdate(global::GUI_HoverpadTerminal __instance)
        {
            if (!Network.IsMultiplayerActive)
            {
                return true;
            }

            if (DayNightCycle.main.timePassedAsFloat > __instance.buildDuration)
            {
                __instance.isBuilding = false;
                __instance.SetScreen();
            }
            else
            {
                var duration = (DayNightCycle.main.timePassedAsFloat - __instance.buildStartTime) / (__instance.buildDuration - __instance.buildStartTime);

                __instance.fillbar.fillAmount = Mathf.Clamp01(duration);
            }

            return false;
        }
    }
}