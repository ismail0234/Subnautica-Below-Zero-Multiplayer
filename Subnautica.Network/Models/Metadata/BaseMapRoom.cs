namespace Subnautica.Network.Models.Metadata
{
    using System.Collections.Generic;

    using MessagePack;

    using Subnautica.Network.Core.Components;
    using Subnautica.Network.Models.Storage.World.Childrens;
    using Subnautica.Network.Models.WorldEntity.DynamicEntityComponents.Shared;
    using Subnautica.Network.Structures;

    [MessagePackObject]
    public class BaseMapRoom : MetadataComponent
    {
        /**
         *
         * ScanTechType değerini barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(0)]
        public TechType ScanTechType { get; set; }

        /**
         *
         * LastScanDate değerini barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(1)]
        public float LastScanDate { get; set; }

        /**
         *
         * StorageContainer değerini barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(2)]
        public StorageContainer StorageContainer { get; set; }

        /**
         *
         * Crafter değerini barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(3)]
        public Crafter Crafter { get; set; }

        /**
         *
         * ProcessType değerini barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(4)]
        public byte ProcessType { get; set; }

        /**
         *
         * PickupItem değerini barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(5)]
        public WorldPickupItem PickupItem { get; set; }

        /**
         *
         * ResourceNodes değerini barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(6)]
        public HashSet<string> ResourceNodes { get; set; } = new HashSet<string>();

        /**
         *
         * LeftDock Değerini barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(7)]
        public VehicleDockingBayItem LeftDock { get; set; } = new VehicleDockingBayItem();

        /**
         *
         * RightDock Değerini barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(8)]
        public VehicleDockingBayItem RightDock { get; set; } = new VehicleDockingBayItem();

        /**
         *
         * IsNextCamera Değerini barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(9)]
        public bool IsNextCamera { get; set; }

        /**
         *
         * IsChanged değerini barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [IgnoreMember]
        public bool IsChanged { get; set; } = false;

        /**
         *
         * Son tarama zamanını değiştirir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public bool SetLastScanDate(float lastScanDate)
        {
            this.LastScanDate = lastScanDate;
            return true;
        }

        /**
         *
         * Bulunan kaynakları temizler.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public bool ResetNodes()
        {
            if (this.ResourceNodes.Count <= 0)
            {
                return false;
            }

            this.ResourceNodes.Clear();
            return true;
        }

        /**
         *
         * Yeni kaynak ekler.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void AddResourceNode(string itemId)
        {
            this.ResourceNodes.Add(itemId);
        }

        /**
         *
         * Taramayı durumu.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public bool IsScanning()
        {
            return this.ScanTechType != TechType.None;
        }

        /**
         *
         * Taramayı başlatır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public bool StartScan(TechType techType)
        {
            if (this.ScanTechType != TechType.None)
            {
                return false;
            }

            this.IsChanged    = true;
            this.ScanTechType = techType;
            this.ResetNodes();
            return true;
        }

        /**
         *
         * Taramayı durdurur.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public bool StopScan()
        {
            if (this.ScanTechType == TechType.None)
            {
                return false;
            }

            this.IsChanged    = true;
            this.ScanTechType = TechType.None;
            this.ResetNodes();
            return true;
        }

        /**
         *
         * Kamerayı demirler.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public bool Dock(WorldDynamicEntity vehicle, bool isLeft, ZeroVector3 endPosition, ZeroQuaternion endRotation, float currentTime)
        {
            if (isLeft)
            {
                return this.LeftDock.Dock(vehicle, endPosition, endRotation, currentTime);
            }

            return this.RightDock.Dock(vehicle, endPosition, endRotation, currentTime);
        }

        /**
         *
         * Kamerayı ayırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public bool Undock(string vehicleId, float currentTime, out WorldDynamicEntity vehicle)
        {
            if (this.LeftDock.VehicleId == vehicleId)
            {
                return this.LeftDock.Undock(currentTime, out vehicle);
            }
            
            if (this.RightDock.VehicleId == vehicleId)
            {
                return this.RightDock.Undock(currentTime, out vehicle);
            }

            vehicle = null;
            return false;
        }
    }

    [MessagePackObject]
    public class VehicleDockingBayItem
    {
        /**
         *
         * IsDocked Değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(0)]
        public bool IsDocked { get; set; }

        /**
         *
         * LastDockTime Değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(1)]
        public float LastDockTime { get; set; }

        /**
         *
         * VehicleId Değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(2)]
        public string VehicleId { get; set; }

        /**
         *
         * Position Değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(3)]
        public ZeroVector3 Position { get; set; }

        /**
         *
         * Rotation Değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(4)]
        public ZeroQuaternion Rotation { get; set; }

        /**
         *
         * Vehicle Değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(5)]
        public WorldDynamicEntity Vehicle { get; set; }

        /**
         *
         * Sınıf ayarlamalarını yapar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public VehicleDockingBayItem()
        {

        }

        /**
         *
         * Sınıf ayarlamalarını yapar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public VehicleDockingBayItem(bool isDocked, float starTime, string vehicleId, ZeroVector3 position, ZeroQuaternion rotation, WorldDynamicEntity vehicle)
        {
            this.IsDocked     = isDocked;
            this.LastDockTime = starTime;
            this.VehicleId    = vehicleId;
            this.Position     = position;
            this.Rotation     = rotation;
            this.Vehicle      = vehicle;
        }

        /**
         *
         * DockTime değerini değiştirir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void SetLastDockTime(float dockTime)
        {
            this.LastDockTime = dockTime;
        }

        /**
         *
         * Aracı rıhtıma kenetler.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public bool Dock(WorldDynamicEntity vehicle, ZeroVector3 endPosition, ZeroQuaternion endRotation, float currentTime)
        {
            if (this.IsDocked)
            {
                return false;
            }

            if (this.LastDockTime + 2f >= currentTime)
            {
                return false;
            }
            
            this.IsDocked  = true;
            this.VehicleId = vehicle.UniqueId;
            this.Vehicle   = vehicle;
            this.Vehicle.Position = endPosition;
            this.Vehicle.Rotation = endRotation;
            this.LastDockTime     = currentTime;
            return true;
        }

        /**
         *
         * Aracın kenetlenmesini kaldırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public bool Undock(float currentTime, out WorldDynamicEntity vehicle)
        {
            vehicle = this.Vehicle;

            if (this.IsDocked)
            {
                this.IsDocked  = false;
                this.VehicleId = null;
                this.Vehicle   = null;
                this.SetLastDockTime(currentTime);
                return true;
            }

            return false;
        }
    }
}
