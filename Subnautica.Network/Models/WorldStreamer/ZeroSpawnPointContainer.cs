namespace Subnautica.Network.Models.WorldStreamer
{
    using System.Collections.Generic;

    using MessagePack;

    using Subnautica.Network.Models.WorldEntity;

    [MessagePackObject]
    public class ZeroSpawnPointContainer
    {
        /**
         *
         * Toplam slot sayısını döner.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(0)]
        public int TotalSpawnPoint { get; set; }

        /**
         *
         * Aktif slotları barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(1)]
        public HashSet<int> ActiveSlots { get; set; } = new HashSet<int>();

        /**
         *
         * Tüm slotları döner.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(2)]
        public Dictionary<int, ZeroSpawnPoint> SpawnPoints { get; set; } = new Dictionary<int, ZeroSpawnPoint>();
    }
}
