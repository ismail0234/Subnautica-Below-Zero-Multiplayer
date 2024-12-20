namespace Subnautica.API.Features.Creatures.Trackers
{
    using System.Collections.Generic;

    using UnityEngine;
    
    public class ArcticRayAnimationTracker : BaseAnimationTracker
    {
        /**
         *
         * Veri gönderimi sağlanması için özel sonuçlar.
         * Kullanılmadığı zaman tüm sonuçlar için geçerlidir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public override List<byte> AllowedCustomResults { get; set; } = new List<byte>()
        {
            1
        };

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

            if (creature.TryGetComponent<CreaturePlayAnimation>(out var animation))
            {
                var isActive = animation.actionStartTime + 0.15f >= Time.time ? 1 : 0;
                if (isActive != oldValue)
                {
                    if (isActive == 1)
                    {
                        result = 1;
                    }

                    return true;
                }
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
            creature.GetAnimator().SetTrigger("twist");
        }
    }
}