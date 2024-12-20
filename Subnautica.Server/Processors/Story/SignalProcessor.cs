namespace Subnautica.Server.Processors.Story
{
    using Server.Core;

    using Subnautica.API.Extensions;
    using Subnautica.API.Features;
    using Subnautica.Network.Models.Core;
    using Subnautica.Network.Models.Storage.Story.StoryGoals;
    using Subnautica.Network.Models.Storage.World.Childrens;
    using Subnautica.Server.Abstracts.Processors;
    using Subnautica.Server.Processors.Player;

    using UnityEngine;

    using static global::Story.UnlockSignalData;

    using ServerModel      = Subnautica.Network.Models.Server;
    using WorldEntityModel = Subnautica.Network.Models.WorldEntity.DynamicEntityComponents;

    public class SignalProcessor : NormalProcessor
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
            var packet = networkPacket.GetPacket<ServerModel.StorySignalArgs>();
            if (packet == null)
            {
                return this.SendEmptyPacketErrorLog(networkPacket);
            }

            packet.Beacon = null;
            packet.Signal = Server.Instance.Storages.Story.AddSignal(packet.Signal.SignalType, packet.Signal.TargetPosition, packet.Signal.TargetDescription, this.IsBeacon(packet.Signal.SignalType));

            if (packet.Signal != null)
            {
                if (this.IsBeacon(packet.Signal.SignalType))
                {
                    packet.Beacon = this.SpawnBeacon(packet.Signal, profile.UniqueId);
                }

                profile.SendPacketToAllClient(packet);
            }

            return true;
        }

        /**
         *
         * Beacon yumurtlar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private WorldDynamicEntity SpawnBeacon(ZeroStorySignal signal, string ownershipId)
        {
            WorldDynamicEntity entity = new WorldDynamicEntity()
            {
                Id              = Server.Instance.Logices.World.GetNextItemId(),
                UniqueId        = Network.Identifier.GenerateUniqueId(),
                Item            = null,
                TechType        = TechType.Beacon,
                Position        = signal.TargetPosition,
                Rotation        = Quaternion.identity.ToZeroQuaternion(),
                OwnershipId     = ownershipId,
                IsGlobalEntity  = API.Features.TechGroup.IsGlobalEntity(TechType.Beacon),
                IsDeployed      = true,
                Component       = ItemDropProcessor.GetEntityComponent(TechType.Beacon),
            };

            var beacon = entity.Component.GetComponent<WorldEntityModel.Beacon>();
            beacon.IsDeployedOnLand = signal.SignalType == SignalType.LandBeacon;
            beacon.Text             = global::Language.main.Get(signal.TargetDescription);

            if (Server.Instance.Storages.World.AddWorldDynamicEntity(entity))
            {
                return entity;
            }

            return null;
        }

        /**
         *
         * Beacon olup olmadığına bakar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private bool IsBeacon(SignalType signalType)
        {
            return signalType == SignalType.LandBeacon || signalType == SignalType.WaterBeacon;
        }
    }
}
