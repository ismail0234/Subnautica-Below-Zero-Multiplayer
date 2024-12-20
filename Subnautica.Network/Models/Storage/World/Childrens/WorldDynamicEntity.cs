namespace Subnautica.Network.Models.Storage.World.Childrens
{
    using MessagePack;

    using Subnautica.API.Extensions;
    using Subnautica.API.Enums;
    using Subnautica.API.Features;
    using Subnautica.Network.Core.Components;
    using Subnautica.Network.Structures;

    using UnityEngine;

    using WorldEntityModel = Subnautica.Network.Models.WorldEntity.DynamicEntityComponents;

    [MessagePackObject]
    public class WorldDynamicEntity
    {
        /**
         *
         * Yapı Kimliği değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(0)]
        public ushort Id { get; set; }

        /**
         *
         * Item değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(1)]
        public string UniqueId { get; set; }

        /**
         *
         * Üst Nesne Kimliği değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(2)]
        public string ParentId { get; set; }

        /**
         *
         * Item değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(3)]
        public byte[] Item { get; set; }

        /**
         *
         * TechType değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(4)]
        public TechType TechType { get; set; }

        /**
         *
         * Position değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(5)]
        public ZeroVector3 Position { get; set; }

        /**
         *
         * Rotation değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(6)]
        public ZeroQuaternion Rotation { get; set; }

        /**
         *
         * AddedTime değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(7)]
        public float AddedTime { get; set; }

        /**
         *
         * OwnershipId değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(8)]
        public string OwnershipId { get; set; }

        /**
         *
         * IsDeployed değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(9)]
        public bool IsDeployed { get; set; }

        /**
         *
         * IsGlobalEntity değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(10)]
        public bool IsGlobalEntity { get; set; }

        /**
         *
         * Component değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(11)]
        public NetworkDynamicEntityComponent Component { get; set; }

        /**
         *
         * IsUsingByPlayer değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [IgnoreMember]
        public bool IsUsingByPlayer { get; set; }

        /**
         *
         * Velocity değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [IgnoreMember]
        public Vector3 Velocity;

        /**
         *
         * RotationVelocity değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [IgnoreMember]
        public Quaternion RotationVelocity;

        /**
         *
         * GameObject değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [IgnoreMember]
        public GameObject GameObject;

        /**
         *
         * Rigidbody değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [IgnoreMember]
        public Rigidbody Rigidbody;

        /**
         *
         * IsKinematic değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [IgnoreMember]
        public ZeroKinematicState KinematicState;

        /**
         *
         * LastPosition değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [IgnoreMember]
        private ZeroVector3 LastPosition;

        /**
         *
         * LastRotation değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [IgnoreMember]
        private ZeroQuaternion LastRotation;

        /**
         *
         * CurrentPosition değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [IgnoreMember]
        private Vector3 CurrentPosition;

        /**
         *
         * CurrentRotation değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [IgnoreMember]
        private Quaternion CurrentRotation;

        /**
         *
         * TeleportDistance değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [IgnoreMember]
        private float TeleportDistance = 100f;

        /**
         *
         * LiveMixin nesnesini döner.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public WorldEntityModel.Shared.LiveMixin GetLiveMixin()
        {
            if (this.Component is WorldEntityModel.SeaTruck)
            {
                return this.Component.GetComponent<WorldEntityModel.SeaTruck>().LiveMixin;
            }
            else if (this.Component is WorldEntityModel.Exosuit)
            {
                return this.Component.GetComponent<WorldEntityModel.Exosuit>().LiveMixin;
            }
            else if (this.Component is WorldEntityModel.Hoverbike)
            {
                return this.Component.GetComponent<WorldEntityModel.Hoverbike>().LiveMixin;
            }
            else if (this.Component is WorldEntityModel.SpyPenguin)
            {
                return this.Component.GetComponent<WorldEntityModel.SpyPenguin>().LiveMixin;
            }
            else if (this.Component is WorldEntityModel.MapRoomCamera)
            {
                return this.Component.GetComponent<WorldEntityModel.MapRoomCamera>().LiveMixin;
            }
            else if (this.Component is WorldEntityModel.SeaTruckAquariumModule)
            {
                return this.Component.GetComponent<WorldEntityModel.SeaTruckAquariumModule>().LiveMixin;
            }
            else if (this.Component is WorldEntityModel.SeaTruckDockingModule)
            {
                return this.Component.GetComponent<WorldEntityModel.SeaTruckDockingModule>().LiveMixin;
            }
            else if (this.Component is WorldEntityModel.SeaTruckFabricatorModule)
            {
                return this.Component.GetComponent<WorldEntityModel.SeaTruckFabricatorModule>().LiveMixin;
            }
            else if (this.Component is WorldEntityModel.SeaTruckSleeperModule)
            {
                return this.Component.GetComponent<WorldEntityModel.SeaTruckSleeperModule>().LiveMixin;
            }
            else if (this.Component is WorldEntityModel.SeaTruckStorageModule)
            {
                return this.Component.GetComponent<WorldEntityModel.SeaTruckStorageModule>().LiveMixin;
            }
            else if (this.Component is WorldEntityModel.SeaTruckTeleportationModule)
            {
                return this.Component.GetComponent<WorldEntityModel.SeaTruckTeleportationModule>().LiveMixin;
            }

            return null;
        }

        /**
         *
         * Bana ait olup olmadığını döner.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public bool IsMine(string playerId)
        {
            return this.OwnershipId == playerId;
        }

        /**
         *
         * Ebeveyni olup olmadığını döner.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public bool IsParentExist()
        {
            return !string.IsNullOrEmpty(this.ParentId);
        }

        /**
         *
         * Üst nesne değerini değiştirir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void SetParent(string parentId)
        {
            this.ParentId = parentId;
        }

        /**
         *
         * IsDeployed değerini değiştirir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void SetDeployed(bool isDeployed)
        {
            this.IsDeployed = isDeployed;
        }

        /**
         *
         * Position değerini değiştirir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void SetPosition(ZeroVector3 position)
        {
            this.Position = position;
        }

        /**
         *
         * Position ve Rotation değerini değiştirir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void SetPositionAndRotation(ZeroVector3 position, ZeroQuaternion rotation)
        {
            this.Position = position;
            this.Rotation = rotation;
        }

        /**
         *
         * Sahibi Değiştirir
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void SetOwnership(string ownershipId)
        {
            this.OwnershipId = ownershipId;
        }

        /**
         *
         * Kinematic durumunu önbelleğe alır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void CacheKinematicStatus()
        {
            if (this.Rigidbody)
            {
                this.KinematicState = this.Rigidbody.isKinematic ? ZeroKinematicState.Kinematic : ZeroKinematicState.NonKinematic;
            }
        }

        /**
         *
         * Component değerini değiştirir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public WorldDynamicEntity SetComponent(NetworkDynamicEntityComponent component)
        {
            this.Component = component;
            return this;
        }

        /**
         *
         * Nesne görünür mü?
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public bool IsVisible(ZeroVector3 playerPosition)
        {
            return this.Position.Distance(playerPosition) < Network.DynamicEntity.VisibilityDistance;
        }

        /**
         *
         * Fizik simülasyonu yapılabilir mi?
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public bool IsPhysicSimulateable(ZeroVector3 playerPosition)
        {
            return this.Position.Distance(playerPosition) < Network.DynamicEntity.PhysicsDistance;
        }

        /**
         *
         * GameObject'i günceller.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void UpdateGameObject()
        {
            if (this.GameObject == null)
            {
                this.GameObject = Network.Identifier.GetGameObject(this.UniqueId, true);

                if (this.GameObject)
                {
                    this.Rigidbody = this.GameObject.GetComponent<Rigidbody>();
                }
            }

            if (this.GameObject == null)
            {
                Log.Error("NOT FOUND Entity ==> " + this.TechType + ", UniqueId: " + this.UniqueId);
            }
        }

        /**
         *
         * Nesneye enterpolasyon yapar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void Interpolate()
        {
            this.UpdateGameObject();

            if (this.GameObject)
            {
                if (this.Position != this.LastPosition)
                {
                    this.LastPosition    = this.Position;
                    this.CurrentPosition = this.Position.ToVector3();
                }

                if (this.Rotation != this.LastRotation)
                {
                    this.LastRotation    = this.Rotation;
                    this.CurrentRotation = this.Rotation.ToQuaternion();
                }

                if (ZeroVector3.Distance(this.GameObject.transform.position, this.CurrentPosition) > this.TeleportDistance)
                {
                    this.GameObject.transform.position = this.CurrentPosition;
                    this.GameObject.transform.rotation = this.CurrentRotation;
                }
                else
                {
                    this.GameObject.transform.position = Vector3.SmoothDamp(this.GameObject.transform.position, this.CurrentPosition, ref this.Velocity, 0.3f);
                    this.GameObject.transform.rotation = BroadcastInterval.QuaternionSmoothDamp(this.GameObject.transform.rotation, this.CurrentRotation, ref this.RotationVelocity, 0.3f);
                }
            }
        }
    }

    [MessagePackObject]
    public class WorldDynamicEntityPosition
    {
        /**
         *
         * Yapı Kimliği değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(0)]
        public ushort Id { get; set; }

        /**
         *
         * Position değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(1)]
        public long Position { get; set; }

        /**
         *
         * Rotation değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(2)]
        public long Rotation { get; set; }
    }
}