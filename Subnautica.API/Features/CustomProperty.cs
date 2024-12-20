namespace Subnautica.API.Features
{
    using System;
    using System.Globalization;

    using MessagePack;

    [MessagePackObject]
    public class CustomProperty
    {
        /**
         *
         * Anahtarı barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(0)]         
        public byte Key { get; set; }

        /**
         *
         * Değeri barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(1)]
        public string Value { get; set; }

        /**
         *
         * Sınıf ayarlamalarını yapar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public CustomProperty(byte key, string value)
        {
            this.Key   = key;
            this.Value = value;
        }

        /**
         *
         * Anahtarı döner.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public T GetKey<T>()
        {
            var type = typeof(T);
            if (type.IsEnum)
            {
                return (T) Enum.ToObject(type, this.Key);
            }

            return (T) Convert.ChangeType(this.Key, type);
        }

        /**
         *
         * Değeri döner.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public T GetValue<T>()
        {
            var type = typeof(T);
            if (type.IsEnum)
            {
                return (T) Enum.Parse(type, this.Value);
            }

            return (T) Convert.ChangeType(this.Value, type, CultureInfo.InvariantCulture);
        }
    }
}