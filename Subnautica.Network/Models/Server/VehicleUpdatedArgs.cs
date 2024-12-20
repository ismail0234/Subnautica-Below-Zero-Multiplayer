using Subnautica.Network.Models.Server;

namespace Subnautica.Network.Models.Server
{
    using System;
    using System.Collections.Generic;

    using LiteNetLib;

    using MessagePack;

    using Subnautica.API.Enums;
    using Subnautica.Network.Models.Core;
    using Subnautica.Network.Structures;

    [MessagePackObject]
    public class VehicleUpdatedArgs : NetworkPacket
    {
        /**
         *
         * Ağ Paket Türü
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(0)]
        public override ProcessType Type { get; set; } = ProcessType.VehicleUpdated;

        /**
         *
         * Packet Kanal Türü
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(1)]
        public override NetworkChannel ChannelType { get; set; } = NetworkChannel.VehicleMovement;

        /**
         *
         * Packet Teslim Türü
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(2)]
        public override DeliveryMethod DeliveryMethod { get; set; } = DeliveryMethod.Unreliable;

        /**
         *
         * Oyuncu id numarasını barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(5)]
        public byte PlayerId { get; set; }

        /**
         *
         * Araç id numarasını barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(6)]
        public ushort EntityId { get; set; }

        /**
         *
         * Nesne Pozisyonu
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(7)]
        public ZeroVector3 Position { get; set; }

        /**
         *
         * Nesne Açısı
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(8)]
        public ZeroQuaternion Rotation { get; set; }

        /**
         *
         * Nesne Açısı
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(9)]
        public VehicleUpdateComponent Component { get; set; }
    }

    [Union(0, typeof(ExosuitUpdateComponent))]
    [Union(1, typeof(SpyPenguinUpdateComponent))]
    [Union(2, typeof(HoverbikeUpdateComponent))]
    [MessagePackObject]
    public abstract class VehicleUpdateComponent
    {
        /**
         *
         * Yeni Veri mi?
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [IgnoreMember]
        public bool IsNew { get; set; }

        /**
         *
         * Komponenti döner.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public T GetComponent<T>()
        {
            if (this is T)
            {
                return (T)Convert.ChangeType(this, typeof(T));
            }

            return default(T);
        }
    }

    [Union(0, typeof(ExosuitDrillArmComponent))]
    [Union(1, typeof(ExosuitGrapplingArmComponent))]
    [Union(2, typeof(ExosuitClawArmComponent))]
    [MessagePackObject]
    public abstract class ExosuitArmComponent
    {
        /**
         *
         * Komponenti döner.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public T GetComponent<T>()
        {
            if (this is T)
            {
                return (T)Convert.ChangeType(this, typeof(T));
            }

            return default(T);
        }
    }

    [MessagePackObject]
    public class ExosuitUpdateComponent : VehicleUpdateComponent
    {
        /**
         *
         * IsOnGround Değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(0)]
        public bool IsOnGround { get; set; }

        /**
         *
         * CameraPosition değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(1)]
        public ZeroVector3 CameraPosition { get; set; }

        /**
         *
         * AngleX değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(2)]
        public float AngleX { get; set; }

        /**
         *
         * IsPlayingJumpSound değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(3)]
        public bool IsPlayingJumpSound { get; set; }

        /**
         *
         * IsPlayingBoostSound değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(4)]
        public bool IsPlayingBoostSound { get; set; }

        /**
         *
         * LeftArm değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(5)]
        public ExosuitArmComponent LeftArm { get; set; }

        /**
         *
         * RightArm değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(6)]
        public ExosuitArmComponent RightArm { get; set; }
    }

    [MessagePackObject]
    public class SpyPenguinUpdateComponent : VehicleUpdateComponent
    {
        /**
         *
         * IsDrilling Değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(0)]
        public bool IsSelfieMode { get; set; }

        /**
         *
         * SelfieNumber Değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(1)]
        public float SelfieNumber { get; set; }

        /**
         *
         * Animations Değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(2)]
        public List<string> Animations { get; set; }
    }

    [MessagePackObject]
    public class HoverbikeUpdateComponent : VehicleUpdateComponent
    {
        /**
         *
         * IsJumping Değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(0)]
        public bool IsJumping { get; set; }

        /**
         *
         * IsBoosting Değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(1)]
        public bool IsBoosting { get; set; }
    }

    [MessagePackObject]
    public class ExosuitDrillArmComponent : ExosuitArmComponent
    {
        /**
         *
         * IsDrilling Değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(0)]
        public bool IsDrilling { get; set; }

        /**
         *
         * IsDrilling Değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(1)]
        public bool IsFxPlaying { get; set; }
    }

    [MessagePackObject]
    public class ExosuitClawArmComponent : ExosuitArmComponent
    {
        /**
         *
         * IsBash Değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(0)]
        public bool IsBash { get; set; }

        /**
         *
         * IsPickup Değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(1)]
        public bool IsPickup { get; set; }

        /**
         *
         * IsUsing Değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(2)]
        public bool IsUsing { get; set; }
    }

    [MessagePackObject]
    public class ExosuitGrapplingArmComponent : ExosuitArmComponent
    {
        /**
         *
         * HookPosition Değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(0)]
        public ZeroVector3 HookPosition { get; set; }

        /**
         *
         * HookRotation Değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(1)]
        public ZeroQuaternion HookRotation { get; set; }

        /**
         *
         * IsFlying Değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(2)]
        public bool IsFlying { get; set; }

        /**
         *
         * IsAttached Değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(3)]
        public bool IsAttached { get; set; }

        /**
         *
         * IsUsing Değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(4)]
        public bool IsUsing { get; set; }

        /**
         *
         * IsStopped Değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(5)]
        public bool IsStopped { get; set; }
    }
}