namespace Subnautica.Client.Synchronizations.Processors.Vehicle
{
    using Subnautica.API.Extensions;
    using Subnautica.API.Features;
    using Subnautica.API.Features.Helper;
    using Subnautica.Client.Abstracts;
    using Subnautica.Client.Core;
    using Subnautica.Events.EventArgs;
    using Subnautica.Network.Models.Core;

    using ServerModel = Subnautica.Network.Models.Server;

    public class LightProcessor : NormalProcessor
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
            var packet = networkPacket.GetPacket<ServerModel.VehicleLightArgs>();
            if (string.IsNullOrEmpty(packet.UniqueId))
            {
                return false;
            }

            var action = new ItemQueueAction();
            action.OnProcessCompleted = this.OnProcessCompleted;
            action.RegisterProperty("UniqueId", packet.UniqueId);
            action.RegisterProperty("IsActive", packet.IsActive);
            action.RegisterProperty("TechType", packet.TechType);

            Entity.ProcessToQueue(action);
            return true;
        }

        /**
         *
         * Nesne işlemi tamamlandıktan sonra tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void OnProcessCompleted(ItemQueueProcess item)
        {
            var techType = item.Action.GetProperty<TechType>("TechType");
            if (techType == TechType.Hoverbike)
            {
                var hoverBike = Network.Identifier.GetComponentByGameObject<global::Hoverbike>(item.Action.GetProperty<string>("UniqueId"));
                if (hoverBike)
                {
                    ZeroGame.SetLightsActive(hoverBike.toggleLights, item.Action.GetProperty<bool>("IsActive"));
                }
            }
            else if (techType == TechType.SeaTruck)
            {
                var seaTruck = Network.Identifier.GetComponentByGameObject<global::SeaTruckLights>(item.Action.GetProperty<string>("UniqueId"));
                if (seaTruck)
                {
                    ZeroGame.SetLightsActive(seaTruck, item.Action.GetProperty<bool>("IsActive"));
                }
            }
            else if (techType == TechType.MapRoomCamera)
            {
                var mapRoomCamera = Network.Identifier.GetComponentByGameObject<global::MapRoomCamera>(item.Action.GetProperty<string>("UniqueId"));
                if (mapRoomCamera)
                {
                    mapRoomCamera.lightsParent.SetActive(item.Action.GetProperty<bool>("IsActive"));
                }
            }
        }

        /**
         *
         * Araç ışıkları yanıp/söndüğünde tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void OnVehicleLightChanged(LightChangedEventArgs ev)
        {
            if (ev.TechType.IsVehicle(true, false))
            {
                var entity = Network.DynamicEntity.GetEntity(ev.UniqueId);
                if (entity != null && entity.IsMine(ZeroPlayer.CurrentPlayer.UniqueId))
                {
                    ServerModel.VehicleLightArgs request = new ServerModel.VehicleLightArgs()
                    {
                        UniqueId = ev.UniqueId,
                        IsActive = ev.IsActive,
                    };

                    NetworkClient.SendPacket(request);
                }
            }
        }
    }
}