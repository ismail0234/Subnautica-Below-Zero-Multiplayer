namespace Subnautica.Network.Models.Metadata
{
    using System.Collections.Generic;

    using MessagePack;

    using Subnautica.Network.Core.Components;

    [MessagePackObject]
    public class Bed : MetadataComponent
    {
        /**
         *
         * MaxPlayerCount Değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(0)]
        public byte MaxPlayerCount { get; set; }

        /**
         *
         * IsSleeping Değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(1)]
        public bool IsSleeping { get; set; }

        /**
         *
         * CurrentSide Değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(2)]
        public BedSideItem CurrentSide { get; set; }

        /**
         *
         * Sides Değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(3)]
        public List<BedSideItem> Sides { get; set; } = new List<BedSideItem>();

        /**
         *
         * Yatağın boş indexini döner.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public int GetBedEmptySideIndex()
        {
            if (this.MaxPlayerCount == 1)
            {
                return this.Sides.FindIndex(q => !q.IsUsing());
            }

            return this.Sides.FindIndex(q => !q.IsUsing() && q.Side == global::Bed.BedSide.None);
        }
    }

    [MessagePackObject]
    public class BedSideItem
    {
        /**
         *
         * PlayerId değerini barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(0)]
        public string PlayerId { get; set; }

        /**
         *
         * Side değerini barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(1)]
        public global::Bed.BedSide Side { get; set; }

        /**
         *
         * SleepTime değerini barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(2)]
        public float SleepTime { get; set; }

        /**
         *
         * PlayerIdv2 değerini barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(3)]
        public byte PlayerId_v2 { get; set; }

        /**
         *
         * Sınıf ayarlamalarını yapar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public BedSideItem()
        {

        }

        /**
         *
         * Sınıf ayarlamalarını yapar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public BedSideItem(byte playerId, global::Bed.BedSide side)
        {
            this.PlayerId_v2 = playerId;
            this.PlayerId    = null;
            this.Side        = side;
        }

        /**
         *
         * Oyuncuya uykuya yatırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void Sleep(byte playerId, global::Bed.BedSide side, float SleepTime)
        {
            this.PlayerId    = null;
            this.PlayerId_v2 = playerId;
            this.Side        = side;
            this.SleepTime   = SleepTime;
        }

        /**
         *
         * Oyuncuya yataktan kaldırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void Standup()
        {
            this.PlayerId    = null;
            this.PlayerId_v2 = 0;
            this.Side        = global::Bed.BedSide.None;
            this.SleepTime   = 0f;
        }

        /**
         *
         * Kullanılma Durumu
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public bool IsUsing()
        {
            return this.PlayerId_v2 > 0;
        }

        /**
         *
         * Uyuma Durumu
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public bool IsSleeping(float currentTime)
        {
            return this.IsUsing() && currentTime >= this.SleepTime + 4f;
        }
    }
}
