namespace Subnautica.Server.Processors.Vehicle
{
    using Server.Core;

    using Subnautica.API.Features;
    using Subnautica.Network.Models.Core;
    using Subnautica.Server.Abstracts.Processors;

    using ServerModel      = Subnautica.Network.Models.Server;
    using WorldEntityModel = Subnautica.Network.Models.WorldEntity.DynamicEntityComponents;

    public class SeaTruckSleeperModuleProcessor : NormalProcessor
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
            var packet = networkPacket.GetPacket<ServerModel.SeaTruckSleeperModuleArgs>();
            if (packet == null)
            {
                return this.SendEmptyPacketErrorLog(networkPacket);
            }

            var seaTruckModule = Server.Instance.Storages.World.GetDynamicEntity(packet.UniqueId);
            if (seaTruckModule == null)
            {
                return false;
            }

            if (packet.IsOpeningPictureFrame)
            {
                if (Server.Instance.Logices.Interact.IsBlocked(packet.UniqueId))
                {
                    return false;
                }

                Server.Instance.Logices.Interact.AddBlock(profile.UniqueId, packet.UniqueId, true);

                profile.SendPacket(packet);
            }
            else if (packet.IsSelectingPictureFrame)
            {
                if (packet.PictureFrameData != null && packet.PictureFrameData.Length > 0)
                {
                    packet.PictureFrameName = this.GetNewImageName();

                    if (Server.Instance.Storages.PictureFrame.AddImage(packet.UniqueId, packet.PictureFrameName, packet.PictureFrameData))
                    {
                        profile.SendPacketToAllClient(packet);
                    }
                }
            }
            else if (packet.JukeboxData != null)
            {
                Server.Instance.Logices.Jukebox.OnDataReceived(profile, packet.UniqueId, packet.JukeboxData);
            }
            else if (packet.SleepingSide != Bed.BedSide.None)
            {
                var component = seaTruckModule.Component.GetComponent<WorldEntityModel.SeaTruckSleeperModule>();
                if (component == null)
                {
                    return false;
                }

                if (packet.IsSleeping)
                {
                    if (component.Bed.IsUsing() || Server.Instance.Storages.World.Storage.SkipTimeMode || Server.Instance.Logices.Interact.IsBlocked(packet.UniqueId))
                    {
                        return false;
                    }

                    Server.Instance.Logices.Bed.ClearPlayerBeds(profile.PlayerId);

                    component.Bed.Sleep(profile.PlayerId, packet.SleepingSide, Server.Instance.Logices.World.GetServerTime());

                    if (Server.Instance.Logices.Interact.AddBlock(profile.UniqueId, packet.UniqueId, true))
                    {
                        profile.SendPacketToAllClient(packet);
                    }
                }
                else
                {
                    if (!component.Bed.IsUsing())
                    {
                        return false;
                    }

                    if (component.Bed.Side != Bed.BedSide.None)
                    {
                        packet.SleepingSide = component.Bed.Side;
                    }

                    component.Bed.Standup();

                    Server.Instance.Logices.Interact.RemoveBlockByPlayerId(profile.UniqueId, Subnautica.Server.Logic.Interact.Bed_Standup);

                    profile.SendPacketToAllClient(packet);
                }
            }

            return true;
        }

        /**
         *
         * Resim dosyası adını döner.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public string GetNewImageName()
        {
            return string.Format("{0}.jpg", Tools.GetShortUniqueId());
        }
    }
}
