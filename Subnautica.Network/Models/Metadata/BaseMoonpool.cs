namespace Subnautica.Network.Models.Metadata
{
    using MessagePack;

    using Subnautica.API.Features;
    using Subnautica.Network.Core.Components;
    using Subnautica.Network.Models.Storage.World.Childrens;
    using Subnautica.Network.Structures;

    [MessagePackObject]
    public class BaseMoonpool : MetadataComponent
    {
        /**
         *
         * IsDocking Değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(0)]
        public bool IsDocking { get; set; }

        /**
         *
         * IsUndocking Değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(1)]
        public bool IsUndocking { get; set; }

        /**
         *
         * IsUndockingLeft Değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(2)]
        public bool IsUndockingLeft { get; set; }

        /**
         *
         * IsCustomizerOpening değerini barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(3)]
        public bool IsCustomizerOpening { get; set; }

        /**
         *
         * IsDocked Değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(4)]
        public bool IsDocked { get; set; }

        /**
         *
         * DockingStartTime Değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(5)]
        public double DockingStartTime { get; set; }

        /**
         *
         * VehicleId Değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(6)]
        public string VehicleId { get; set; }

        /**
         *
         * EndPosition Değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(7)]
        public ZeroVector3 EndPosition { get; set; }

        /**
         *
         * EndRotation Değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(8)]
        public ZeroQuaternion EndRotation { get; set; }

        /**
         *
         * Vehicle Değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(9)]
        public WorldDynamicEntity Vehicle { get; set; }

        /**
         *
         * ColorCustomizer değerini barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(10)]
        public ZeroColorCustomizer ColorCustomizer { get; set; }

        /**
         *
         * BackModulePosition Değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(11)]
        public ZeroVector3 BackModulePosition { get; set; }

        /**
         *
         * ExpansionManager Değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(12)]
        public BaseMoonpoolExpansionManager ExpansionManager { get; set; } = new BaseMoonpoolExpansionManager();

        /**
         *
         * Aracı rıhtıma kenetler.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public bool Dock(WorldDynamicEntity entity, ZeroVector3 endPosition, ZeroQuaternion endRotation, double currentTime)
        {
            if (this.IsDocked)
            {
                return false;
            }
            
            this.IsDocked  = true;
            this.VehicleId = entity.UniqueId;
            this.Vehicle   = entity;
            this.Vehicle.Position = endPosition;
            this.Vehicle.Rotation = endRotation;
            this.DockingStartTime = currentTime;
            return true;
        }

        /**
         *
         * Aracın kenetlenmesini kaldırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public bool Undock(out WorldDynamicEntity vehicle)
        {
            vehicle = this.Vehicle;

            if (this.IsDocked)
            {
                this.IsDocked  = false;
                this.VehicleId = null;
                this.Vehicle   = null;
                this.DockingStartTime = 0;
                return true;
            }

            return false;
        }
    }
}