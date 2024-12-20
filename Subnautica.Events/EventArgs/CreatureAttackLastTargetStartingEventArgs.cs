namespace Subnautica.Events.EventArgs
{
    using System;

    using UnityEngine;

    public class CreatureAttackLastTargetStartingEventArgs : EventArgs
    {
        /**
         *
         * Sınıf ayarlamalarını yapar
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public CreatureAttackLastTargetStartingEventArgs(global::Creature creature, string uniqueId, GameObject target, float minAttackDuration, float maxAttackDuration, bool isAllowed = true)
        {
            this.UniqueId   = uniqueId;
            this.Creature   = creature;
            this.Target     = target;
            this.MinAttackDuration = minAttackDuration;
            this.MaxAttackDuration = maxAttackDuration;
            this.IsAllowed  = isAllowed;
        }

        /**
         *
         * Yaratık benzersiz ID değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public string UniqueId { get; set; }

        /**
         *
         * Creature değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public global::Creature Creature { get; set; }

        /**
         *
         * Target değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public GameObject Target { get; set; }

        /**
         *
         * MinAttackDuration değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public float MinAttackDuration { get; set; }

        /**
         *
         * MaxAttackDuration değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public float MaxAttackDuration { get; set; }

        /**
         *
         * Olayın çalıştırılıp/çalıştırılmayacağı
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public bool IsAllowed { get; set; }
    }
}
