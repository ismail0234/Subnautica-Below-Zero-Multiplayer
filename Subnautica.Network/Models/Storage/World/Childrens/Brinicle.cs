namespace Subnautica.Network.Models.Storage.World.Childrens
{
    using MessagePack;

    using UnityEngine;

    using Subnautica.Network.Models.WorldEntity.DynamicEntityComponents.Shared;
    using Subnautica.Network.Structures;
    using Subnautica.API.Features;

    [MessagePackObject]
    public class Brinicle
    {
        /**
         *
         * UniqueId barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(0)]
        public string UniqueId { get; set; }

        /**
         *
         * LiveMixin barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(1)]
        public LiveMixin LiveMixin { get; set; }

        /**
         *
         * EularAngles barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(2)]
        public ZeroVector3 EularAngles { get; set; }

        /**
         *
         * MinFullScale barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(3)]
        public ZeroVector3 MinFullScale { get; set; }

        /**
         *
         * MaxFullScale barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(4)]
        public ZeroVector3 MaxFullScale { get; set; }

        /**
         *
         * FullScale barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(5)]
        public ZeroVector3 FullScale { get; set; }

        /**
         *
         * StartedTime barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(6)]
        public double StartedTime { get; set; }

        /**
         *
         * LifeTime barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(7)]
        public float LifeTime { get; set; }

        /**
         *
         * Brinicle oluşturur.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static Brinicle Create(string uniqueId, ZeroVector3 minFullScale, ZeroVector3 maxFullScale)
        {
            return new Brinicle()
            {
                UniqueId    = uniqueId,
                LiveMixin   = new LiveMixin(100f, 100f),
                FullScale   = ZeroVector3.Lerp(minFullScale, maxFullScale, Random.value),
                EularAngles = new ZeroVector3(0.0f, Random.Range(0.0f, 360f), 0.0f)
            };
        }

        /**
         *
         * Durumunu değiştirir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void UpdateRandomState(double currentTime)
        {
            this.StartedTime = currentTime;
            this.LifeTime    = Tools.GetRandomInt(60, 140);
        }

        /**
         *
         * Öldürme işlemi yapar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void Kill(double currentTime)
        {
            this.StartedTime = currentTime + this.LifeTime;
        }

        /**
         *
         * Scle oranını döner.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public float GetScaleAmount(double currentTime)
        {
            return Mathf.Clamp01((float) (((currentTime - this.StartedTime) % ((double) this.LifeTime * 2)) / 5));
        }

        /**
         *
         * Aktiflik durumunu döner.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public bool IsActive(double currentTime)
        {
            if (this.StartedTime > currentTime)
            {
                return false;
            }

            var differentTime = currentTime - this.StartedTime;
            if (Mathf.FloorToInt((float) (differentTime / this.LifeTime)) % 2 == 0)
            {
                return true;
            }

            return differentTime % this.LifeTime <= this.LifeTime / 2f;
        }
    }
}