namespace Subnautica.Events.Patches.Events.Items
{
    using System;

    using HarmonyLib;

    using Subnautica.API.Features;
    using Subnautica.Events.EventArgs;

    using UnityEngine;

    [HarmonyPatch(typeof(global::HoverbikePlayerTool), nameof(global::HoverbikePlayerTool.OnToolUseAnim))]
    public class HoverbikeDeploying
    {
        /**
            *
            * Fonksiyonu yamalar.
            *
            * @author Ismail <ismaiil_0234@hotmail.com>
            *
            */
        private static bool Prefix(global::HoverbikePlayerTool __instance)
        {
            if (Network.IsMultiplayerActive && !EventBlocker.IsEventBlocked(TechType.Hoverbike))
            {
                try
                {
                    HoverbikeDeployingEventArgs args = new HoverbikeDeployingEventArgs(Network.Identifier.GetIdentityId(__instance.hoverbike.gameObject), __instance.hoverbike, ZeroGame.FindDropPosition(__instance.transform.position + (MainCamera.camera.transform.forward * 0.7f) + (Vector3.down * 0.3f) + (Vector3.up * 0.6f) ), MainCamera.camera.transform.forward);

                    Handlers.Items.OnHoverbikeDeploying(args);

                    if (!args.IsAllowed)
                    {
                        global::Inventory.main.quickSlots.SetIgnoreHotkeyInput(false);
                    }

                    return args.IsAllowed;
                }
                catch (Exception e)
                {
                    Log.Error($"HoverbikeDeploying.Prefix: {e}\n{e.StackTrace}");
                }
            }

            return true;
        }
    }
}