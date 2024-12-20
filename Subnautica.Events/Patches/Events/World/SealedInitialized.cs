namespace Subnautica.Events.Patches.Events.World
{
    using HarmonyLib;

    using Subnautica.API.Features;
    using Subnautica.Events.EventArgs;

    using System;

    [HarmonyPatch(typeof(global::LaserCutObject), nameof(global::LaserCutObject.OnEnable))]
    public static class SealedInitialized
    {
        private static void Prefix(global::LaserCutObject __instance)
        {
            if (Network.IsMultiplayerActive)
            {
                try
                {
                    SealedInitializedEventArgs args = new SealedInitializedEventArgs(Network.Identifier.GetIdentityId(__instance.gameObject, false), __instance.sealedScript);

                    Handlers.World.OnSealedInitialized(args);
                }
                catch (Exception e)
                {
                    Log.Error($"SealedInitialized.Prefix: {e}\n{e.StackTrace}");
                }
            }
        }
    }
}
