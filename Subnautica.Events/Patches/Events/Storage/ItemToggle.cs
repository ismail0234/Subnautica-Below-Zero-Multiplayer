namespace Subnautica.Events.Patches.Events.Storage
{
    using System;

    using HarmonyLib;

    using Subnautica.API.Extensions;
    using Subnautica.API.Features;
    using Subnautica.Events.EventArgs;
    using Subnautica.Events.Patches.Fixes.Interact;

    using UnityEngine;

    [HarmonyPatch(typeof(global::Inventory), nameof(global::Inventory.AddOrSwap), new Type[] { typeof(InventoryItem), typeof(IItemsContainer), typeof(InventoryItem) })]
    public static class ItemToggle
    {
        private static bool Prefix(InventoryItem itemA, IItemsContainer containerB, InventoryItem itemB)
        {
            if (!Network.IsMultiplayerActive)
            {
                return true;
            }

            if (itemA == null || !itemA.CanDrag(verbose: true))
            {
                return false;
            }

            var item = itemA.item;
            if (item == null)
            {
                return false;
            }

            var itemsContainer = itemA.container;
            if (itemsContainer == null)
            {
                return false;
            }

            if (itemB != null)
            {
                containerB = itemB.container;
            }

            if (containerB == null)
            {
                return false;
            }

            var equipment = itemsContainer as Equipment;
            var equipment2 = containerB as Equipment;

            var flag = equipment != null;
            var num = equipment2 != null;

            TechData.GetEquipmentType(item.GetTechType());

            if (num)
            {
                string slot = string.Empty;
                if (itemB != null && !equipment2.GetItemSlot(itemB.item, ref slot))
                {
                    return false;
                }

                return global::Inventory.AddOrSwap(itemA, equipment2, slot);
            }

            if (itemB != null)
            {
                // Storage içinde diğer tarafa sürükle bırak ile nesne üzerine bırakılınca tetiklenir.
                Log.Info("BLOCK...");
                return false;
            }

            var slot2 = string.Empty;
            if (flag && !equipment.GetItemSlot(item, ref slot2))
            {
                return false;
            }

            if (itemsContainer == containerB)
            {
                return false;
            }

            // Equipment
            // ItemsContainer
            // StorageSlot
            if (containerB is global::ItemsContainer && itemA.container is global::ItemsContainer)
            {
                return !AddItemToItemsContainer(containerB as global::ItemsContainer, itemA);
            }

            return true;
        }

        /**
         *
         * Kapsayıcıya nesneyi eklemeye çalışır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private static bool AddItemToItemsContainer(global::ItemsContainer targetContainer, InventoryItem item)
        {
            if (item == null || item.item == null)
            {
                return true;
            }

            if (!((IItemsContainer)targetContainer).AllowedToAdd(item.item, true) || !targetContainer.HasRoomFor(item.item))
            {
                if (targetContainer.errorSound)
                {
                    FMODUWE.PlayOneShot(targetContainer.errorSound, Vector3.zero);
                }

                return true;
            }

            if (item.container != null && !item.container.AllowedToRemove(item.item, true))
            {
                return true;
            }

            try
            {
                var container = ItemContainer.GetInformation(targetContainer, item.item.inventoryItem.container as ItemsContainer);
                if (string.IsNullOrEmpty(container.UniqueId) || container.TechType == TechType.None)
                {
                    return true;
                }

                if (ItemContainer.IsRemoving(targetContainer))
                {
                    StorageItemRemovingEventArgs args = new StorageItemRemovingEventArgs(container.UniqueId, container.TechType, Network.Identifier.GetIdentityId(item.item.gameObject), item.item);

                    Handlers.Storage.OnItemRemoving(args);

                    return !args.IsAllowed;
                }
                else
                {
                    StorageItemAddingEventArgs args = new StorageItemAddingEventArgs(container.UniqueId, container.TechType, Network.Identifier.GetIdentityId(item.item.gameObject), item.item);

                    Handlers.Storage.OnItemAdding(args);

                    return !args.IsAllowed;
                }
            }
            catch (Exception e)
            {
                Log.Error($"Storage.ItemAdding/ItemRemoving: {e}\n{e.StackTrace}");
            }

            return true;
        }
    }

    public class ItemContainer
    {
        /**
         *
         * UniqueId değerini barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public string UniqueId { get; set; }

        /**
         *
         * TechType değerini barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public TechType TechType { get; set; }

        /**
         *
         * Sınıf ayarlamalarını yapar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public ItemContainer()
        {
        }

        /**
         *
         * Sınıf ayarlamalarını yapar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public ItemContainer(string uniqueId, TechType techType)
        {
            this.UniqueId = uniqueId;
            this.TechType = techType;
        }

        /**
         *
         * TechType değerini barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static ItemContainer GetInformation(global::ItemsContainer targetContainer, global::ItemsContainer itemContainer)
        {
            var gameObject = GetContainer(targetContainer, itemContainer);
            if (gameObject == null)
            {
                return new ItemContainer();
            }

            if (gameObject.TryGetComponent<global::Constructable>(out var constructable))
            {
                return new ItemContainer(Network.Identifier.GetIdentityId(constructable.gameObject), constructable.techType);
            }

            if (gameObject.TryGetComponent<global::LifepodDrop>(out var lifepod))
            {
                return new ItemContainer(Network.Identifier.GetIdentityId(lifepod.gameObject, false), TechType.EscapePod);
            }

            if (gameObject.TryGetComponent<global::BaseBioReactor>(out var bioReactor))
            {
                return new ItemContainer(Network.Identifier.GetIdentityId(bioReactor.GetModel().gameObject, false), TechType.BaseBioReactor);
            }

            if (gameObject.TryGetComponent<global::BaseNuclearReactor>(out var nuclearReactor))
            {
                return new ItemContainer(Network.Identifier.GetIdentityId(nuclearReactor.GetModel().gameObject, false), TechType.BaseNuclearReactor);
            }

            if (gameObject.TryGetComponent<global::SpyPenguin>(out var spyPenguin))
            {
                return new ItemContainer(Network.Identifier.GetIdentityId(spyPenguin.gameObject, false), TechType.SpyPenguin);
            }

            if (gameObject.TryGetComponent<global::FiltrationMachine>(out var filtrationMachine))
            {
                return new ItemContainer(Network.Identifier.GetIdentityId(filtrationMachine.GetModel().gameObject, false), TechType.BaseFiltrationMachine);
            }

            if (gameObject.TryGetComponent<global::MapRoomFunctionality>(out var mapRoomFunctionality))
            {
                return new ItemContainer(mapRoomFunctionality.GetBaseDeconstructable()?.gameObject?.GetIdentityId(), TechType.BaseMapRoom);
            }

            var waterPark = gameObject.GetComponentInParent<WaterPark>();
            if (waterPark)
            {
                return new ItemContainer(waterPark.gameObject.GetIdentityId(), TechType.BaseWaterPark);
            }

            var quantumLocker = gameObject.GetComponentInParent<global::QuantumLockerStorage>();
            if (quantumLocker)
            {
                if (global::Player.main.guiHand.GetActiveTarget())
                {
                    return new ItemContainer(Network.Identifier.GetIdentityId(global::Player.main.guiHand.GetActiveTarget().GetComponentInParent<LargeWorldEntity>().gameObject, false), TechType.QuantumLocker);
                }

                return new ItemContainer();
            }

            if (gameObject.TryGetComponent<global::SeaTruckSegment>(out var seaTruckSegment))
            {
                if (global::Player.main.guiHand.GetActiveTarget())
                {
                    return new ItemContainer(Network.Identifier.GetIdentityId(global::Player.main.guiHand.GetActiveTarget().gameObject, false), CraftData.GetTechType(gameObject));
                }

                return new ItemContainer();
            }

            if (gameObject.TryGetComponent<global::LargeWorldEntity>(out var lwe))
            {
                return new ItemContainer(Network.Identifier.GetIdentityId(lwe.gameObject, false), CraftData.GetTechType(lwe.gameObject));
            }

            return new ItemContainer();
        }

        /**
         *
         * Kaldırma işlemi olup olmadığına bakar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static bool IsRemoving(ItemsContainer targetContainer)
        {
            var techType = CraftData.GetTechType(targetContainer.tr.parent.gameObject);
            if (techType == TechType.Player)
            {
                return true;
            }

            return false;
        }

        /**
         *
         * Container sınıfını döner.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private static GameObject GetContainer(global::ItemsContainer targetContainer, global::ItemsContainer itemContainer)
        {
            if (IsRemoving(targetContainer))
            {
                return itemContainer.tr.parent.gameObject;
            }

            return targetContainer.tr.parent.gameObject;
        }
    }
}
