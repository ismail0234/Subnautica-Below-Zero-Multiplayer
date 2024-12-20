namespace Subnautica.API.Enums
{
    public enum NetworkChannel : byte
    {   
        /**
         *
         * Varsayılan Kanal
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        Default,

        /**
         *
         * İnşaat Kanalı
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        Construction,

        /**
         *
         * Başlangıç Kanalı
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        Startup,

        /**
         *
         * Başlangıç Kanalı
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        StartupWorldLoaded,

        /**
         *
         * Enerji İletim Kanalı
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        EnergyTransmission,

        /**
         *
         * Oyuncu Animasyon İletim Kanalı
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        PlayerAnimation,

        /**
         *
         * Hareket Kanalları
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        PlayerMovement,
        VehicleMovement,
        EntityMovement,
        FishMovement,
    }
}