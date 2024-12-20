namespace Subnautica.Events.Patches.Events.World
{
    using HarmonyLib;

    using Subnautica.API.Features;
    using Subnautica.Events.EventArgs;

    using System;

    [HarmonyPatch(typeof(global::EntityCell), nameof(global::EntityCell.SleepAsync))]
    public static class CellUnLoading
    {
        private static void Prefix(global::EntityCell __instance)
        {
            if (Network.IsMultiplayerActive)
            {
                try
                {
                    CellUnLoadingEventArgs args = new CellUnLoadingEventArgs(__instance, __instance.BatchId, __instance.CellId);

                    Handlers.World.OnCellUnLoading(args);
                }
                catch (Exception e)
                {
                    Log.Error($"CellUnLoading.Prefix: {e}\n{e.StackTrace}");
                }
            }
        }
    }
}