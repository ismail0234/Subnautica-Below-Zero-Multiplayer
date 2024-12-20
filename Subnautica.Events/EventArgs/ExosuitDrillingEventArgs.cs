namespace Subnautica.Events.EventArgs
{
    using System;
    using System.Collections.Generic;

    using Subnautica.API.Features;
    using Subnautica.Network.Structures;

    public class ExosuitDrillingEventArgs : EventArgs
    {
        /**
         *
         * Sınıf ayarlamalarını yapar
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public ExosuitDrillingEventArgs(string uniqueId, string slotId, float maxHealth, TechType dropTechType, List<ZeroVector3> dropPositions, bool isMultipleDrill, bool isAllowed = true)
        {
            this.UniqueId            = uniqueId;
            this.SlotId              = slotId;
            this.MaxHealth           = maxHealth;
            this.DropTechType        = dropTechType;
            this.DropPositions       = dropPositions;
            this.IsMultipleDrill     = isMultipleDrill;
            this.IsAllowed           = isAllowed;
            this.IsStaticWorldEntity = Network.StaticEntity.IsStaticEntity(slotId);
        }

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
         * SlotId değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public string SlotId { get; set; }

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
         * DropTechType değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public TechType DropTechType { get; set; }

        /**
         *
         * DropPositions değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public List<ZeroVector3> DropPositions { get; set; }

        /**
         *
         * IsMultipleDrill değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public bool IsMultipleDrill { get; set; }

        /**
         *
         * Olayın çalıştırılıp/çalıştırılmayacağı
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public bool IsAllowed { get; set; }

        /**
         *
         * Static nesne mi?
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public bool IsStaticWorldEntity { get; set; }
    }
}
