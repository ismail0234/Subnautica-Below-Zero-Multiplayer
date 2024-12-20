namespace Subnautica.Events.Patches.Events.Game
{
    using HarmonyLib;
    
    using Subnautica.API.Features;

    using System;

    [HarmonyPatch(typeof(global::UWE.GameApplication), nameof(global::UWE.GameApplication.OnApplicationQuitting))]
    public class Quitting
    {
        private static void Prefix()
        {
            try
            {
                Handlers.Game.OnQuitting();
            }
            catch (Exception e)
            {
                Log.Error($"Qutting.Prefix: {e}\n{e.StackTrace}");
            }
        }
    }
}
