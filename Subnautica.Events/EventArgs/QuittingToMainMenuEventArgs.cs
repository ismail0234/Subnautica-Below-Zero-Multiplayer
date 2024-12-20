namespace Subnautica.Events.EventArgs
{
    using System;

    public class QuittingToMainMenuEventArgs : EventArgs
    {
        /**
         *
         * Sınıf ayarlamalarını yapar
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public QuittingToMainMenuEventArgs(bool isQuitToDesktop, bool isAllowed = true)
        {
            this.IsQuitToDesktop = isQuitToDesktop;
            this.IsAllowed       = isAllowed;
        }

        /**
         *
         * Masaüstüne çıkış yapılsın mı?
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public bool IsQuitToDesktop { get; private set; }

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
