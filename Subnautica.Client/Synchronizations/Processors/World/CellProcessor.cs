namespace Subnautica.Client.Synchronizations.Processors.World
{
    using Subnautica.Network.Models.Core;
    using Subnautica.Client.Abstracts;
    using Subnautica.API.Features;

    using Subnautica.Events.EventArgs;

    public class CellProcessor : NormalProcessor
    {
        /**
         *
         * Gelen veriyi işler
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public override bool OnDataReceived(NetworkPacket networkPacket)
        {
            return true;
        }

        /**
         *
         * Cell yüklendikten sonra tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void OnCellLoading(CellLoadingEventArgs ev)
        {
            Network.CellManager.SetLoaded(ev.BatchId, ev.CellId, true);
        }

        /**
         *
         * Cell kaldırılırken tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void OnCellUnLoading(CellUnLoadingEventArgs ev)
        {
            Network.CellManager.SetLoaded(ev.BatchId, ev.CellId, false);
        }
    }
}
