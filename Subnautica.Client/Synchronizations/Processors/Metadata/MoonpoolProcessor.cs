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
    using Subnautica.Network.Models.Server;
    using Subnautica.Network.Models.Storage.World.Childrens;
    using Subnautica.Network.Structures;

    using UnityEngine;

    using Metadata    = Subnautica.Network.Models.Metadata;
    using ServerModel = Subnautica.Network.Models.Server;

    public class MoonpoolProcessor : MetadataProcessor
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
            var component = packet.Component.GetComponent<Metadata.BaseMoonpool>();
            if (component == null)
            {
                return false;
            }

            if (isSilence)
            {
                if (component.IsDocked)
                {
                    Vehicle.CraftVehicle(component.Vehicle, notify: false, onCompleted: this.OnSpawnedVehicle, customProperty: uniqueId);
                }

                if (component.ExpansionManager.IsTailDocked())
                {
                    var dockingBay = Network.Identifier.GetComponentByGameObject<VehicleDockingBay>(uniqueId)?.gameObject?.EnsureComponent<MultiplayerVehicleDockingBay>();
                    if (dockingBay)
                    {
                        dockingBay.SetDockingTail(component.ExpansionManager.TailId);
                    }
                }

                return true;
            }

            if (component.IsDocking || component.IsUndocking)
            {
                var action = new ItemQueueAction();
                action.RegisterProperty("UniqueId" , uniqueId);
                action.RegisterProperty("IsDocking", component.IsDocking);
                action.RegisterProperty("PlayerId" , packet.GetPacketOwnerId());
                action.RegisterProperty("VehicleId", component.VehicleId);
                action.RegisterProperty("Vehicle"  , component.Vehicle);
                action.RegisterProperty("IsLeft"   , component.IsUndockingLeft);
                action.RegisterProperty("DockTime" , component.DockingStartTime);
                action.RegisterProperty("BackModulePosition", component.BackModulePosition);
                action.OnProcessCompleted = this.OnDockProcessCompleted;

                Entity.ProcessToQueue(action);
            }
            else if (component.ColorCustomizer != null)
            {
                var gameObject = Network.Identifier.GetGameObject(uniqueId);
                if (gameObject)
                {
                    var customizeable = gameObject.transform.parent.GetComponentInChildren<global::BaseUpgradeConsoleGeometry>();
                    if (customizeable)
                    {
                        customizeable.subNameInput.CopyFrom(component.ColorCustomizer);
                    }
                }

                var vehicle = Network.Identifier.GetComponentByGameObject<global::ICustomizeable>(component.VehicleId);
                if (vehicle != null)
                {
                    vehicle.CopyFrom(component.ColorCustomizer);
                }

                if (gameObject)
                {
                    var dockingBay = gameObject.GetComponentInChildren<MultiplayerVehicleDockingBay>();
                    if (dockingBay && dockingBay.ExpansionManager.IsActive() && dockingBay.ExpansionManager.IsTailOccupied())
                    {
                        dockingBay.ExpansionManager.UpdateTailColors();
                    }
                }
            }

            return true;
        }

        /**
         *
         * İşlem tamamlandığında tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private void OnDockProcessCompleted(ItemQueueProcess item)
        {
            var isDocking = item.Action.GetProperty<bool>("IsDocking");
            var isLeft    = item.Action.GetProperty<bool>("IsLeft");
            var dockTime  = item.Action.GetProperty<double>("DockTime");
            var uniqueId  = item.Action.GetProperty<string>("UniqueId");
            var vehicleId = item.Action.GetProperty<string>("VehicleId");
            var playerId  = item.Action.GetProperty<byte>("PlayerId");
            var vehicle   = item.Action.GetProperty<WorldDynamicEntity>("Vehicle");
            var backModulePosition = item.Action.GetProperty<ZeroVector3>("BackModulePosition");
            if (isDocking)
            {
                this.StartDocking(uniqueId, vehicleId, dockTime, backModulePosition, playerId);
            }
            else
            {
                this.StartUndocking(uniqueId, vehicle, isLeft, playerId);
            }
        }

        /**
         *
         * Araç üretildiğinde tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void OnSpawnedVehicle(WorldDynamicEntity entity, ItemQueueAction item, GameObject gameObject)
        {
            WorldDynamicEntityProcessor.ExecuteItemSpawnProcessor(entity.TechType, entity.Component, entity.IsDeployed, null, gameObject);

            var moonpoolId = item.GetProperty<string>("CustomProperty");
            if (moonpoolId.IsNotNull())
            {
                this.StartDocking(moonpoolId, entity.UniqueId, 0);
            }
        }

        /**
         *
         * Demirlemeyi başlatır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void StartDocking(string moonpoolId, string vehicleId, double startDockingTime, ZeroVector3 backModulePosition = null, byte playerId = 0)
        {
            Network.DynamicEntity.RemoveEntity(vehicleId);

            var dockingBay = Network.Identifier.GetComponentByGameObject<VehicleDockingBay>(moonpoolId);
            if (dockingBay)
            {
                var docking = dockingBay.gameObject.EnsureComponent<MultiplayerVehicleDockingBay>();
                if (docking)
                {
                    if (playerId != 0)
                    {
                        if (ZeroPlayer.IsPlayerMine(playerId))
                        {
                            docking.SetDockPlayer(true);
                            docking.SetManuelDockingPlayerId(0);
                        }
                        else
                        {
                            docking.SetDockPlayer(false);
                            docking.SetManuelDockingPlayerId(playerId);
                        }
                    }
                    else
                    {
                        docking.SetDockPlayer(false);
                        docking.SetManuelDockingPlayerId(0);
                    }

                    docking.SetBackModulePosition(backModulePosition);
                    docking.SetDockingStartTime(startDockingTime);
                    docking.StartDocking(vehicleId);
                }
            }
        }

        /**
         *
         * Demirleme durumundan çıkar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private bool StartUndocking(string moonpoolId, WorldDynamicEntity entity, bool isLeft, byte playerId)
        {
            entity.IsUsingByPlayer = true;

            Network.DynamicEntity.AddEntity(entity);

            var player = ZeroPlayer.GetPlayerById(playerId);
            if (player != null)
            {
                Network.DynamicEntity.ChangeOwnership(entity.Id, player.UniqueId);
            }

            var dockingBay = Network.Identifier.GetComponentByGameObject<VehicleDockingBay>(moonpoolId);
            if (dockingBay)
            {
                var docking = dockingBay.gameObject.EnsureComponent<MultiplayerVehicleDockingBay>();
                if (docking)
                {
                    docking.StartUndocking(playerId, isLeft);
                }
            }

            return true;
        }

        /**
         *
         * Renk değiştirme paleti seçildiğinde tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void OnSubNameInputSelecting(SubNameInputSelectingEventArgs ev)
        {
            if (ev.TechType == TechType.BaseMoonpool || ev.TechType == TechType.BaseMoonpoolExpansion)
            {
                if (!Interact.IsBlocked(ev.UniqueId))
                {
                    MoonpoolProcessor.SendPacketToServer(ev.UniqueId, isCustomizerOpening: true);
                }
                else if (Interact.IsBlocked(ev.UniqueId, true))
                {
                    ev.IsAllowed = false;
                }
            }
        }

        /**
         *
         * Renk değiştirme paleti seçimden çıktığında tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void OnSubNameInputDeselected(SubNameInputDeselectedEventArgs ev)
        {
            if (ev.TechType == TechType.BaseMoonpool || ev.TechType == TechType.BaseMoonpoolExpansion)
            {
                MoonpoolProcessor.SendPacketToServer(ev.UniqueId, colorCustomizer: new ZeroColorCustomizer(ev.Name, ev.BaseColor.ToZeroColor(), ev.StripeColor1.ToZeroColor(), ev.StripeColor2.ToZeroColor(), ev.NameColor.ToZeroColor()));
            }
        }

        /**
         *
         * Expansion -> Seatruck ayrılma tamamlandığında tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void OnBaseMoonpoolExpansionUndockingTimelineCompleting(BaseMoonpoolExpansionUndockingTimelineCompletingEventArgs ev)
        {
            ev.IsAllowed = false;

            var dockingBay = ev.GameObject.GetComponentInChildren<MultiplayerVehicleDockingBay>();
            if (dockingBay && dockingBay.ExpansionManager.IsActive())
            {
                dockingBay.ExpansionManager.OnUndockingTimelineCompleted();
            }
        }

        /**
         *
         * Expansion -> Seatruck yanaşma tamamlanırken tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void OnBaseMoonpoolExpansionDockingTimelineCompleting(BaseMoonpoolExpansionDockingTimelineCompletingEventArgs ev)
        {
            ev.IsAllowed = false;

            var dockingBay = ev.GameObject.GetComponentInChildren<MultiplayerVehicleDockingBay>();
            if (dockingBay && dockingBay.ExpansionManager.IsActive())
            {
                dockingBay.ExpansionManager.OnDockingTimelineCompleted();
            }
        }

        /**
         *
         * Expansion -> Seatruck kuyruk kenetlenme işlemi tamamlanırken tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void OnBaseMoonpoolExpansionDockTail(BaseMoonpoolExpansionDockTailEventArgs ev)
        {
            ev.IsAllowed = false;

            var dockingBay = ev.GameObject.GetComponentInChildren<MultiplayerVehicleDockingBay>();
            if (dockingBay && dockingBay.ExpansionManager.IsActive())
            {
                dockingBay.ExpansionManager.OnDockTail(ev.NewTail);
            }
        }

        /**
         *
         * Expansion -> Seatruck kuyruk ayrılma işlemi tamamlanırken tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void OnBaseMoonpoolExpansionUndockTail(BaseMoonpoolExpansionUndockTailEventArgs ev)
        {
            ev.IsAllowed = false;

            var dockingBay = ev.GameObject.GetComponentInChildren<MultiplayerVehicleDockingBay>();
            if (dockingBay && dockingBay.ExpansionManager.IsActive())
            {
                dockingBay.ExpansionManager.OnUndockTail(ev.WithEjection);
            }
        }

        /**
         *
         * SeaTruck/Exosuit rıhtıma yanaşırken tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void OnVehicleDocking(VehicleDockingEventArgs ev)
        {
            if (ev.MoonpoolType == TechType.BaseMoonpool || ev.MoonpoolType == TechType.BaseMoonpoolExpansion)
            {
                ev.IsAllowed = false;

                if (Network.DynamicEntity.IsMine(ev.VehicleId))
                {
                    MoonpoolProcessor.SendPacketToServer(ev.UniqueId, vehicleId: ev.VehicleId, isDocking: true, backModulePosition: ev.BackModulePosition.ToZeroVector3(), endPosition: ev.EndPosition.ToZeroVector3(), endRotation: ev.EndRotation.ToZeroQuaternion());
                }
            }
        }

        /**
         *
         * SeaTruck/Exosuit rıhtımdan ayrılırken tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void OnVehicleUndocking(VehicleUndockingEventArgs ev)
        {
            if (ev.MoonpoolType == TechType.BaseMoonpool || ev.MoonpoolType == TechType.BaseMoonpoolExpansion)
            {
                ev.IsAllowed = false;

                if (!Interact.IsBlocked(ev.UniqueId) && !Interact.IsBlocked(ev.VehicleId))
                {
                    MoonpoolProcessor.SendPacketToServer(ev.UniqueId, isUndocking: true, isUndockingLeft: ev.IsLeft);
                }
            }
        }

        /**
         *
         * Sunucuya paketi gönderir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void SendPacketToServer(string uniqueId, string vehicleId = null, bool isDocking = false, bool isUndocking = false, bool isUndockingLeft = false, bool isCustomizerOpening = false, ZeroVector3 backModulePosition = null, ZeroVector3 endPosition = null, ZeroQuaternion endRotation = null, ZeroColorCustomizer colorCustomizer = null)
        {
            ServerModel.MetadataComponentArgs result = new ServerModel.MetadataComponentArgs()
            {
                UniqueId  = uniqueId,
                Component = new Metadata.BaseMoonpool()
                {
                    IsCustomizerOpening = isCustomizerOpening,
                    ColorCustomizer     = colorCustomizer,
                    VehicleId           = vehicleId,
                    IsDocking           = isDocking,
                    IsUndocking         = isUndocking,
                    IsUndockingLeft     = isUndockingLeft,
                    BackModulePosition  = backModulePosition,
                    EndPosition         = endPosition,
                    EndRotation         = endRotation,
                },
            };

            NetworkClient.SendPacket(result);
        }
    }
}