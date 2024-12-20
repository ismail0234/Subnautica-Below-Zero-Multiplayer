namespace Subnautica.Events.EventArgs
{
    using System;

    using Subnautica.API.Extensions;

    public class CreatureDisabledEventArgs : EventArgs
    {
        /**
         *
         * Sınıf ayarlamalarını yapar
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public CreatureDisabledEventArgs(global::Creature creature)
        {
            this.Instance = creature;
            this.UniqueId = creature.gameObject.GetIdentityId();
            this.TechType = creature.gameObject.GetTechType();
        }

        /**
         *
         * Instance değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public global::Creature Instance { get; set; }

        /**
         *
         * UniqueId değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public string UniqueId { get; set; }

        /**
         *
         * TechType değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public TechType TechType { get; set; }
    }
}