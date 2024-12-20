namespace Subnautica.API.Features.Creatures.MonoBehaviours
{ 
    public class GlowWhaleMonoBehaviour : BaseMultiplayerCreature
    {
        /**
         *
         * Balina sınıfını barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private global::GlowWhale GlowWhale { get; set; }

        /**
         *
         * Sınıf uyanırken tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void Awake()
        {
            this.GlowWhale = this.GetComponent<global::GlowWhale>();
        }

        /**
         *
         * Her karede tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void Update()
        {
            if (this.MultiplayerCreature.CreatureItem.IsNotMine())
            {
                this.GlowWhale.Update();
            }
        }
    }
}
