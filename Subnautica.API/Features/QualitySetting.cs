namespace Subnautica.API.Features
{
    using UnityEngine;

    public class QualitySetting
    {
        /**
         *
         * Eski FPS Değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private static int OldFrameRate  = 0;

        /**
         *
         * Eski VSYNC Durumu
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private static bool OldVsync = false;

        /**
         *
         * FPS Miktarını değiştirir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void EnableFastMode()
        {
            if (OldFrameRate != 501)
            {
                OldFrameRate = GraphicsUtil.GetFrameRate();
                OldVsync     = GraphicsUtil.GetVSyncEnabled();
            }

            Application.targetFrameRate = 501;
            UnityEngine.QualitySettings.vSyncCount = 0;
        }

        /**
         *
         * FPS Miktarını değiştirir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void DisableFastMode()
        {
            Reset();
        }

        /**
         *
         * Ayarları varsayılan yapar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void Reset()
        {
            if (OldFrameRate != 0)
            {
                Application.targetFrameRate = Mathf.Min(OldFrameRate, 500);
                UnityEngine.QualitySettings.vSyncCount = OldVsync ? 1 : 0;

                OldFrameRate = 0;
                OldVsync     = false;
            }
        }
    }
}
