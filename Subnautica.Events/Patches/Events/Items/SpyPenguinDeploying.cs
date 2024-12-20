namespace Subnautica.Events.Patches.Events.Items
{
    using System;
    using System.Collections.Generic;

    using HarmonyLib;

    using Subnautica.API.Features;
    using Subnautica.Events.EventArgs;

    using UnityEngine;

    [HarmonyPatch(typeof(global::SpyPenguinPlayerTool), nameof(global::SpyPenguinPlayerTool.OnToolUseAnim))]
    public class SpyPenguinDeploying
    {
        /**
         *
         * Fonksiyonu yamalar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private static bool Prefix(global::SpyPenguinPlayerTool __instance)
        {
            if (!Network.IsMultiplayerActive)
            {
                return true;
            }

            if (!__instance.CheckCanDrop())
            {
                return false;
            }

            try
            {
                SpyPenguinDeployingEventArgs args = new SpyPenguinDeployingEventArgs(Network.Identifier.GetIdentityId(__instance.pickupable.gameObject), __instance.pickupable, __instance.spyPenguin.liveMixin.health, GetName(__instance), GetDropPosition(__instance), GetDropRotation());

                Handlers.Items.OnSpyPenguinDeploying(args);

                if (!args.IsAllowed)
                {
                    global::Inventory.main.quickSlots.SetIgnoreHotkeyInput(false);
                }

                return args.IsAllowed;
            }
            catch (Exception e)
            {
                Log.Error($"SpyPenguinDeploying.Prefix: {e}\n{e.StackTrace}");
            }

            return true;
        }

        /**
         *
         * Konumu döner.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private static string GetName(global::SpyPenguinPlayerTool __instance)
        {
            return __instance.spyPenguin.GetPenguinName();
        }

        /**
         *
         * Konumu döner.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private static Vector3 GetDropPosition(global::SpyPenguinPlayerTool __instance)
        {
            return ZeroGame.FindDropPosition(__instance.deployPosition + Tools.GetCameraForward(true, true) * 0.75f + Vector3.down * 0.35f);
        }

        /**
         *
         * Açıyı döner.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private static Quaternion GetDropRotation()
        {
            return Quaternion.LookRotation(Tools.GetCameraForward(true, true), Vector3.up);
        }
    }
}