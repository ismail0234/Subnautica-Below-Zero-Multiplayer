namespace Subnautica.Server.Processors.WorldEntities
{
    using Subnautica.API.Enums;
    using Subnautica.API.Extensions;
    using Subnautica.Network.Models.Server;
    using Subnautica.Server.Abstracts.Processors;
    using Subnautica.Server.Core;

    using EntityModel = Subnautica.Network.Models.WorldEntity;

    public class FruitHarvestProcessor : WorldEntityProcessor
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
            var component = packet.Entity.GetComponent<EntityModel.PlantEntity>();
            if (component == null)
            {
                return false;
            }

            var entity = Server.Instance.Storages.World.GetPersistentEntity(packet.Entity.UniqueId);
            if (entity == null || entity.ProcessType != EntityProcessType.Plant)
            {
                entity = new EntityModel.PlantEntity(component.TechType, 0, component.MaxFruit, 0f, 0f);
            }

            var fruit = entity.GetComponent<EntityModel.PlantEntity>();
            if (fruit.SpawnInterval == 0f)
            {
                fruit.SpawnInterval = this.CalculateSpawnInterval(fruit.TechType);
            }

            if (string.IsNullOrEmpty(entity.UniqueId))
            {
                fruit.UniqueId = component.UniqueId;
            }

            fruit.SyncFruits(Server.Instance.Logices.World.GetServerTime());

            if (fruit.ActiveFruitCount > 0 && Server.Instance.Storages.World.SetPersistentEntity(entity))
            {
                fruit.SyncFruits(Server.Instance.Logices.World.GetServerTime(), true);

                if (fruit.ActiveFruitCount <= 0 && fruit.SpawnInterval == -1f)
                {
                    fruit.DisableSpawn();
                }

                packet.Entity = entity;

                profile.SendPacketToAllClient(packet);
            }

            return true;
        }
        
        /**
         *
         * Meyve yeniden doğma zamanını hesaplar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private float CalculateSpawnInterval(TechType techType)
        {
            switch (techType)
            {
                case TechType.HeatFruitPlant:
                case TechType.LeafyFruitPlant:
                case TechType.Creepvine:
                case TechType.CreepvineSeedCluster:
                    return (float)techType.GetRespawnDuration();
                case TechType.IceFruit:
                case TechType.IceFruitPlant:
                    return 0f;
            }

            return 0f;
        }
    }
}