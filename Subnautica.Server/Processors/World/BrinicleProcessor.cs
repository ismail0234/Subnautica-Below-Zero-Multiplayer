namespace Subnautica.Server.Processors.World
{
    using System.Linq;

    using Server.Core;

    using Subnautica.Network.Models.Core;
    using Subnautica.Network.Models.Storage.World.Childrens;
    using Subnautica.Server.Abstracts.Processors;

    using ServerModel = Subnautica.Network.Models.Server;

    public class BrinicleProcessor : NormalProcessor
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
            var packet = networkPacket.GetPacket<ServerModel.BrinicleArgs>();
            if (packet == null)
            {
                return this.SendEmptyPacketErrorLog(networkPacket);
            }

            if (packet.WaitingForRegistry?.Count > 0)
            {
                foreach (var brinicle in packet.WaitingForRegistry)
                {
                    if (this.RegisterBrinicle(brinicle, out var newBrinicle))
                    {
                        packet.Brinicles.Add(newBrinicle);
                    }
                }

                if (packet.Brinicles.Count > 0)
                {
                    packet.WaitingForRegistry.Clear();

                    profile.SendPacketToAllClient(packet);
                }
            }
            else if (packet.Damage > 0f)
            {
                var brinicle = Server.Instance.Storages.World.Storage.Brinicles.FirstOrDefault(q => q.UniqueId == packet.UniqueId);
                if (brinicle == null)
                {
                    return false;
                }

                if (brinicle.LiveMixin.TakeDamage(packet.Damage))
                {
                    if (brinicle.LiveMixin.IsDead)
                    {
                        brinicle.Kill(Server.Instance.Logices.World.GetServerTimeAsDouble());
                        brinicle.LiveMixin.ResetHealth();

                        packet.Brinicles.Add(brinicle);

                        profile.SendPacketToAllClient(packet);
                    }
                }
            }

            return true;
        }

        /**
         *
         * Dünyaya brinicle kaydeder.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private bool RegisterBrinicle(Brinicle brinicle, out Brinicle newBrinicle)
        {
            if (Server.Instance.Storages.World.Storage.Brinicles.Any(q => q.UniqueId == brinicle.UniqueId))
            {
                newBrinicle = null;
                return false;
            }

            newBrinicle = Brinicle.Create(brinicle.UniqueId, brinicle.MinFullScale, brinicle.MaxFullScale);
            newBrinicle.UpdateRandomState(Server.Instance.Logices.World.GetServerTimeAsDouble());

            Server.Instance.Storages.World.Storage.Brinicles.Add(newBrinicle);
            return true;
        }
    }
}
