namespace Subnautica.Events.EventArgs
{
    using System;

    public class MenuSaveUpdateLoadedButtonStateEventArgs : EventArgs
    {
        /**
         *
         * Sınıf ayarlamalarını yapar
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public MenuSaveUpdateLoadedButtonStateEventArgs(MainMenuLoadButton button)
        {
            Button = button;
        }

        /**
         *
         * Button değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public MainMenuLoadButton Button { get; set; }
    }
}
