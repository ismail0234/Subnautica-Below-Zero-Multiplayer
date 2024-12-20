namespace Subnautica.Events.EventArgs
{
    using System;

    public class PlayerFreezedEventArgs : EventArgs
    {
        /**
         *
         * Sınıf ayarlamalarını yapar
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public PlayerFreezedEventArgs(float endTime)
        {
            this.EndTime = endTime;
        }

        /**
         *
         * EndTime Değerini barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public float EndTime { get; set; }
    }
}
