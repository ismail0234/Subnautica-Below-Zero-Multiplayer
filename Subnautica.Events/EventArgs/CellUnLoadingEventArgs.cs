namespace Subnautica.Events.EventArgs
{
    using System;

    public class CellUnLoadingEventArgs : EventArgs
    {
        /**
         *
         * Sınıf ayarlamalarını yapar
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public CellUnLoadingEventArgs(EntityCell entityCell, Int3 batchId, Int3 cellId)
        {
            this.EntityCell = entityCell;
            this.BatchId    = batchId;
            this.CellId     = cellId;
        }

        /**
         *
         * EntityCell Değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public EntityCell EntityCell { get; private set; }

        /**
         *
         * BatchId Değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public Int3 BatchId { get; private set; }

        /**
         *
         * CellId Değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public Int3 CellId { get; private set; }
    }
}
