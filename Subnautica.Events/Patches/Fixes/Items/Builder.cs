namespace Subnautica.Events.Patches.Fixes.Items
{
    using HarmonyLib;

    using Subnautica.API.Features;

    [HarmonyPatch(typeof(global::BuilderTool), nameof(global::BuilderTool.OnHolster))]
    public class Builder
    {
        /**
         *
         * Fonksiyonu yamalar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private static bool Prefix(global::BuilderTool __instance)
        {
            if (!Network.IsMultiplayerActive)
            {
                return true;
            }

            return ItemMain.CheckOnHolster(__instance);
        }
    }
}
