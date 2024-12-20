namespace Subnautica.Server.Processors.Vehicle
{
    using Server.Core;

    using Subnautica.Network.Models.Core;
    using Subnautica.Server.Abstracts.Processors;

    using WorldEntityModel = Subnautica.Network.Models.WorldEntity.DynamicEntityComponents;
    using ServerModel      = Subnautica.Network.Models.Server;

    public class LightProcessor : NormalProcessor
    {
        /**
         *
         * Gelen veriyi işler
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public override bool OnExecute(AuthorizationProfile profile, NetworkPacket networkPacket)
        {
            var packet = networkPacket.GetPacket<ServerModel.VehicleLightArgs>();
            if (packet == null)
            {
                return this.SendEmptyPacketErrorLog(networkPacket);
            }

            var entity = Server.Instance.Storages.World.GetDynamicEntity(packet.UniqueId);
            if (entity == null)
            {
                return false;
            }
            
            packet.TechType = entity.TechType;

            switch (entity.TechType)
            {
                case TechType.Hoverbike:

                    var hoverbike = entity.Component.GetComponent<WorldEntityModel.Hoverbike>();
                    hoverbike.IsLightActive = packet.IsActive;

                    profile.SendPacketToOtherClients(packet);

                    break;

                case TechType.SeaTruck:

                    var seaTruck = entity.Component.GetComponent<WorldEntityModel.SeaTruck>();
                    seaTruck.IsLightActive = packet.IsActive;

                    profile.SendPacketToOtherClients(packet);

                    break;

                case TechType.MapRoomCamera:

                    var mapRoomCamera = entity.Component.GetComponent<WorldEntityModel.MapRoomCamera>();
                    mapRoomCamera.IsLightEnabled = packet.IsActive;

                    profile.SendPacketToOtherClients(packet);

                    break;
            }

            return true;
        }
    }
}
