namespace Subnautica.Client.Synchronizations.Processors.World
{
    using System.Collections.Generic;

    using Subnautica.API.Extensions;
    using Subnautica.API.Features;
    using Subnautica.API.Features.Helper;
    using Subnautica.Client.Abstracts;
    using Subnautica.Network.Models.Core;

    using ServerModel = Subnautica.Network.Models.Server;

    public class WorldDynamicEntityOwnershipChangedProcessor : NormalProcessor
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
            var packet = networkPacket.GetPacket<ServerModel.WorldDynamicEntityOwnershipChangedArgs>();
            if (packet == null)
            {
                return false;
            }
        
            var action = new ItemQueueAction();
            action.OnProcessCompleted = this.OnEntityProcessCompleted;
            action.RegisterProperty("Entities", packet.Entities);

            Entity.ProcessToQueue(action);
            return true;
        }

        /**
         *
         * Nesne işlemi tamamlanınca tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void OnEntityProcessCompleted(ItemQueueProcess item)
        {
            var entities = item.Action.GetProperty<Dictionary<string, List<ushort>>>("Entities");
            if (entities != null)
            {
                foreach (var entity in entities)
                {
                    foreach (var entityId in entity.Value)
                    {
                        Network.DynamicEntity.ChangeOwnership(entityId, entity.Key);
                    }
                }
            }
        }
    }
}
