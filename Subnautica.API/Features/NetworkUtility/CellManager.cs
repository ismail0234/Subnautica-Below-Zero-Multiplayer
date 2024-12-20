namespace Subnautica.API.Features.NetworkUtility
{
    using System.Collections.Generic;

    using Subnautica.API.Extensions;
    using Subnautica.Network.Structures;

    public class CellManager
    {
        /**
         *
         * Grupları barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public readonly Dictionary<Int3, WorldStreamerBatchItem> Batches = new Dictionary<Int3, WorldStreamerBatchItem>(Int3.equalityComparer);

        /**
         *
         * Hücrenin yüklenme durumunu değiştirir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void SetLoaded(Int3 batchId, Int3 cellId, bool isLoaded)
        {
            if (Batches.TryGetValue(batchId, out var batch))
            {
                batch.Cells[cellId] = isLoaded;
            }
            else
            {
                Batches[batchId] = new WorldStreamerBatchItem();
                Batches[batchId].Cells[cellId] = isLoaded;
            }
        }

        /**
         *
         * Bölgenin yüklenip/yüklenmediğini döner.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public bool IsLoaded(Int3 batchId, Int3 cellId)
        {
            if (Batches.TryGetValue(batchId, out var batch))
            {
                return batch.IsLoaded(cellId);
            }

            return false;
        }

        /**
         *
         * Bölgenin yüklenip/yüklenmediğini döner.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public bool IsLoaded(ZeroVector3 position)
        {
            var block     = LargeWorldStreamer.main.GetBlock(position.ToVector3());
            var batchId   = block / LargeWorldStreamer.main.blocksPerBatch;
            var int3      = block % LargeWorldStreamer.main.blocksPerBatch;
            var cellLevel = 0;

            var cellSize = BatchCells.GetCellSize(cellLevel, LargeWorldStreamer.main.blocksPerBatch);
            var cellId   = int3 / cellSize;

            return IsLoaded(batchId, cellId);
        }

        /**
         *
         * Bütün verileri temizler.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void Dispose()
        {
            this.Batches.Clear();
        }
    }

    public class WorldStreamerBatchItem
    {
        /**
         *
         * Hücreleri barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public readonly Dictionary<Int3, bool> Cells = new Dictionary<Int3, bool>(Int3.equalityComparer);

        /**
         *
         * Hücre yüklenmiş mi?
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public bool IsLoaded(Int3 cellId)
        {
            return this.Cells.TryGetValue(cellId, out var isLoaded) && isLoaded;
        }
    }
}
