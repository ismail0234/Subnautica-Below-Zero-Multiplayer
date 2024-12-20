namespace Subnautica.Events.Patches.Events.World
{
    using HarmonyLib;

    using Subnautica.API.Features;
    using Subnautica.Events.EventArgs;

    using System;

    [HarmonyPatch(typeof(global::JukeboxDisk), nameof(global::JukeboxDisk.OnHandClick))]
    public static class JukeboxDiskPickedUp
    {
        private static void Prefix(global::JukeboxDisk __instance)
        {
            if (Network.IsMultiplayerActive)
            {
                try
                {
                    JukeboxDiskPickedUpEventArgs args = new JukeboxDiskPickedUpEventArgs(Network.Identifier.GetIdentityId(__instance.gameObject, false));

                    Handlers.World.OnJukeboxDiskPickedUp(args);
                }
                catch (Exception e)
                {
                    Log.Error($"EntitySpawningPrefabPlaceholder.Prefix: {e}\n{e.StackTrace}");
                }
            }
        }
    }
}