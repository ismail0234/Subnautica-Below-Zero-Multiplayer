namespace Subnautica.Client.Synchronizations.Processors.PDA
{
    using System.Linq;

    using Subnautica.API.Enums;
    using Subnautica.API.Features;
    using Subnautica.Client.Abstracts;
    using Subnautica.Client.Core;
    using Subnautica.Events.EventArgs;
    using Subnautica.Network.Models.Core;

    using ServerModel = Subnautica.Network.Models.Server;

    public class JukeboxDiskAddedProcessor : NormalProcessor
    {
        /**
         *
         * Gelen veriyi işler
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public override bool OnDataReceived(NetworkPacket networkPacket)
        {
            var packet = networkPacket.GetPacket<ServerModel.JukeboxDiskAddedArgs>();

            using (EventBlocker.Create(ProcessType.JukeboxDiskAdded))
            {
                var music = Jukebox.unlockableMusic.Where(q => q.Value == packet.TrackFile).FirstOrDefault();
                if (music.Value != null && music.Key != Jukebox.UnlockableTrack.None)
                {
                    Jukebox.Unlock(music.Key, packet.Notify);
                }
            }

            return true;
        }

        /**
         *
         * Şarkı diski açıldığında tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void OnJukeboxDiskAdded(JukeboxDiskAddedEventArgs ev)
        {
            ServerModel.JukeboxDiskAddedArgs result = new ServerModel.JukeboxDiskAddedArgs()
            {
                TrackFile = ev.TrackFile,
                Notify = ev.Notify,
            };

            NetworkClient.SendPacket(result);
        }
    }
}