namespace Subnautica.Server.Core
{
    public class Storages
    {        
        /**
         *
         * Encyclopedia sınıfını barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public Storage.Encyclopedia Encyclopedia { get; set; } = new Storage.Encyclopedia();

        /**
         *
         * Construction sınıfını barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public Storage.Construction Construction { get; set; } = new Storage.Construction();

        /**
         *
         * Technology sınıfını barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public Storage.Technology Technology { get; set; } = new Storage.Technology();

        /**
         *
         * Player sınıfını barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public Storage.Player Player { get; set; } = new Storage.Player();

        /**
         *
         * World sınıfını barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public Storage.World World { get; set; } = new Storage.World();

        /**
         *
         * Scanner sınıfını barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public Storage.Scanner Scanner { get; set; } = new Storage.Scanner();

        /**
         *
         * PictureFrame sınıfını barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public Storage.PictureFrame PictureFrame { get; set; } = new Storage.PictureFrame();

        /**
         *
         * Story sınıfını barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public Storage.Story Story { get; set; } = new Storage.Story();

        /**
         *
         * Depolamaları başlatır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void Start(string serverId)
        {
            this.Encyclopedia.Start(serverId);
            this.Construction.Start(serverId);
            this.PictureFrame.Start(serverId);
            this.Technology.Start(serverId);
            this.Scanner.Start(serverId);
            this.Player.Start(serverId);
            this.World.Start(serverId);
            this.Story.Start(serverId);
        }

        /**
         *
         * Sınıfı temizler.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void Dispose()
        {
            this.Encyclopedia = null;
            this.Construction = null;
            this.PictureFrame = null;
            this.Technology   = null;
            this.Scanner      = null;
            this.Player       = null;
            this.World        = null;
            this.Story        = null;
        }
    }
}
