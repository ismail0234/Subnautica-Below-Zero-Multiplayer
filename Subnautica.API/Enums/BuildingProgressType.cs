namespace Subnautica.API.Enums
{
    public enum BuildingProgressType : byte
    {
        /**
         *
         * Varsayılan
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        None,

        /**
         *
         * Oluşturuluyor
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        Initializing,

        /**
         *
         * Hayalet Model Hareket Ediyor
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        GhostModelMoving,

        /**
         *
         * İnşaa ediliyor
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        Constructing,

        /**
         *
         * Tamamlandı
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        Completed,

        /**
         *
         * Kaldırıldı
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        Removed,
    }
}
