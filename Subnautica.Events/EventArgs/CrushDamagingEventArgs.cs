namespace Subnautica.Events.EventArgs
{
    using System;

    public class CrushDamagingEventArgs : EventArgs
    {
        /**
         *
         * Sınıf ayarlamalarını yapar
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public CrushDamagingEventArgs(string uniqueId, TechType techType, float maxHealth, float damage, bool isAllowed = true)
        {
            this.UniqueId  = uniqueId;
            this.TechType  = techType;
            this.Damage    = damage;
            this.MaxHealth = maxHealth;
            this.IsAllowed = isAllowed;
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
         * TechType değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public TechType TechType { get; set; }

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
         * Damage değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public float Damage { get; set; }

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