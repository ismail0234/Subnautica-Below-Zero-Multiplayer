namespace Subnautica.Client.Synchronizations.Processors.Metadata
{
    using Subnautica.API.Enums;
    using Subnautica.API.Extensions;
    using Subnautica.API.Features;
    using Subnautica.API.Features.Helper;
    using Subnautica.Client.Abstracts.Processors;
    using Subnautica.Client.Core;
    using Subnautica.Events.EventArgs;
    using Subnautica.Network.Models.Server;
    using Subnautica.Network.Models.Storage.World.Childrens;
    using Subnautica.Network.Models.WorldEntity.DynamicEntityComponents.Shared;

    using UnityEngine;

    using Metadata    = Subnautica.Network.Models.Metadata;
    using ServerModel = Subnautica.Network.Models.Server;

    public class BaseMapRoomProcessor : MetadataProcessor
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
            var component = packet.Component.GetComponent<Metadata.BaseMapRoom>();
            if (component == null)
            {
                return false;
            }

            if (isSilence)
            {
                this.InitializeBaseMapRoom(uniqueId, component);
                return true;
            }

            if (component.ProcessType == 1)
            {
                Network.Storage.AddItemToStorage(packet.UniqueId, packet.GetPacketOwnerId(), component.PickupItem);
            }
            else if (component.ProcessType == 2)
            {
                Network.Storage.AddItemToInventory(packet.GetPacketOwnerId(), component.PickupItem);
            }
            else if (component.ProcessType == 3 || component.ProcessType == 4)
            {
                var mapRoom = Network.Identifier.GetComponentByGameObject<global::BaseDeconstructable>(packet.UniqueId)?.GetMapRoomFunctionality();
                if (mapRoom)
                {
                    mapRoom.StartScanning(component.ProcessType == 3 ? component.ScanTechType : TechType.None);

                    var scannerGui = mapRoom.GetComponentInChildren<uGUI_MapRoomScanner>(true);
                    if (scannerGui)
                    {
                        scannerGui.UpdateGUIState();
                    }
                }
            }
            else if (component.ProcessType == 5)
            {
                var action = new ItemQueueAction();
                action.OnProcessCompleted = this.OnProcessVehicleDocked;
                action.RegisterProperty("UniqueId", uniqueId);
                action.RegisterProperty("Vehicle", component.LeftDock.Vehicle);
                action.RegisterProperty("VehicleId", component.LeftDock.Vehicle.UniqueId);
                action.RegisterProperty("IsLeft", component.LeftDock.IsDocked);
                action.RegisterProperty("IsMine", ZeroPlayer.IsPlayerMine(packet.GetPacketOwnerId()));

                Entity.ProcessToQueue(action);
            }
            else if (component.ProcessType == 8)
            {
                Network.Storage.AddItemToInventory(packet.GetPacketOwnerId(), component.PickupItem);
            }

            return true;
        }

        /**
         *
         * BaseMapRoom Başlangıç ayarlamalarını yapar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private void InitializeBaseMapRoom(string uniqueId, Metadata.BaseMapRoom component)
        {
            Network.Storage.InitializeStorage(uniqueId, component.StorageContainer);

            if (component.Crafter != null)
            {
                MetadataProcessor.ExecuteProcessor(TechType.Fabricator, uniqueId, component.Crafter, true);
            }

            if (component.LeftDock.IsDocked)
            {
                Network.DynamicEntity.Spawn(component.LeftDock.Vehicle, this.OnVehicleDocked, uniqueId, true, true);
            }

            if (component.RightDock.IsDocked)
            {
                Network.DynamicEntity.Spawn(component.RightDock.Vehicle, this.OnVehicleDocked, uniqueId, false, true);
            }

            var mapRoom = Network.Identifier.GetComponentByGameObject<global::BaseDeconstructable>(uniqueId)?.GetMapRoomFunctionality();
            if (mapRoom)
            {
                mapRoom.StartScanning(component.IsScanning() ? component.ScanTechType : TechType.None);

                var scannerGui = mapRoom.GetComponentInChildren<uGUI_MapRoomScanner>(true);
                if (scannerGui)
                {
                    scannerGui.UpdateGUIState();
                }
            }
        }

        /**
         *
         * Araç demirlendiğinde tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void OnVehicleDocked(ItemQueueProcess item, global::Pickupable pickupable, GameObject gameObject)
        {
            var entity = item.Action.GetProperty<WorldDynamicEntity>("Entity");
            if (entity != null)
            {
                WorldDynamicEntityProcessor.ExecuteItemSpawnProcessor(entity.TechType, entity.Component, true, pickupable, gameObject);
            }

            this.DockVehicle(item.Action.GetProperty<string>("CustomProperty"), entity.UniqueId, item.Action.GetProperty<bool>("CustomProperty2"));
        }

        /**
         *
         * Araç demirleme veya ayrılma işlemi yapıldığında tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private void OnProcessVehicleDocked(ItemQueueProcess item)
        {
            this.DockVehicle(item.Action.GetProperty<string>("UniqueId"), item.Action.GetProperty<string>("VehicleId"), item.Action.GetProperty<bool>("IsLeft"), item.Action.GetProperty<bool>("IsMine"));
        }

        /**
         *
         * Aracı demirler.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private bool DockVehicle(string uniqueId, string vehicleId, bool isLeft, bool isMine = false)
        {
            var mapRoom = Network.Identifier.GetComponentByGameObject<global::BaseDeconstructable>(uniqueId)?.GetMapRoomFunctionality();
            var camera  = Network.Identifier.GetComponentByGameObject<global::MapRoomCamera>(vehicleId);
            if (mapRoom == null || camera == null)
            {
                return false;
            }

            if (isMine && camera.controllingPlayer && camera.controllingPlayer == global::Player.main)
            {
                using (EventBlocker.Create(ProcessType.VehicleExit))
                {
                    camera.FreeCamera();
                    camera.ExitLockedMode();
                }
            }

            Network.DynamicEntity.RemoveEntity(vehicleId);

            foreach (var docking in mapRoom.GetComponentsInChildren<MapRoomCameraDocking>())
            {
                if ((isLeft && docking.name.Contains("dockingPoint2")) || (!isLeft && docking.name.Contains("dockingPoint1")))
                {
                    docking.DockCamera(camera);
                }
            }

            return true;
        }

        /**
         *
         * Depolamaya eşya eklendiğinde tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void OnStorageItemAdding(StorageItemAddingEventArgs ev)
        {
            if (ev.TechType == TechType.BaseMapRoom)
            {
                ev.IsAllowed = false;

                BaseMapRoomProcessor.SendPacketToServer(ev.UniqueId, processType: 1, pickupItem: WorldPickupItem.Create(ev.Item, PickupSourceType.PlayerInventory));
            }
        }

        /**
         *
         * Depolama'dan eşya kaldırıldığında tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void OnStorageItemRemoving(StorageItemRemovingEventArgs ev)
        {
            if (ev.TechType == TechType.BaseMapRoom)
            {
                ev.IsAllowed = false;

                BaseMapRoomProcessor.SendPacketToServer(ev.UniqueId, processType: 2, pickupItem: WorldPickupItem.Create(ev.Item, PickupSourceType.StorageContainer));
            }
        }

        /**
         *
         * Map Room tarama başlatılırken tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void OnBaseMapRoomScanStarting(BaseMapRoomScanStartingEventArgs ev)
        {
            ev.IsAllowed = false;

            BaseMapRoomProcessor.SendPacketToServer(ev.UniqueId, processType: 3, scanType: ev.ScanType);
        }

        /**
         *
         * Map Room tarama iptal edilirken tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void OnBaseMapRoomScanStopping(BaseMapRoomScanStoppingEventArgs ev)
        {
            ev.IsAllowed = false;

            BaseMapRoomProcessor.SendPacketToServer(ev.UniqueId, processType: 4);
        }

        /**
         *
         * MapRoomCamera yanaşırken tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void OnMapRoomCameraDocking(MapRoomCameraDockingEventArgs ev)
        {
            ev.IsAllowed = false;

            BaseMapRoomProcessor.SendPacketToServer(ev.UniqueId, processType: 5, dockingBay: new Metadata.VehicleDockingBayItem(ev.IsLeft, 0, ev.VehicleId, ev.EndPosition.ToZeroVector3(), ev.EndRotation.ToZeroQuaternion(), null));
        }

        /**
         *
         * Kamera değiştiğinde tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void OnBaseMapRoomCameraChanging(MapRoomCameraChangingEventArgs ev)
        {
            ev.IsAllowed = false;

            BaseMapRoomProcessor.SendPacketToServer(ev.UniqueId, processType: 7, isNextCamera: ev.IsNext);
        }

        /**
         *
         * Oyuncu yerden eşya aldığında tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void OnPlayerItemPickedUp(PlayerItemPickedUpEventArgs ev)
        {
            if (!ev.IsStaticWorldEntity && ev.TechType == TechType.MapRoomCamera && !Network.DynamicEntity.HasEntity(ev.UniqueId))
            {
                if (ev.Pickupable.TryGetComponent<MapRoomCamera>(out var mapRoomCamera) && mapRoomCamera.dockingPoint)
                {
                    ev.IsAllowed = false;
                    
                    var uniqueId = mapRoomCamera.dockingPoint.GetComponentInParent<MapRoomFunctionality>()?.GetBaseDeconstructable().gameObject.GetIdentityId();
                    if (uniqueId.IsNotNull())
                    {
                        BaseMapRoomProcessor.SendPacketToServer(uniqueId, processType: 8, pickupItem: WorldPickupItem.Create(ev.Pickupable));
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
        public static void SendPacketToServer(string uniqueId, WorldPickupItem pickupItem = null, TechType scanType = TechType.None, Metadata.VehicleDockingBayItem dockingBay = null, bool isNextCamera = false, byte processType = 0)
        {
            ServerModel.MetadataComponentArgs result = new ServerModel.MetadataComponentArgs()
            {
                UniqueId  = uniqueId,
                Component = new Metadata.BaseMapRoom()
                {
                    ProcessType  = processType,
                    PickupItem   = pickupItem,
                    ScanTechType = scanType,
                    LeftDock     = dockingBay,  
                    IsNextCamera = isNextCamera,
                }
            };

            NetworkClient.SendPacket(result);
        }
    }
}