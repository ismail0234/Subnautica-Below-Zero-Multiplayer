namespace Subnautica.Events.EventArgs
{
    using System;

    public class SettingsRunInBackgroundChangingEventArgs : EventArgs
    {
        /**
         *
         * Sınıf ayarlamalarını yapar
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public SettingsRunInBackgroundChangingEventArgs(bool isAllowed = true)
        {
            this.IsAllowed = isAllowed;
        }

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