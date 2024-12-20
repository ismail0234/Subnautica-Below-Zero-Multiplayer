namespace Subnautica.API.Features.Helper
{
    public class GenericProperty
    {
        /**
         *
         * Anahtarı barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public string Key { get; set; }

        /**
         *
         * Değeri barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public object Value { get; set; }

        /**
         *
         * Sınıf ayarlamalarını yapar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public GenericProperty(string key, object value)
        {
            this.Key   = key;
            this.Value = value;
        }

        /**
         *
         * Value değerini değiştirir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void SetValue(object value)
        {
            this.Value = value;
        }

        /**
         *
         * Key döner.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public string GetKey()
        {
            return this.Key;
        }

        /**
         *
         * Özellik döner.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public T GetValue<T>()
        {
            if (this.Value == null)
            {
                return default(T);
            }

            return (T) this.Value;
        }
    }
}
