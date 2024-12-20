namespace Subnautica.Events.Patches.Fixes.Items
{
    using HarmonyLib;

    using Subnautica.API.Features;

    [HarmonyPatch(typeof(global::OxygenPipe), nameof(global::OxygenPipe.OnDestroy))]
    public class OxygenPipe
    {
        /**
         *
         * Fonksiyonu yamalar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private static void Prefix(global::OxygenPipe __instance)
        {
            if (Network.IsMultiplayerActive)
            {
                __instance.SetParent(null);
                __instance.SetRoot(null);
            }
        }
    }
}
