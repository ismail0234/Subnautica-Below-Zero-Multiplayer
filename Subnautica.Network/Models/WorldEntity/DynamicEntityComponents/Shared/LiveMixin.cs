namespace Subnautica.Network.Models.WorldEntity.DynamicEntityComponents.Shared
{
    using MessagePack;

    using UnityEngine;

    [MessagePackObject]
    public class LiveMixin
    {
        /**
         *
         * UniqueId Değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(0)]
        public float Health { get; set; }

        /**
         *
         * UniqueId Değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(1)]
        public float MaxHealth { get; set; }

        /**
         *
         * Öldü mü?
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [IgnoreMember]
        public bool IsDead
        {
            get
            {
                return this.Health == 0;
            }
        }

        /**
         *
         * Sağlık Full mü?
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [IgnoreMember]
        public bool IsHealthFull
        {
            get
            {
                return this.Health == this.MaxHealth;
            }
        }

        /**
         *
         * Tek saldırıda max hasar yüzdesini barındırır
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [IgnoreMember]
        public float MaxDamagePercent { get; set; } = 0.16f;

        /**
         *
         * Sınıf ayarlamalarını yapar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public LiveMixin()
        {

        }

        /**
         *
         * Sınıf ayarlamalarını yapar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public LiveMixin(float health, float maxHealth)
        {
            this.Health    = health;
            this.MaxHealth = maxHealth;
        }

        /**
         *
         * Sağlığa ekleme yapar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public bool AddHealth(float health)
        {
            if (this.IsHealthFull)
            {
                return false;
            }

            this.Health = Mathf.Min(this.MaxHealth, this.Health + health);
            return true;
        }

        /**
         *
         * Sağlığı max yapar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void ResetHealth()
        {
            this.Health = this.MaxHealth;
        }

        /**
         *
         * Yeni sağlığı ayarlar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void SetHealth(float health)
        {
            this.Health = health;
        }

        /**
         *
         * Hasarı hesaplar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public float CalculateDamage(float damage, DamageType damageType)
        {
            if (damageType == DamageType.Starve)
            {
                return damage;
            }

            float maxDamage = this.MaxHealth * this.MaxDamagePercent;

            if (damage > maxDamage)
            {
                damage = maxDamage;
            }

            return damage;
        }

        /**
         *
         * Nesneye hasar verir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public bool TakeDamage(float damage)
        {
            if (this.IsDead || damage <= 0.0f)
            {
                return false;
            }

            this.Health = Mathf.Max(0, this.Health - damage);
            return true;
        }

        /**
         *
         * Nesneyi öldürür.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public bool Kill()
        {
            if (this.IsDead)
            {
                return false;
            }

            this.Health = 0;
            return true;
        }
    }
}
