namespace Subnautica.Events.Patches.Events.PDA
{
    using HarmonyLib;

    using Subnautica.API.Features;
    using Subnautica.Events.EventArgs;

    using System;

    [HarmonyPatch(typeof(global::Jukebox), nameof(global::Jukebox.Unlock))]
    public static class JukeboxDiskAdded
    {
        private static void Prefix(global::Jukebox __instance, Jukebox.UnlockableTrack track, bool notify = true)
        {
            if(Network.IsMultiplayerActive)
            {
                if (global::Player.main.unlockedTracks == null || global::Player.main.unlockedTracks.Contains(track))
                {
                    return;
                }

                if (!global::Jukebox.unlockableMusic.TryGetValue(track, out string trackFile))
                {
                    return;
                }

                try
                {
                    JukeboxDiskAddedEventArgs args = new JukeboxDiskAddedEventArgs(trackFile, notify);

                    Handlers.PDA.OnJukeboxDiskAdded(args);
                }
                catch (Exception e)
                {
                    Log.Error($"JukeboxDiskAdded.Prefix: {e}\n{e.StackTrace}");
                }
            }
        }
    }
}