namespace Subnautica.Events.EventArgs
{
    using System;

    using Subnautica.API.Features;

    public class PlayerItemPickedUpEventArgs : EventArgs
    {
        /**
         *
         * Sınıf ayarlamalarını yapar
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public PlayerItemPickedUpEventArgs(string uniqueId, TechType techType, Pickupable pickupable, bool isAllowed = true)
        {
            this.UniqueId   = uniqueId;
            this.TechType   = techType;
            this.Pickupable = pickupable;
            this.IsAllowed  = isAllowed;
            this.IsStaticWorldEntity = Network.StaticEntity.IsStaticEntity(uniqueId);
        }

        /**
         *
         * Yapı Kimliği değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public string UniqueId { get; private set; }

        /**
         *
         * TechType değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public TechType TechType { get; private set; }

        /**
         *
         * Pickupable değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public Pickupable Pickupable { get; private set; }

        /**
         *
         * IsAllowed değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public bool IsAllowed { get; set; }

        /**
         *
         * IsStaticWorldEntity değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public bool IsStaticWorldEntity { get; private set; }
    }
}
