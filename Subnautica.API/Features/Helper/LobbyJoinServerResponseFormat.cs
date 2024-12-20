namespace Subnautica.API.Features.Helper
{
    public class LobbyJoinServerResponseFormat
    {
        /**
         *
         * Hata durumu
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public bool IsError { get; set; }

        /**
         *
         * Hata kodu
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public string ErrorMessage { get; set; }

        /**
         *
         * ServerIp
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public string ServerIp { get; set; }

        /**
         *
         * Port değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public int ServerPort { get; set; }
    }
}
