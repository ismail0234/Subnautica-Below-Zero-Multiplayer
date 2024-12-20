namespace Subnautica.API.Features.Creatures.MonoBehaviours.Shared
{
    public class MultiplayerCreatureAggressionManager : BaseMultiplayerCreature
    {
        /**
         *
         * CreatureAggressionManager değerini barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public global::CreatureAggressionManager CreatureAggressionManager { get; private set; }

        /**
         *
         * Sınıf uyanırken tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void Awake()
        {
            this.CreatureAggressionManager = this.GetComponent<global::CreatureAggressionManager>();
        }

        /**
         *
         * Aktif olduğunda tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void OnEnable()
        {
            this.OnChangedOwnership();
        }

        /**
         *
         * Sahiplik değiştiğinde tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void OnChangedOwnership()
        {
            this.CancelInvokes();

            if (this.MultiplayerCreature.CreatureItem.IsMine())
            {
                this.CreatureAggressionManager.EnableAggressionToFish();
                this.CreatureAggressionManager.EnableAggressionToSharks();
            }
        }

        /**
         *
         * İşlemler iptal ederç
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private void CancelInvokes()
        {
            if (this.CreatureAggressionManager.aggressionToSharksPaused)
            {
                this.CreatureAggressionManager.CancelInvoke("EnableAggressionToSharks");
            }

            if (this.CreatureAggressionManager.aggressionToFishPaused)
            {
                this.CreatureAggressionManager.CancelInvoke("EnableAggressionToFish");
            }
        }
    }
}
