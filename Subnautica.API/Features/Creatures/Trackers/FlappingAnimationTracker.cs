namespace Subnautica.API.Features.Creatures.Trackers
{
    public class FlappingAnimationTracker : BaseAnimationTracker
    {
        /**
         *
         * Animasyon anahtarı
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private string Animation { get; set; } = "flapping";

        /**
         *
         * Animasyon izleyici kontrol yapılırken tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public override bool OnTrackerChecking(Creature creature, byte oldValue, out byte result)
        {
            result = 0;

            var flapping = creature.GetAnimator().GetBool(this.Animation) ? 1 : 0;
            if (flapping != oldValue)
            {
                if (flapping == 1)
                {
                    result = 1;
                }
                else 
                {
                    result = 0;
                }

                return true;
            }

            return false;
        }

        /**
         *
         * Animasyon izleyici işleme yapılırken tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public override void OnTrackerExecuting(Creature creature, byte result)
        {
            creature.GetAnimator().SetBool(this.Animation, result == 1);
        }
    }
}
