namespace Subnautica.Server.Processors.General
{
    using System.Linq;

    using Server.Core;

    using Subnautica.Network.Models.Core;
    using Subnautica.Network.Models.Storage.Construction;
    using Subnautica.Server.Abstracts.Processors;

    using ServerModel    = Subnautica.Network.Models.Server;
    using WorldChildrens = Subnautica.Network.Models.Storage.World.Childrens;

    public class IntroProcessor : NormalProcessor
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
            var packet = networkPacket.GetPacket<ServerModel.IntroStartArgs>();
            if (packet == null)
            {
                return this.SendEmptyPacketErrorLog(networkPacket);
            }

            Server.Instance.Storages.World.Storage.IsFirstLogin = false;

            if (!packet.IsFinished)
            {
                
                profile.SendPacketToAllClient(packet);
            }
            else
            {
                packet.ServerTime = Server.Instance.Logices.World.GetServerTime();
                packet.SupplyDrop = this.CreateOrGetSupplyDrop();
                
                profile.SendPacket(packet);
            }

            return true;
        }

        /**
         *
         * Supply drop oluşturur veya döner.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private WorldChildrens.SupplyDrop CreateOrGetSupplyDrop()
        {
            if (Server.Instance.Storages.World.Storage.SupplyDrops.Count > 0)
            {
                return Server.Instance.Storages.World.Storage.SupplyDrops.Where(q => q.Key == API.Constants.SupplyDrop.Lifepod).FirstOrDefault();
            }

            var supplyDrop = new WorldChildrens.SupplyDrop();
            supplyDrop.SetKey(API.Constants.SupplyDrop.Lifepod);
            supplyDrop.SetConfiguration(Server.Instance.Logices.World.GetServerTime());
            supplyDrop.Initialize();

            Server.Instance.Storages.World.Storage.SupplyDrops.Add(supplyDrop);

            if (Server.Instance.Storages.Construction.GetConstruction(supplyDrop.FabricatorUniqueId) == null)
            {
                Server.Instance.Storages.Construction.AddConstructionItem(ConstructionItem.CreateStaticItem(supplyDrop.FabricatorUniqueId, TechType.Fabricator));
            }

            return supplyDrop;
        }
    }
}