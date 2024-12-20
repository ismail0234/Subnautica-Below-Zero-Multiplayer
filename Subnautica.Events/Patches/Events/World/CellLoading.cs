namespace Subnautica.Events.Patches.Events.World
{
    using HarmonyLib;

    using Subnautica.API.Features;
    using Subnautica.Events.EventArgs;

    using System;

    [HarmonyPatch(typeof(global::EntityCell), nameof(global::EntityCell.AwakeAsync))]
    public static class CellLoading
    {
        private static void Postfix(global::EntityCell __instance)
        {
            if (Network.IsMultiplayerActive)
            {
                try
                {
                    CellLoadingEventArgs args = new CellLoadingEventArgs(__instance, __instance.BatchId, __instance.CellId, __instance.level);

                    Handlers.World.OnCellLoading(args);
                }
                catch (Exception e)
                {
                    Log.Error($"CellLoading.Prefix: {e}\n{e.StackTrace}");
                }
            }
        }
    }
}