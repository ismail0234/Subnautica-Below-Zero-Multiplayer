namespace Subnautica.Network.Models.Storage.Player
{
    using System.Collections.Generic;

    using MessagePack;

    using Subnautica.Network.Structures;

    [MessagePackObject]
    public class PlayerItem
    {
        /**
         *
         * Oyuncu Benzersiz Id Kimliği
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(0)]
        public string UniqueId { get; set; }

        /**
         *
         * Oyuncu Id Kimliği
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(1)]
        public byte PlayerId { get; set; }

        /**
         *
         * Oyuncu Adı
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(2)]
        public string PlayerName { get; set; }

        /**
         *
         * Nesne Açısı
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(3)]
        public ZeroQuaternion Rotation { get; set; }

        /**
         *
         * Nesne Pozisyonu
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(4)]
        public ZeroVector3 Position { get; set; }

        /**
         *
         * SubrootId Değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(5)]
        public string SubrootId { get; set; }

        /**
         *
         * InteriorId Değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(6)]
        public string InteriorId { get; set; }
    }
}
