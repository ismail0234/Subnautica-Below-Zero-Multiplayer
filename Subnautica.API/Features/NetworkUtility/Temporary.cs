namespace Subnautica.API.Features.NetworkUtility
{
    using System.Collections.Generic;
    using System.Linq;

    using Subnautica.API.Features.Helper;

    public class Temporary
    {
        /**
         *
         * Properties barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
       private Dictionary<string, List<GenericProperty>> Properties = new Dictionary<string, List<GenericProperty>>();

        /**
         *
         * Özellik kaydı yapar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void SetProperty(string mainId, string key, object value)
        {
            if (this.Properties.TryGetValue(mainId, out var properties))
            {
                var data = properties.FirstOrDefault(q => q.Key == key);
                if (data == null)
                {
                    properties.Add(new GenericProperty(key, value));
                }
                else
                {
                    data.SetValue(value); 
                }
            }
            else 
            {
                this.Properties[mainId] = new List<GenericProperty>()
                {
                    new GenericProperty(key, value)
                };
            }
        }

        /**
         *
         * Özellik kaydı yapar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public T GetProperty<T>(string mainId, string key)
        {
            if (this.Properties.TryGetValue(mainId, out var properties))
            {
                var property = properties.FirstOrDefault(q => q.Key == key);
                if (property == null || property.Value == null)
                {
                    return default(T);
                }
                
                return (T) property.Value;
            }

            return default(T);
        }

        /**
         *
         * Verileri temizler.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void Dispose()
        {
            this.Properties.Clear();
        }
    }
}