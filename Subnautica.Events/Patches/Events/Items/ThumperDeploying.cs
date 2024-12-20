namespace Subnautica.Events.Patches.Events.Items
{
    using System;

    using HarmonyLib;

    using Subnautica.API.Features;
    using Subnautica.Events.EventArgs;

    [HarmonyPatch(typeof(global::Thumper), nameof(global::Thumper.Deploy))]
    public class ThumperDeploying
    {
        /**
         *
         * Fonksiyonu yamalar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private static bool Prefix(global::Thumper __instance, bool state)
        {
            if (Network.IsMultiplayerActive && state && !EventBlocker.IsEventBlocked(TechType.Thumper))
            {
                try
                {
                    ThumperDeployingEventArgs args = new ThumperDeployingEventArgs(Network.Identifier.GetIdentityId(__instance.pickupable.gameObject), __instance.pickupable, ZeroGame.FindDropPosition(__instance.placementPos), __instance.energyMixin.battery == null ? -1f : __instance.energyMixin.battery.charge);

                    Handlers.Items.OnThumperDeploying(args);

                    if (!args.IsAllowed)
                    {
                        __instance.Deploy(false);
                        global::Inventory.main.quickSlots.DeselectImmediate();
                    }

                    return args.IsAllowed;
                }
                catch (Exception e)
                {
                    Log.Error($"ThumperDeployed.Postfix: {e}\n{e.StackTrace}");
                }

                return false;
            }

            return true;
        }
    }
}