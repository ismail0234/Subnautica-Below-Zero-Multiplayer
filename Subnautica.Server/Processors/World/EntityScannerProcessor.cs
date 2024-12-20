namespace Subnautica.Server.Processors.World
{
    using System.Linq;

    using Server.Core;

    using Subnautica.API.Extensions;
    using Subnautica.Network.Models.Core;
    using Subnautica.Server.Abstracts.Processors;

    using ServerModel = Subnautica.Network.Models.Server;

    public class EntityScannerProcessor : NormalProcessor
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
            var packet = networkPacket.GetPacket<ServerModel.EntityScannerCompletedArgs>();
            if (packet == null)
            {
                return this.SendEmptyPacketErrorLog(networkPacket);
            }

            if (packet.Entity.UniqueId.IsWorldStreamer())
            {
                var spawnPoint = Core.Server.Instance.Storages.World.Storage.SpawnPoints.FirstOrDefault(q => q.SlotId == packet.Entity.UniqueId.WorldStreamerToSlotId());
                if (spawnPoint == null || !spawnPoint.IsRespawnable(Core.Server.Instance.Logices.World.GetServerTime()))
                {
                    return false;
                }

                spawnPoint.DisableRespawn();

                profile.SendPacketToAllClient(packet);
            }
            else
            {
                if (!Server.Instance.Storages.World.IsPersistentEntityExists(packet.Entity.UniqueId))
                {
                    packet.ScannerPlayerUniqueId = profile.UniqueId;
                    packet.Entity.DisableSpawn();

                    if (Server.Instance.Storages.World.SetPersistentEntity(packet.Entity))
                    {
                        profile.SendPacketToAllClient(packet);
                    }
                }
            }

            return true;
        }
    }
}