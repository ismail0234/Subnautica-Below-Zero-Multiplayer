namespace Subnautica.API.Features
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Reflection;

    using Subnautica.API.Enums;

    public static class Log
    {
        /**
         *
         * Log Mesajlarını Barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static List<string> Messages { get; set; } = new List<string>();

        /**
         *
         * Zamanlayıcıyı barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private static System.Timers.Timer Timer { get; set; } = null;

        /**
         *
         * Zamanlayıcı başlatılma durumunu barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private static bool IsTimerInitialized { get; set;} = false;

        /**
         *
         * Diske Yazma Durumu
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static bool IsWritingToDisk { get; set; } = false;

        /**
         *
         * Bilgi mesajı gönderir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void Info(object message)
        {
            Log.Send($"[{Assembly.GetCallingAssembly().GetName().Name}] {message}", LogLevel.Info);
        }

        /**
         *
         * Uyarı mesajı gönderir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void Warn(object message)
        {
            Log.Send($"[{Assembly.GetCallingAssembly().GetName().Name}] {message}", LogLevel.Warn);
        }

        /**
         *
         * Hata mesajı gönderir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void Error(object message)
        {
            Log.Send($"[{Assembly.GetCallingAssembly().GetName().Name}] {message}", LogLevel.Error);
        }

        /**
         *
         * Bilgi mesajı gönderir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void Info(string message)
        {
            Log.Send($"[{Assembly.GetCallingAssembly().GetName().Name}] {message}", LogLevel.Info);
        }
        
        /**
         *
         * Uyarı mesajı gönderir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void Warn(string message)
        {
            Log.Send($"[{Assembly.GetCallingAssembly().GetName().Name}] {message}", LogLevel.Warn);
        }

        /**
         *
         * Hata mesajı gönderir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void Error(string message)
        {
            Log.Send($"[{Assembly.GetCallingAssembly().GetName().Name}] {message}", LogLevel.Error);
        }

        /**
         *
         * Hata/Uyarı/Bilgi mesajı gönderir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void Send(string message, LogLevel level)
        {
            Log.SendRaw($"[{level.ToString().ToUpper()}] {message}");
        }

        /**
         *
         * Mesajı dosyaya yazar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void SendRaw(string message)
        {
            lock (Log.Messages)
            {
                Log.Messages.Add(String.Format("[{0}] {1}\n", DateTime.Now.ToString("HH:mm:ss.fff"), message));
            }

            if (!Log.IsTimerInitialized)
            {
                Log.IsTimerInitialized = true;

                Log.Timer = new System.Timers.Timer();
                Log.Timer.Interval = 200;
                Log.Timer.Elapsed += Log.OnTimerElapsed;
                Log.Timer.Start();
            }
        }

        /**
         *
         * Zamanlayıcı tetiklenme olayı
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private static void OnTimerElapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            if (!Log.IsWritingToDisk && Log.Messages.Count > 0)
            {
                Log.IsWritingToDisk = true;

                lock (Log.Messages)
                {
                    try
                    {
                        File.AppendAllText(Log.GetErrorFilePath(), string.Join("", Log.Messages));
                    }
                    catch (Exception ex)
                    {
                        Log.Messages.Add($"Exception Log: {ex}");
                    }
                    finally
                    {
                        Log.Messages.Clear();
                    }
                }

                Log.IsWritingToDisk = false;
            }
        }

        /**
         *
         * Hata dosya adını döner.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static string GetErrorFilePath()
        {
            if (Settings.IsAppLog)
            {
                return String.Format("{0}{1}.log", Paths.GetLauncherLogPath(), DateTime.Now.ToString("yyyy-MM-dd"));
            }

            return String.Format("{0}{1}.log", Paths.GetGameLogsPath(), DateTime.Now.ToString("yyyy-MM-dd"));
        }
    }
}
