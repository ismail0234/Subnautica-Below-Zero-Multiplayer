namespace Subnautica.API.Extensions
{
    using System;
    using System.IO;

    using Subnautica.API.Features;
    using Subnautica.API.Features.Compressors;
    using Subnautica.API.Features.NetworkUtility;
    using Subnautica.Network.Structures;
    
    using UnityEngine;

    public static class ZeroExtensions
    {
        /**
         *
         * Konumun hücresini döner.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static ZeroInt3 GetCellId(this ZeroVector3 position)
        {
            var cellX = (int) Math.Round(position.X / WorldStreamer.CellSize);
            var cellY = (int) Math.Round(position.Y / WorldStreamer.CellSize);
            var cellZ = (int) Math.Round(position.Z / WorldStreamer.CellSize);
            
            return new ZeroInt3(cellX, cellY, cellZ);
        }

        /**
         *
         * Byte'ı flota çevirir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static float ToFloat(this byte value)
        {
            return value / 100f;
        }

        /**
         *
         * Float'ı byte'ye çevirir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static byte ToByte(this float value)
        {
            return (byte) (value * 100f);
        }

        /**
         *
         * Float'ı shorta çevirir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static short ToShort(this float value)
        {
            return (short)(value * 100f);
        }

        /**
         *
         * Short'u flota çevirir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static float ToFloat(this short value)
        {
            return value / 100f;
        }

        /**
         *
         * Quaternion'u sıkıştırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static long Compress(this ZeroQuaternion quaternion)
        {
            return UnityCompressor_Long.QuaternionCompress(quaternion.ToQuaternion());
        }

        /**
         *
         * Quaternion'u sıkıştırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static long Compress(this Quaternion quaternion)
        {
            return UnityCompressor_Long.QuaternionCompress(quaternion);
        }

        /**
         *
         * long'u vector'e dönüştürür.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static Quaternion ToQuaternion(this long value)
        {
            return UnityCompressor_Long.QuaternionDecompress(value);
        }

        /**
         *
         * long'u zerovector'e dönüştürür.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static ZeroQuaternion ToZeroQuaternion(this long value)
        {
            return UnityCompressor_Long.QuaternionDecompress(value).ToZeroQuaternion();
        }

        /**
         *
         * Vector3'ü sıkıştırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static long Compress(this ZeroVector3 vector3)
        {
            return UnityCompressor_Long.Vector3Compress(vector3.X, vector3.Y, vector3.Z);
        }

        /**
         *
         * Vector3'ü sıkıştırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static int CompressToInt(this ZeroVector3 vector3)
        {
            return UnityCompressor_Int.Compress(vector3.X, vector3.Y, vector3.Z);
        }

        /**
         *
         * Vector3'ü sıkıştırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static int CompressToInt(this Vector3 vector3)
        {
            return UnityCompressor_Int.Compress(vector3.x, vector3.y, vector3.z);
        }

        /**
         *
         * int'i vector'e dönüştürür.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static Vector3 ToVector3(this int value)
        {
            return UnityCompressor_Int.Decompress(value);
        }

        /**
         *
         * Vector3'ü sıkıştırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static long Compress(this Vector3 vector3)
        {
            return UnityCompressor_Long.Vector3Compress(vector3.x, vector3.y, vector3.z);
        }

        /**
         *
         * long'u vector'e dönüştürür.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static Vector3 ToVector3(this long value)
        {
            return UnityCompressor_Long.Vector3Decompress(value);
        }

        /**
         *
         * long'u zerovector'e dönüştürür.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static ZeroVector3 ToZeroVector3(this long value)
        {
            return UnityCompressor_Long.ZeroVector3Decompress(value);
        }

        /**
         *
         * ZeroColor'u color'a çevirir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static Color ToColor(this ZeroColor zeroColor)
        {
            return new Color(zeroColor.R, zeroColor.G, zeroColor.B, zeroColor.A);
        }

        /**
         *
         * Color'u ZeroColor'a çevirir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static ZeroColor ToZeroColor(this Color color)
        {
            return new ZeroColor(color.r, color.g, color.b, color.a);
        }

        /**
         *
         * ZeroVector3'u Vector3'a çevirir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static Vector3 ToVector3(this ZeroVector3 zeroVector3, bool isDefault = false)
        {
            if (zeroVector3 == null && isDefault)
            {
                return Vector3.zero;
            }

            return new Vector3(zeroVector3.X, zeroVector3.Y, zeroVector3.Z);
        }

        /**
         *
         * Vector3'u ZeroVector3'a çevirir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static ZeroVector3 ToZeroVector3(this Vector3 vector3)
        {
            return new ZeroVector3(vector3.x, vector3.y, vector3.z);
        }

        /**
         *
         * ZeroInt3'ü Vector3'a çevirir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static Int3 ToInt3(this ZeroInt3 zeroInt3)
        {
            return new Int3(zeroInt3.X, zeroInt3.Y, zeroInt3.Z);
        }

        /**
         *
         * Vector3'u ZeroInt3'E çevirir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static ZeroInt3 ToZeroInt3(this Int3 int3)
        {
            return new ZeroInt3(int3.x, int3.y, int3.z);
        }

        /**
         *
         * ZeroQuaternion'u Quaternion'a çevirir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static Quaternion ToQuaternion(this ZeroQuaternion zeroQuaternion, bool isDefault = false)
        {
            if (zeroQuaternion == null && isDefault)
            {
                return Quaternion.identity;
            }

            return new Quaternion(zeroQuaternion.X, zeroQuaternion.Y, zeroQuaternion.Z, zeroQuaternion.W);
        }

        /**
         *
         * Quaternion'u ZeroQuaternion'a çevirir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static ZeroQuaternion ToZeroQuaternion(this Quaternion quaternion)
        {
            return new ZeroQuaternion(quaternion.x, quaternion.y, quaternion.z, quaternion.w);
        }

        /**
         *
         * Transform'u ZeroTransform'a çevirir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static ZeroTransform ToZeroTransform(this Transform transform)
        {
            return new ZeroTransform(transform.forward.ToZeroVector3(), transform.position.ToZeroVector3(), transform.rotation.ToZeroQuaternion());
        }

        /**
         *
         * Aradaki yaklaşık değeri hesaplar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static bool Approximately(this float value, float otherValue, float nearValue)
        {
            return Mathf.Abs(value - otherValue) >= nearValue;
        }

        /**
         *
         * Oyuncuya komponent ekler.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static T AddComponent<T>(this ZeroPlayer player) where T : MonoBehaviour
        {
            return player.PlayerModel.AddComponent<T>();
        }

        /**
         *
         * Oyuncunun local komponentlerini döner.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static T GetLocalComponent<T>(this ZeroPlayer player)
        {
            if (player.PlayerObject == null)
            {
                return default;
            }

            return player.PlayerObject.GetComponent<T>();
        }

        /**
         *
         * Oyuncunun komponentlerini döner.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static T GetComponent<T>(this ZeroPlayer player)
        {
            if (player.PlayerModel == null)
            {
                return default;
            }

            return player.PlayerModel.GetComponent<T>();
        }

        /**
         *
         * Oyuncunun çocuk komponentlerini döner.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static T[] GetComponentsInChildren<T>(this ZeroPlayer player, bool isActive = false)
        {
            return player.PlayerModel.GetComponentsInChildren<T>(isActive);
        }

        /**
         *
         * Transform nesnesinin tam yolunu döner.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static string GetPath(this Transform current)
        {
            if (current.parent == null)
            {
                return "/" + current.name;
            }
                
            return current.parent.GetPath() + "/" + current.name;
        }

        /**
         *
         * Byte'lerin doğruluğunu döner.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static bool IsValid(this byte[] datas)
        {
            var length = datas.Length;
            if (length > 32)
            {
                length = 32;
            }

            for (int i = 0; i < length; i++)
            {
                if (datas[i] != 0x0)
                {
                    return true;
                }
            }

            return false;
        }

        /**
         *
         * Byte'lerin doğruluğunu döner.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static bool WriteToDisk(this byte[] data, string filePath)
        {
            try
            {
                using (var fileStream = new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.Write))
                {
                    fileStream.Write(data, 0, data.Length);
                    fileStream.Flush(true);
                }

                return true;
            }
            catch (Exception e)
            {
                Log.Error($"ZeroExtension.WriteToDisk: {e}");
            }

            return false;
        }
    }
}
