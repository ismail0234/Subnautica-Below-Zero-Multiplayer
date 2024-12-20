namespace Subnautica.API.Enums
{
    public enum ConnectionSignal : byte
    {
        /**
         *
         * Bilinmiyor
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        Unknown,

        /**
         *
         * Oyuncu bağlandı
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        Connected,

        /**
         *
         * Oyuncu bağlantı kesildi
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        Disconnected,

        /**
         *
         * Oyuncu bağlantısı reddedildi
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        Rejected,

        /**
         *
         * Sunucu versiyon uyuşmazlığı
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        VersionMismatch,

        /**
         *
         * Sunucu dolu
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        ServerFull,
    }
}
