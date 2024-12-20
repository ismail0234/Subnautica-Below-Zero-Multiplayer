namespace Subnautica.Server.Logic
{
    using System.Collections.Generic;
    using System.Linq;

    using Core;

    using Subnautica.API.Enums;
    using Subnautica.Network.Models.Storage.Construction;
    using Subnautica.Network.Models.Storage.World.Childrens;
    using Subnautica.Network.Structures;
    using Subnautica.Server.Abstracts;
    using Subnautica.Server.Processors.Player;

    using EntityModel    = Subnautica.Network.Models.WorldEntity;
    using WorldChildrens = Subnautica.Network.Models.Storage.World.Childrens;

    public class World : BaseLogic
    {
        /**
         *
         * StaticFabricators Listesi
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private List<string> StaticFabricators = new List<string>()
        {
            "-1510194677",
            "-1510153298",
            "-1510146471",
        };

        /**
         *
         * Sınıfı başlatır
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public override void OnStart()
        {
            foreach (var entity in Core.Server.Instance.Storages.World.Storage.PersistentEntities.Where(q => q.Value.ProcessType == EntityProcessType.Plant).ToList())
            {
                var component = entity.Value.GetComponent<EntityModel.PlantEntity>();
                if (component != null && (component.TechType == TechType.IceFruit || component.TechType == TechType.IceFruitPlant))
                {
                    Core.Server.Instance.Storages.World.Storage.PersistentEntities.Remove(entity.Key);
                }
            }

            foreach (var uniqueId in this.StaticFabricators)
            {
                if (Server.Instance.Storages.Construction.GetConstruction(uniqueId) == null)
                {
                    Server.Instance.Storages.Construction.AddConstructionItem(ConstructionItem.CreateStaticItem(uniqueId, TechType.Fabricator));
                }
            }
            
            if (Server.Instance.Storages.World.Storage.IsFirstLogin == false && !Server.Instance.Storages.World.TryGetSupplyDrop(out var supplyDrop))
            {
                Server.Instance.Storages.World.Storage.IsFirstLogin = true;
            }
        }

        /**
         *
         * Her sabit tick'den sonra tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public override void OnUpdate(float deltaTime)
        {
            if (!Server.Instance.Storages.World.Storage.IsFirstLogin)
            {
                this.UpdateServerTime((double) deltaTime);
            }
        }

        /**
         *
         * Sunucu zamanını döner.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public float GetServerTime()
        {
            return (float) Server.Instance.Storages.World.Storage.ServerTime;
        }

        /**
         *
         * Sunucu zamanını döner.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public double GetServerTimeAsDouble()
        {
            return Server.Instance.Storages.World.Storage.ServerTime;
        }

        /**
         *
         * Zamanı günceller.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private void UpdateServerTime(double deltaTime)
        {
            Server.Instance.Storages.World.Storage.ServerTime += deltaTime * Server.Instance.Storages.World.Storage.WorldSpeed;

            if (Server.Instance.Storages.World.Storage.SkipTimeMode && this.GetServerTime() >= Server.Instance.Storages.World.Storage.SkipModeEndTime)
            {
                Server.Instance.Storages.World.Storage.SkipTimeMode = false;
                Server.Instance.Storages.World.Storage.WorldSpeed   = 1f;
            }
        }

        /**
         *
         * Sonraki yapı id'sini döner.
         *
         * @author Ismail  <ismaiil_0234@hotmail.com>
         *
         */
        public uint GetNextConstructionId()
        {
            if (Server.Instance.Storages.World.Storage.LastConstructionId >= uint.MaxValue)
            {
                Server.Instance.Storages.World.Storage.LastConstructionId = 0;
            }

            uint constructionId = Server.Instance.Storages.World.Storage.LastConstructionId + 1;

            while (Server.Instance.Storages.Construction.Storage.Constructions.Any(q => q.Value.Id == constructionId))
            {
                constructionId++;

                if (constructionId >= uint.MaxValue)
                {
                    constructionId = 1;
                }
            }

            Server.Instance.Storages.World.Storage.LastConstructionId = constructionId;
            return constructionId;
        }

        /**
         *
         * Sonraki eşya id'sini döner.
         *
         * @author Ismail  <ismaiil_0234@hotmail.com>
         *
         */
        public ushort GetNextItemId()
        {
            if (Server.Instance.Storages.World.Storage.LastItemId >= ushort.MaxValue)
            {
                Server.Instance.Storages.World.Storage.LastItemId = 0;
            }

            ushort itemId = (ushort) (Server.Instance.Storages.World.Storage.LastItemId + 1);

            while (Server.Instance.Storages.World.Storage.DynamicEntities.Any(q => q.Id == itemId))
            {
                itemId++;

                if (itemId >= ushort.MaxValue)
                {
                    itemId = 1;
                }
            }

            Server.Instance.Storages.World.Storage.LastItemId = itemId;
            return itemId;
        }

        /**
         *
         * Static fabricator olup olmadığına bakar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public bool IsStaticFabricator(string uniqueId)
        {
            return this.StaticFabricators.Contains(uniqueId);
        }

        /**
         *
         * WorldDynamicEntity oluşturur.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public WorldDynamicEntity CreateDynamicEntity(string uniqueId, TechType techType, ZeroVector3 position, ZeroQuaternion rotation, string ownershipId = null, bool isDeployed = true)
        {
            return CreateDynamicEntity(uniqueId, null, techType, position, rotation, ownershipId, isDeployed);
        }

        /**
         *
         * WorldDynamicEntity oluşturur.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public WorldDynamicEntity CreateDynamicEntity(string uniqueId, byte[] item, TechType techType, ZeroVector3 position, ZeroQuaternion rotation, string ownershipId = null, bool isDeployed = true)
        {
            if (Server.Instance.Storages.World.GetDynamicEntity(uniqueId) != null)
            {
                return null;
            }

            var entity = new WorldDynamicEntity()
            {
                Id             = Server.Instance.Logices.World.GetNextItemId(),
                UniqueId       = uniqueId,
                Item           = item,
                TechType       = techType,
                Position       = position,
                Rotation       = rotation,
                IsDeployed     = isDeployed,
                IsGlobalEntity = API.Features.TechGroup.IsGlobalEntity(techType),
                Component      = ItemDropProcessor.GetEntityComponent(techType),
                OwnershipId    = ownershipId,
                AddedTime      = Server.Instance.Logices.World.GetServerTime(),
            };

            if (Server.Instance.Storages.World.AddWorldDynamicEntity(entity))
            {
                return entity;
            }

            return null;
        }
    }
}