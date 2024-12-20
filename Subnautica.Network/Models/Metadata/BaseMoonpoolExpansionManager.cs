namespace Subnautica.Network.Models.Metadata
{
    using MessagePack;

    using Subnautica.API.Extensions;
    using Subnautica.Network.Core.Components;

    [MessagePackObject]
    public class BaseMoonpoolExpansionManager : MetadataComponent
    {
        /**
         *
         * TailId Değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(0)]
        public string TailId { get; set; }

        /**
         *
         * Kuyruğu demirler.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void DockTail(string tailId)
        {
            this.TailId = tailId;
        }

        /**
         *
         * Kuyruğu kaldırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void UndockTail()
        {
            this.TailId = null;
        }

        /**
         *
         * Kuyruğu demirlenmiş mi?
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public bool IsTailDocked()
        {
            return this.TailId.IsNotNull();
        }
    }
}