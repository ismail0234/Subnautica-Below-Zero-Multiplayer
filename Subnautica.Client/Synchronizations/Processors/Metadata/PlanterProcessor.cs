namespace Subnautica.Client.Synchronizations.Processors.Metadata
{
    using Subnautica.API.Extensions;
    using Subnautica.API.Features;
    using Subnautica.API.Features.Helper;
    using Subnautica.Client.Abstracts.Processors;
    using Subnautica.Client.Core;
    using Subnautica.Client.Extensions;
    using Subnautica.Client.MonoBehaviours.World;
    using Subnautica.Events.EventArgs;
    using Subnautica.Network.Models.Metadata;
    using Subnautica.Network.Models.Server;

    using UnityEngine;

    using Metadata    = Subnautica.Network.Models.Metadata;
    using ServerModel = Subnautica.Network.Models.Server;

    public class PlanterProcessor :  MetadataProcessor
    {
        /**
         *
         * Gelen veriyi işler
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public override bool OnDataReceived(string uniqueId, TechType techType, MetadataComponentArgs packet, bool isSilence)
        {
            var component = packet.Component.GetComponent<Metadata.Planter>();
            if (component == null)
            {
                return false;
            }

            var planter = Network.Identifier.GetComponentByGameObject<global::Planter>(packet.UniqueId);
            if (planter == null)
            {
                return false;
            }

            if (isSilence)
            {
                foreach (var item in component.Items)
                {
                    Entity.SpawnToQueue(item.TechType, item.ItemId, planter.storageContainer.container, this.GetItemAction(planter, item, 1));
                }

                return true;
            }

            if (component.IsOpening)
            {
                planter.storageContainer.Open(planter.storageContainer.transform);
            }
            else
            {
                if (component.CurrentItem != null)
                {
                    if (component.IsAdding)
                    {
                        if (ZeroPlayer.IsPlayerMine(packet.GetPacketOwnerId()))
                        {
                            Entity.ProcessToQueue(this.GetItemAction(planter, component.CurrentItem, 0, true));
                        }
                        else
                        {
                            Entity.SpawnToQueue(component.CurrentItem.TechType, component.CurrentItem.ItemId, planter.storageContainer.container, this.GetItemAction(planter, component.CurrentItem, 1));
                        }
                    }
                    else if (component.IsHarvesting)
                    {
                        if (ZeroPlayer.IsPlayerMine(packet.GetPacketOwnerId()))
                        {
                            Entity.ProcessToQueue(this.GetItemAction(planter, component.CurrentItem, 3, true));
                        }
                        else
                        {
                            Entity.ProcessToQueue(this.GetItemAction(planter, component.CurrentItem, 3));
                        }
                    }
                    else if (component.CurrentItem.Health != -1f)
                    {
                        if (component.CurrentItem.Health <= 0f)
                        {
                            Entity.RemoveToQueue(component.CurrentItem.ItemId);
                        }
                        else
                        {
                            Entity.ProcessToQueue(this.GetItemAction(planter, component.CurrentItem, 2));
                        }
                    }
                }

            }
            return true;
        }

        /**
         *
         * Nesne Olayını döner.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private ItemQueueAction GetItemAction(global::Planter planter, Metadata.PlanterItem item, int processType, bool isMine = false)
        {
            var action = new ItemQueueAction();
            action.RegisterProperty("Item"   , item);
            action.RegisterProperty("IsMine" , isMine);
            action.RegisterProperty("Planter", planter);

            switch (processType)
            {
                case 0:
                    action.OnProcessCompleted = this.OnItemProcessCompleted;
                    break;
                case 1:
                    action.OnEntitySpawned    = this.OnEntitySpawned;
                    break;
                case 2:
                    action.OnProcessCompleted = this.OnHealthProcessCompleted;
                    break;
                case 3:
                    action.OnProcessCompleted = this.OnHarvestingProcessCompleted;
                    break;
            }

            return action;
        }

        /**
         *
         * PDA Açılırken tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void OnStorageOpening(StorageOpeningEventArgs ev)
        {
            if (TechGroup.Planters.Contains(ev.TechType) || ev.TechType == TechType.BaseWaterPark)
            {
                ev.IsAllowed = false;

                if (!Interact.IsBlocked(ev.ConstructionId))
                {
                    PlanterProcessor.SendPacketToServer(ev.ConstructionId, isOpening: true);
                }
            }
        }

        /**
         *
         * Bir nesne hasar aldığında tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void OnTakeDamaging(TakeDamagingEventArgs ev)
        {
            if (!ev.IsStaticWorldEntity && ev.IsDestroyable && ev.Damage > 0)
            {
                var planter = ev.LiveMixin.GetComponentInParent<global::Planter>();
                if (planter)
                {
                    var grownPlant = ev.LiveMixin.GetComponentInParent<GrownPlant>();
                    if (grownPlant && grownPlant.seed)
                    {
                        PlanterProcessor.SendPacketToServer(planter.gameObject.GetIdentityId(), grownPlant.seed.gameObject.GetIdentityId(), health: ev.NewHealth);
                    }
                }
            }
        }

        /**
         *
         * Bitki hasat değildiğinde tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void OnFruitHarvesting(FruitHarvestingEventArgs ev)
        {
            if (!ev.IsStaticWorldEntity)
            {
                var planter = ev.PickPrefab.GetComponentInParent<global::Planter>();
                if (planter)
                {
                    var grownPlant = ev.PickPrefab.GetComponentInParent<GrownPlant>();
                    if (grownPlant && grownPlant.seed)
                    {
                        ev.IsAllowed = false;

                        PlanterProcessor.SendPacketToServer(planter.gameObject.GetIdentityId(), grownPlant.seed.gameObject.GetIdentityId(), maxSpawnableFruit: ev.MaxSpawnableFruit, fruitSpawnInterval: ev.SpawnInterval, isHarvesting: true);
                    }
                }
            }
        }

        /**
         *
         * Bitki hasat değildiğinde tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void OnGrownPlantHarvesting(GrownPlantHarvestingEventArgs ev)
        {
            var planter = ev.GrownPlant.GetComponentInParent<global::Planter>();
            if (planter)
            {
                ev.IsAllowed = false;

                PlanterProcessor.SendPacketToServer(planter.gameObject.GetIdentityId(), ev.UniqueId, isHarvesting: true);
            }
        }

        /**
         *
         * Saksıya bitki eklenince tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void OnPlanterItemAdded(PlanterItemAddedEventArgs ev)
        {
            if (ev.Plantable.TryGetComponent<LiveMixin>(out var liveMixin))
            {
                PlanterProcessor.SendPacketToServer(ev.UniqueId, item: ev.Plantable, itemId: ev.ItemId, slotId: ev.SlotId, health: liveMixin.health == -1 ? liveMixin.maxHealth : liveMixin.health, isAdding: true);
            }
            else
            {
                PlanterProcessor.SendPacketToServer(ev.UniqueId, item: ev.Plantable, itemId: ev.ItemId, slotId: ev.SlotId, isAdding: true);
            }
        }

        /**
         *
         * Nesne işlemi tamamlandıktan sonra tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void OnItemProcessCompleted(ItemQueueProcess item)
        {
            var planterItem = item.Action.GetProperty<Metadata.PlanterItem>("Item");
            var planter     = item.Action.GetProperty<global::Planter>("Planter");
            if (planterItem != null && planter != null)
            {
                var slot = planter.GetSlotByID(planterItem.SlotId);
                if (slot != null && slot.isOccupied)
                {
                    var component = slot.plantable.gameObject.EnsureComponent<PlanterItemComponent>();
                    component.SetHealth(planterItem.Health);
                    component.SetTimeNextFruit(planterItem.TimeNextFruit);
                    component.SetActiveFruitCount(planterItem.ActiveFruitCount);
                    component.SetStartingTime(planterItem.TimeStartGrowth);

                    if (slot.plantable && slot.plantable.growingPlant)
                    {
                        slot.plantable.growingPlant.timeStartGrowth = planterItem.TimeStartGrowth;
                    }
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
        public void OnEntitySpawned(ItemQueueProcess item, Pickupable pickupable, GameObject gameObject)
        {
            var planterItem = item.Action.GetProperty<Metadata.PlanterItem>("Item");
            var planter     = item.Action.GetProperty<global::Planter>("Planter");
            if (planterItem != null && planter != null)
            {
                var plantable = pickupable.inventoryItem.item.GetComponent<Plantable>();
                pickupable.inventoryItem.item.SetTechTypeOverride(plantable.plantTechType);
                pickupable.inventoryItem.isEnabled = false;

                using (EventBlocker.Create(TechType.PlanterPot))
                using (EventBlocker.Create(TechType.PlanterPot2))
                using (EventBlocker.Create(TechType.PlanterPot3))
                using (EventBlocker.Create(TechType.PlanterBox))
                using (EventBlocker.Create(TechType.PlanterShelf))
                using (EventBlocker.Create(TechType.FarmingTray))
                using (EventBlocker.Create(TechType.BaseWaterPark))
                {
                    planter.AddItem(plantable, planterItem.SlotId);
                }

                if (plantable != null && plantable.gameObject != null)
                {
                    var component = plantable.gameObject.EnsureComponent<PlanterItemComponent>();
                    component.SetHealth(planterItem.Health);
                    component.SetTimeNextFruit(planterItem.TimeNextFruit);
                    component.SetActiveFruitCount(planterItem.ActiveFruitCount);
                    component.SetStartingTime(planterItem.TimeStartGrowth);                    
                }

                var slot = planter.GetSlotByID(planterItem.SlotId);
                if (slot != null && slot.isOccupied && slot.plantable)
                {
                    if (slot.plantable.growingPlant)
                    {
                        slot.plantable.growingPlant.timeStartGrowth = planterItem.TimeStartGrowth;
                    }
                }
            }
        }

        /**
         *
         * Nesne işlemi tamamlandıktan sonra tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void OnHealthProcessCompleted(ItemQueueProcess item)
        {
            var planterItem = item.Action.GetProperty<Metadata.PlanterItem>("Item");
            if (planterItem != null)
            {
                var plantable = Network.Identifier.GetComponentByGameObject<Plantable>(planterItem.ItemId);
                if (plantable)
                {
                    plantable.gameObject.EnsureComponent<PlanterItemComponent>().SetHealth(planterItem.Health);
                }
            }
        }

        /**
         *
         * Saksı nesnesi büyüdüğünde tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void OnPlanterProgressCompleted(PlanterProgressCompletedEventArgs ev)
        {
            if (ev.Plantable.TryGetComponent<PlanterItemComponent>(out var planterItem))
            {
                if (planterItem.Health > 0 && ev.GrownPlant.TryGetComponent<global::LiveMixin>(out var liveMixin))
                {
                    liveMixin.health = planterItem.Health;
                }
            }
        }

        /**
         *
         * Nesne işlemi tamamlandıktan sonra tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void OnHarvestingProcessCompleted(ItemQueueProcess item)
        {
            var planterItem = item.Action.GetProperty<Metadata.PlanterItem>("Item");
            if (planterItem != null)
            {
                var plantable = Network.Identifier.GetComponentByGameObject<global::Plantable>(planterItem.ItemId);
                if (plantable && plantable.linkedGrownPlant && plantable.linkedGrownPlant.TryGetComponent<FruitPlant>(out var fruitPlant))
                {
                    fruitPlant.inactiveFruits.Clear();
                    fruitPlant.timeNextFruit = planterItem.TimeNextFruit;

                    foreach (var fruit in fruitPlant.fruits)
                    {
                        if (planterItem.ActiveFruitCount > 0)
                        {
                            planterItem.ActiveFruitCount--;

                            if (fruit.pickedState)
                            {
                                fruit.SetPickedState(false);
                            }
                        }
                        else if (!fruit.pickedState)
                        {
                            fruit.SetPickedState(true);
                        }

                        if (fruit.pickedState)
                        {
                            fruitPlant.inactiveFruits.Add(fruit);
                        }
                    }

                    if (item.Action.GetProperty<bool>("IsMine"))
                    {
                        CraftData.AddToInventory(fruitPlant.fruits[0].pickTech, spawnIfCantAdd: false);
                    }
                }
                else if (plantable.linkedGrownPlant)
                {
                    plantable.linkedGrownPlant.seed.currentPlanter.RemoveItem(plantable.linkedGrownPlant.seed);

                    if (plantable.linkedGrownPlant.TryGetComponent<PickPrefab>(out var pickPrefab))
                    {
                        if (item.Action.GetProperty<bool>("IsMine"))
                        {
                            CraftData.AddToInventory(pickPrefab.pickTech, spawnIfCantAdd: false);
                        }

                        World.DestroyGameObject(plantable.gameObject);
                    }
                    else
                    {
                        if (item.Action.GetProperty<bool>("IsMine"))
                        {
                            global::Inventory.Get().Pickup(plantable.linkedGrownPlant.seed.pickupable);
                            global::Player.main.PlayGrab();
                        }
                        else
                        {
                            World.DestroyGameObject(plantable.gameObject);
                        }
                    }
                }
            }
        }

        /**
         *
         * Saksı'daki toplanabilir bitki büyüdüğünde tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void OnPlanterGrowned(PlanterGrownedEventArgs ev)
        {
            if (ev.FruitPlant.TryGetComponent<GrownPlant>(out var grownPlant))
            {
                if (grownPlant.seed.TryGetComponent<PlanterItemComponent>(out var component))
                {
                    ev.FruitPlant.timeNextFruit = component.TimeNextFruit;

                    for (int i = 0; i < component.ActiveFruitCount; i++)
                    {
                        ev.FruitPlant.fruits[i].SetPickedState(false);
                    }
                }
            }
        }

        /**
         *
         * Sunucuya veri gönderir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private static void SendPacketToServer(string uniqueId, string itemId = null, byte maxSpawnableFruit = 0, float fruitSpawnInterval = -1, Plantable item = null, int slotId = -1, float health = -1f, bool isHarvesting = false, bool isAdding = false, bool isOpening = false)
        {
            var planterItem = new PlanterItem();
            planterItem.ItemId = itemId;
            planterItem.SlotId = (byte) slotId;
            planterItem.Health = health;
            planterItem.MaxSpawnableFruit  = maxSpawnableFruit;
            planterItem.FruitSpawnInterval = fruitSpawnInterval;

            if (item != null)
            {
                planterItem.TechType = CraftData.GetTechType(item.pickupable.gameObject);

                if (item.growingPlant)
                {
                    planterItem.Duration = (short) item.growingPlant.growthDuration;
                }
            }

            ServerModel.MetadataComponentArgs result = new ServerModel.MetadataComponentArgs()
            {
                UniqueId  = uniqueId,
                Component = new Metadata.Planter()
                {
                    IsOpening    = isOpening,
                    IsHarvesting = isHarvesting,
                    IsAdding     = isAdding,
                    CurrentItem  = itemId == null ? null : planterItem,
                }
            };

            NetworkClient.SendPacket(result);
        }
    }
}