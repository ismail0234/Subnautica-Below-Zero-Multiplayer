namespace Subnautica.Events.Patches.Events.Furnitures
{
    using System;
    using System.Collections.Generic;

    using HarmonyLib;

    using Subnautica.API.Features;
    using Subnautica.Events.EventArgs;

    [HarmonyPatch]
    public static class BaseMapRoomResourceDiscovering
    {
        [HarmonyPrefix]
        [HarmonyPatch(typeof(global::uGUI_MapRoomScanner), nameof(global::uGUI_MapRoomScanner.OnResourceDiscovered))]
        private static bool OnResourceDiscovered(global::uGUI_MapRoomScanner __instance, ResourceTrackerDatabase.ResourceInfo info)
        {
            if (!Network.IsMultiplayerActive)
            {
                return true;
            }

            if (__instance.availableTechTypes.Contains(info.techType))
            {
                return false;
            }

            try
            {
                BaseMapRoomResourceDiscoveringEventArgs args = new BaseMapRoomResourceDiscoveringEventArgs(info.techType);

                Handlers.Furnitures.OnBaseMapRoomResourceDiscovering(args);

                return args.IsAllowed;
            }
            catch (Exception e)
            {
                Log.Error($"BaseMapRoomResourceDiscovering.OnResourceDiscovered: {e}\n{e.StackTrace}");
                return true;
            }
        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(global::uGUI_MapRoomScanner), nameof(global::uGUI_MapRoomScanner.UpdateAvailableTechTypes))]
        private static bool UpdateAvailableTechTypes(global::uGUI_MapRoomScanner __instance)
        {
            if (!Network.IsMultiplayerActive)
            {
                return true;
            }

            var techTypes = new HashSet<TechType>();

            ResourceTrackerDatabase.GetTechTypesInRange(__instance.mapRoom.transform.position, __instance.mapRoom.GetScanRange(), techTypes);

            foreach (var techType in techTypes)
            {
                if (__instance.availableTechTypes.Contains(techType))
                {
                    continue;
                }

                try
                {
                    BaseMapRoomResourceDiscoveringEventArgs args = new BaseMapRoomResourceDiscoveringEventArgs(techType);

                    Handlers.Furnitures.OnBaseMapRoomResourceDiscovering(args);
                }
                catch (Exception e)
                {
                    Log.Error($"BaseMapRoomResourceDiscovering.UpdateAvailableTechTypes: {e}\n{e.StackTrace}");
                }  
            }

            return false;
        }
    }
}
