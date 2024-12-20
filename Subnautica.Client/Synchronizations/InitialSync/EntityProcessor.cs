namespace Subnautica.Client.Synchronizations.InitialSync
{
    using System.Linq;
    using Oculus.Platform;

    using Subnautica.API.Extensions;
    using Subnautica.API.Features;
    using Subnautica.API.Features.Helper;
    using Subnautica.Client.Abstracts.Processors;
    using Subnautica.Client.Extensions;
    using Subnautica.Network.Models.Storage.World.Childrens;
    using Subnautica.Network.Structures;

    using UnityEngine;

    public class EntityProcessor
    {
        /**
         *
         * Yumurtlamayacak nesneleri ayarlar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void OnEntityRestrictedInitialized()
        {
            if (Network.Session.Current.PersistentEntities != null)
            {
                Network.StaticEntity.SetStaticEntities(Network.Session.Current.PersistentEntities);
            }
        }

        /**
         *
         * Dünyadaki nesneleri yumurtlar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void OnWorldItemsSpawn()
        {
            if (QuantumLockerStorage.main == null)
            {
                QuantumLockerStorage.GetStorageContainer(true);
            }
                
            if (Network.Session.Current.QuantumLocker?.Items.Count > 0)
            {
                foreach (var item in Network.Session.Current.QuantumLocker.Items)
                {
                    Entity.SpawnToQueue(item.Item, item.ItemId, QuantumLockerStorage.main.storageContainer.container);
                }
            }

            if (Network.Session.Current.DynamicEntities != null)
            {
                foreach (var entity in Network.Session.Current.DynamicEntities)
                {
                    Network.DynamicEntity.Spawn(entity, OnEntitySpawned);
                }
            }

            if (Network.Session.Current.CosmeticItems != null)
            {
                foreach (var entity in Network.Session.Current.CosmeticItems)
                {
                    var action = new ItemQueueAction(null, OnCosmeticItemSpawned);
                    action.RegisterProperty("BaseId", entity.BaseId);

                    Entity.SpawnToQueue(entity.StorageItem.TechType, entity.StorageItem.ItemId, new ZeroTransform(entity.Position, entity.Rotation), action);
                }
            }
        }

        /**
         *
         * Nesne spawnlandıktan sonra tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void OnEntitySpawned(ItemQueueProcess item, Pickupable pickupable, GameObject gameObject)
        {
            var entity = item.Action.GetProperty<WorldDynamicEntity>("Entity");
            if (entity.IsDeployed == false)
            {
                pickupable.MultiplayerDrop();
            }

            if (entity.Component != null)
            {
                WorldDynamicEntityProcessor.ExecuteItemSpawnProcessor(entity.TechType, entity.Component, entity.IsDeployed, pickupable, gameObject);
            }
        }

        /**
         *
         * Nesne spawnlandıktan sonra tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void OnCosmeticItemSpawned(ItemQueueProcess item, Pickupable pickupable, GameObject gameObject)
        {
            pickupable.MultiplayerPlace(item.Action.GetProperty<string>("BaseId"));
        }
    }
}
