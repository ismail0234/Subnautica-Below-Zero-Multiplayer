namespace Subnautica.Events.Patches.Fixes.Furnitures
{
    using HarmonyLib;

    using Subnautica.API.Features;

    [HarmonyPatch(typeof(global::CoffeeVendingMachine), nameof(global::CoffeeVendingMachine.Update))]
    public static class CoffeeVendingMachine
    {
        /**
         *
         * Fonksiyonu yamalar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private static bool Prefix()
        {
            return !Network.IsMultiplayerActive;
        }
    }
}
