namespace Subnautica.Events.Patches.Events.Furnitures
{
    using System;

    using HarmonyLib;

    using Subnautica.API.Extensions;
    using Subnautica.API.Features;
    using Subnautica.Events.EventArgs;

    using UnityEngine;

    [HarmonyPatch(typeof(global::uGUI_MapRoomScanner), nameof(global::uGUI_MapRoomScanner.OnStartScan))]
    public static class BaseMapRoomScanStarting
    {
        /**
         *
         * Fonksiyonu yamalar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private static bool Prefix(global::uGUI_MapRoomScanner __instance, int index)
        {
            if (!Network.IsMultiplayerActive)
            {
                return true;
            }

            try
            {
                index = Mathf.Clamp(index + __instance.currentPage * __instance.resourceList.Length, 0, __instance.sortedTechTypes.Count - 1);

                BaseMapRoomScanStartingEventArgs args = new BaseMapRoomScanStartingEventArgs(BaseMapRoomScanStarting.GetUniqueId(__instance), __instance.sortedTechTypes[index]);

                Handlers.Furnitures.OnBaseMapRoomScanStarting(args);

                return args.IsAllowed;
            }
            catch (Exception e)
            {
                Log.Error($"BaseMapRoomScanStarting.Prefix: {e}\n{e.StackTrace}");
                return true;
            }
        }

        /**
         *
         * UniqueId döner.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static string GetUniqueId(global::uGUI_MapRoomScanner __instance)
        {
            return __instance.GetComponentInParent<MapRoomFunctionality>()?.GetBaseDeconstructable()?.gameObject?.GetIdentityId();
        }
    }
}