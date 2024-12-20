namespace Subnautica.Server.Processors.World
{
    using Server.Core;

    using Subnautica.Network.Models.Core;
    using Subnautica.Server.Abstracts.Processors;

    using ClientModel = Subnautica.Network.Models.Client;

    public class WeatherProcessor : NormalProcessor
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
            var packet = networkPacket.GetPacket<ClientModel.WeatherChangedArgs>();
            if (packet == null)
            {
                return this.SendEmptyPacketErrorLog(networkPacket);
            }

            profile.SetWeatherProfile(packet.ProfileId);

            Server.Instance.Logices.Weather.SendWeatherToClient(profile);
            return true;
        }
    }
}