namespace Subnautica.API.Features
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;

    using Subnautica.API.Enums;

    using UWE;

    public class WaitingScreen
    {
        /**
         *
         * Bloklu listeyi barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private static HashSet<ProcessType> List { get; set; } = new HashSet<ProcessType>();

        /**
         *
         * İşlem tamamlandı sinyali alınana kadar bekler.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static IEnumerator AddWaitScreen(ProcessType type, Action startingCallback, Action failureCallback)
        {
            AddWaitItem(type);

            startingCallback.Invoke();

            float sleepTime = Settings.ModConfig.ConnectionTimeout.GetInt() * 1000f;

            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            while (IsLoading(type, stopwatch.ElapsedMilliseconds, sleepTime))
            {
                yield return CoroutineUtils.waitForNextFrame;
            }

            if (IsLoading(type))
            {
                failureCallback.Invoke();
            }
        }

        /**
         *
         * İşlemi listeden kaldırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void RemoveScreen(ProcessType type)
        {
            List.Remove(type);
        }

        /**
         *
         * İşlem türünü listeye ekler.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private static void AddWaitItem(ProcessType type)
        {
            List.Add(type);
        }

        /**
         *
         * İşlem'in bitip/bitmediğini kontrol eder.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private static bool IsLoading(ProcessType type, float currentTime = 0f, float sleepTime = 0f)
        {
            if (sleepTime <= 0)
            {
                return List.Contains(type);
            }

            if (!List.Contains(type))
            {
                return false;
            }

            return currentTime < sleepTime;
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
            List.Clear();
        }
    }
}