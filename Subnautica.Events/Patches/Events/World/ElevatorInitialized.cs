namespace Subnautica.Events.Patches.Events.World
{
    using HarmonyLib;

    using Subnautica.API.Features;
    using Subnautica.Events.EventArgs;

    using System;

    [HarmonyPatch]
    public static class ElevatorInitialized
    {
        [HarmonyPrefix]
        [HarmonyPatch(typeof(global::Rocket), nameof(global::Rocket.Start))]
        private static void Rocket_Start(global::Rocket __instance)
        {
            if (Network.IsMultiplayerActive)
            {
                try
                {
                    ElevatorInitializedEventArgs args = new ElevatorInitializedEventArgs(__instance);

                    Handlers.World.OnElevatorInitialized(args);
                }
                catch (Exception e)
                {
                    Log.Error($"ElevatorInitialized.Rocket_Start: {e}\n{e.StackTrace}");
                }
            }
        }
    }
}