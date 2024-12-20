namespace Subnautica.Events.EventArgs
{
    using System;

    public class CreatureAttackLastTargetStoppedEventArgs : EventArgs
    {
        /**
         *
         * Sınıf ayarlamalarını yapar
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public CreatureAttackLastTargetStoppedEventArgs(global::Creature creature, string uniqueId, bool isAttackAnimationActive)
        {
            this.UniqueId = uniqueId;
            this.Creature = creature;
            this.IsAttackAnimationActive = isAttackAnimationActive;
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
         * IsAttackAnimationActive değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public bool IsAttackAnimationActive { get; set; }
    }
}
