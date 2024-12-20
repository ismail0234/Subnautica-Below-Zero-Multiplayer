namespace Subnautica.Network.Structures
{
    using MessagePack;

    using Subnautica.API.Extensions;
    using Subnautica.API.Features;

    using UnityEngine;

    [MessagePackObject]
    public class ZeroLastTarget
    {
        /**
         *
         * TargetId değerini barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(0)]
        public string TargetId { get; set; }

        /**
         *
         * Type değerini barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(1)]
        public TechType Type { get; set; }

        /**
         *
         * IsDead değerini barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(2)]
        public bool IsDead { get; set; }

        /**
         *
         * Sınıf ayarlamalarını yapar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public ZeroLastTarget()
        {

        }

        /**
         *
         * Sınıf ayarlamalarını yapar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public ZeroLastTarget(string targetId, TechType type)
        {
            this.TargetId = targetId;
            this.Type     = type;
        }

        /**
         *
         * Ölüm durumunu değiştirir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void Kill()
        {
            this.IsDead = true;
        }

        /**
         *
         * Oyun nesnesini döner.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public GameObject GetGameObject(bool supressMessage = false)
        {
            return Network.Identifier.GetGameObject(this.TargetId, supressMessage);
        }

        /**
         *
         * Oyuncu mu?
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public bool IsPlayer()
        {
            return this.Type.IsPlayer();
        }

        /**
         *
         * Yaratık mu?
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public bool IsCreature()
        {
            return this.Type.IsCreature();
        }

        /**
         *
         * Araç mı?
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public bool IsVehicle()
        {
            return this.Type.IsVehicle();
        }

        /**
         *
         * Seatruck olup olmadığını döner.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public bool IsSeatruck()
        {
            return this.Type == TechType.SeaTruck;
        }

        /**
         *
         * Exosuit olup olmadığını döner.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public bool IsExosuit()
        {
            return this.Type == TechType.Exosuit;
        }
    }
}
