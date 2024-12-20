namespace Subnautica.Network.Models.Server
{
    using MessagePack;

    using Subnautica.API.Enums;
    using Subnautica.Network.Models.Core;

    [MessagePackObject]
    public class CreatureFreezeArgs : NetworkPacket
    {
        /**
         *
         * Ağ Paket Türü
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(0)]
        public override ProcessType Type { get; set; } = ProcessType.CreatureFreeze;

        /**
         *
         * CreatureId Değerini Barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(5)]
        public ushort CreatureId { get; set; }

        /**
         *
         * LifeTime değerini barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(6)]
        public float LifeTime { get; set; }

        /**
         *
         * BrinicleId değerini barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(7)]
        public string BrinicleId { get; set; }

        /**
         *
         * EndTime değerini barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(8)]
        public float EndTime { get; set; }

        /**
         *
         * EndTime değerini günceller.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void UpdateEndTime(float currentTime)
        {
            if (!this.IsInfinityLifeTime())
            {
                this.EndTime = this.LifeTime + currentTime;
            }
        }

        /**
         *
         * Sonsuz yaşam süresi?
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public bool IsInfinityLifeTime()
        {
            return this.LifeTime == float.PositiveInfinity;
        }
    }
}
