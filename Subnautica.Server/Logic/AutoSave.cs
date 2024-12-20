namespace Subnautica.Server.Logic
{
    using System.Diagnostics;

    using Core;

    using Subnautica.API.Features;
    using Subnautica.Server.Abstracts;

    public class AutoSave : BaseLogic
    {
        /**
         *
         * Timing nesnesini barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public StopwatchItem Timing { get; set; } = new StopwatchItem(5000f);

        /**
         *
         * Belirli aralıklarla tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public override void OnAsyncUpdate()
        {
            if (API.Features.World.IsLoaded && this.Timing.IsFinished())
            {
                this.Timing.Restart();
                this.SaveAll();
            }
        }

        /**
         *
         * Tüm verileri diske yazar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void SaveAll()
        {
            Server.Instance.Storages.Encyclopedia.SaveToDisk();
            Server.Instance.Storages.Construction.SaveToDisk();
            Server.Instance.Storages.PictureFrame.SaveToDisk();
            Server.Instance.Storages.Technology.SaveToDisk();
            Server.Instance.Storages.Scanner.SaveToDisk();
            Server.Instance.Storages.Player.SaveToDisk();
            Server.Instance.Storages.World.SaveToDisk();
            Server.Instance.Storages.Story.SaveToDisk();
        }
    }
}
