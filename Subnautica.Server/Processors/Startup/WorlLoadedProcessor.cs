namespace Subnautica.Server.Processors.Startup
{
    using System.Collections.Generic;
    using System.Linq;

    using Server.Core;

    using Subnautica.Network.Models.Core;
    using Subnautica.Network.Models.Storage.Player;
    using Subnautica.Server.Abstracts.Processors;

    using ClientModel = Subnautica.Network.Models.Client;
    using Metadata    = Subnautica.Network.Models.Metadata;
    using ServerModel = Subnautica.Network.Models.Server;

    public class WorlLoadedProcessor : NormalProcessor
    {
        /**
         *
         * Gelen veriyi işler
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public override bool OnExecute(AuthorizationProfile profile, NetworkPacket networkPacket)
        {
            var packet = networkPacket.GetPacket<ServerModel.WorldLoadedArgs>();
            if (packet.IsSpawnPointRequest)
            {
                Server.Instance.Logices.PlayerJoin.AddQueue(profile.UniqueId, packet.SpawnPointCount);
            }
            else
            {
                ClientModel.WorldLoadedArgs request = new ClientModel.WorldLoadedArgs()
                {
                    Images      = this.GetImages(packet.Images),
                    ExistImages = this.GetImageNames(),
                    Players     = this.GetPlayers(profile),
                };

                profile.OnFullConnected();
                profile.SendPacket(request);

                ClientModel.AnotherPlayerConnectedArgs otherRequest = new ClientModel.AnotherPlayerConnectedArgs()
                {
                    UniqueId   = profile.UniqueId,
                    PlayerId   = profile.PlayerId,
                    SubrootId  = profile.SubrootId,
                    InteriorId = profile.InteriorId,
                    PlayerName = profile.PlayerName,
                    Position   = profile.Position,
                    Rotation   = profile.Rotation,
                };

                profile.SendPacketToOtherClients(otherRequest);
            }

            return true;
        }

        /**
         *
         * Resimleri döner.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private Dictionary<string, Metadata.PictureFrame> GetImages(List<string> existImages)
        {
            var pictureFrames = new Dictionary<string, Metadata.PictureFrame>();
            var addedImages   = new List<string>();
            foreach (var item in Server.Instance.Storages.PictureFrame.Storage.Images)
            {
                if (existImages.Contains(item.Value.ImageName) || addedImages.Contains(item.Value.ImageName))
                {
                    pictureFrames.Add(item.Key, new Metadata.PictureFrame(item.Value.ImageName, null, false));
                }
                else
                {
                    pictureFrames.Add(item.Key, new Metadata.PictureFrame(item.Value.ImageName, item.Value.ImageData, false));
                }

                addedImages.Add(item.Value.ImageName);
            }

            return pictureFrames;
        }

        /**
         *
         * Resim isimlerini döner.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private List<string> GetImageNames()
        {
            var images = new List<string>();
            foreach (var item in Server.Instance.Storages.PictureFrame.Storage.Images)
            {
                if (!images.Contains(item.Value.ImageName))
                {
                    images.Add(item.Value.ImageName);
                }
            }

            return images;
        }

        /**
         *
         * Bağlı oyuncu listesini döner.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private List<PlayerItem> GetPlayers(AuthorizationProfile myProfile)
        {
            List<PlayerItem> result = new List<PlayerItem>();

            foreach (var player in Server.Instance.Players.Where(q => q.Value.UniqueId != myProfile.UniqueId))
            {
                result.Add(new PlayerItem()
                {
                    UniqueId   = player.Value.UniqueId,
                    PlayerId   = player.Value.PlayerId,
                    PlayerName = player.Value.PlayerName,
                    Position   = player.Value.Position,
                    Rotation   = player.Value.Rotation,
                    SubrootId  = player.Value.SubrootId,
                    InteriorId = player.Value.InteriorId,
                });
            }

            return result;
        }
    }
}
