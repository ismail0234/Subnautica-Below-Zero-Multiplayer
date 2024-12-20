namespace Subnautica.Client.Synchronizations.Processors.WorldEntities
{
    using System.Linq;

    using Subnautica.API.Extensions;
    using Subnautica.API.Features;
    using Subnautica.Client.Abstracts.Processors;
    using Subnautica.Client.Core;
    using Subnautica.Events.EventArgs;
    using Subnautica.Network.Core.Components;

    using EntityModel = Subnautica.Network.Models.WorldEntity;
    using ServerModel = Subnautica.Network.Models.Server;

    public class FruitHarvestProcessor : WorldEntityProcessor
    {
        /**
         *
         * Gelen veriyi işler
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public override bool OnDataReceived(NetworkWorldEntityComponent packet, byte requesterId, bool isSpawning)
        {
            var plant = packet.GetComponent<EntityModel.PlantEntity>();
            if (plant.UniqueId.IsNull())
            {
                return false;
            }

            if (isSpawning)
            {
                this.FruitHarvestSync(plant.UniqueId);
            }
            else
            {
                Network.StaticEntity.AddStaticEntity(plant);

                var player = ZeroPlayer.GetPlayerById(requesterId);
                if (player != null && player.IsMine)
                {
                    this.FruitHarvestSync(plant.UniqueId, true);
                }
                else
                {
                    this.FruitHarvestSync(plant.UniqueId);
                }
            }

            return true;
        }

        /**
         *
         * Biti/Ağacı hasatlar ve senkronize eder.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private bool FruitHarvestSync(string uniqueId, bool isHarvest = false)
        {
            var entity = Network.StaticEntity.GetEntity(uniqueId);
            if (entity == null)
            {
                return false;
            }

            var plant = entity.GetComponent<EntityModel.PlantEntity>();

            if (!entity.IsSpawnable)
            {
                if (isHarvest)
                {
                    var pickPrefab = Network.Identifier.GetComponentByGameObject<global::PickPrefab>(plant.UniqueId, true);
                    if (pickPrefab)
                    {
                        CraftData.AddToInventory(pickPrefab.pickTech, spawnIfCantAdd: false);
                    }
                }

                World.DestroyItem(entity.UniqueId);
                return true;
            }

            switch (plant.TechType)
            {
                case TechType.HeatFruitPlant:
                case TechType.LeafyFruitPlant:
                case TechType.Creepvine:
                case TechType.CreepvineSeedCluster:

                    var fruitPlant = Network.Identifier.GetComponentByGameObject<global::FruitPlant>(plant.UniqueId, true);
                    if (fruitPlant)
                    {
                        fruitPlant.inactiveFruits.Clear();
                        fruitPlant.fruitSpawnEnabled  = true;
                        fruitPlant.fruitSpawnInterval = plant.SpawnInterval;
                        fruitPlant.timeNextFruit      = plant.TimeNextFruit;

                        for (int i = 0; i < plant.MaxFruit; i++)
                        {
                            if (i >= fruitPlant.fruits.Length)
                            {
                                break;
                            }

                            var fruit = fruitPlant.fruits.ElementAt(i);
                            if (fruit)
                            {
                                if (i < plant.ActiveFruitCount)
                                {
                                    fruit.SetPickedState(false);
                                }
                                else
                                {
                                    fruit.SetPickedState(true);
                                }

                                if (fruit.pickedState)
                                {
                                    fruitPlant.inactiveFruits.Add(fruit);
                                }
                            }
                        }

                        if (isHarvest)
                        {
                            CraftData.AddToInventory(fruitPlant.fruits[0].pickTech, spawnIfCantAdd: false);
                        }
                    }

                    break;
            }

            return true;
        }

        /**
         *
         * Bir meyve hasat edildğinde tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void OnFruitHarvesting(FruitHarvestingEventArgs ev)
        {
            if (ev.IsStaticWorldEntity)
            {
                ev.IsAllowed = false;

                ServerModel.WorldEntityActionArgs request = new ServerModel.WorldEntityActionArgs()
                {
                    Entity = new EntityModel.PlantEntity()
                    {
                        UniqueId = ev.UniqueId,
                        TechType = ev.TechType,
                        MaxFruit = ev.MaxSpawnableFruit,
                    },
                };

                NetworkClient.SendPacket(request);
            }
        }
    }
}