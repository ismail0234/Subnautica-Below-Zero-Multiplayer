namespace Subnautica.Network.Structures
{
    using MessagePack;

    [MessagePackObject]
    public class ZeroTransform
    {
        /**
         *
         * Forward
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(0)]
        public ZeroVector3 Forward;

        /**
         *
         * Position
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(1)]
        public ZeroVector3 Position;

        /**
         *
         * Rotation
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(2)]
        public ZeroQuaternion Rotation;

        /**
         *
         * Sınıf ayarlamalarını yapar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public ZeroTransform()
        {
        }

        /**
         *
         * Sınıf ayarlamalarını yapar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public ZeroTransform(ZeroVector3 forward, ZeroVector3 position, ZeroQuaternion rotation)
        {
            this.Forward  = forward;
            this.Position = position;
            this.Rotation = rotation;
        }

        /**
         *
         * Sınıf ayarlamalarını yapar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public ZeroTransform(ZeroVector3 position, ZeroQuaternion rotation)
        {
            this.Position = position;
            this.Rotation = rotation;
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
            return $"[ZeroTransform: {this.Forward}, {this.Position}, {this.Rotation}]";
        }
    }
}
