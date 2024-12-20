namespace Subnautica.Events.Patches.Events.Vehicle
{
    using HarmonyLib;

    using Subnautica.API.Features;
    using Subnautica.Events.EventArgs;

    using System;

    [HarmonyPatch(typeof(global::VehicleUpgradeConsoleInput), nameof(global::VehicleUpgradeConsoleInput.OnHandClick))]
    public static class UpgradeConsoleOpening
    {
        /**
         *
         * Fonksiyonu yamalar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private static bool Prefix(global::VehicleUpgradeConsoleInput __instance)
        {
            if (!Network.IsMultiplayerActive)
            {
                return true;
            }

            try
            {
                UpgradeConsoleOpeningEventArgs args = new UpgradeConsoleOpeningEventArgs(Network.Identifier.GetIdentityId(__instance.GetComponentInParent<LargeWorldEntity>().gameObject));

                Handlers.Vehicle.OnUpgradeConsoleOpening(args);

                return args.IsAllowed;
            }
            catch (Exception e)
            {
                Log.Error($"UpgradeConsoleOpening.Prefix: {e}\n{e.StackTrace}");
                return true;
            }
        }
    }
}