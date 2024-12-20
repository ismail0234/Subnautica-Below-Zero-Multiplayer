namespace Subnautica.Events.EventArgs
{
    using System;

    public class SettingsPdaGamePauseChangingEventArgs : EventArgs
    {
        /**
         *
         * Sınıf ayarlamalarını yapar
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public SettingsPdaGamePauseChangingEventArgs(bool isAllowed = true)
        {
            IsAllowed = isAllowed;
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