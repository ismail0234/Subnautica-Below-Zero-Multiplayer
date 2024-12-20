namespace Subnautica.Events.EventArgs
{
    using System;
    using Subnautica.API.Extensions;
    using Subnautica.API.Features;

    using UnityEngine;

    public class TakeDamagingEventArgs : EventArgs
    {
        /**
         *
         * Sınıf ayarlamalarını yapar
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public TakeDamagingEventArgs(global::LiveMixin liveMixin, TechType techType, float damage, float oldHealth, float maxHealth, float newHealth, DamageType damageType, bool isDestroyable, GameObject dealer, bool isAllowed = true)
        {
            this.LiveMixin     = liveMixin;
            this.UniqueId      = Network.Identifier.GetIdentityId(liveMixin.gameObject, false);
            this.Dealer        = dealer;
            this.DealerId      = dealer != null ? Network.Identifier.GetIdentityId(dealer.gameObject, false) : null;
            this.TechType      = techType;
            this.Damage        = damage;
            this.OldHealth     = oldHealth;
            this.MaxHealth     = maxHealth;
            this.NewHealth     = newHealth;
            this.DamageType    = damageType;
            this.IsDead        = newHealth <= 0f;
            this.IsDestroyable = isDestroyable;
            this.IsAllowed     = isAllowed;

            if (this.UniqueId.IsNotNull())
            {
                this.IsStaticWorldEntity = Network.StaticEntity.IsStaticEntity(this.UniqueId);
            }
        }

        /**
         *
         * LiveMixin değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public global::LiveMixin LiveMixin { get; set; }

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
         * Dealer değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public GameObject Dealer { get; set; }

        /**
         *
         * DealerId değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public string DealerId { get; set; }

        /**
         *
         * TechType değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public TechType TechType { get; set; }

        /**
         *
         * Damage değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public float Damage { get; set; }

        /**
         *
         * OldHealth değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public float OldHealth { get; set; }

        /**
         *
         * MaxHealth değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public float MaxHealth { get; set; }

        /**
         *
         * NewHealth değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public float NewHealth { get; set; }

        /**
         *
         * DamageType değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public DamageType DamageType { get; set; }

        /**
         *
         * IsDestroyable değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public bool IsDestroyable { get; private set; }

        /**
         *
         * IsStaticWorldEntity değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public bool IsStaticWorldEntity { get; private set; }

        /**
         *
         * IsDead değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public bool IsDead { get; set; }

        /**
         *
         * IsAllowed değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public bool IsAllowed { get; set; }
    }
}