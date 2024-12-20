namespace Subnautica.API.Features.Compressors
{
    using System;

    using Subnautica.Network.Structures;

    using UnityEngine;
    
    public class UnityCompressor_Int
    {
        /**
         *
         * X / Y / Z Meta verilerini barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public enum Metadata
        {
            None = 0x0000000,
            X    = 0x0000001,
            Y    = 0x0000002,
            Z    = 0x0000004
        }

        /**
         *
         * Büyük sayıyı barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private static int BigNumber = 1000000;

        /**
         *
         * Vector3'ü 12 bayttan 8 bayta sıkıştırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static int Compress(float x, float y, float z)
        {
            var qData = Metadata.None;
            
            if (x < 0)
            {
                qData |= Metadata.X;
            }

            if (y < 0)
            {
                qData |= Metadata.Y;
            }

            if (z < 0)
            {
                qData |= Metadata.Z;
            }

            var xData = (int)(Math.Abs(x) * 10);
            var yData = (int)(Math.Abs(y) * 10) * 100;
            var zData = (int)(Math.Abs(z) * 10) * 100 * 100;

            return (1000000 * (int) qData) + (xData + yData + zData);
        }

        /**
         *
         * Sıkıştırılmış Vector3'ü normal haline getirir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static Vector3 Decompress(int longNumber)
        {
            var flag = (byte)(longNumber / BigNumber);
            longNumber -= BigNumber * flag;

            var zData = longNumber / (100 * 100);
            longNumber -= (100 * 100) * zData;

            var yData = longNumber / (10 * 10);
            longNumber -= (10 * 10) * yData;

            if ((flag & 0x0000001) == 0x0000001)
            {
                longNumber *= -1;
            }

            if ((flag & 0x0000002) == 0x0000002)
            {
                yData *= -1;
            }

            if ((flag & 0x0000004) == 0x0000004)
            {
                zData *= -1;
            }

            return new Vector3(longNumber / 10f, yData / 10f, zData / 10f);
        }

        /**
         *
         * Sıkıştırılmış ZeroVector3'ü normal haline getirir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static ZeroVector3 ZeroDecompress(int longNumber)
        {
            var flag = (byte)(longNumber / BigNumber);
            longNumber -= BigNumber * flag;

            var zData = longNumber / (100 * 100);
            longNumber -= (100 * 100) * zData;

            var yData = longNumber / (10 * 10);
            longNumber -= (10 * 10) * yData;

            if ((flag & 0x0000001) == 0x0000001)
            {
                longNumber *= -1;
            }

            if ((flag & 0x0000002) == 0x0000002)
            {
                yData *= -1;
            }

            if ((flag & 0x0000004) == 0x0000004)
            {
                zData *= -1;
            }

            return new ZeroVector3(longNumber / 10f, yData / 10f, zData / 10f);
        }
    }
}