namespace Subnautica.API.Features
{
    using System.Collections.Generic;
    using System.Text;

    using Subnautica.API.Extensions;
    using Subnautica.API.Features.Helper;
    
    public class Settings
    {
        /**
         *
         * Launcher api adresi
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        // public const string LauncherApiUrl = "https://raw.githubusercontent.com/ismail0234/Subnautica-Below-Zero-Multiplayer/main/";
        public const string LauncherApiUrl = "https://repo.subnauticamultiplayer.com/beta/";

        /**
         *
         * Github api adresi
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public const string GithubApiUrl = "https://raw.githubusercontent.com/ismail0234/Subnautica-Below-Zero-Multiplayer/";

        /**
         *
         * Github api adresi
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public const string CreditsApiUrl = "https://raw.githubusercontent.com/ismail0234/Subnautica-Below-Zero-Multiplayer/main/credits.json";

        /**
         *
         * Yazar Adı
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public const string AuthorName = "BOT Benson";

        /**
         *
         * Discord Client Id
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public const string DiscordClientId = "806248184405688380";

        /**
         *
         * Launcher api dosyası
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public const string LauncherApiFile = "Api.json";

        /**
         *
         * Launcher credits api dosyası
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public const string LauncherCreditsApiFile = "credits.json";

        /**
         *
         * Oyunun barındığı en üst dosya
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public const string RootFolder = ".botbenson";

        /**
         *
         * Uygulama klasör adı
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public const string ApplicationFolder = "App";

        /**
         *
         * Oyun klasör adı
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public const string GameFolder = "Game";

        /**
         *
         * Uygulama tmp klasör adı
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public const string ApplicationTempFolder = "Tmp";

        /**
         *
         * Uygulama image klasör adı
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public const string ApplicationImageFolder = "Images";

        /**
         *
         * Launcher oyun dosyası
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public const string LauncherGameFolder = "Subnautica Below Zero";

        /**
         *
         * Api ayarlarını barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static ApiDataFormat Api { get; set; }

        /**
         *
         * CreditsApi ayarlarını barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static ApiCreditsDataFormat CreditsApi { get; set; }

        /**
         *
         * ModConfig ayarlarını barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static ModConfigFormat ModConfig 
        { 
            get
            {
                if (modConfig == null)
                {
                    modConfig = new ModConfigFormat();
                    modConfig.Initialize();
                }

                return modConfig;
            }
        }

        /**
         *
         * Uygulama Logları aktif mi?
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static bool IsAppLog { get; set; } = false;

        /**
         *
         * IsBepinexInstalled Değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static bool IsBepinexInstalled { get; set; } = false;

        /**
         *
         * Launcher Version'unu barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static string LauncherVersion { get; set; }

        /**
         *
         * modConfig ayarlarını barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private static ModConfigFormat modConfig;

        /**
         *
         * Watermark metnini döner.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static string GetWatermarkText()
        {
            return string.Format("<size=18>Beta {0} (by BOT Benson)</size>", Tools.GetLauncherVersion(true));
        }

        /**
         *
         * Watermark metnini döner.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static string GetCreditsText()
        {
            var api = Tools.GetCreditsApiData();
            if (api != null)
            {
                var testers = new List<string>();
                var credits = new StringBuilder();

                credits.AppendLine("<style=role>Multiplayer Mod Creator / Programmer</style>");
                foreach (var item in api.ProjectOwner.Members)
                {
                    credits.AppendLine(item.Name);
                }

                credits.AppendLine("\n<style=role>Multiplayer Mod Patreon Supporters</style>");
                foreach (var item in api.PatreonSupporters.Members)
                {
                    credits.AppendLine(item.Name);
                }

                credits.AppendLine("\n<style=role>Multiplayer Mod Translators</style>");
                foreach (var item in api.Translators.Members)
                {
                    credits.AppendLine(item.Name);
                }

                credits.AppendLine("\n<style=role>Multiplayer Mod Alpha Testers</style>");

                if (api.AlphaTesters.Members.Count > 500)
                {
                    credits.AppendLine("2000+ Alpha Testers <3");
                }
                else
                {
                    foreach (var items in api.AlphaTesters.Members.Split(3))
                    {
                        testers.Clear();

                        foreach (var item in items)
                        {
                            testers.Add(item.Name);
                        }

                        credits.AppendLine(string.Join(" / ", testers));
                    }
                }

                return credits.ToString();
            }

            return null;
        }
    }
}
