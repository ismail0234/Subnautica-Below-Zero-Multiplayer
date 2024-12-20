namespace Subnautica.Network.Structures
{
    using System;

    using MessagePack;

    [MessagePackObject]
    public class ZeroQuaternion : IEquatable<ZeroQuaternion>
    {
        /**
         *
         * Açı (X)
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(0)]
        public float X;

        /**
         *
         * Açı (Y)
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(1)]
        public float Y;

        /**
         *
         * Açı (Z)
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(2)]
        public float Z;

        /**
         *
         * Açı (W)
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(3)]
        public float W;

        /**
         *
         * Sınıf ayarlamalarını yapar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public ZeroQuaternion()
        {
        }

        /**
         *
         * Sınıf ayarlamalarını yapar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public ZeroQuaternion(float x, float y, float z, float w)
        {
            this.X = x;
            this.Y = y;
            this.Z = z;
            this.W = w;
        }

        /**
         *
         * Karşılaştırma işlemi yapar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static bool operator ==(ZeroQuaternion u, ZeroQuaternion v)
        {
            if (u is null && v is null)
            {
                return true;
            }

            return u is ZeroQuaternion && u.Equals(v);
        }

        /**
         *
         * Karşılaştırma işlemi yapar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static bool operator !=(ZeroQuaternion u, ZeroQuaternion v)
        {
            return !(u == v);
        }

        /**
         *
         * Eşitlik işlemi yapar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public bool Equals(ZeroQuaternion other)
        {
            if (other is null)
            {
                return false;
            }

            return other.X == this.X && other.Y == this.Y && other.Z == this.Z && other.W == this.W;
        }

        /**
         *
         * Eşitlik işlemi yapar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public override bool Equals(object obj)
        {
            return obj is ZeroQuaternion other && this.Equals(other);
        }

        /**
         *
         * Sayısal değeri döner.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public override int GetHashCode()
        {
            unchecked
            {
                var hash = 0;
                hash = (hash * 397) ^ this.X.GetHashCode();
                hash = (hash * 397) ^ this.Y.GetHashCode();
                hash = (hash * 397) ^ this.Z.GetHashCode();
                hash = (hash * 397) ^ this.W.GetHashCode();

                return hash;
            }
        }

        /**
         *
         * Metin olarak bastırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public override string ToString()
        {
            return $"[ZeroQuaternion: {this.X}, {this.Y}, {this.Z}, {this.W}]";
        }
    }
}
