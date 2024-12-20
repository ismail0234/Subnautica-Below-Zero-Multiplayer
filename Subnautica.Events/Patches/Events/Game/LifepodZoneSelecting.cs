namespace Subnautica.Events.Patches.Events.Game
{
    using System;

    using HarmonyLib;

    using Subnautica.API.Features;
    using Subnautica.Events.EventArgs;

    [HarmonyPatch(typeof(global::SupplyDropData), nameof(global::SupplyDropData.PickDropZone))]
    public static class LifepodZoneSelecting
    {
        private static bool Prefix(global::SupplyDropData __instance, ref SupplyDropZone __result)
        {
            if (!Network.IsMultiplayerActive)
            {
                return true;
            }

            try
            {
                LifepodZoneSelectingEventArgs args = new LifepodZoneSelectingEventArgs(__instance.precondition);

                Handlers.Game.OnLifepodZoneSelecting(args);

                if (args.IsAllowed)
                {
                    return true;
                }

                if (args.ZoneId != -1)
                {
                    __result = __instance.dropZones[args.ZoneId];
                }

                return false;
            }
            catch (Exception e)
            {
                Log.Error($"LifepodZoneSelecting.Prefix: {e}\n{e.StackTrace}");
                return false;
            }
        }
    }
}