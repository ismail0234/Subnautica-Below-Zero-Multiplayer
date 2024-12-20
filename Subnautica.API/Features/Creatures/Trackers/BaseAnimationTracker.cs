namespace Subnautica.API.Features.Creatures.Trackers
{
    using System.Collections.Generic;

    public abstract class BaseAnimationTracker
    {
        /**
         *
         * Veri gönderimi sağlanması için özel sonuçlar.
         * Kullanılmadığı zaman tüm sonuçlar için geçerlidir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public virtual List<byte> AllowedCustomResults { get; set; } = new List<byte>();

        /**
         *
         * Animasyon izleyici kontrol yapılırken tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public abstract bool OnTrackerChecking(global::Creature creature, byte oldValue, out byte result);

        /**
         *
         * Animasyon izleyici işleme yapılırken tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public abstract void OnTrackerExecuting(global::Creature creature, byte result);
    }
}
