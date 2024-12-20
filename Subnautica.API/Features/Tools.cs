namespace Subnautica.API.Features
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Text;

    using HarmonyLib;

    using Subnautica.API.Enums;
    using Subnautica.API.Extensions;
    using Subnautica.API.Features.Helper;

    using UnityEngine;
    using UnityEngine.Events;
    using UnityEngine.Playables;
    using UnityEngine.Timeline;

    using UWE;

    using SystemVersion = System.Version;

    public class Tools
    {
        /**
         *
         * Random sınıfını barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static System.Random Random { get; set; } = new System.Random();

        /**
         *
         * Launcher versiyonunu barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private static string[] LauncherVersionData { get; set; }

        /**
         *
         * Protobuf ile veriyi serilize eder.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static byte[] SerializeGameObject(UnityEngine.GameObject gameObject)
        {
            using (MemoryStream memoryStream = new MemoryStream())
            {
                using (PooledObject<ProtobufSerializer> proxy = ProtobufSerializerPool.GetProxy())
                {
                    proxy.Value.SerializeObjectTree(memoryStream, gameObject);
                    return memoryStream.ToArray();
                }
            }
        }

        /**
         *
         * Launcher'ın yeni versiyonunu kontrol eder.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static VersionType CheckCreditsPage()
        {
            string content = null;

            try
            {
                content = Request.GetContent(Paths.GetCreditsPageUrl());
                if (String.IsNullOrEmpty(content))
                {
                    return VersionType.EmptyContent;
                }

                var response = Newtonsoft.Json.JsonConvert.DeserializeObject<ApiCreditsDataFormat>(content);
                if (response == null)
                {
                    return VersionType.EmptyContent;
                }

                File.WriteAllText(Paths.GetLauncherCreditsApiFilePath(), content);

                Settings.CreditsApi = response;

                return VersionType.None;
            }
            catch (Exception e)
            {
                Log.Error($"CheckCreditsPage Exception: {e} {e.StackTrace}");
                return VersionType.ExceptionError;
            }
        }

        /**
         *
         * Launcher'ın yeni versiyonunu kontrol eder.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static VersionType CheckLauncherVersion(bool save = false)
        {
            string content = null;

            try
            {
                content = Request.GetContent(Paths.GetLauncherApiFileUrl());
                if (String.IsNullOrEmpty(content))
                {
                    return VersionType.EmptyContent;
                }

                var response = Newtonsoft.Json.JsonConvert.DeserializeObject<ApiDataFormat>(content);
                if (!response.IsStatus)
                {
                    return VersionType.SerializeError;
                }

                if (save)
                {
                    File.WriteAllText(Paths.GetLauncherApiFilePath(), content);
                    Settings.Api = Tools.GetApiData();
                }

                if (Tools.CheckVersion(Tools.GetLauncherVersion(false, true), response.Version))
                {
                    return VersionType.NewVersionFound;
                }

                return VersionType.NewVersionNotFound;
            }
            catch (Exception e)
            {
                Log.Error("CTX: " + content);
                Log.Error($"CheckLauncherVersion Exception: {e} {e.StackTrace}");
                return VersionType.ExceptionError;
            }
        }

        /**
         *
         * Oyuncu adını döner.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static string GetLoggedInName()
        {
            return global::PlatformUtils.main.GetLoggedInUserName();
        }

        /**
         *
         * Oyuncu idsini döner.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static string GetLoggedId()
        {
            var userId = global::PlatformUtils.main.GetCurrentUserId();
            if (userId == "0" || userId.IsNull())
            {
                return null;
            }

            return userId;
        }

        /**
         *
         * Method izleme içinde var mı?
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static bool IsInStackTrace(string text)
        {
            var stackTrace = new StackTrace();
            for (int i = 0; i < stackTrace.FrameCount; i++)
            {
                var methodBase = stackTrace.GetFrame(i).GetMethod();
                if (methodBase.Name.Contains(text))
                {
                    return true;
                }
            }

            return false;
        }

        /**
         *
         * Rastgele sayı üretir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static int GetRandomInt(int min, int max)
        {
            return Random.Next(min, max + 1);
        }

        /**
         *
         * Launcher yazarın adını döner.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static string GetLauncherAuthor(bool addAuthorString = false)
        {
            if(!addAuthorString)
            {
                return Settings.AuthorName;
            }

            return String.Format("{0}: {1}", ZeroLanguage.Get("APP_AUTHOR"), Settings.AuthorName);
        }

        /**
         *
         * 2 versiyonu kontrol eder.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static bool CheckVersion(string localVersionNumber, string remoteVersionNumber)
        {
            var versions1 = localVersionNumber.Split('.');
            var versions2 = remoteVersionNumber.Split('.');
            int maxLength = versions1.Length > versions2.Length ? versions1.Length : versions2.Length;

            for (int i = 0; i < maxLength; i++)
            {
                if (versions1.Length < i)
                {
                    versions1[i] = "0";
                }

                if (versions2.Length < i)
                {
                    versions2[i] = "0";
                }
            }

            SystemVersion localVersion  = new SystemVersion(string.Join(".", versions1));
            SystemVersion remoteVersion = new SystemVersion(string.Join(".", versions2));

            if (localVersion.CompareTo(remoteVersion) < 0)
            {
                return true;
            }

            return false;
        }

        /**
         *
         * Launcher versiyonunu döner.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static string GetLauncherVersion(bool addVersionString = false, bool addImplodeDot = true)
        {
            if (LauncherVersionData == null)
            {
                string[] data = Assembly.GetExecutingAssembly().GetName().Version.ToString().Split('.');
                if (!string.IsNullOrEmpty(Settings.LauncherVersion))
                {
                    data = Settings.LauncherVersion.Split('.');
                }

                Array.Resize(ref data, data.Length - 1);
                LauncherVersionData = data;
            }

            string launcherVersion = "";
            if (addVersionString)
            {
                launcherVersion = "v";
            }

            if (addImplodeDot)
            {
                return String.Format("{0}{1}", launcherVersion, String.Join(".", LauncherVersionData));
            }
            
            return String.Format("{0}{1}", launcherVersion, String.Join("", LauncherVersionData));
        }

        /**
         *
         * Base64 şifrelemesi yapar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static string Base64Encode(string plainText, int limit = 1)
        {
            if (plainText.IsNull())
            {
                return null;
            }

            for (int i = 0; i < limit; i++)
            {
                plainText = System.Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(plainText));
            }

            return plainText;
        }

        /**
         *
         * Kamera ileri yönünü döner.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static Vector3 GetCameraForward(bool yReset = false, bool isNormalize = false)
        {
            var forward = new Vector3()
            {
                x = MainCamera.camera.transform.forward.x,
                z = MainCamera.camera.transform.forward.z
            };

            if (!yReset)
            {
                forward.y = MainCamera.camera.transform.forward.y;
            }

            if (isNormalize)
            {
                forward.Normalize();
            }

            return forward;
        }

        /**
         *
         * Base64 şifresi Çözer.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static string Base64Decode(string base64EncodedData)
        {
            var base64EncodedBytes = System.Convert.FromBase64String(base64EncodedData);
            return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
        }

        /**
         *
         * İşlem yüzdesi hesaplar
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static double CalculatePercent(long one, long two, int max = 100)
        {
            return ((double) one / (double) two) * max;
        }

        /**
         *
         * İşlem bindesi hesaplar
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static double CalculateThousandToPercent(int thousand)
        {
            return ((double) thousand / 10.0);
        }

        /**
         *
         * Api ayarlarını döner. 
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static ApiDataFormat GetApiData()
        {
            return Newtonsoft.Json.JsonConvert.DeserializeObject<ApiDataFormat>(File.ReadAllText(Paths.GetLauncherApiFilePath()));
        }

        /**
         *
         * Credits Api ayarlarını döner. 
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static ApiCreditsDataFormat GetCreditsApiData()
        {
            return Newtonsoft.Json.JsonConvert.DeserializeObject<ApiCreditsDataFormat>(File.ReadAllText(Paths.GetLauncherCreditsApiFilePath()));
        }

        /**
         *
         * Klasör açar
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void OpenFolder(string folderPath)
        {
            try
            {
                Directory.CreateDirectory(folderPath);
                Process.Start(folderPath);
            }
            catch (Exception e)
            {
                Log.Error($"Tools.OpenFolder: {e}");
            }
        }

        /**
         *
         * Alt klasörleri kontrol eder ve oluşturur
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void CreateSubFolders(string localFile)
        {
            var paths = localFile.Split('\\');
            if (paths.Length > 1)
            {
                paths = paths.Take(paths.Length - 1).ToArray();
                if (paths.Length > 1)
                {
                    string folderPath = String.Join("\\", paths);
                    if (folderPath.IsNotNull())
                    {
                        Directory.CreateDirectory(folderPath);
                    }
                }
            }
        }

        /**
         *
         * Veriyi MD5 ile şifreler.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static string CreateMD5(string input)
        {
            if (input.IsNull())
            {
                return null;
            }
            
            using (System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create())
            {
                byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(input);
                byte[] hashBytes = md5.ComputeHash(inputBytes);

                StringBuilder sb = new StringBuilder();

                for (int i = 0; i < hashBytes.Length; i++)
                {
                     sb.Append(hashBytes[i].ToString("X2"));
                }
                
                return sb.ToString();
            }
        }

        /**
         *
         * Kısa Benzersiz Id oluşturur.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static string GetShortUniqueId()
        {
            var historicalDate = new DateTime(1970, 1, 1, 0, 0, 0);
            var spanTillNow    = DateTime.UtcNow.Subtract(historicalDate);

            string shortId = String.Format("{0:0}", spanTillNow.TotalMilliseconds);

            return long.Parse(shortId).ToString("X");
        }

        /**
         *
         * Boyutu metine çevirir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static string GetSizeByTextFormat(long size)
        {
            if(size < 1024)
            {
                return string.Format("{0} {1}", size, ZeroLanguage.Get("GAME_SIZE_B"));
            }

            if(size < 1024 * 1024)
            {
                return string.Format("{0} {1}", System.Math.Round((double) size / 1024, 2), ZeroLanguage.Get("GAME_SIZE_KB"));
            }

            if(size < 1024 * 1024 * 1024)
            {
                return string.Format("{0} {1}", System.Math.Round((double) size / 1024 / 1024, 2), ZeroLanguage.Get("GAME_SIZE_MB"));
            }

            return string.Format("{0} {1}", System.Math.Round((double) size / 1024 / 1024 / 1024, 2), ZeroLanguage.Get("GAME_SIZE_GB"));
        }

        /**
         *
         * Tarihi metine çevirir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static string GetDateByTextFormat(int unixTimeStamp)
        {
            DateTime dateTime = UnixTimeStampToDateTime(unixTimeStamp);

            string hourText   = dateTime.Hour > 10 ? dateTime.Hour.ToString() : "0" + dateTime.Hour;
            string minuteText = dateTime.Minute > 10 ? dateTime.Minute.ToString() : "0" + dateTime.Minute;

            return string.Format("{0} {1} {2} {3}:{4}", dateTime.Day, ZeroLanguage.Get("GAME_MONTH_" + dateTime.Month), dateTime.Year, hourText, minuteText);
        }

        /**
         *
         * Sayıyı tarihe çevirir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static DateTime UnixTimeStampToDateTime(int unixTimeStamp)
        {
            var dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            dateTime = dateTime.AddSeconds((double) unixTimeStamp).ToLocalTime();
            return dateTime;
        }

        /**
         *
         * Klasör boyutunu döner.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static long GetFolderSize(string folderPath)
        {
            DirectoryInfo di = new DirectoryInfo(folderPath);
            return di.EnumerateFiles("*", SearchOption.AllDirectories).Sum(fi => fi.Length);
        }

        /**
         *
         * Bepinex kontrolü yapar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static bool IsBepinexInstalled()
        {
            string gamePath = null;

            foreach (var item in AppDomain.CurrentDomain.GetAssemblies())
            {
                if (item.Location.Contains("Managed\\Assembly-CSharp.dll"))
                {
                    gamePath = item.Location.Replace("SubnauticaZero_Data\\Managed\\Assembly-CSharp.dll", "");
                    break;
                }
            }

            if (gamePath.IsNull())
            {
                Log.Error("IsBepinexInstalled: Null Problem");
                return true;
            }
            else
            {
                if (File.Exists(string.Format("{0}doorstop_config.ini", gamePath)))
                {
                    return true;
                }

                if (File.Exists(string.Format("{0}winhttp.dll", gamePath)))
                {
                    return true;
                }

                if (Directory.Exists(string.Format("{0}BepInEx", gamePath)))
                {
                    return true;
                }
            }

            return false;
        }

        /**
         *
         * Mevcut zamanı döner.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static int GetUnixTime()
        {
            return (int) DateTimeOffset.Now.ToUnixTimeSeconds();
        }

        /**
         *
         * Bilgisayar dilini uygulama diline çevirir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static string GetLanguage()
        {
            string language = CultureInfo.CurrentCulture.ToString();
            switch (language)
            {
                //case "ar-SA":
                //case "fa-IR":
                case "az-AZ":
                case "bg-BG":
                case "cs-CZ":
                case "de-DE":
                case "en-US":
                case "es-ES":
                case "fi-FI":
                case "fr-FR":
                case "hr-HR":
                case "is-IS":
                case "it-IT":
                case "ja-JP":
                case "ko-KR":
                case "nl-NL":
                case "pl-PL":
                case "pt-BR":
                case "ro-RO":
                case "ru-RU":
                case "sr-CS":
                case "sv-SE":
                case "tr-TR":
                case "uk-UA":
                case "zh-CN":
                    return language;
            }

            Log.Info($"Language Not Found: {language}");
            return "en-US";
        }

        /**
         *
         * Bilgisayar dilini döner.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static string GetComputerLanguage()
        {
            return CultureInfo.CurrentCulture.ToString();
        }
    }
}
