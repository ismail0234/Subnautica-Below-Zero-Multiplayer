namespace Subnautica.Server.Processors.World
{
    using System.Collections.Generic;

    using Server.Core;

    using Subnautica.API.Extensions;
    using Subnautica.Network.Models.Core;
    using Subnautica.Network.Structures;
    using Subnautica.Server.Abstracts.Processors;

    using ServerModel = Subnautica.Network.Models.Server;

    public class WelderProcessor : NormalProcessor
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
            var packet = networkPacket.GetPacket<ServerModel.WelderArgs>();
            if (packet == null)
            {
                return this.SendEmptyPacketErrorLog(networkPacket);
            }

            if (packet.TechType.IsVehicle())
            {
                var entity = Server.Instance.Storages.World.GetVehicle(packet.UniqueId);
                if (entity == null)
                {
                    return false;
                }

                var liveMixin = entity.GetLiveMixin();
                if (liveMixin != null && liveMixin.AddHealth(packet.Health))
                {
                    packet.Health = liveMixin.Health;

                    profile.SendPacketToAllClient(packet);
                }
            }
            else if (packet.TechType.IsBasePiece())
            {
                var construction = Server.Instance.Storages.Construction.GetConstruction(packet.UniqueId);
                if (construction == null)
                {
                    return false;
                }

                if (construction.LiveMixin.AddHealth(packet.Health))
                {
                    packet.Health = construction.LiveMixin.Health;

                    profile.SendPacketToAllClient(packet);

                    if (Server.Instance.Storages.World.TryGetBase(construction.BaseId, out var baseComponent))
                    {
                        if (baseComponent.UpdateLeakPoints(construction.UniqueId, construction.LiveMixin.Health, construction.LiveMixin.MaxHealth, playerPosition: profile.Position))
                        {
                            if (baseComponent.TryGetLeaker(construction.UniqueId, out var leaker))
                            {
                                this.SendBaseHullRepairPacket(construction.UniqueId, leaker.Points);
                            }
                        }         
                    }
                }
            }

            return true;
        }

        /**
         *
         * Temel gövde tamir paketi gönderir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private void SendBaseHullRepairPacket(string uniqueId, List<ZeroVector3> leakPoints)
        {
            ServerModel.BaseHullStrengthTakeDamagingArgs request = new ServerModel.BaseHullStrengthTakeDamagingArgs()
            {
                UniqueId      = uniqueId,
                LeakPoints    = leakPoints,
                CurrentHealth = -1f,
            };

            Server.SendPacketToAllClient(request);
        }
    }
}