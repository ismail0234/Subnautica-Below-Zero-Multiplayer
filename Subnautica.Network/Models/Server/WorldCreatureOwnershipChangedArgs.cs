namespace Subnautica.Network.Models.Server
{
    using System.Collections.Generic;

    using MessagePack;

    using Subnautica.API.Enums;
    using Subnautica.Network.Models.Core;

    [MessagePackObject]
    public class WorldCreatureOwnershipChangedArgs : NetworkPacket
    {
        /**
         *
         * Ağ Paket Türü
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(0)]
        public override ProcessType Type { get; set; } = ProcessType.WorldCreatureOwnershipChanged;

        /**
         *
         * Temel Yaratık sahiplerini Değerini Barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(5)]
        public List<WorldCreatureOwnershipItem> Creatures { get; set; } = new List<WorldCreatureOwnershipItem>();
    }

    [MessagePackObject]
    public class WorldCreatureOwnershipItem
    {
        /**
         *
         * OwnerId Değerini Barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(0)]
        public byte OwnerId { get; set; }

        /**
         *
         * Id Değerini Barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(1)]
        public ushort Id { get; set; }

        /**
         *
         * Position Değerini Barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(2)]
        public long Position { get; set; }

        /**
         *
         * LeashRotation Değerini Barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(3)]
        public long Rotation { get; set; }

        /**
         *
         * TechType Değerini Barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(4)]
        public TechType TechType { get; set; }

        /**
         *
         * Sınıf ayarlamalarını yapar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public WorldCreatureOwnershipItem()
        {

        }

        /**
         *
         * Sınıf ayarlamalarını yapar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public WorldCreatureOwnershipItem(byte ownerId, ushort id, long position, long rotation, TechType techType)
        {
            this.OwnerId   = ownerId;
            this.Id        = id;
            this.Position  = position;
            this.Rotation  = rotation;
            this.TechType  = techType;
        }

        /**
         *
         * Yaratık sahibi var mı?
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public bool IsExistsOwnership()
        {
            return this.OwnerId != 0;
        }

        /**
         *
         * Kayıt için bekleniyor mu?
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public bool IsWaitingRegistation()
        {
            return this.TechType != TechType.None;
        }

        /**
         *
         * Ölme animasyonu başlasın mı?
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public bool IsPlayDeathAnimation()
        {
            return this.Position == -1;
        }
    }
}
