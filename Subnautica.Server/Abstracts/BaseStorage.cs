namespace Subnautica.Server.Abstracts
{
    using System;
    using System.IO;

    using Subnautica.API.Extensions;
    using Subnautica.API.Features;
    using Subnautica.Network.Core;

    public abstract class BaseStorage
    {   /**
         *
         * Multi Thread Kilidi
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public object ProcessLock { get; set; } = new object();

        /**
         *
         * Sunucu id numarasını barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public string ServerId { get; set; }

        /**
         *
         * Doysa yolunu barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public string FilePath { get; set; }

        /**
         *
         * Verileri dosyadan yükler.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public abstract void Load();

        /**
         *
         * İşlemleri başlatır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public abstract void Start(string serverId);

        /**
         *
         * Verileri diske yazar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public abstract void SaveToDisk();

        /**
         *
         * Verileri diske yazar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public bool WriteToDisk<T>(T storage)
        {
            if (storage == null)
            {
                Log.Error(string.Format("Storage.WriteToDisk -> Error Code (0x01): {0}", this.FilePath));
                return false;
            }

            var data = NetworkTools.Serialize(storage);
            if (data == null)
            {
                Log.Error(string.Format("Storage.WriteToDisk -> Error Code (0x02): {0}", this.FilePath));
                return false;
            }

            if (!data.IsValid())
            {
                Log.Error(string.Format("Storage.WriteToDisk -> Error Code (0x03): {0}", this.FilePath));
                return false;
            }

            return data.WriteToDisk(this.FilePath);
        }

        /**
         *
         * Verileri diske yazar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public bool InitializePath()
        {
            if (this.FilePath.IsNull())
            {
                return false;
            }

            Directory.CreateDirectory(Path.GetDirectoryName(this.FilePath));
            return true;
        }
    }
}
