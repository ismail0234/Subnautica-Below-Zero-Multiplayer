namespace Subnautica.Client.Synchronizations.InitialSync
{
    using System.Linq;

    using Subnautica.API.Enums;
    using Subnautica.API.Features;
    using Subnautica.Client.Core;

    public class JukeboxDiskProcessor
    {
        /**
         *
         * Şarkı Disk verileri yüklendikten sonra tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void OnJukeboxInitialized()
        {
            if (Network.Session.Current.JukeboxDisks != null)
            {
                using (EventBlocker.Create(ProcessType.JukeboxDiskAdded))
                {
                    foreach (var trackFile in Network.Session.Current.JukeboxDisks)
                    {
                        var music = Jukebox.unlockableMusic.Where(q => q.Value == trackFile).FirstOrDefault();
                        if (music.Value != null && music.Key != Jukebox.UnlockableTrack.None)
                        {
                            Player.main.unlockedTracks.Add(music.Key);
                            Jukebox.main.OnUnlock(music.Key, false);
                        }
                    }
                }
            }
        }
    }
}
