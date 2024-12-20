namespace Subnautica.API.Features
{
    using System.Diagnostics;

    public class StopwatchItem : Stopwatch
    {
        /**
         *
         * Gecikme zamanı
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public float DelayTime { get; set; }

        /**
         *
         * Veri barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public object CustomData { get; set; }

        /**
         *
         * Sınıf ayarlarını yapar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public StopwatchItem(float delayTime = -1f, object customData = null, bool autoStart = true)
        {
            this.DelayTime  = delayTime;
            this.CustomData = customData;

            if (autoStart)
            {
                this.Start();
            }
        }

        /**
         *
         * Belirtilen süre doldu mu?
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public bool IsFinished()
        {
            return this.ElapsedMilliseconds >= this.DelayTime;
        }

        /**
         *
         * Geçen zamanı döner.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public float ElapsedTime()
        {
            return this.ElapsedMilliseconds;
        }

        /**
         *
         * Belirtilen süre doldu mu?
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public T GetCustomData<T>()
        {
            if (this.CustomData == null)
            {
                return default;
            }
            
            return (T) this.CustomData;
        }
    }
}
