namespace Subnautica.Client.Synchronizations.Processors.Vehicle
{
    using Subnautica.API.Extensions;
    using Subnautica.API.Features;
    using Subnautica.API.Features.Helper;
    using Subnautica.Client.Abstracts;
    using Subnautica.Client.Core;
    using Subnautica.Client.MonoBehaviours.World;
    using Subnautica.Events.EventArgs;
    using Subnautica.Network.Models.Core;
    using Subnautica.Network.Models.Storage.World.Childrens;
    using Subnautica.Network.Structures;

    using ServerModel = Subnautica.Network.Models.Server;

    public class SeaTruckDockingModuleProcessor : NormalProcessor
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
            var packet = networkPacket.GetPacket<ServerModel.SeaTruckDockingModuleArgs>();
            if (packet.UniqueId.IsNull())
            {
                return false;
            }

            var action = new ItemQueueAction();
            action.RegisterProperty("UniqueId" , packet.UniqueId);
            action.RegisterProperty("VehicleId", packet.VehicleId);
            action.RegisterProperty("PlayerId" , packet.GetPacketOwnerId());
            action.RegisterProperty("Vehicle"  , packet.Vehicle);
            action.RegisterProperty("IsDocking", packet.IsDocking);
            action.RegisterProperty("IsEnterUndock" , packet.IsEnterUndock);
            action.RegisterProperty("UndockPosition", packet.UndockPosition);
            action.OnProcessCompleted = this.OnDockProcessCompleted;
            
            Entity.ProcessToQueue(action);
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
            var uniqueId  = item.Action.GetProperty<string>("UniqueId");
            var vehicleId = item.Action.GetProperty<string>("VehicleId");
            var playerId  = item.Action.GetProperty<byte>("PlayerId");
            var vehicle   = item.Action.GetProperty<WorldDynamicEntity>("Vehicle");
            var isDocking = item.Action.GetProperty<bool>("IsDocking");
            var isEnterUndock  = item.Action.GetProperty<bool>("IsEnterUndock");
            var undockPosition = item.Action.GetProperty<ZeroVector3>("UndockPosition");
            if (isDocking)
            {
                this.StartDocking(uniqueId, vehicleId);
            }
            else
            {
                this.StartUndocking(uniqueId, vehicle, playerId, isEnterUndock, undockPosition);
            }
        }

        /**
         *
         * Demirlemeyi başlatır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private void StartDocking(string uniqueId, string vehicleId)
        {
            Network.DynamicEntity.RemoveEntity(vehicleId);

            var dockingBay = Network.Identifier.GetComponentByGameObject<MultiplayerSeaTruckDockingBay>(uniqueId);
            if (dockingBay)
            {
                dockingBay.StartDocking(vehicleId);
            }
        }

        /**
         *
         * Demirlemeyi çözer.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private void StartUndocking(string uniqueId, WorldDynamicEntity entity, byte playerId, bool isEnterUndock, ZeroVector3 undockPosition)
        {
            Network.DynamicEntity.AddEntity(entity);

            var dockingBay = Network.Identifier.GetComponentByGameObject<MultiplayerSeaTruckDockingBay>(uniqueId);
            if (dockingBay)
            {
                dockingBay.StartUndocking(playerId, entity.OwnershipId, isEnterUndock, undockPosition);
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
            if (ev.MoonpoolType == TechType.SeaTruckDockingModule)
            {
                ev.IsAllowed = false;

                SeaTruckDockingModuleProcessor.SendPacketToServer(ev.UniqueId, ev.VehicleId, isDocking: true);
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
            if (ev.MoonpoolType == TechType.SeaTruckDockingModule)
            {
                ev.IsAllowed = false;

                if (!Interact.IsBlocked(ev.VehicleId))
                {
                    SeaTruckDockingModuleProcessor.SendPacketToServer(ev.UniqueId, ev.VehicleId, ev.UndockPosition.ToZeroVector3(), ev.UndockRotation.ToZeroQuaternion(), false, ev.IsLeft);
                }
            }
        }

        /**
         *
         * SeaTruck modülü başlatılırken tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void OnSeaTruckModuleInitialized(SeaTruckModuleInitializedEventArgs ev)
        {
            if (ev.TechType == TechType.SeaTruckDockingModule)
            {
                ev.Module.gameObject.EnsureComponent<MultiplayerSeaTruckDockingBay>();
            }
        }

        /**
         *
         * Sunucuya paketi gönderir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void SendPacketToServer(string uniqueId, string vehicleId, ZeroVector3 undockPosition = null, ZeroQuaternion undockRotation = null, bool isDocking = false, bool isEnterUndock = false)
        {
            ServerModel.SeaTruckDockingModuleArgs request = new ServerModel.SeaTruckDockingModuleArgs()
            {
                UniqueId       = uniqueId,
                VehicleId      = vehicleId,
                IsDocking      = isDocking,
                IsEnterUndock  = isEnterUndock,
                UndockPosition = undockPosition,
                UndockRotation = undockRotation,
            };

            NetworkClient.SendPacket(request);
        }
    }
}