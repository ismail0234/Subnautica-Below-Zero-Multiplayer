namespace Subnautica.Server.Processors.WorldEntities
{
    using System.Linq;

    using Subnautica.API.Extensions;
    using Subnautica.Network.Models.Metadata;
    using Subnautica.Network.Models.Server;
    using Subnautica.Network.Models.WorldEntity.DynamicEntityComponents.Shared;
    using Subnautica.Server.Abstracts.Processors;
    using Subnautica.Server.Core;

    using EntityModel = Subnautica.Network.Models.WorldEntity;

    public class DestroyableDynamicEntityProcessor : WorldEntityProcessor
    {
        /**
         *
         * Gelen veriyi işler
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public override bool OnDataReceived(AuthorizationProfile profile, WorldEntityActionArgs packet)
        {
            var component = packet.Entity.GetComponent<EntityModel.DestroyableDynamicEntity>();
            if (component == null)
            {
                return false;
            }

            if (component.IsWorldStreamer)
            {
                this.DisableCreatureHome(component.UniqueId.WorldStreamerToSlotId());

                if (Core.Server.Instance.Storages.World.DisableSlot(component.UniqueId))
                {
                    component.PickupItem = WorldPickupItem.Create(StorageItem.Create(component.UniqueId, TechType.None), API.Enums.PickupSourceType.EntitySlot);
                    component.PickupItem.NextRespawnTime = Core.Server.Instance.Storages.World.GetSlotNextRespawnTime(component.UniqueId);

                    profile.SendPacketToAllClient(packet);
                }
            }
            else
            {
                if (Server.Instance.Storages.World.RemoveDynamicEntity(component.UniqueId))
                {
                    profile.SendPacketToAllClient(packet);
                }
            }

            return true;
        }

        /**
         *
         * Yaratık yuvasını pasif yapar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private bool DisableCreatureHome(int slotId)
        {
            var slot = Subnautica.API.Features.Network.WorldStreamer.GetSlotById(slotId);
            if (slot == null || slot.TechType != TechType.CrashHome)
            {
                return false;
            }

            foreach (var creature in Server.Instance.Logices.CreatureWatcher.GetCreatures(TechType.Crash))
            {
                if (creature.Value.LeashPosition == slot.LeashPosition)
                {
                    Server.Instance.Logices.CreatureWatcher.UnRegisterCreature(creature.Value.Id);
                    return true;
                }
            }

            return false;
        }
    }
}
