namespace Subnautica.Events.Patches.Events.Vehicle
{
    using System;

    using HarmonyLib;
    using Subnautica.API.Features;

    using Subnautica.Events.EventArgs;

    [HarmonyPatch(typeof(global::SeaTruckSegment), nameof(global::SeaTruckSegment.Start))]
    public class SeaTruckModuleInitialized
    {
        public static void Prefix(global::SeaTruckSegment __instance)
        {
            try
            {
                SeaTruckModuleInitializedEventArgs args = new SeaTruckModuleInitializedEventArgs(__instance, CraftData.GetTechType(__instance.gameObject));
                
                Handlers.Vehicle.OnSeaTruckModuleInitialized(args);
            }
            catch (Exception e)
            {
                Log.Error($"SeaTruckModuleInitialized.Prefix: {e}\n{e.StackTrace}");
            }
        }
    }
}
