namespace Subnautica.API.Features
{
    using System;
    using System.IO;

    using Subnautica.API.Extensions;

    public class Paths
    {
        /**
         *
         * AppData Önbelleği
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private static string _AppData { get; set; }

        /**
         *
         * CustomAppData Önbelleği
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private static string _CustomAppData { get; set; }

        /**
         *
         * App data yolunu döner
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static string AppData
        {
            get
            {
                if (_CustomAppData.IsNotNull())
                {
                    return _CustomAppData;
                }

                if (_AppData == null)
                {
                    _AppData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
                }

                return _AppData;
            }
        }

        /**
         *
         * Klasör ayracını döner
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static char DS
        {
            get
            {
                return Path.DirectorySeparatorChar;
            }
        }

        /**
         *
         * Özel app data yolunu değiştirir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void SetCustomAppDataPath(string customAppDataPath)
        {
            _CustomAppData = customAppDataPath;
        }

        /**
         *
         * Launcher api dosyasının linkini döner.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         * @example https://raw.githubusercontent.com/ismail0234/bb-subnautica-coop-mod/main/api.json
         *
         */
        public static string GetLauncherApiFileUrl()
        {
            return string.Format("{0}{1}", Settings.LauncherApiUrl, Settings.LauncherApiFile);
        }

        /**
         *
         * Launcherın dil dosyasının sunucudan linkini döner.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         * @example https://raw.githubusercontent.com/ismail0234/bb-subnautica-coop-mod/main/app/languages/tr_TR.json
         *
         */
        public static string GetLauncherLanguageUrl(string language)
        {
            return string.Format("{0}languages/{1}.json", Settings.GithubApiUrl, language);
        }

        /**
         *
         * Launcherın krediler dosyasının sunucudan linkini döner.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         * @example https://raw.githubusercontent.com/ismail0234/Subnautica-Below-Zero-Multiplayer/main/credits.json
         *
         */
        public static string GetCreditsPageUrl()
        {
            return Settings.CreditsApiUrl;
        }

        /**
         *
         * Launcherın sunucudaki versiyon linkini döner.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         * @example https://github.com/ismail0234/mc/blob/main/launchers/v2.0.0.exe
         *
         */
        public static string GetLauncherRemoteUrl(string version)
        {
            return string.Format("{0}Launchers/{1}.exe", Settings.LauncherApiUrl, version);
        }

        /**
         *
         * Ana klasör yolu
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         * @example /.botbenson/
         *
         */
        public static string GetLauncherRootPath()
        {
            return string.Format("{0}{1}{2}{3}", AppData, DS, Settings.RootFolder, DS);
        }

        /**
         *
         * Launcher Oyun klasör yolu
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         * @example /.botbenson/Subnautica Below Zero/
         *
         */
        public static string GetLauncherSubnauticaPath()
        {
            return string.Format("{0}{1}{2}", GetLauncherRootPath(), Settings.LauncherGameFolder, DS);
        }

        /**
         *
         * Launcher için application yolunu döner.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         * @example /.botbenson/Subnautica Below Zero/App/
         *
         */
        public static string GetLauncherApplicationPath(string subPath = null, bool addDS = true)
        {
            var applicationPath = GetLauncherSubnauticaPath();
            if (Settings.ApplicationFolder.Length <= 0)
            {
                return applicationPath;
            }

            if (subPath == null)
            {
                return string.Format("{0}{1}{2}", applicationPath, Settings.ApplicationFolder, DS);
            }

            if (addDS)
            {
                return string.Format("{0}{1}{2}{3}{4}", applicationPath, Settings.ApplicationFolder, DS, subPath, DS);
            }

            return string.Format("{0}{1}{2}{3}", applicationPath, Settings.ApplicationFolder, DS, subPath);
        }

        /**
         *
         * Launcher için game klasör yolunu döner.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         * @example /.botbenson/Subnautica Below Zero/Game/
         *
         */
        public static string GetLauncherGamePath(string subPath = null, bool addDS = true)
        {
            var applicationPath = GetLauncherSubnauticaPath();
            if (Settings.ApplicationFolder.Length <= 0)
            {
                return applicationPath;
            }

            if (subPath == null)
            {
                return string.Format("{0}{1}{2}", applicationPath, Settings.GameFolder, DS);
            }

            if (addDS)
            {
                return string.Format("{0}{1}{2}{3}{4}", applicationPath, Settings.GameFolder, DS, subPath, DS);
            }

            return string.Format("{0}{1}{2}{3}", applicationPath, Settings.GameFolder, DS, subPath);
        }

        /**
         *
         * Oyun çekirdek klasör yolunu döner.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         * @example /.botbenson/Subnautica Below Zero/Game/Core/
         * @example /.botbenson/Subnautica Below Zero/Game/Core/Assembly-CSharp.dll
         *
         */
        public static string GetLauncherGameCorePath(string filename = null)
        {
            if (filename == null)
            {
                return GetLauncherGamePath("Core");
            }

            return String.Format("{0}{1}", GetLauncherGamePath("Core"), filename);
        }

        /**
         *
         * Oyun çekirdek klasör yolunu döner.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         * @example /.botbenson/Subnautica Below Zero/Game/Core/Test/
         * @example /.botbenson/Subnautica Below Zero/Game/Core/Test/File.txt
         *
         */
        public static string GetLauncherGameCorePath(string foldername, string filename = null)
        {
            if (foldername == null)
            {
                return String.Format("{0}{1}{2}", GetLauncherGamePath("Core"), foldername, DS);
            }

            return String.Format("{0}{1}{2}{3}", GetLauncherGamePath("Core"), foldername, DS, filename);
        }

        /**
         *
         * Netbird çekirdek klasör yolunu döner.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         * @example /.botbenson/Subnautica Below Zero/Game/Core/Netbird/File.txt
         *
         */
        public static string GetNetbirdPath(string filename = null)
        {
            return GetLauncherGameCorePath("NetBird", filename);
        }

        /**
         *
         * Oyun bağımlılık klasör yolunu döner.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         * @example /.botbenson/Subnautica Below Zero/Game/Dependencies/
         * @example /.botbenson/Subnautica Below Zero/Game/Dependencies/Subnautica.API.dll
         *
         */
        public static string GetGameDependenciesPath(string filename = null)
        {
            if (filename == null)
            {
                return GetLauncherGamePath("Dependencies");
            }

            return String.Format("{0}{1}", GetLauncherGamePath("Dependencies"), filename);
        }

        /**
         *
         * Oyun Plugin klasör yolunu döner.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         * @example /.botbenson/Subnautica Below Zero/Game/Plugins/
         * @example /.botbenson/Subnautica Below Zero/Game/Plugins/Subnautica.Client.dll
         *
         */
        public static string GetGamePluginsPath(string filename = null)
        {
            if (filename == null)
            {
                return GetLauncherGamePath("Plugins");
            }

            return String.Format("{0}{1}", GetLauncherGamePath("Plugins"), filename);
        }

        /**
         *
         * Oyun Log klasör yolunu döner.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         * @example /.botbenson/Subnautica Below Zero/Game/Log/
         *
         */
        public static string GetGameLogsPath()
        {
            return GetLauncherGamePath("Logs");
        }

        /**
         *
         * Multiplayer Oyun Kayıt klasör yolunu döner.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         * @example /.botbenson/Subnautica Below Zero/Game/Saves/
         *
         */
        public static string GetMultiplayerSavePath(string foldername = null, string innerFolderName = null)
        {
            if (foldername == null)
            {
                return GetLauncherGamePath("Saves");
            }

            if (innerFolderName != null)
            {
                return string.Format("{0}{1}{2}{3}{4}", GetLauncherGamePath("Saves"), foldername, DS, innerFolderName, DS);
            }

            return string.Format("{0}{1}{2}", GetLauncherGamePath("Saves"), foldername, DS);
        }

        /**
         *
         * Multiplayer Server Kayıt klasör yolunu döner.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         * @example /.botbenson/Subnautica Below Zero/Game/Saves/Server/
         * @example /.botbenson/Subnautica Below Zero/Game/Saves/Server/ASUSKOXMASJQYSDTSRXCMVLP/
         * @example /.botbenson/Subnautica Below Zero/Game/Saves/Server/ASUSKOXMASJQYSDTSRXCMVLP/gameinfo.json
         *
         */
        public static string GetMultiplayerServerSavePath(string serverId = null, string filename = null)
        {
            if (serverId == null)
            {
                return GetMultiplayerSavePath("Server");
            }

            if (filename == null)
            {
                return GetMultiplayerSavePath("Server", serverId);
            }

            return string.Format("{0}{1}", GetMultiplayerSavePath("Server", serverId), filename);
        }

        /**
         *
         * Multiplayer Client Kayıt klasör yolunu döner.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         * @example /.botbenson/Subnautica Below Zero/Game/Saves/Client/
         * @example /.botbenson/Subnautica Below Zero/Game/Saves/Client/ASUSKOXMASJQYSDTSRXCMVLP/
         * @example /.botbenson/Subnautica Below Zero/Game/Saves/Client/ASUSKOXMASJQYSDTSRXCMVLP/gameinfo.json
         *
         */
        public static string GetMultiplayerClientSavePath(string serverId = null, string filename = null)
        {
            if (serverId == null)
            {
                return GetMultiplayerSavePath("Client");
            }

            if (filename == null)
            {
                return GetMultiplayerSavePath("Client", serverId);
            }

            return string.Format("{0}{1}", GetMultiplayerSavePath("Client", serverId), filename);
        }

        /**
         *
         * Multiplayer Client spawn point yolunu döner.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         * @example /.botbenson/Subnautica Below Zero/Game/Saves/Client/ASUSKOXMASJQYSDTSRXCMVLP/SpawnPoint.bin
         *
         */
        public static string GetMultiplayerClientSpawnPointPath(string serverId)
        {
            return GetMultiplayerClientSavePath(serverId, "SpawnPoint.bin");
        }

        /**
         *
         * Multiplayer Client Kayıt klasör yolunu döner.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         * @example /.botbenson/Subnautica Below Zero/Game/Saves/Client/
         * @example /.botbenson/Subnautica Below Zero/Game/Saves/Client/RemoteScreenshots/
         * @example /.botbenson/Subnautica Below Zero/Game/Saves/Client/RemoteScreenshots/abc.jpg
         *
         */
        public static string GetMultiplayerClientRemoteScreenshotsPath(string serverId, string filename = null)
        {
            var dataPath = GetMultiplayerClientSavePath(serverId, "RemoteScreenshots");
            if (filename == null)
            {
                return dataPath;
            }

            return string.Format("{0}{1}{2}", dataPath, DS, filename);
        }

        /**
         *
         * Multiplayer Client Kayıt klasör yolunu döner.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         * @example /.botbenson/Subnautica Below Zero/Game/Saves/Client/
         * @example /.botbenson/Subnautica Below Zero/Game/Saves/Client/RemoteScreenshots/
         * @example /.botbenson/Subnautica Below Zero/Game/Saves/Client/RemoteScreenshots/abc.jpg
         *
         */
        public static string GetMultiplayerClientRemoteScreenshotsThumbnailPath(string serverId, string filename)
        {
            var dataPath = GetMultiplayerClientSavePath(serverId, "RemoteScreenshots");
            if (filename == null)
            {
                return dataPath;
            }

            return GetMultiplayerClientRemoteScreenshotsPath(serverId, filename).Replace(".jpg", "_thumbnail.jpg");
        }

        /**
         *
         * Multiplayer Server / Players klasör yolunu döner.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         * @example /.botbenson/Subnautica Below Zero/Game/Saves/Server/ASUSKOXMASJQYSDTSRXCMVLP/Players/
         * @example /.botbenson/Subnautica Below Zero/Game/Saves/Server/ASUSKOXMASJQYSDTSRXCMVLP/Players/abc.bin
         *
         */
        public static string GetMultiplayerServerPlayerSavePath(string serverId, string playerUniqueId = null)
        {
            var dataPath = string.Format("{0}Players{1}", GetMultiplayerServerSavePath(serverId), DS);
            if (playerUniqueId == null)
            {
                return dataPath;
            }

            return string.Format("{0}{1}", dataPath, playerUniqueId);
        }

        /**
         *
         * Sunucular dosyasını döner.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         * @example /.botbenson/Subnautica Below Zero/Game/servers.json
         *
         */
        public static string GetGameServersPath()
        {
            return GetLauncherGamePath("servers.json", false);
        }

        /**
         *
         * Launcher Log klasör yolu
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         * @example /.botbenson/Subnautica Below Zero/App/Logs/
         *
         */
        public static string GetLauncherLogPath()
        {
            return string.Format("{0}", GetLauncherApplicationPath("Logs"));
        }

        /**
         *
         * Launcher'ın dil klaör yolunu döner.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         * @example /.botbenson/Subnautica Below Zero/App/Languages/
         *
         */
        public static string GetLauncherLanguagePath()
        {
            return string.Format("{0}", GetLauncherApplicationPath("Languages"));
        }

        /**
         *
         * Launcher'ın dil dosyası yolunu döner.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         * @example /.botbenson/Subnautica Below Zero/App/Languages/tr_TR.json
         *
         */
        public static string GetLauncherLanguageFile(string language)
        {
            return string.Format("{0}{1}.json", GetLauncherApplicationPath("Languages"), language);
        }

        /**
         *
         * Slider resmini döner.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         * @example /.botbenson/Subnautica Below Zero/App/Images/Subnautica.jpg
         *
         */
        public static string GetSliderImagePath(string image = "")
        {
            return string.Format("{0}{1}", GetLauncherApplicationPath(Settings.ApplicationImageFolder), image);
        }

        /**
         *
         * Launcher api dosyasının yolunu döner.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         * @example /.botbenson/Subnautica Below Zero/App/api.json
         *
         */
        public static string GetLauncherApiFilePath()
        {
            return string.Format("{0}{1}", GetLauncherApplicationPath(), Settings.LauncherApiFile);
        }

        /**
         *
         * Launcher credits api dosyasının yolunu döner.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         * @example /.botbenson/Subnautica Below Zero/App/credits.json
         *
         */
        public static string GetLauncherCreditsApiFilePath()
        {
            return string.Format("{0}{1}", GetLauncherApplicationPath(), Settings.LauncherCreditsApiFile);
        }

        /**
         *
         * Launcher'ın temp klasörünü döner.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         * @example /.botbenson/Subnautica Below Zero/App/Tmp/
         *
         */
        public static string GetLauncherTempPath()
        {
            return string.Format("{0}", GetLauncherApplicationPath(Settings.ApplicationTempFolder));
        }

        /**
         *
         * Launcher'ın temp dosyasını döner.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         * @example /.botbenson/Subnautica Below Zero/App/Tmp/Temp.exe
         *
         */
        public static string GetLauncherTempFile()
        {
            return string.Format("{0}Temp.exe", GetLauncherApplicationPath(Settings.ApplicationTempFolder));
        }

        /**
         *
         * Launcher'ın yeni sürüm dosyasını döner.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         * @example /.botbenson/Subnautica Below Zero/App/Tmp/NewVersion.exe
         *
         */
        public static string GetLauncherNewVersionFile()
        {
            return string.Format("{0}NewVersion.exe", GetLauncherApplicationPath(Settings.ApplicationTempFolder));
        }

        /**
         *
         * Launcher'ın şuanki dinamik dosya yolunu döner.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static string GetNowLauncherFile()
        {
            return System.Reflection.Assembly.GetEntryAssembly().Location;
        }
    }
}