namespace Subnautica.Client.MonoBehaviours.Vehicle
{
    using Subnautica.API.Features;
    using Subnautica.Client.MonoBehaviours.Player;

    using UnityEngine;

    public class MultiplayerSeaTruckSleeperModule : MonoBehaviour
    {
        /**
         *
         * Yatağı barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private global::Bed Bed;

        /**
         *
         * Nesne başlatılırken tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void Start()
        {
            this.Bed = this.GetComponentInChildren<global::Bed>(true);
        }

        /**
         *
         * Oyuncu çıktığında tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void OnMultiplayerPlayerDisconnected(ZeroPlayer player)
        {
            if (this.IsSamePlayer(this.GetPlayer(), player))
            {
                this.Bed.animator.Rebind();
            }
        }

        /**
         *
         * Nesne yok edilirken tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void OnDestroy()
        {
            var player = this.GetPlayer();
            if (player != null)
            {
                Multiplayer.Furnitures.Bed.ClearBed(player.UniqueId);

                player.SetParent(null);
                player.Animator.Rebind();
            }
        }

        /**
         *
         * Aynı oyuncu mu?
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private bool IsSamePlayer(ZeroPlayer player1, ZeroPlayer player2)
        {
            return player1 != null && player2 != null && player1.UniqueId == player2.UniqueId;
        }

        /**
         *
         * Yataktaki oyuncuyu döner.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private ZeroPlayer GetPlayer()
        {
            return this.Bed.GetComponentInChildren<PlayerAnimation>()?.Player;
        }
    }
}
