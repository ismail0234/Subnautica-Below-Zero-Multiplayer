namespace Subnautica.Events.EventArgs
{
    using System;
    using Subnautica.API.Enums.Creatures;

    public class GlowWhaleSFXTriggeredEventArgs : EventArgs
    {
        /**
         *
         * Sınıf ayarlamalarını yapar
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public GlowWhaleSFXTriggeredEventArgs(string uniqueId, GlowWhaleSFXType sfxType)
        {
            this.UniqueId = uniqueId;
            this.SFXType  = sfxType;
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
         * sfxType değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public GlowWhaleSFXType SFXType { get; set; }
    }
}
