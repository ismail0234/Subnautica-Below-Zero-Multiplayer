namespace Subnautica.Events.Patches.Events.World
{
    using HarmonyLib;

    using Subnautica.API.Features;
    using Subnautica.Events.EventArgs;

    using System;

    [HarmonyPatch(typeof(global::BlueprintHandTarget), nameof(global::BlueprintHandTarget.UnlockBlueprint))]
    public static class DataboxItemPickedUp
    {
        private static void Prefix(global::BlueprintHandTarget __instance)
        {
            if (Network.IsMultiplayerActive && !__instance.used)
            {
                try
                {
                    DataboxItemPickedUpEventArgs args = new DataboxItemPickedUpEventArgs(Network.Identifier.GetIdentityId(__instance.gameObject, false));

                    Handlers.World.OnDataboxItemPickedUp(args);
                }
                catch (Exception e)
                {
                    Log.Error($"DataboxItemPickedUp.Prefix: {e}\n{e.StackTrace}");
                }
            }
        }
    }
}