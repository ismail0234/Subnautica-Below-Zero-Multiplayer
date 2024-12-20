namespace Subnautica.Network.Models.Construction
{
    using MessagePack;

    using Subnautica.Network.Models.Construction.Shared;
    using Subnautica.Network.Structures;

    [MessagePackObject]
    public class BaseAddCellGhostComponent : BaseGhostComponent
    {
        /**
         *
         * TargetOffset değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(2)]
        public ZeroInt3 TargetOffset { get; set; }

        /**
         *
         * AboveFaceType değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(3)]
        public global::Base.FaceType AboveFaceType { get; set; }

        /**
         *
         * BelowFaceType değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(4)]
        public global::Base.FaceType BelowFaceType { get; set; }
    }
}
