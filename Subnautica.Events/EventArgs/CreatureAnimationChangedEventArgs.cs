namespace Subnautica.Events.EventArgs
{
    using System;

    public class CreatureAnimationChangedEventArgs : EventArgs
    {
        /**
         *
         * Sınıf ayarlamalarını yapar
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public CreatureAnimationChangedEventArgs(ushort creatureId, byte animationId, byte result)
        {
            this.CreatureId  = creatureId;
            this.AnimationId = animationId;
            this.Result      = result;
        }

        /**
         *
         * CreatureId değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public ushort CreatureId { get; private set; }

        /**
         *
         * AnimationId değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public byte AnimationId { get; private set; }

        /**
         *
         * Result değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public byte Result { get; private set; }
    }
}