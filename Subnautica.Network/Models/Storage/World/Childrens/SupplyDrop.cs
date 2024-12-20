namespace Subnautica.Network.Models.Storage.World.Childrens
{
    using MessagePack;

    using Subnautica.API.Extensions;
    using Subnautica.Network.Structures;

    using UnityEngine;

    [MessagePackObject]
    public class SupplyDrop
    {
        /**
         *
         * UniqueId Değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(0)]
        public string UniqueId { get; set; } = null;

        /**
         *
         * FabricatorUniqueId Değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(1)]
        public string FabricatorUniqueId { get; set; } = null;

        /**
         *
         * StorageUniqueId Değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(2)]
        public string StorageUniqueId { get; set; } = null;

        /**
         *
         * Key Değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(3)]
        public string Key { get; set; } = null;

        /**
         *
         * StartedTime Değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(4)]
        public float StartedTime { get; set; } = 0f;

        /**
         *
         * ZoneId Değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(5)]
        public sbyte ZoneId { get; set; } = -1;

        /**
         *
         * ZoneId Değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(6)]
        public ZeroQuaternion Rotation { get; set; }

        /**
         *
         * StorageContainer Değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(7)]
        public Metadata.StorageContainer StorageContainer { get; set; }

        /**
         *
         * Sınf ayarlamalarını yapar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void SetConfiguration(float startedTime)
        {
            this.StartedTime = startedTime;
            this.ZoneId      = (sbyte)Random.Range(0, 3);
        }

        /**
         *
         * Sınf ayarlamalarını yapar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void SetKey(string key)
        {
            this.Key = key;
        }

        /**
         *
         * Sınf ayarlamalarını yapar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void Initialize()
        {
            this.FabricatorUniqueId = API.Features.Network.Identifier.GenerateUniqueId();
            this.StorageUniqueId    = API.Features.Network.Identifier.GenerateUniqueId();
            this.UniqueId           = API.Features.Network.Identifier.GenerateUniqueId();
            this.Rotation           = Quaternion.Euler(0.0f, Random.Range(0, 360), 0.0f).ToZeroQuaternion();
        }

        /**
         *
         * Tamamlanma durumu
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public bool IsCompleted(float currentTime)
        {
            return this.StartedTime != 0 && currentTime > this.StartedTime + 32f;
        }
    }
}
