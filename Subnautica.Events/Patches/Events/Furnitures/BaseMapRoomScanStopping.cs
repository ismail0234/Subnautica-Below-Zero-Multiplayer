namespace Subnautica.Events.Patches.Events.Furnitures
{
    using System;

    using HarmonyLib;

    using Subnautica.API.Features;
    using Subnautica.Events.EventArgs;

    [HarmonyPatch(typeof(global::uGUI_MapRoomScanner), nameof(global::uGUI_MapRoomScanner.OnCancelScan))]
    public static class BaseMapRoomScanStopping
    {
        /**
         *
         * Fonksiyonu yamalar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private static bool Prefix(global::uGUI_MapRoomScanner __instance)
        {
            if (!Network.IsMultiplayerActive)
            {
                return true;
            }

            try
            {
                BaseMapRoomScanStoppingEventArgs args = new BaseMapRoomScanStoppingEventArgs(BaseMapRoomScanStarting.GetUniqueId(__instance));

                Handlers.Furnitures.OnBaseMapRoomScanStopping(args);

                return args.IsAllowed;
            }
            catch (Exception e)
            {
                Log.Error($"BaseMapRoomScanStarting.Prefix: {e}\n{e.StackTrace}");
                return true;
            }
        }
    }
}