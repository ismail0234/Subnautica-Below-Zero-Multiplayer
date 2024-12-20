namespace Subnautica.API.Features.Helper
{
    using System;
    using System.IO;

    using Newtonsoft.Json;

    public class ModConfigFormat
    {
        /**
         *
         * Sunucuya bağlanma zaman aşımı süresi.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public ModConfigFormatItem ConnectionTimeout { get; set; } = new ModConfigFormatItem(120, "Connection timeout period. (Type: Number/Second, Default: 120, Min: 60, Max: 300)");

        /**
         *
         * Sınıf ayarlamalarını yapar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void Initialize()
        {
            var filePath = Paths.GetLauncherGameCorePath("Config.json");
            if (!File.Exists(filePath))
            {
                File.WriteAllText(filePath, JsonConvert.SerializeObject(this, Formatting.Indented));
            }

            try
            {
                var config = JsonConvert.DeserializeObject<ModConfigFormat>(File.ReadAllText(filePath));
                if (config.ConnectionTimeout.GetInt() >= 60 && config.ConnectionTimeout.GetInt() <= 300)
                {
                    this.ConnectionTimeout.SetValue(config.ConnectionTimeout.GetInt());
                }
            }
            catch (Exception ex)
            {
                Log.Error($"ModConfigFormat.Initialize - Exception: {ex}");
            }
        }
    }

    public class ModConfigFormatItem
    {
        /**
         *
         * Açıklamayı barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public string Description { get; set; }

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
        public ModConfigFormatItem(object value, string description)
        {
            this.Value       = value;
            this.Description = description;
        }

        /**
         *
         * Değeri gğnceller
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
         * Int Değeri döner.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public int GetInt(int defaultValue = -1)
        {
            try
            {
                return Convert.ToInt32(this.Value);
            }
            catch (Exception)
            {
                return defaultValue;
            }
        }
    }
}
