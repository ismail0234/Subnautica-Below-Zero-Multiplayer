namespace Subnautica.Events.Patches.Events.Items
{
    using System;

    using HarmonyLib;

    using Subnautica.API.Features;
    using Subnautica.Events.EventArgs;

    [HarmonyPatch(typeof(global::Flare), nameof(global::Flare.Throw))]
    public class FlareDeploying
    {
        /**
         *
         * Fonksiyonu yamalar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private static bool Prefix(global::Flare __instance)
        {
            if (Network.IsMultiplayerActive)
            {
                try
                {
                    FlareDeployingEventArgs args = new FlareDeployingEventArgs(Network.Identifier.GetIdentityId(__instance.pickupable.gameObject), __instance.pickupable, ZeroGame.FindDropPosition(__instance.transform.position), __instance.energyLeft);

                    Handlers.Items.OnFlareDeploying(args);

                    if (!args.IsAllowed)
                    {
                        __instance._isInUse   = false;
                        __instance.isThrowing = false;
                    }

                    return args.IsAllowed;
                }
                catch (Exception e)
                {
                    Log.Error($"FlareDeploying.Prefix: {e}\n{e.StackTrace}");
                }
            }

            return true;
        }
    }
}