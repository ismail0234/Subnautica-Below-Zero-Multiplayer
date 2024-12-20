namespace Subnautica.API.Features.Creatures.MonoBehaviours.Shared
{
    public class MultiplayerCreaturedShared : BaseMultiplayerCreature
    {
        /**
         *
         * FrozenMixin değerini barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public global::CreatureFrozenMixin FrozenMixin { get; private set; }

        /**
         *
         * Sınıf uyanırken tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void Awake()
        {
            this.FrozenMixin = this.GetComponent<global::CreatureFrozenMixin>();
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
            if (this.FrozenMixin != null && this.FrozenMixin.IsFrozen() && this.MultiplayerCreature.CreatureItem.IsMine())
            {
                this.FrozenMixin.FreezeForTime(4f);
            }
        }
    }
}
