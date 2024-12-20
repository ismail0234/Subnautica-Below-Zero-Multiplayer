namespace Subnautica.Network.Structures
{
    using System;
    using System.Collections.Generic;

    using MessagePack;

    [MessagePackObject]
    public class ZeroInt3 : IEquatable<ZeroInt3>
    {
        /**
         *
         * Koordinat (X)
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(0)]
        public int X;

        /**
         *
         * Koordinat (Y)
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(1)]
        public int Y;

        /**
         *
         * Koordinat (Z)
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(2)]
        public int Z;

        /**
         *
         * Sınıf ayarlamalarını yapar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public ZeroInt3()
        {
        }

        /**
         *
         * Sınıf ayarlamalarını yapar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public ZeroInt3(int x, int y, int z)
        {
            this.X = x;
            this.Y = y;
            this.Z = z;
        }

        /**
         *
         * Karşılaştırma işlemi yapar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static bool operator ==(ZeroInt3 u, ZeroInt3 v)
        {
            if (u is null && v is null)
            {
                return true;
            }

            return u is ZeroInt3 && u.Equals(v);
        }

        /**
         *
         * Karşılaştırma işlemi yapar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static bool operator !=(ZeroInt3 u, ZeroInt3 v)
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
        public bool Equals(ZeroInt3 other)
        {
            if (other is null)
            {
                return false;
            }

            return other.X == this.X && other.Y == this.Y && other.Z == this.Z;
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
            return obj is ZeroInt3 other && this.Equals(other);
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

                return hash;
            }
        }

        /**
         *
         * Koordinatların komşularını döner.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public IEnumerable<ZeroInt3> GetNeighbors(int max = 1)
        {
            for (int dx = -max; dx <= max; ++dx)
            {
                for (int dy = -max; dy <= max; ++dy)
                {
                    for (int dz = -max; dz <= max; ++dz)
                    {
                        yield return new ZeroInt3(this.X + dx, this.Y + dy, this.Z + dz);
                    }
                }
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
            return $"[ZeroInt3: {this.X}, {this.Y}, {this.Z}]";
        }
    }
}
