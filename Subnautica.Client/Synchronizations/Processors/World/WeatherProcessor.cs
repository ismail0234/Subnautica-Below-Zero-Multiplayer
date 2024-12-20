namespace Subnautica.Client.Synchronizations.Processors.World
{
    using Subnautica.Client.Abstracts;
    using Subnautica.Client.Core;
    using Subnautica.Events.EventArgs;
    using Subnautica.Network.Models.Core;

    using ServerModel = Subnautica.Network.Models.Server;
    using ClientModel = Subnautica.Network.Models.Client;

    public class WeatherProcessor : NormalProcessor
    {
        /**
         *
         * Hava durumu olayını barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private WeatherEvent WeatherEvent { get; set; } = new WeatherEvent()
        {
            weatherSet = new WeatherEventDataSet(),
            parameters = new WeatherParameters(),
        };

        /**
         *
         * Gelen veriyi işler
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public override bool OnDataReceived(NetworkPacket networkPacket)
        {
            var packet = networkPacket.GetPacket<ServerModel.WeatherChangedArgs>();
            if (packet == null)
            {
                return false;
            }

            this.WeatherEvent.weatherSet.dangerLevel             = packet.DangerLevel;
            this.WeatherEvent.parameters.windDir                 = packet.WindDir;
            this.WeatherEvent.parameters.windSpeed               = packet.WindSpeed;
            this.WeatherEvent.parameters.fogDensity              = packet.FogDensity;
            this.WeatherEvent.parameters.fogHeight               = packet.FogHeight;
            this.WeatherEvent.parameters.smokinessIntensity      = packet.SmokinessIntensity;
            this.WeatherEvent.parameters.snowIntensity           = packet.SnowIntensity;
            this.WeatherEvent.parameters.cloudCoverage           = packet.CloudCoverage;
            this.WeatherEvent.parameters.rainIntensity           = packet.RainIntensity;
            this.WeatherEvent.parameters.hailIntensity           = packet.HailIntensity;
            this.WeatherEvent.parameters.meteorIntensity         = packet.MeteorIntensity;
            this.WeatherEvent.parameters.lightningIntensity      = packet.LightningIntensity;
            this.WeatherEvent.parameters.temperature             = packet.Temperature;
            this.WeatherEvent.parameters.auroraBorealisIntensity = packet.AuroraBorealisIntensity;

            WeatherManager.main.activeScriptedWeather = null;
            WeatherManager.main.currentWeatherTrigger = null;
            WeatherManager.main.currentWeatherProfile = null;

            if (!packet.IsProfile)
            {
                WeatherManager.main.activeScriptedWeather = this.WeatherEvent;
            }

            WeatherManager.main.SetTargetWeatherEvent(this.WeatherEvent);
            return true;
        }

        /**
         *
         * Hava durumu profili değiştiğinde tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void OnWeatherProfileChanged(WeatherProfileChangedEventArgs ev)
        {
            ev.IsAllowed = false;

            ClientModel.WeatherChangedArgs request = new ClientModel.WeatherChangedArgs()
            {
                ProfileId = ev.ProfileId,
                IsProfile = ev.IsProfile,
            };

            NetworkClient.SendPacket(request);
        }
    }
}