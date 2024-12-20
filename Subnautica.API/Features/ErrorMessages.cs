namespace Subnautica.API.Features
{
    using Subnautica.API.Enums;
    using Subnautica.API.Extensions;

    using System.Text;

    public class ErrorMessages
    {
        /**
         *
         * Bağlantı kurulduğunda tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void ShowConnectionSuccess()
        {
            ZeroGame.ClearScreenErrorMessage();
        }

        /**
         *
         * Bağlantı reddedildiğinde tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void ShowConnectionRejected()
        {
            ShowNormalErrorMessage(ZeroLanguage.Get("GAME_CONNECTION_REJECTED"));
        }

        /**
         *
         * Sunucu dolu olduğunda tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void ShowConnectionServerFull()
        {
            ShowNormalErrorMessage(ZeroLanguage.Get("GAME_CONNECTION_SERVER_FULL"));
        }

        /**
         *
         * Sürüm uyuşmazlığı olduğunda tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void ShowConnectionVersionMismatch()
        {
            ShowNormalErrorMessage(ZeroLanguage.Get("GAME_CONNECTION_SERVER_VERSION_MISMATCH"));
        }

        /**
         *
         * Anahtar ile hata mesajı gösterir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void ShowErrorMessage(string key)
        {
            ShowNormalErrorMessage(ZeroLanguage.Get(key));
        }

        /**
         *
         * Bağlantı koptuğunda tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void ShowConnectionError()
        {
            // Şuanlık hiçbir şey yapma. Genel Hata...
        }

        /**
         *
         * Bağlantı sorun düzeltme mesajını gösterir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void ShowConnectionFixErrorMessage()
        {
            var errorMessage = new StringBuilder();

            foreach (var item in ZeroLanguage.Get("GAME_CONNECTION_FIX_ERROR").Split('\n'))
            {
                if (item.IsNull())
                {
                    errorMessage.Append("<br>");
                    continue;
                }

                errorMessage.Append("<align=left><size=14px>");
                errorMessage.Append(item.ToString());
                errorMessage.Append("</size></align>");
                errorMessage.Append("<br>");
            }

            errorMessage.Remove(errorMessage.Length - 4, 4);

            ZeroModal.Show(ZeroLanguage.Get("GAME_CONNECTION_ERROR_POPUP_TITLE"), errorMessage.ToString(), ZeroModalSize.Size_600x300);
        }

        /**
         *
         * Normal Hata mesajını gösterir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private static void ShowNormalErrorMessage(string content)
        {
            var errorMessage = string.Format("<br><align=center><size=16px>{0}</size></align><br><br><br><br>", content);

            ZeroModal.Show(ZeroLanguage.Get("GAME_CONNECTION_ERROR_POPUP_TITLE"), errorMessage, ZeroModalSize.Size_500x200);
        }
    }
}
