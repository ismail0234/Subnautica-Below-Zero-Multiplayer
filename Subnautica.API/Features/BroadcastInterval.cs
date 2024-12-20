namespace Subnautica.API.Features
{
    using UnityEngine;

    public class BroadcastInterval
    {
        /**
         *
         * Oyuncu paket veri gönderme hızı (50ms)
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public const float PlayerUpdated = 50f;

        /**
         *
         * Araç paket veri gönderme hızı (50ms)
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public const float VehicleUpdated = 50f;

        /**
         *
         * Oyuncu istatistikleri veri gönderme hızı (2000ms)
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public const float PlayerStatsUpdated = 2000f;

        /**
         *
         * Yapı hayalet model veri gönderme hızı (100ms)
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public const float ConstructingGhostMoved = 100f;

        /**
         *
         * Kontrol Odası harita hareket veri gönderme hızı (100ms)
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public const float BaseControlRoomMinimapMoving = 100f;

        /**
         *
         * VehicleDocking tetiklenme hızı (500ms)
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public const float VehicleDocking = 500f;

        /**
         *
         * Yaratık Konum gönderme hızı (min: 100ms, max: 200ms)
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public const float CreaturePosition = 100f;

        /**
         *
         * Yapı tamamlanma oranı veri gönderme hızı
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public const float ConstructingAmountChanged = 0.1f;

        /**
         *
         * Tarayıcı tarama veri gönderme hızı
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public const float ScannerUsing = 0.1f;

        /**
        *
        * Oyuncunun 2 açı arasındaki yumuşatılmış farkını döner.
        *
        * @author https://gist.github.com/maxattack/4c7b4de00f5c1b95a33b
        *
        */
        public static Quaternion QuaternionSmoothDamp(Quaternion rot, Quaternion target, ref Quaternion deriv, float time)
        {
            if (Time.deltaTime < Mathf.Epsilon)
            {
                return rot;
            }

            // account for double-cover
            var dot = Quaternion.Dot(rot, target);
            var multi = dot > 0f ? 1f : -1f;
            target.x *= multi;
            target.y *= multi;
            target.z *= multi;
            target.w *= multi;
            
            // smooth damp (nlerp approx)
            var result = new Vector4(
                Mathf.SmoothDamp(rot.x, target.x, ref deriv.x, time),
                Mathf.SmoothDamp(rot.y, target.y, ref deriv.y, time),
                Mathf.SmoothDamp(rot.z, target.z, ref deriv.z, time),
                Mathf.SmoothDamp(rot.w, target.w, ref deriv.w, time)
            ).normalized;

            // ensure deriv is tangent
            var derivError = Vector4.Project(new Vector4(deriv.x, deriv.y, deriv.z, deriv.w), result);
            deriv.x -= derivError.x;
            deriv.y -= derivError.y;
            deriv.z -= derivError.z;
            deriv.w -= derivError.w;

            return new Quaternion(result.x, result.y, result.z, result.w);
        }
    }
}
