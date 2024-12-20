namespace Subnautica.Network.Models.WorldStreamer
{
    using MessagePack;

    using Subnautica.API.Extensions;
    using Subnautica.API.Features;
    using Subnautica.Network.Structures;

    using UnityEngine;

    [MessagePackObject]
    public class ZeroSpawnPointSimple
    {
        /**
         *
         * Slot Id numarasını barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(0)]
        public int SlotId { get; set; }

        /**
         *
         * Slot teknoloji sınıfını barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(1)]
        public string ClassId { get; set; }

        /**
         *
         * NextRespawnTime değerini barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(2)]
        public float NextRespawnTime { get; set; } = 0;

        /**
         *
         * Health değerini barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(3)]
        public float Health { get; set; } = -1;

        /**
         *
         * TechType değerini barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [IgnoreMember]
        public TechType TechType { get; set; }

        /**
         *
         * Aktiflik durumu?
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [IgnoreMember]
        public bool IsActive { get; set; }

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
         * Sınıf ayarlamalarını yapar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public ZeroSpawnPointSimple()
        {

        }

        /**
         *
         * Sınıf ayarlamalarını yapar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public ZeroSpawnPointSimple(int slotId, string classId, float nextRespawnTime)
        {
            this.SlotId          = slotId;
            this.ClassId         = classId;
            this.NextRespawnTime = nextRespawnTime;
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
         * Nesneye hasar verir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public bool TakeDamage(float damage, float maxHealth)
        {
            if (this.Health == -1)
            {
                this.Health = maxHealth;
            }

            if (this.Health == 0)
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
            if (this.Health == 0)
            {
                return false;
            }

            this.Health = 0;
            return true;
        }

        /**
         *
         * Nesneni spawn olup olamayacağına bakar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public bool IsRespawnable(float currentTime)
        {
            return this.NextRespawnTime != -1 && (this.NextRespawnTime == 0 || currentTime >= this.NextRespawnTime);
        }

        /**
         *
         * Sonraki yumurtlama zamanını döner
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public float GetNextRespawnTime(float currentTime)
        {
            var duration = this.TechType.GetRespawnDuration();
            if (duration == -1f)
            {
                return -1f;
            }

            return currentTime + duration;
        }

        /**
         *
         * Yeniden doğmayı kapatır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void DisableRespawn()
        {
            this.NextRespawnTime = -1;
        }

        /**
         *
         * ZeroSpawnPoint Sınıfına dönüştürür.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public ZeroSpawnPoint ConvertToZeroSpawnPoint()
        {
            return new ZeroSpawnPoint()
            {
                IsActive        = true,
                SlotId          = this.SlotId,
                ClassId         = this.ClassId,
                NextRespawnTime = this.NextRespawnTime,
            };
        }
    }
}