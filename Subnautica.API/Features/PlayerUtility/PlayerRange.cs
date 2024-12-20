namespace Subnautica.API.Features.PlayerUtility
{
    using System.Collections.Generic;
    using System.Linq;

    public class PlayerRange
    {
        /**
         *
         * En Yakın Oyuncu
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public ZeroPlayer NearestPlayer { get; private set; }

        /**
         *
         * En Yakın Oyuncu Mesafesi
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public float NearestPlayerDistance { get; private set; } = 99999f;

        /**
         *
         * En Uzak Oyuncu
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public ZeroPlayer FarthestPlayer { get; private set; }

        /**
         *
         * En Uzak Oyuncu Mesafesi
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public float FarthestPlayerDistance { get; private set; } = -99999f;

        /**
         *
         * Rastgele Oyuncu 
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public ZeroPlayer RandomPlayer
        {
            get
            {
                if (this.randomPlayer == null)
                {
                    this.randomPlayer = ZeroPlayer.GetPlayerById(this.Players.ElementAt(Tools.GetRandomInt(0, this.Players.Count - 1)));
                }

                return this.randomPlayer;
            }
        }

        /**
         *
         * RandomPlayer değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private ZeroPlayer randomPlayer;

        /**
         *
         * Tüm oyuncular 
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private List<byte> Players { get; set; } = new List<byte>();

        /**
         *
         * En yakındaki oyuncu günceller.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void SetNearestPlayer(ZeroPlayer player, float distance)
        {
            this.NearestPlayer = player;
            this.NearestPlayerDistance = distance;
        }

        /**
         *
         * En uzaktaki oyuncu günceller.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void SetFarthestPlayer(ZeroPlayer player, float distance)
        {
            this.FarthestPlayer = player;
            this.FarthestPlayerDistance = distance;
        }

        /**
         *
         * Oyuncu ekler.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void AddPlayer(ZeroPlayer player, float distance)
        {
            this.Players.Add(player.PlayerId);
        }

        /**
         *
         * Oyuncu var mı?
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public bool IsExistsPlayer()
        {
            return this.Players.Count > 0;
        }
    }
}
