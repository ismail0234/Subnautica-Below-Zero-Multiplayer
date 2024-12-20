namespace Subnautica.Network.Models.Server
{
    using MessagePack;

    using Subnautica.API.Enums;
    using Subnautica.Network.Models.Core;

    [MessagePackObject]
    public class WeatherChangedArgs : NetworkPacket
    {
        /**
         *
         * Ağ Paket Türü
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(0)]
        public override ProcessType Type { get; set; } = ProcessType.WeatherChanged;

        /**
         *
         * DangerLevel değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(5)]
        public WeatherDangerLevel DangerLevel { get; set; }

        /**
         *
         * StartTime değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(6)]
        public float StartTime { get; set; }

        /**
         *
         * Duration değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(7)]
        public float Duration { get; set; }

        /**
         *
         * WindDir değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(8)]
        public float WindDir { get; set; }

        /**
         *
         * WindSpeed değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(9)]
        public float WindSpeed { get; set; }

        /**
         *
         * FogDensity değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(10)]
        public float FogDensity { get; set; }

        /**
         *
         * FogHeight değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(11)]
        public float FogHeight { get; set; }

        /**
         *
         * SmokinessIntensity değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(12)]
        public float SmokinessIntensity { get; set; }

        /**
         *
         * SnowIntensity değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(13)]
        public float SnowIntensity { get; set; }

        /**
         *
         * CloudCoverage değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(14)]
        public float CloudCoverage { get; set; }

        /**
         *
         * RainIntensity değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(15)]
        public float RainIntensity { get; set; }

        /**
         *
         * HailIntensity değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(16)]
        public float HailIntensity { get; set; }

        /**
         *
         * MeteorIntensity değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(17)]
        public float MeteorIntensity { get; set; }

        /**
         *
         * LightningIntensity değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(18)]
        public float LightningIntensity { get; set; }

        /**
         *
         * Temperature değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(19)]
        public float Temperature { get; set; }

        /**
         *
         * AuroraBorealisIntensity değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(20)]
        public float AuroraBorealisIntensity { get; set; }

        /**
         *
         * IsProfile değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(21)]
        public bool IsProfile { get; set; }
    }
}