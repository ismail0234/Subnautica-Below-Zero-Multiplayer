namespace Subnautica.Client.Multiplayer.Constructing
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    using FMODUnity;

    using Subnautica.API.Enums;
    using Subnautica.API.Extensions;
    using Subnautica.API.Features;
    using Subnautica.API.MonoBehaviours;
    using Subnautica.Client.Extensions;
    using Subnautica.Client.MonoBehaviours;
    using Subnautica.Client.MonoBehaviours.Construction;
    using Subnautica.Client.Multiplayer.Constructing.BaseGhostExtensions;
    using Subnautica.Network.Models.Construction;
    using Subnautica.Network.Models.Construction.Shared;
    using Subnautica.Network.Structures;

    using UnityEngine;

    using UWE;
    
    public class Builder
    {
        /**
         *
         * İnşaa sınıfı oluşturur.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static Builder CreateBuilder(string uniqueId, TechType techType)
        {
            Builder builder = GetBuilder(uniqueId);
            if (builder == null)
            {
                builder = new Builder(techType, uniqueId);
            }

            return builder;
        }

        /**
         *
         * İnşaa edilecek yapı döner.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static Builder GetBuilder(string uniqueId)
        {
            if (uniqueId == null)
            {
                return null;
            }

            if (Constructing.TryGetValue(uniqueId, out var builder))
            {
                return builder;
            }

            return null;
        }

        /**
         *
         * İnşaa edilecek yapı ekler.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void AddBuilder(string uniqueId, Builder builder)
        {
            if (!Constructing.ContainsKey(uniqueId))
            {
                Constructing.Add(uniqueId, builder);
            }
        }

        /**
         *
         * İnşaa edilecek yapı kaldırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static BuildingProgressType GetBuildingProgressType(string uniqueId)
        {
            var builder = GetBuilder(uniqueId);
            if (builder == null)
            {
                return BuildingProgressType.None;
            }

            return builder.CurrentProgress;
        }

        /**
         *
         * Yapıyı kaldırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static bool Destroy(string uniqueId, bool isRemove = true, bool callSound = false)
        {
            Builder builder = GetBuilder(uniqueId);
            if (builder == null)
            {
                return false;
            }

            builder.IsActive = false;
            builder.CurrentProgress = BuildingProgressType.Removed;

            if (builder.AimTransformGameObject != null)
            {
                UnityEngine.Object.Destroy(builder.AimTransformGameObject);
            }

            if (builder.BuilderNormalAnimation != null)
            {
                UnityEngine.Object.Destroy(builder.BuilderNormalAnimation);
            }

            if (builder.GhostModel != null)
            {
                builder.GhostModel.SetActive(false);

                var constructableBase = builder.GhostModel.GetComponentInParent<ConstructableBase>();
                if (constructableBase != null)
                {
                    UnityEngine.Object.Destroy(constructableBase.gameObject);
                }

                UnityEngine.Object.Destroy(builder.GhostModel);
            }

            if (callSound)
            {
                if (builder.ConstructableBase != null)
                {
                    global::Utils.PlayFMODAsset(World.DeconstructCompleteSound, builder.ConstructableBase.transform.position);
                }
                else if (builder.Constructable != null)
                {
                    global::Utils.PlayFMODAsset(World.DeconstructCompleteSound, builder.Constructable.transform.position);
                }
            }

            if (isRemove)
            {
                if (builder.Constructable != null)
                {
                    builder.Constructable.constructedAmount = 0f;
                    builder.Constructable.UpdateMaterial();
                }

                if (builder.ConstructableBase != null && builder.ConstructableBase.model != null)
                {
                    var component  = builder.ConstructableBase.model.GetComponent<global::BaseGhost>();
                    var targetBase = component != null ? component.TargetBase : null;

                    builder.ConstructableBase.SetModuleConstructAmount(targetBase, builder.ConstructableBase.amount);

                    if (targetBase != null)
                    {
                        targetBase.DestroyIfEmpty(component);
                    }
                }

                if (builder.Constructable != null)
                {
                    UnityEngine.Object.Destroy(builder.Constructable.gameObject);
                }

                if (builder.ConstructableBase != null)
                {
                    UnityEngine.Object.Destroy(builder.ConstructableBase.gameObject);
                }
            }

            Resources.UnloadAsset(builder.GhostStructureMaterial);

            builder.IsCanPlace             = false;
            builder.TechType               = TechType.None;
            builder.Prefab                 = null;
            builder.GhostModel             = null;
            builder.Constructable          = null;
            builder.AimTransformGameObject = null;
            builder.GhostStructureMaterial = null;

            Constructing.Remove(uniqueId);

            Log.Error("Destroyed Construction: " + uniqueId);
            return true;
        }
     
        /**
         *
         * Yapı kurucusunu serilize eder.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static byte[] SerializeGlobalRoot(bool isOptimize = true)
        {
            try
            {
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    using (PooledObject<ProtobufSerializer> proxy = ProtobufSerializerPool.GetProxy())
                    {
                        proxy.Value.SetConstructionModeActive(isOptimize);
                        proxy.Value.SerializeObjectTree(memoryStream, LargeWorldStreamer.main.globalRoot);
                        proxy.Value.SetConstructionModeActive(false);

                        return memoryStream.ToArray();
                    }
                }
            }
            catch (Exception e)
            {
                Log.Error($"Builder.SerializeGlobalRoot Exception: {e}");
                return null;
            }
        }

        /**
         *
         * Yapıları yükler.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static IEnumerator LoadConstructions(byte[] constructionData)
        {
            if (constructionData == null || constructionData.Length <= 0)
            {
                GameObject globalRoot = new GameObject("Global Root");
                globalRoot.AddComponent<StoreInformationIdentifier>();
 
                LargeWorldStreamer.main.OnGlobalRootLoaded(globalRoot);
            }
            else
            {
                LargeWorldStreamer.main.UnloadGlobalRoot();

                using (MemoryStream memoryStream = new MemoryStream(constructionData))
                {
                    using (PooledObject<ProtobufSerializer> proxy = ProtobufSerializerPool.GetProxy())
                    {
                        CoroutineTask<GameObject> task = proxy.Value.DeserializeObjectTreeAsync(memoryStream, true, false, 0);

                        yield return task;
          
                        try
                        {
                            LargeWorldStreamer.main.OnGlobalRootLoaded(task.GetResult());
                        }
                        catch (Exception e)
                        {
                            Log.Error($"Builder.LoadConstructions Exception: {e}");
                        }
                    }
                }
            }
        }

        /**
         *
         * Belirtilen konumda hata sesini çalar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void CallErrorSound(Vector3 position)
        {
            RuntimeManager.PlayOneShot("event:/bz/ui/item_error", position);
        }

        /**
         *
         * Belirtilen konumda başarılı sesini çalar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */        
        public static void CallSuccessSound(Vector3 position)
        {
            RuntimeManager.PlayOneShot("event:/tools/builder/place", position);
        }

        /**
         *
         * Sınıf ayarlamalarını yapar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public Builder(TechType techType, string uniqueId)
        {
            this.CurrentProgress = BuildingProgressType.Initializing;
            this.TechType        = techType;
            this.UniqueId        = uniqueId;
            this.LastRotation    = 0;
            this.OldLastRotation = 0;

            this.AimTransformGameObject = new GameObject(); 
            this.GhostStructureMaterial = new Material(global::Builder.originalGhostStructureMaterial);
            
            AddBuilder(uniqueId, this);
        }

        /**
         *
         * SubrootId değerini değiştirir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void SetSubRootId(string subrootId)
        {
            this.SubRootId = subrootId;
        }

        /**
         *
         * Yapı koordinatını değiştirir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void SetPosition(Vector3 position)
        {
            this.PlacePosition = position;
        }

        /**
         *
         * Yapı açısını değiştirir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void SetRotation(Quaternion rotation)
        {
            this.PlaceRotation = rotation;
        }

        /**
         *
         * Son güncelleme zamanını değiştirir
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void SetUpdatedTime(float time)
        {
            this.UpdatedTime = time;
        }

        /**
         *
         * Son açıyı değiştirir
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void SetLastRotation(int lastRotation)
        {
            this.LastRotation = lastRotation;

            if (this.GhostModel && this.GhostModel.TryGetComponent<BaseGhostRotationComponent>(out var component))
            {
                component.SetLastRotation(lastRotation);
            }
        }

        /**
         *
         * BaseGhostComponent değerini değiştirir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void SetBaseGhostComponent(BaseGhostComponent ghostComponent)
        {
            this.BaseGhostComponent = ghostComponent;
        }

        /**
         *
         * Yapı inşaa edilebilirlik durumunu değiştirir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void SetIsCanPlace(bool isCanPlace)
        {
            this.IsCanPlace = isCanPlace;
        }

        /**
         *
         * Yapı inşaa edilebilirlik durumunu değiştirir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void SetUpdatePlacement(bool updatePlacement)
        {
            this.UpdatePlacement = updatePlacement;
        }

        /**
         *
         * Animasyon çalışma durumunu değiştirir
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void SetIsGhostModelAnimation(bool isAnimation)
        {
            this.IsGhostModelAnimation = isAnimation;
        }

        /**
         *
         * Animasyon çalışma durumunu değiştirir
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void SetIsAmountChangedAnimation(bool isAnimation)
        {
            this.IsAmountChangedAnimation = isAnimation;
        }

        /**
         *
         * Varsayılan kurulum durumunu değiştirir
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void SetIsTryDefaultPlace(bool isTryDefaultPlace)
        {
            this.IsTryDefaultPlace = isTryDefaultPlace;
        }

        /**
         *
         * Varsayılan kurulum durumunu değiştirir
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void SetAimTransform(ZeroTransform aimTransform)
        {
            this.AimTransformGameObject.transform.forward  = aimTransform.Forward.ToVector3();
            this.AimTransformGameObject.transform.position = aimTransform.Position.ToVector3();
            this.AimTransformGameObject.transform.rotation = aimTransform.Rotation.ToQuaternion();
        }

        /**
         *
         * Tamamlanma oranını değiştirir
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void SetConstructedAmount(float amount, bool isFastUpdate = false)
        {
            if (isFastUpdate)
            {
                if (this.Constructable)
                {
                    this.Constructable.constructedAmount = amount;
                    this.Constructable.UpdateMaterial();
                }
            }
            else if (this.BuilderNormalAnimation != null)
            {
                this.BuilderNormalAnimation.SetTargetConstructedAmount(amount);
            }
        }

        /**
         *
         * Yapının hayalet model inşaasını başlatır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void StartBuild(Action onCallback)
        {
            CoroutineHost.StartCoroutine(this.CreateSubBuild(onCallback));
        }

        /**
         *
         * Yapıyı tamamlar
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void Complete(string baseId, uint constructionId, bool callSound = false)
        {
            this.CurrentProgress = BuildingProgressType.Completed;

            if (this.ConstructableBase != null)
            {
                this.ConstructableBase.SetState(true, true);
            }
            else if (this.Constructable != null)
            {
                this.Constructable.SetState(true, true);
            }

            if (callSound)
            {
                if (this.ConstructableBase != null)
                {
                    global::Utils.PlayFMODAsset(World.ConstructionCompleteSound, this.ConstructableBase.transform.position);
                }
                else if (this.Constructable != null)
                {
                    global::Utils.PlayFMODAsset(World.ConstructionCompleteSound, this.Constructable.transform.position);
                }
            }

            if (baseId.IsNotNull())
            {
                if (UniqueIdentifier.TryGetIdentifier(this.UniqueId, out var uniqueIdentifier))
                {
                    var baseComponent = uniqueIdentifier.GetComponentInParent<Base>();
                    if (baseComponent != null)
                    {
                        if (Network.Identifier.GetIdentityId(baseComponent.gameObject) != baseId)
                        {
                            Log.Info("SET NEW BASE ID: " + baseId);
                            Network.Identifier.SetIdentityId(baseComponent.gameObject, baseId);
                        }
                    }
                }
            }

            Builder.AddBuilding(constructionId, this.UniqueId);

            this.Destroy(false);
        }

        /**
         *
         * Yapı verilerini temizler ve kaldırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void Destroy(bool isRemoved = true)
        {
            Destroy(this.UniqueId, isRemoved);
        }

        /**
         *
         * Yapı yıkma işlemini başlatır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static bool Deconstruct(string uniqueId, uint constructionId, bool isFurniture = false)
        {
            Builder.RemoveBuilding(constructionId);

            if (isFurniture)
            {
                return DeconstructFurniture(uniqueId);
            }

            return DeconstructBasePiece(uniqueId);
        }

        /**
         *
         * İç Mobilya yıkma işlemini başlatır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private static bool DeconstructFurniture(string uniqueId)
        {
            if (!UniqueIdentifier.TryGetIdentifier(uniqueId, out var uid))
            {
                return false;
            }

            var constructable = uid.GetComponent<Constructable>();
            if (constructable == null)
            {
                return false;
            }

            constructable.SetState(false, false);

            var builder = CreateBuilder(uniqueId, constructable.techType);
            builder.InitializeConstructionMode(null, constructable);

            return true;
        }

        /**
         *
         * İç Yapı yıkma işlemini başlatır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private static bool DeconstructBasePiece(string uniqueId)
        {
            if (!UniqueIdentifier.TryGetIdentifier(uniqueId, out var uid))
            {
                return false;
            }

            var componentInParent = uid.GetComponentInParent<Base>();
            if (componentInParent == null)
            {
                return false;
            }

            var baseDeconstructable = uid.GetComponentInParent<BaseDeconstructable>();
            if (baseDeconstructable == null)
            {
                return false;
            }

            try
            {
                var world             = componentInParent.GridToWorld(baseDeconstructable.bounds.mins);
                var gameObject        = UnityEngine.Object.Instantiate<GameObject>(BaseDeconstructable.baseDeconstructablePrefab, world, componentInParent.transform.rotation);
                var constructableBase = gameObject.GetComponent<ConstructableBase>();
                var constructable     = gameObject.GetComponent<Constructable>();
                var baseGhost         = constructableBase.model.GetComponent<global::BaseGhost>();

                var builder = CreateBuilder(uniqueId, baseDeconstructable.recipe);
                builder.InitializeConstructionMode(constructableBase, constructable);

                Network.Identifier.CopyToUniqueIdentifier(baseDeconstructable.gameObject, constructableBase.gameObject);

                baseGhost.Deconstruct(componentInParent, baseDeconstructable.bounds, baseDeconstructable.face, baseDeconstructable.faceType);
                baseGhost.GhostBase.isPlaced = true;
                gameObject.transform.position = componentInParent.GridToWorld(baseGhost.TargetOffset);
                constructableBase.techType = baseDeconstructable.recipe;
                constructableBase.SetState(false, false);

                if (baseDeconstructable.face.HasValue)
                {
                    Log.Info("DeconstructBasePiece - FACE: CELL: " + baseDeconstructable.face.Value.cell + ", direction: " + baseDeconstructable.face.Value.direction + ", type => " + baseDeconstructable.faceType + ", type => " + constructableBase.techType + ", basePosition => " + baseDeconstructable.transform.position + ", UNIQUE ID: " + Network.Identifier.GetIdentityId(baseDeconstructable.gameObject) + ", UNIQUE ID 2: " + Network.Identifier.GetIdentityId(constructableBase.gameObject) + ", UNIQUE ID 3: " + Network.Identifier.GetIdentityId(constructable.gameObject));
                    componentInParent.ClearFace(baseDeconstructable.face.Value, baseDeconstructable.faceType);
                }
                else
                {
                    Log.Info("DeconstructBasePiece - NORMAL: type => " + constructableBase.techType + ", basePosition => " + baseDeconstructable.transform.position + ", UNIQUE ID: " + Network.Identifier.GetIdentityId(baseDeconstructable.gameObject) + ", UNIQUE ID 2: " + Network.Identifier.GetIdentityId(constructableBase.gameObject) + ", UNIQUE ID 3: " + Network.Identifier.GetIdentityId(constructable.gameObject));
                    componentInParent.ClearCell(baseDeconstructable.bounds.mins);
                }

                constructableBase.LinkModule(baseDeconstructable.moduleFace);

                if (componentInParent.IsEmpty())
                {
                    componentInParent.OnPreDestroy();
                    UnityEngine.Object.Destroy(componentInParent.gameObject);
                    baseGhost.ClearTargetBase();

                    if (!LargeWorld.main)
                    {
                        return false;
                    }

                    LargeWorld.main.streamer.cellManager.RegisterEntity(constructableBase.gameObject);
                }
                else
                {
                    constructableBase.transform.parent = componentInParent.transform;
                    componentInParent.RegisterBaseGhost(baseGhost);
                    componentInParent.FixRoomFloors();
                    componentInParent.FixCorridorLinks();
                    componentInParent.RebuildGeometry();
                }

                return true;
            }
            catch (Exception ex)
            {
                Log.Error("Builder.DeconstructBasePiece Exception: " + ex);
            }

            return false;
        }

        /**
         *
         * Güncelleme işlemi
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public bool Update()
        {
            if (this.Prefab == null || !this.IsActive)
            {
                return false;
            }

            this.CreateGhost();

            if (this.UpdateAllowedTechnologies.Contains(this.TechType) || this.LastRotation != this.OldLastRotation)
            {
                this.UpdateAllowed();
            }

            Color color = this.PlaceColorDeny;
            if (this.IsCanPlace)
            {
                color = this.PlaceColorAllow;
            }

            foreach (IBuilderGhostModel component in this.GhostModel.GetComponents<IBuilderGhostModel>())
            {
                component.UpdateGhostModelColor(this.IsCanPlace, ref color);
            }

            this.GhostStructureMaterial.SetColor(ShaderPropertyID._Tint, color);

            return true;
        }

        /**
         *
         * Yapının hayalet model inşaasını başlatır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public IEnumerator CreateSubBuild(Action onCallback)
        {
            CoroutineTask<GameObject> request = CraftData.GetPrefabForTechTypeAsync(this.TechType);
            yield return request;

            this.Prefab = request.GetResult();
            this.InitializeFinished();

            if (onCallback != null)
            {
                onCallback.Invoke();
            }
        }

        /**
         *
         * Başlatma tamamlandığında tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void InitializeFinished()
        {
            this.CurrentProgress = BuildingProgressType.GhostModelMoving;
            this.IsActive        = true;
            this.Update();

            if (this.IsTryDefaultPlace)
            {
                this.TryPlace();
            }
        }

        /**
         *
         * Hayalet modeli oluşturur.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public bool CreateGhost()
        {
            if (this.GhostModel != null)
            {
                return false;
            }

            var constructableBase = this.Prefab.GetComponent<ConstructableBase>();
            var constructable     = this.Prefab.GetComponent<Constructable>();

            this.IsAllowedOutside    = constructable.allowedOutside;
            this.IsAllowedInSub      = constructable.allowedInSub;
            this.IsAllowedInBase     = constructable.allowedInBase;
            this.IsAllowedUnderwater = constructable.allowedUnderwater;
            this.IsForceUpright      = constructable.forceUpright;
            this.IsAlignWithSurface  = constructable.alignWithSurface;
            this.IsRotationEnabled   = constructable.rotationEnabled;
            this.PlaceMaxDistance    = constructable.placeMaxDistance;

            if (constructableBase != null)
            {
                var gameObject = UnityEngine.Object.Instantiate<GameObject>(this.Prefab, this.PlacePosition, this.PlaceRotation, false);
                if (gameObject)
                {
                    this.GhostModel = gameObject.GetComponent<ConstructableBase>().model;

                    if (this.GhostModel.TryGetComponent<global::BaseGhost>(out var _baseGhost))
                    {
                        _baseGhost.gameObject.EnsureComponent<BaseGhostRotationComponent>().SetLastRotation(this.LastRotation);
                    }

                    gameObject.SetActive(true);

                    this.GhostModel.GetComponent<global::BaseGhost>().SetupGhost();

                    MaterialExtensions.AssignMaterial(this.GhostModel, this.GhostStructureMaterial, true);

                    this.InitBounds(this.GhostModel);
                }
            }
            else
            {
                this.GhostModel = UnityEngine.Object.Instantiate<GameObject>(constructable.model, this.PlacePosition, this.PlaceRotation);
                this.GhostModel.SetActive(true);

                foreach (UnityEngine.Object componentsInChild in this.GhostModel.GetComponentsInChildren<Collider>())
                {
                    UnityEngine.Object.Destroy(componentsInChild);
                }

                MaterialExtensions.AssignMaterial(this.GhostModel, this.GhostStructureMaterial, true);

                string poweredPrefabName = TechData.GetPoweredPrefabName(constructable.techType);
                if (!string.IsNullOrEmpty(poweredPrefabName))
                {
                    CoroutineUtils.PumpCoroutine(global::Builder.CreatePowerPreviewAsync(this.GhostModel, poweredPrefabName));
                }

                this.InitBounds(this.Prefab);
            }

            this.GhostModel.transform.position = this.PlacePosition;
            this.GhostModel.transform.rotation = this.PlaceRotation;

            if (this.IsGhostModelAnimation)
            {
                this.GhostModel.AddComponent<BuilderGhostAnimation>().Builder = this;
            }

            return true;
        }

        /**
         *
         * Geomerty işlemlerini yapar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public bool UpdateAllowed()
        {
            this.OldLastRotation = this.LastRotation;

            var component = this.GhostModel.GetComponentInParent<ConstructableBase>();
            if (component)
            {
                var response = this.UpdateGhostModel(component);
                if (response)
                {
                    MaterialExtensions.AssignMaterial(this.GhostModel, this.GhostStructureMaterial, true);
                    this.InitBounds(this.GhostModel);
                }

                return response;
            }

            return false;
        }

        /**
         *
         * Hayalet yapı modelini günceller.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public bool UpdateGhostModel(ConstructableBase constructableBase)
        {
            if (this.GhostModel == null)
            {
                return false;
            }

            var positionFound   = false;
            var geometryChanged = false;

            if (this.GhostModel.TryGetComponent<global::BaseAddFaceGhost>(out var faceGhost))
            {
                faceGhost.UpdateMultiplayerPlacement(this.UpdatePlacement, out positionFound, out geometryChanged, this.BaseGhostComponent?.GetComponent<BaseAddFaceGhostComponent>());
            }
            else if (this.GhostModel.TryGetComponent<global::BaseAddLadderGhost>(out var ladderGhost))
            {
                ladderGhost.UpdateMultiplayerPlacement(this.UpdatePlacement, out positionFound, out geometryChanged, this.BaseGhostComponent?.GetComponent<BaseAddLadderGhostComponent>());
            }
            else if (this.GhostModel.TryGetComponent<global::BaseAddBulkheadGhost>(out var bulkheadGhost))
            {
                bulkheadGhost.UpdateMultiplayerPlacement(this.UpdatePlacement, out positionFound, out geometryChanged, this.BaseGhostComponent?.GetComponent<BaseAddBulkheadGhostComponent>());
            }
            else if (this.GhostModel.TryGetComponent<global::BaseAddPartitionGhost>(out var partitionGhost))
            {
                partitionGhost.UpdateMultiplayerPlacement(this.UpdatePlacement, out positionFound, out geometryChanged, this.BaseGhostComponent?.GetComponent<BaseAddPartitionGhostComponent>());
            }
            else if (this.GhostModel.TryGetComponent<global::BaseAddPartitionDoorGhost>(out var partitionDoorGhost))
            {
                partitionDoorGhost.UpdateMultiplayerPlacement(this.UpdatePlacement, out positionFound, out geometryChanged, this.BaseGhostComponent?.GetComponent<BaseAddPartitionDoorGhostComponent>());
            }
            else if (this.GhostModel.TryGetComponent<global::BaseAddModuleGhost>(out var moduleGhost))
            {
                moduleGhost.UpdateMultiplayerPlacement(this.UpdatePlacement, out positionFound, out geometryChanged, this.BaseGhostComponent?.GetComponent<BaseAddModuleGhostComponent>());
            }
            else if (this.GhostModel.TryGetComponent<global::BaseAddCellGhost>(out var cellGhost))
            {
                cellGhost.UpdateMultiplayerPlacement(this.UpdatePlacement, out positionFound, out geometryChanged, this.BaseGhostComponent?.GetComponent<BaseAddCellGhostComponent>());
            }
            else if (this.GhostModel.TryGetComponent<global::BaseAddCorridorGhost>(out var corridorGhost))
            {
                corridorGhost.UpdateMultiplayerPlacement(this.UpdatePlacement, out positionFound, out geometryChanged, this.BaseGhostComponent?.GetComponent<BaseAddCorridorGhostComponent>());
            }
            else if (this.GhostModel.TryGetComponent<global::BaseAddConnectorGhost>(out var connectorGhost))
            {
                connectorGhost.UpdateMultiplayerPlacement(this.UpdatePlacement, out positionFound, out geometryChanged, this.BaseGhostComponent?.GetComponent<BaseAddConnectorGhostComponent>());
            }
            else if (this.GhostModel.TryGetComponent<global::BaseAddMapRoomGhost>(out var mapRoomGhost))
            {
                mapRoomGhost.UpdateMultiplayerPlacement(this.UpdatePlacement, out positionFound, out geometryChanged, this.BaseGhostComponent?.GetComponent<BaseAddMapRoomGhostComponent>());
            }
            else if (this.GhostModel.TryGetComponent<global::BaseAddWaterPark>(out var waterParkGhost))
            {
                waterParkGhost.UpdateMultiplayerPlacement(this.UpdatePlacement, out positionFound, out geometryChanged, this.BaseGhostComponent?.GetComponent<BaseAddWaterParkGhostComponent>());
            }
            else
            {
                Log.Info("Not Synced => " + this.TechType + ", UNIQ: " + this.UniqueId);
            }

            constructableBase.SetGhostVisible(!positionFound);
            return geometryChanged;
        }

        /**
         *
         * Hayalet modelin yapım aşamasını başlatır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public bool TryPlace(bool isCallSound = false)
        {
            if (this.Prefab == null || !this.IsActive)
            {
                if (isCallSound)
                {
                    CallErrorSound(this.PlacePosition);
                }

                return false;
            }

            if (isCallSound)
            {
                CallSuccessSound(this.PlacePosition);
            }

            this.IsActive = false;
            this.CurrentProgress = BuildingProgressType.Constructing;

            this.UpdateAllowed();
            this.UpdateAllowed();

            this.ConstructableBase = this.GhostModel.GetComponentInParent<ConstructableBase>();
            if (this.ConstructableBase != null)
            {
                this.ConstructableBase.transform.position = this.PlacePosition;
                this.ConstructableBase.transform.rotation = this.PlaceRotation;
            }

            this.GhostModel.transform.position = this.PlacePosition;
            this.GhostModel.transform.rotation = this.PlaceRotation;

            this.Constructable = this.GhostModel.GetComponentInParent<Constructable>();
            if (this.Constructable != null)
            {
                Network.Identifier.SetIdentityId(this.Constructable.gameObject, this.UniqueId);
            }

            if (this.ConstructableBase != null)
            {
                var component = this.GhostModel.GetComponent<global::BaseGhost>();
                component.Place();

                if (component.TargetBase != null)
                {
                    Log.Info("TryPlace TargetBase -> True -> " + this.UniqueId + ", BaseId: " + this.SubRootId + ", AIM: " + this.AimTransformGameObject.transform.position + ", Forward: " + this.AimTransformGameObject.transform.forward + ", rotation: " + this.AimTransformGameObject.transform.rotation + ", TARGET BAASE ID: " + component.TargetBase.gameObject.GetIdentityId() + ", touchCells: " + component.targetBase.touchedCells.Count);
                    this.ConstructableBase.transform.SetParent(component.TargetBase.transform, true);
                }
                else
                {
                    Log.Info("TryPlace TargetBase -> False -> " + this.UniqueId + ", BaseId: " + this.SubRootId + ", AIM: " + this.AimTransformGameObject.transform.position + ", Forward: " + this.AimTransformGameObject.transform.forward + ", rotation: " + this.AimTransformGameObject.transform.rotation);
                }

                this.InitializeConstructionMode(this.ConstructableBase, this.Constructable);

                this.ConstructableBase.SetState(false, true);
                return this.TryPlaceEnd();
            }

            GameObject target = UnityEngine.Object.Instantiate<GameObject>(this.Prefab);
            bool flag1 = false;
            bool flag2 = false;
            SubRoot currentSub = null;

            if (this.SubRootId.IsNotNull())
            {
                currentSub = Network.Identifier.GetComponentByGameObject<SubRoot>(this.SubRootId);
            }
            
            if (currentSub != null)
            {
                flag1 = currentSub.isBase;
                flag2 = currentSub.isCyclops;
                target.transform.parent = currentSub.GetModulesRoot();  
            }
            else if (this.IsAllowedOutside)
            {
                var placementTarget = this.GetPlacementTarget();
                if (placementTarget != null)
                {
                    var componentInParent2 = placementTarget.GetComponentInParent<SubRoot>();
                    if (componentInParent2 != null)
                    {
                        target.transform.parent = componentInParent2.GetModulesRoot();
                    }
                }
            }

            Transform transform = target.transform;
            transform.position = this.PlacePosition;
            transform.rotation = this.PlaceRotation;

            var constructable = target.GetComponentInParent<Constructable>();
            constructable.SetState(false);

            this.InitializeConstructionMode(null, constructable);

            Network.Identifier.SetIdentityId(constructable.gameObject, this.UniqueId);

            if (this.GhostModel != null)
            {
                UnityEngine.Object.Destroy(this.GhostModel);
            }

            constructable.SetIsInside(flag1 | flag2);
            SkyEnvironmentChanged.Send(target, currentSub);
            
            return this.TryPlaceEnd();
        }

        /**
         *
         * İnşaa modu animasyonlarını ayarlar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void InitializeConstructionMode(ConstructableBase constructableBase, Constructable constructable)
        {
            this.ConstructableBase = constructableBase;
            this.Constructable     = constructable;
            this.CurrentProgress   = BuildingProgressType.Constructing;

            if (this.IsAmountChangedAnimation)
            {
                if (this.ConstructableBase != null)
                {
                    this.BuilderNormalAnimation = this.ConstructableBase.model.GetComponent<global::BaseGhost>().gameObject.AddComponent<BuilderNormalAnimation>();
                    this.BuilderNormalAnimation.Builder = this;
                }
                else if (this.Constructable != null)
                {
                    this.BuilderNormalAnimation = this.Constructable.gameObject.AddComponent<BuilderNormalAnimation>();
                    this.BuilderNormalAnimation.Builder = this;
                }
            }
        }

        /**
         *
         * Kemikleri ayarlar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void InitBounds(GameObject gameObject)
        {
            this.CacheBounds(gameObject.transform, gameObject, this.Bounds);

            this.AaBounds.center  = Vector3.zero;
            this.AaBounds.extents = Vector3.zero;

            int count = this.Bounds.Count;
            if (count <= 0)
            {
                return;
            }

            Vector3 min = new Vector3(float.MaxValue, float.MaxValue, float.MaxValue);
            Vector3 max = new Vector3(float.MinValue, float.MinValue, float.MinValue);

            for (int index = 0; index < count; ++index)
            {
                OrientedBounds bound = this.Bounds[index];
                OrientedBounds.MinMaxBounds(OrientedBounds.TransformMatrix(bound.position, bound.rotation), Vector3.zero, bound.extents, ref min, ref max);
            }

            this.AaBounds.extents = (max - min) * 0.5f;
            this.AaBounds.center = min + this.AaBounds.extents;
        }

        /**
         *
         * Kemik önbelleğini döner.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private void CacheBounds(Transform transform, GameObject target, List<OrientedBounds> results, bool append = false)
        {
            if (!append)
            {
                results.Clear();
            }

            if (target == null)
            {
                return;
            }

            using (ListPool<ConstructableBounds> listPool = Pool<ListPool<ConstructableBounds>>.Get())
            {
                List<ConstructableBounds> list = listPool.list;
                target.GetComponentsInChildren<ConstructableBounds>(true, list);

                for (int index = 0; index < list.Count; ++index)
                {
                    ConstructableBounds constructableBounds = list[index];
                    OrientedBounds bounds      = constructableBounds.bounds;
                    OrientedBounds worldBounds = OrientedBounds.ToWorldBounds(constructableBounds.transform, bounds);
                    if (transform != null)
                    {
                        worldBounds = OrientedBounds.ToLocalBounds(transform, worldBounds);
                    }

                    results.Add(worldBounds);
                }
            }
        }

        /**
         *
         * Hedef oyun nesnesini döner.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public GameObject GetPlacementTarget()
        {
            if (!Physics.Raycast(this.AimTransformGameObject.transform.position, this.AimTransformGameObject.transform.forward, out var hitInfo, this.PlaceMaxDistance, global::Builder.placeLayerMask.value, QueryTriggerInteraction.Ignore))
            {
                return null;
            }

            if (!Constructable.CheckFlags(this.IsAllowedInBase, this.IsAllowedInSub, this.IsAllowedOutside, this.IsAllowedUnderwater, hitInfo.point))
            {
                return null;
            }

            return hitInfo.collider.gameObject;
        }

        /**
         *
         * Hayalet model verilerini temizler.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private bool TryPlaceEnd()
        {
            this.GhostModel = null;
            this.Prefab     = null;
            this.IsCanPlace = false;
            return true;
        }

        /**
         *
         * Yapıyı sözlüğe ekler.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void AddBuilding(uint constructionId, string uniqueId)
        {
            Buildings[constructionId] = uniqueId;
        }

        /**
         *
         * Yapıyı sözlükten kaldırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void RemoveBuilding(uint constructionId)
        {
            Buildings.Remove(constructionId);
        }

        /**
         *
         * Yapıyı sözlükten kaldırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static bool TryGetBuildingValue(uint constructionId, out string result)
        {
            if (Buildings.TryGetValue(constructionId, out result))
            {
                return true;
            }

            return false;
        }

        /**
         *
         * Tamamlanıp tamamlanmadığını döner.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static bool IsCompleted(string uniqueId)
        {
            if (uniqueId == null)
            {
                return false;
            }

            return Buildings.Where(q => q.Value == uniqueId).Any();
        }

        /**
         *
         * Tamamlanıp tamamlanmadığını döner.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public bool IsCompleted()
        {
            return IsCompleted(this.UniqueId);
        }

        /**
         *
         * İnşaa edilecek yapıları barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static Dictionary<string, Builder> Constructing { get; private set; } = new Dictionary<string, Builder>();

        /**
         *
         * İnşaa edilecek yapıları barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static Dictionary<uint, string> Buildings { get; private set; } = new Dictionary<uint, string>();

        /**
         *
         * Yapı Kimliği
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public string UniqueId { get; private set; }

        /**
         *
         * SubrootId Kimliği
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public string SubRootId { get; private set; }

        /**
         *
         * İnşaa Açısını barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public int LastRotation { get; private set; } = 0;

        /**
         *
         * OldLastRotation Değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public int OldLastRotation { get; private set; } = 0;

        /**
         *
         * BaseGhostComponent Değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public BaseGhostComponent BaseGhostComponent { get; private set; }

        /**
         *
         * Hayalet Model hareket Animasyon çalışma durumu
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public bool IsGhostModelAnimation { get; private set; } = true;

        /**
         *
         * Animasyon çalışma durumu
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public bool IsAmountChangedAnimation { get; private set; } = true;

        /**
         *
         * Varsayılan kurulum durumu
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public bool IsTryDefaultPlace { get; private set; } = false;

        /**
         *
         * Construction Bool değişkenleri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public bool IsActive { get; set; }                    = false;
        public bool IsCanPlace { get; private set; }          = false;
        public bool UpdatePlacement { get; private set; }     = false;
        public bool IsAllowedOutside { get; private set; }    = false;
        public bool IsAllowedInSub { get; private set; }      = false;
        public bool IsAllowedInBase { get; private set; }     = false;
        public bool IsAllowedUnderwater { get; private set; } = false;
        public bool IsForceUpright { get; private set; }      = false;
        public bool IsAlignWithSurface { get; private set; }  = false;
        public bool IsRotationEnabled { get; private set; }   = false;

        /**
         *
         * Construction Float değişkenleri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public float PlaceMaxDistance { get; private set; } = 0.0f;

        /**
         *
         * Son güncelleme zamanı
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public float UpdatedTime { get; private set; }

        /**
         *
         * İnşaa ilerleme durumunu barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public BuildingProgressType CurrentProgress { get; private set; } = BuildingProgressType.None;  

        /**
         *
         * Yapı türü
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public TechType TechType { get; private set; }

        /**
         *
         * Yapı pozisyon barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public Vector3 PlacePosition;

        /**
         *
         * Yapı açısını barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public Quaternion PlaceRotation;

        /**
         *
         * Prefabrik değerini barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public GameObject Prefab { get; set; }

        /**
         *
         * Hayalet Modeli barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public GameObject GhostModel { get; private set; }

        /**
         *
         * AimTransformGameObject barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public GameObject AimTransformGameObject { get; private set; }

        /**
         *
         * Yapı hayalet renkleri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public Color PlaceColorAllow { get; private set; } = new Color(0.0f, 1f, 0.0f, 1f);
        public Color PlaceColorDeny  { get; private set; } = new Color(1f, 0.0f, 0.0f, 1f);

        /**
         *
         * Yapı materyalleri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public Material GhostStructureMaterial { get; private set; }

        /**
         *
         * Model kemiklerini barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private Bounds AaBounds = new Bounds();

        /**
         *
         * Yapı liste değerleri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public List<OrientedBounds> Bounds { get; private set; } = new List<OrientedBounds>();        

        /**
         *
         * ConstructableBase sınıfını barınrdırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public ConstructableBase ConstructableBase { get; private set; }

        /**
         *
         * Constructable sınıfını barınrdırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public Constructable Constructable { get; private set; }

        /**
         *
         * Constructable sınıfını barınrdırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public BuilderNormalAnimation BuilderNormalAnimation { get; private set; }

        /**
         *
         * Bir yapıya bağlanıp modeli değişen teknolojiler
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public List<TechType> UpdateAllowedTechnologies = new List<TechType>()
        {
            TechType.BaseHatch,
            TechType.BaseReinforcement,
            TechType.BaseConnector,
            TechType.BaseGlassDome,
            TechType.BaseWindow,
            TechType.BaseLargeGlassDome,
            TechType.BaseObservatory,
            TechType.BaseLadder,
            TechType.BaseFiltrationMachine,
            TechType.BaseBulkhead,
            TechType.BaseUpgradeConsole,
            TechType.BaseBioReactor,
            TechType.BaseNuclearReactor,
            TechType.BaseWaterPark,
            TechType.BasePartition,
            TechType.BasePartitionDoor,
            TechType.BasePlanter,
        };
    }
}