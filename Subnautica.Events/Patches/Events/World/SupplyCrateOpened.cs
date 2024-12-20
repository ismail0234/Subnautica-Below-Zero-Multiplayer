namespace Subnautica.Events.Patches.Events.World
{
    using HarmonyLib;

    using Subnautica.API.Features;
    using Subnautica.Events.EventArgs;

    using System;

    [HarmonyPatch(typeof(global::SupplyCrate), nameof(global::SupplyCrate.OnHandClick))]
    public static class SupplyCrateOpened
    {
        /**
         *
         * Fonksiyonu yamalar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private static bool Prefix(global::SupplyCrate __instance)
        {
            if (!Network.IsMultiplayerActive || __instance.open)
            {
                return true;
            }

            if (__instance.sealedComp != null && __instance.sealedComp.IsSealed())
            {
                return false;
            }

            try
            {
                SupplyCrateOpenedEventArgs args = new SupplyCrateOpenedEventArgs(Network.Identifier.GetIdentityId(__instance.gameObject));

                Handlers.World.OnSupplyCrateOpened(args);
                return true;
            }
            catch (Exception e)
            {
                Log.Error($"SupplyCrateOpened.Prefix: {e}\n{e.StackTrace}");
                return true;
            }
        }
    }
}