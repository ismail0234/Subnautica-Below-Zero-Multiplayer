namespace Subnautica.Client.Multiplayer.Furnitures
{
    using System.Collections.Generic;
    using System.Linq;

    using Subnautica.API.Extensions;

    using Metadata = Subnautica.Network.Models.Metadata;

    public class Bed
    {
        /**
         *
         * Yatak verilerini barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private static HashSet<string> Beds { get; set; } = new HashSet<string>();

        /**
         *
         * Uyuyan toplam oyuncu sayısını döner.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static int GetSleepingPlayerCount()
        {
            return Beds.Count;
        }

        /**
         *
         * Yatağı günceller.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static bool UpdateBed(string playerId)
        {
            return Bed.Beds.Add(playerId);
        }

        /**
         *
         * Yatağı günceller.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static bool IsSleeping(string playerId)
        {
            return Bed.Beds.Contains(playerId);
        }

        /**
         *
         * Eski yataklardaki oyuncuyu siler.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static bool ClearBed(string playerId)
        {
            return Bed.Beds.Remove(playerId);
        }

        /**
         *
         * Tüm verileri temizler.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void Dispose()
        {
            Beds.Clear();
        }
    }
}