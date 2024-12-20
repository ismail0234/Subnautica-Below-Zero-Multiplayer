namespace Subnautica.Events.EventArgs
{
    using System;

    public class PlayerDeadEventArgs : EventArgs
    {
        /**
         *
         * Sınıf ayarlamalarını yapar
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public PlayerDeadEventArgs(DamageType damageType)
        {
            this.DamageType = damageType;
        }

        /**
         *
         * DamageType değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public DamageType DamageType { get; set; }
    }
}