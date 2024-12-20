namespace Subnautica.Server.Storage
{
    using System;
    using System.IO;
    using System.Linq;

    using Subnautica.API.Extensions;
    using Subnautica.API.Features;
    using Subnautica.Network.Core;
    using Subnautica.Network.Core.Components;
    using Subnautica.Network.Structures;
    using Subnautica.Server.Abstracts;

    using ConstructionStorage = Network.Models.Storage.Construction;
    using MetadataModel       = Subnautica.Network.Models.Metadata;

    public class Construction : BaseStorage
    {
        /**
         *
         * Dünya sınıfını barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public ConstructionStorage.Construction Storage { get; set; }

        /**
         *
         * Sınıf ayarlamalarını yapar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public override void Start(string serverId)
        {
            this.ServerId = serverId;
            this.FilePath = Paths.GetMultiplayerServerSavePath(this.ServerId, "Construction.bin");
            this.InitializePath();
            this.Load();
        }

        /**
         *
         * Sunucu dünya verilerini belleğe yükler.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public override void Load()
        {
            if (File.Exists(this.FilePath))
            {
                lock (this.ProcessLock)
                {
                    try
                    {
                        this.Storage = NetworkTools.Deserialize<ConstructionStorage.Construction>(File.ReadAllBytes(this.FilePath));
                    }
                    catch (Exception e)
                    {
                        Log.Error($"Construction.Load: {e}");
                    }
                }
            }
            else
            {
                this.Storage = new ConstructionStorage.Construction();
                this.SaveToDisk();
            }

            if (Core.Server.DEBUG)
            {
                Log.Info($"Construction Details: {this.Storage.Constructions.Count}");
                Log.Info("---------------------------------------------------------------");
                foreach (var item in this.Storage.Constructions)
                {
                    Log.Info(string.Format("TechType: {0}, IsBasePiece: {1}, ConstructedAmount: {2}, UniqueId: {3}", item.Value.TechType, item.Value.IsBasePiece, item.Value.ConstructedAmount, item.Value.UniqueId));
                }
                Log.Info("---------------------------------------------------------------");
            }
        }        

        /**
         *
         * Verileri diske yazar
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public override void SaveToDisk()
        {
            lock (this.ProcessLock)
            {
                this.WriteToDisk(this.Storage);
            }
        }

        /**
         *
         * Bir yapının kaldırılabilirlik durumuna bakar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public bool IsDeconstructable(string uniqueId)
        {
            var construction = this.GetConstruction(uniqueId);
            if (construction == null)
            {
                return false;
            }

            if (construction.TechType == TechType.BaseMapRoom)
            {
                foreach (var item in Server.Core.Server.Instance.GetPlayers())
                {
                    if (item.UsingRoomId == uniqueId)
                    {
                        return false;
                    }
                }

                var component = construction.Component.GetComponent<MetadataModel.BaseMapRoom>();
                if (component != null)
                {
                    if (component.LeftDock.IsDocked || component.RightDock.IsDocked)
                    {
                        return false;
                    }
                }
            }
            else if (construction.TechType == TechType.BaseMoonpool || construction.TechType == TechType.BaseMoonpoolExpansion)
            {
                var component = construction.Component.GetComponent<MetadataModel.BaseMoonpool>();
                if (component != null && component.IsDocked)
                {
                    return false;
                }
            }

            return true;
        }

        /**
         *
         * Yapı detaylarını döner.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public ConstructionStorage.ConstructionItem GetConstruction(string uniqueId)
        {
            if (uniqueId != null && this.Storage.Constructions.TryGetValue(uniqueId, out var construction))
            {
                return construction;
            }

            return null;
        }

        /**
         *
         * Yapı detaylarını ekler.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public bool AddConstructionItem(ConstructionStorage.ConstructionItem construction)
        {
            lock (this.ProcessLock)
            {
                this.Storage.Constructions[construction.UniqueId] = construction;
                return true;
            }
        }

        /**
         *
         * Yapı tamamlanma oranını günceller.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public bool UpdateConstructionAmount(string uniqueId, float constructedAmount)
        {
            lock (this.ProcessLock)
            {
                if (this.Storage.Constructions.ContainsKey(uniqueId))
                {
                    this.Storage.Constructions[uniqueId].ConstructedAmount = constructedAmount;
                    return true;
                }

                return false;
            }
        }

        /**
         *
         * Metadata verisini günceller.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public bool UpdateMetadata(string uniqueId, MetadataComponent component)
        {
            lock (this.ProcessLock)
            {
                if (this.Storage.Constructions.ContainsKey(uniqueId))
                {
                    this.Storage.Constructions[uniqueId].Component = component;
                    return true;
                }

                return false;
            }
        }

        /**
         *
         * Yapı'yı sözlükten kaldırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public bool ConstructionRemove(string uniqueId, ZeroInt3 cell = null)
        {
            lock (this.ProcessLock)
            {
                if (!this.Storage.Constructions.ContainsKey(uniqueId))
                {
                    return false;
                }

                var constructionId = this.Storage.Constructions[uniqueId].Id;
                var baseId         = this.Storage.Constructions[uniqueId].BaseId;

                if (this.Storage.Constructions.Remove(uniqueId))
                {
                    if (baseId.IsNotNull())
                    {
                        if (Core.Server.Instance.Storages.World.TryGetBase(baseId, out var baseComponent))
                        {
                            baseComponent.RemoveConstruction(uniqueId, cell);

                            if (!this.Storage.Constructions.Values.Any(q => q.BaseId == baseId))
                            {
                                Log.Info("removed --> xxx -> " + baseId);
                                Core.Server.Instance.Storages.World.RemoveBase(baseComponent);
                            }
                            else
                            {
                                Log.Info("not removed --> xxx -> " + this.Storage.Constructions.Values.Count(q => q.BaseId == baseId));
                            }
                        }
                    }

                    Core.Server.Instance.Logices.EnergyTransmission.OnConstructionRemove(constructionId, uniqueId);
                    return true;
                }

                return false;
            }
        }

        /**
         *
         * Yapı'yı tamamlar
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public bool ConstructionComplete(string uniqueId, string baseId, ZeroVector3 cellPosition, bool isFaceHasValue, ZeroVector3 localPosition, ZeroQuaternion localRotation, Base.Direction faceDirection = Base.Direction.North, Base.FaceType faceType = Base.FaceType.None)
        {
            lock (this.ProcessLock)
            {
                if (this.Storage.Constructions.TryGetValue(uniqueId, out var construction))
                {
                    Log.Info("NEW BASE ID: " + baseId + ", current: " + construction.BaseId + ", uniqueId: " + uniqueId);
                    if (construction.BaseId.IsNull())
                    {
                        construction.BaseId = baseId;
                    }

                    construction.CellPosition      = cellPosition;
                    construction.IsFaceHasValue    = isFaceHasValue;
                    construction.LocalPosition     = localPosition;
                    construction.LocalRotation     = localRotation;
                    construction.FaceDirection     = faceDirection;
                    construction.FaceType          = faceType;
                    construction.ConstructedAmount = 1f;

                    Core.Server.Instance.Logices.EnergyTransmission.OnConstructionComplete(construction);

                    if (construction.BaseId.IsNotNull())
                    {
                        Core.Server.Instance.Storages.World.TryGetBase(construction.BaseId, out var baseComponent);
                    }

                    return true;
                }

                return false;
            }
        }
    }
}