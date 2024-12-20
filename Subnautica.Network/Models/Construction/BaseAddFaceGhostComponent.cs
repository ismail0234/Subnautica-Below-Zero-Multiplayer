namespace Subnautica.Network.Models.Construction
{
    using MessagePack;

    using Subnautica.Network.Models.Construction.Shared;

    [MessagePackObject]
    public class BaseAddFaceGhostComponent : BaseGhostComponent
    {
        /**
         *
         * Face değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(2)]
        public BaseFaceComponent Face { get; set; }
    }
}
