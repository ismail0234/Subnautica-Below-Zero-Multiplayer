namespace Subnautica.Network.Models.WorldStreamer
{
    using MessagePack;

    using Subnautica.API.Extensions;
    using Subnautica.API.Features;
    using Subnautica.API.MonoBehaviours;
    using Subnautica.Network.Structures;

    using UnityEngine;

    using static EntitySlotData;

    [MessagePackObject]
    public class ZeroSpawnPoint
    {
        /**
         *
         * Slot aktiflik durumunu barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(0)]
        public bool IsActive { get; set; }

        /**
         *
         * Slot aktiflik durumunu barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(1)]
        public int SlotId { get; set; }

        /**
         *
         * Biome türünü barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(2)]
        public BiomeType BiomeType { get; set; }

        /**
         *
         * Olasılığı barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(3)]
        public float Density { get; set; }

        /**
         *
         * Slot türünü barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(4)]
        public EntitySlotType SlotType { get; set; }

        /**
         *
         * Başlangıç konumunu barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(5)]
        public ZeroVector3 LeashPosition { get; set; }

        /**
         *
         * Başlangıç açısını barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(6)]
        public ZeroQuaternion LeashRotation { get; set; }

        /**
         *
         * Slot teknoloji sınıfını barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [IgnoreMember]
        public string ClassId { get; set; }

        /**
         *
         * Slot teknoloji türünü barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [IgnoreMember]
        public TechType TechType { get; set; }

        /**
         *
         * GameObject değerini barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [IgnoreMember]
        public GameObject GameObject { get; set; }

        /**
         *
         * NextRespawnTime değerini barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [IgnoreMember]
        public float NextRespawnTime { get; set; }

        /**
         *
         * Health değerini barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [IgnoreMember]
        public float Health { get; set; } = -1f;

        /**
         *
         * Sınıfı klonlar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public ZeroSpawnPoint Clone()
        {
            return new ZeroSpawnPoint()
            {
                IsActive        = this.IsActive, 
                SlotId          = this.SlotId, 
                BiomeType       = this.BiomeType, 
                Density         = this.Density, 
                SlotType        = this.SlotType,
                LeashPosition   = this.LeashPosition,
                LeashRotation   = this.LeashRotation,
            };
        }

        /**
         *
         * Olasılığı döner.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void SetActive(bool isActive, TechType techType, string classId)
        {
            this.IsActive = isActive;
            this.TechType = techType;
            this.ClassId  = classId;
        }

        /**
         *
         * Olasılığı döner.
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
         * Olasılığı döner.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public float GetDensity()
        {
            return this.Density;
        }

        /**
         *
         * Tür dumunu kontrol eder.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public bool IsTypeAllowed(EntitySlot.Type slotType)
        {
            return EntitySlotData.IsTypeAllowed(this.SlotType, slotType);
        }

        /**
         *
         * Komponenti döner.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public SpawnPointComponent GetComponent()
        {
            return Network.Identifier.GetComponentByGameObject<SpawnPointComponent>(this.SlotId.ToWorldStreamerId(), true);
        }

        /**
         *
         * Nesne doğması aktif mi?
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public bool IsRespawnActive()
        {
            return this.IsActive && this.NextRespawnTime != -1;
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
            return this.IsRespawnActive() && (this.NextRespawnTime == 0 || currentTime >= this.NextRespawnTime);
        }
    }
}
