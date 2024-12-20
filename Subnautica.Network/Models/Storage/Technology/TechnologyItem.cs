namespace Subnautica.Network.Models.Storage.Technology
{
    using System.Collections.Generic;

    using MessagePack;

    [MessagePackObject]
    public class TechnologyItem
    {
        /**
         *
         * Araştırma Anahtarı
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(0)]
        public TechType TechType { get; set; } = TechType.None;

        /**
         *
         * Toplam Parça Sayısı
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(1)]
        public int TotalFragment { get; set; } = 0;

        /**
         *
         * Açılan Parça Sayısı
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(2)]
        public int Unlocked { get; set; } = 0;

        /**
         *
         * Taranan yok edilemeyen parçalar
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(3)]
        public HashSet<string> Fragments { get; set; } = new HashSet<string>();
    }
}
