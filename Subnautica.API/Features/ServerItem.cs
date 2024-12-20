namespace Subnautica.API.Features
{
    using System;

    using Newtonsoft.Json;

    public class LocalServerItem
    {
        /**
         *
         * Sunucu Id
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public string Id { get; set; }

        /**
         *
         * Sunucu Adı
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public string Name { get; set; }

        /**
         *
         * Sunucu Ip Address
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public string IpAddress { get; set; }

        /**
         *
         * Sunucu Port
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public int Port { get; set; }
    }

    public class HostServerItem
    {
        /**
         *
         * Sunucu Id
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [JsonIgnore]
        public string Id { get; set; }

        /**
         *
         * Sunucu Oyun Modu
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public int GameMode { get; set; }

        /**
         *
         * Sunucu Oluşturulma Tarihi
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public int CreationDate { get; set; }

        /**
         *
         * Son Oynama Tarihi
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public int LastPlayedDate { get; set; }

        /**
         *
         * Oyun modu geçerli mi?
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public bool IsValidGameMode()
        {
            foreach (int item in Enum.GetValues(typeof(GameModePresetId)))
            {
                if (item == this.GameMode)
                {
                    return true;
                }
            }

            return false;
        }

        /**
         *
         * Oyun modunu döner.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public GameModePresetId GetGameMode()
        {
            return (GameModePresetId)this.GameMode;
        }
    }
}
