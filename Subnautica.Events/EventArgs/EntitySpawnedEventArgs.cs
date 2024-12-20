namespace Subnautica.Events.EventArgs
{
    using System;

    using Subnautica.API.Enums;
    using Subnautica.API.Extensions;
    using Subnautica.API.Features;

    using UnityEngine;

    public class EntitySpawnedEventArgs : EventArgs
    {
        /**
         *
         * Sınıf ayarlamalarını yapar
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public EntitySpawnedEventArgs(string uniqueId, GameObject gameObject, string classId, TechType techType, EntitySpawnLevel level, bool isPersistent)
        {
            this.UniqueId   = uniqueId;
            this.GameObject = gameObject;
            this.ClassId    = classId;
            this.TechType   = techType;
            this.Level      = level;

            if (uniqueId.IsWorldStreamer())
            {
                this.SlotType = SlotType.WorldStreamer;
            }
            else if (isPersistent && !techType.IsCreature())
            {
                this.SlotType = SlotType.Static;
                
                Network.StaticEntity.AddStaticEntitySlot(this.UniqueId);
            }
        }

        /**
         *
         * UniqueId Değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public string UniqueId { get; set; }

        /**
         *
         * GameObject Değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public GameObject GameObject { get; private set; }

        /**
         *
         * ClassId Değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public string ClassId { get; private set; }

        /**
         *
         * Level Değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public EntitySpawnLevel Level { get; private set; }

        /**
         *
         * TechType Değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public TechType TechType { get; private set; }

        /**
         *
         * SlotType Değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public SlotType SlotType { get; set; }
    }
}
