namespace Subnautica.API.Features
{
    using System.Collections.Generic;
    using System.Linq;

    using Subnautica.API.Extensions;

    public class Interact
    {
        /**
         *
         * ServerHost Anahtarı
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public const string ServerHost = "[localhost]";

        /**
         *
         * Bloklu listeyi barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static Dictionary<string, string> List { get; private set; } = new Dictionary<string, string>();

        /**
         *
         * Bloklu listeyi temizler.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void ClearAll()
        {
            List.Clear();
        }

        /**
         *
         * Bloklu listeyi günceller.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void SetList(Dictionary<string, string> list)
        {
            List = list;
        }

        /**
         *
         * Özel interact id döner.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */      
        public static string GetCustomId(string customId)
        {
            return string.Format("{0}_{1}", ServerHost, customId);
        }

        /**
         *
         * Bloklu olup olmadığını döner.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static bool IsBlocked(string constructionId, bool isMineIgnore = false)
        {
            if (string.IsNullOrEmpty(constructionId))
            {
                return false;
            }

            var interact = List.Where(q => q.Value == constructionId).FirstOrDefault();
            if (interact.Value == null)
            {
                return false;
            }

            if (isMineIgnore)
            {
                return interact.Key != ZeroPlayer.CurrentPlayer.UniqueId;
            }

            return true;
        }

        /**
         *
         * Bloklu olup olmadığını döner.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static bool IsBlocked(string constructionId, string playerId, bool ignoreServer = false)
        {
            var interact = List.Where(q => q.Value == constructionId).FirstOrDefault();
            if (interact.Value == null)
            {
                return false;
            }

            if (playerId.IsNotNull())
            {
                if (ignoreServer && interact.Key.Contains(playerId))
                {
                    return false;
                }

                return interact.Key != playerId;
            }

            return true;
        }

        /**
         *
         * Benim tarafımdan engellenen bir yapı olup olmadığını döner.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static bool IsBlockedByMe(string constructionId = null)
        {
            if (string.IsNullOrEmpty(constructionId))
            {
                return List.ContainsKey(ZeroPlayer.CurrentPlayer.UniqueId);
            }

            return List.TryGetValue(ZeroPlayer.CurrentPlayer.UniqueId, out string _constructionId) && _constructionId == constructionId;
        }

        /**
         *
         * Oyuncuya ekran ortasında kullanımda mesajı gösterir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void ShowUseDenyMessage()
        {
            ShowDenyMessage("GAME_ITEM_USED_ANOTHER_PLAYER");
        }

        /**
         *
         * Oyuncuya ekran ortasında etkileşim başarısız mesajı gösterir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void ShowDenyMessage(string text, bool isLang = true)
        {
            if (isLang)
            {
                text = ZeroLanguage.Get(text);
            }

            HandReticle.main.SetText(HandReticle.TextType.Hand, text, false, GameInput.Button.None);
            HandReticle.main.SetText(HandReticle.TextType.HandSubscript, string.Empty, false);
            HandReticle.main.SetIcon(HandReticle.IconType.HandDeny);
        }

        /**
         *
         * Tüm verileri siler.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void Dispose()
        {
            ClearAll();
        }
    }
}
