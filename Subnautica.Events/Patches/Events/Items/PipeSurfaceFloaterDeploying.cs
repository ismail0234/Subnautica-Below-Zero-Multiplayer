namespace Subnautica.Events.Patches.Events.Items
{
    using System;

    using HarmonyLib;

    using Subnautica.API.Extensions;
    using Subnautica.API.Features;
    using Subnautica.Events.EventArgs;

    using UnityEngine;

    [HarmonyPatch(typeof(global::PipeSurfaceFloater), nameof(global::PipeSurfaceFloater.OnToolUseAnim))]
    public class PipeSurfaceFloaterDeploying
    {
        /**
         *
         * Fonksiyonu yamalar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private static bool Prefix(global::PipeSurfaceFloater __instance)
        {
            if (Network.IsMultiplayerActive)
            {
                try
                {
                    PipeSurfaceFloaterDeployingEventArgs args = new PipeSurfaceFloaterDeployingEventArgs(__instance.pickupable.gameObject.GetIdentityId(), __instance.pickupable, GetDropPosition(__instance), __instance.transform.rotation);

                    Handlers.Items.OnPipeSurfaceFloaterDeploying(args);

                    return args.IsAllowed;
                }
                catch (Exception e)
                {
                    Log.Error($"PipeSurfaceFloaterDeploying.Prefix: {e}\n{e.StackTrace}");
                }
            }

            return true;
        }

        /**
         *
         * Koordinatı döner.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private static Vector3 GetDropPosition(global::PipeSurfaceFloater __instance)
        {
            return ZeroGame.FindDropPosition(__instance.GetDropPosition());
        }
    }
}