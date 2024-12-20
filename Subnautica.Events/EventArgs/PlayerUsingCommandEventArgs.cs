namespace Subnautica.Events.EventArgs
{
    using System;

    public class PlayerUsingCommandEventArgs : EventArgs
    {
        /**
         *
         * Sınıf ayarlamalarını yapar
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public PlayerUsingCommandEventArgs(string command, string fullCommand, bool isAllowed = true)
        {
            this.Command     = command.Trim();
            this.FullCommand = fullCommand.Trim();
            this.IsAllowed   = isAllowed;
        }

        /**
         *
         * Command Değerini barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public string Command { get; set; }

        /**
         *
         * FullCommand Değerini barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public string FullCommand { get; set; }

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
