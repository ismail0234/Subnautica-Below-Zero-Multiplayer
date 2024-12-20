namespace Subnautica.Network.Models.Construction
{
    using MessagePack;

    using Subnautica.Network.Models.Construction.Shared;

    [MessagePackObject]
    public class BaseAddLadderGhostComponent : BaseGhostComponent
    {
        /**
         *
         * FaceStart değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(2)]
        public BaseFaceComponent FaceStart { get; set; }

        /**
         *
         * FaceEnd değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(3)]
        public BaseFaceComponent FaceEnd { get; set; }
    }
}
