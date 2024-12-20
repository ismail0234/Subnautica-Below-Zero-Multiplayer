namespace Subnautica.Events.Patches.Events.Items
{
    using System;

    using HarmonyLib;

    using Subnautica.API.Features;
    using Subnautica.Events.EventArgs;

    [HarmonyPatch(typeof(global::BeaconLabel), nameof(global::BeaconLabel.SetLabel))]
    public class BeaconLabelChanged
    {
        /**
         *
         * Fonksiyonu yamalar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private static void Prefix(global::BeaconLabel __instance, string label)
        {
            if (Network.IsMultiplayerActive && !EventBlocker.IsEventBlocked(TechType.Beacon))
            {
                try
                {
                    BeaconLabelChangedEventArgs args = new BeaconLabelChangedEventArgs(Network.Identifier.GetIdentityId(__instance.GetComponentInParent<Beacon>().gameObject), label);

                    Handlers.Items.OnBeaconLabelChanged(args);
                }
                catch (Exception e)
                {
                    Log.Error($"BeaconLabelChanged.Prefix: {e}\n{e.StackTrace}");
                }
            }
        }
    }
}