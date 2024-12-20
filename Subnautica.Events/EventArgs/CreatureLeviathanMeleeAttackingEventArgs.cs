namespace Subnautica.Events.EventArgs
{
    using System;

    using Subnautica.API.Extensions;

    using UnityEngine;

    public class CreatureLeviathanMeleeAttackingEventArgs : EventArgs
    {
        /**
         *
         * Sınıf ayarlamalarını yapar
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public CreatureLeviathanMeleeAttackingEventArgs(global::LeviathanMeleeAttack instance, GameObject target, bool isAllowed = true)
        {
            this.Instance   = instance;
            this.UniqueId   = instance.creature.gameObject.GetIdentityId();
            this.BiteDamage = instance.biteDamage;
            this.Target     = target;
            this.TargetId   = target.GetIdentityId();
            this.TargetType = target.GetTechType();
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
         * Instance değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public global::LeviathanMeleeAttack Instance { get; set; }

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
         * TargetId değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public string TargetId { get; set; }

        /**
         *
         * TargetType değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public TechType TargetType { get; set; }

        /**
         *
         * BiteDamage değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public float BiteDamage { get; set; }

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
