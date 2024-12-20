namespace Subnautica.API.Features
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;

    using Story;

    using Subnautica.API.Extensions;
    using Subnautica.API.Features.Helper;
    using Subnautica.Network.Models.Storage.World.Childrens;
    using Subnautica.Network.Models.WorldEntity.DynamicEntityComponents.Shared;
    using Subnautica.Network.Structures;

    using UnityEngine;

    using UWE;

    using Metadata         = Subnautica.Network.Models.Metadata;
    using WorldEntityModel = Subnautica.Network.Models.WorldEntity.DynamicEntityComponents;

    public class Vehicle
    {
        /**
         *
         * Fabrikator yapısında Araç üretir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void CraftVehicle(WorldDynamicEntity entity, global::ConstructorInput constructorInput = null, Action<WorldDynamicEntity, ItemQueueAction, GameObject> onCompleted = null, float finishTime = 0f, bool notify = false, bool isMine = false, object customProperty = null)
        {
            if (isMine)
            {
                CrafterLogic.ConsumeResources(entity.TechType);

                if (constructorInput)
                {
                    uGUI.main.craftingMenu.Close(constructorInput);
                    constructorInput.cinematicController.DisengageConstructor();
                }
            }

            var action = new ItemQueueAction();
            action.OnProcessCompletedAsync = VehicleCraftCompletedAsync;
            action.RegisterProperty("Entity"        , entity);
            action.RegisterProperty("FinishTime"    , finishTime);
            action.RegisterProperty("IsNotify"      , notify);
            action.RegisterProperty("OnCompleted"   , onCompleted);
            action.RegisterProperty("Constructor"   , constructorInput);
            action.RegisterProperty("CustomProperty", customProperty);

            Entity.ProcessToQueue(action);
        }

        /**
         *
         * Async araç üretimini başlatır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private static IEnumerator VehicleCraftCompletedAsync(ItemQueueProcess item)
        {
            var entity      = item.Action.GetProperty<WorldDynamicEntity>("Entity");
            var finishTime  = item.Action.GetProperty<float>("FinishTime");
            var isNotify    = item.Action.GetProperty<bool>("IsNotify");
            var constructor = item.Action.GetProperty<ConstructorInput>("Constructor");
            var onCompleted = item.Action.GetProperty<Action<WorldDynamicEntity, ItemQueueAction, GameObject>>("OnCompleted");
            if (entity != null)
            {
                var result = new TaskResult<GameObject>();
                yield return CraftData.InstantiateFromPrefabAsync(entity.TechType, result);

                var gameObject = result.Get();

                if (entity == null)
                {
                    World.DestroyGameObject(gameObject);
                }
                else
                {
                    Network.Identifier.SetIdentityId(gameObject, entity.UniqueId);

                    gameObject.transform.position = entity.Position.ToVector3();
                    gameObject.transform.rotation = entity.Rotation.ToQuaternion();

                    if (isNotify)
                    {
                        CrafterLogic.NotifyCraftEnd(gameObject, entity.TechType);

                        ItemGoalTracker.OnConstruct(entity.TechType);
                    }

                    onCompleted?.Invoke(entity, item.Action, gameObject);

                    if (finishTime > 0f)
                    {
                        var componentInChildren = gameObject.GetComponentInChildren<VFXConstructing>();
                        if (componentInChildren)
                        {
                            componentInChildren.timeToConstruct = finishTime;
                            componentInChildren.StartConstruction();
                        }

                        if (constructor)
                        {
                            if (gameObject.GetComponentInChildren<BuildBotPath>() == null && constructor)
                            {
                                var leftTime = finishTime - DayNightCycle.main.timePassedAsFloat;
                                new GameObject("ConstructorBeam").AddComponent<TwoPointLine>().Initialize(constructor.beamMaterial, constructor.transform, constructor.transform, 0.1f, 1f, leftTime < 0f ? 0f : leftTime);
                            }
                            else
                            {
                                constructor.constructor.SendBuildBots(gameObject);
                            }
                        }
                    }
                }
            }
        }

        /**
         *
         * Hoverbike üretir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void CraftHoverbike(global::HoverpadConstructor hoverpad, string uniqueId, float finishedTime, bool isMine, Action<GameObject> callBackAction = null)
        {
            var leftTime = finishedTime - DayNightCycle.main.timePassedAsFloat;
            if (isMine)
            {
                CrafterLogic.ConsumeResources(TechType.Hoverbike);
            }

            CoroutineHost.StartCoroutine(SpawnHoverbikeAsync(hoverpad, uniqueId, finishedTime, leftTime, callBackAction));
        }

        /**
         *
         * Hoverbike'yi park eder.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void DockHoverbike(global::Hoverpad hoverpad, global::Hoverbike hoverbike, bool isMine, Func<bool> callback = null)
        {
            CoroutineHost.StartCoroutine(DockHoverbikeAsync(hoverpad, hoverbike, isMine, callback));
        }

        /**
         *
         * Async hoverbike üretir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private static IEnumerator SpawnHoverbikeAsync(global::HoverpadConstructor hoverpadConstructor, string uniqueId, float finishedTime, float leftTime, Action<GameObject> callBackAction = null)
        {
            if (leftTime > 2f)
            {
                hoverpadConstructor.hoverpadTerminal.StartBuild(finishedTime);
                hoverpadConstructor.isConstructing = true;
                
                global::Utils.PlayFMODAsset(hoverpadConstructor.startConstructSound, hoverpadConstructor.soundSource);

                hoverpadConstructor.Invoke("StartLoopingSound", 1.8f);
            }

            var result = new TaskResult<GameObject>();
            yield return CraftData.InstantiateFromPrefabAsync(TechType.Hoverbike, result);

            var target = result.Get();
            if (target)
            {
                Network.Identifier.SetIdentityId(target, uniqueId);

                if (leftTime > 2f)
                {                
                    target.transform.position = hoverpadConstructor.hoverpad.bikeSpawnPoint.position;
                    target.transform.rotation = hoverpadConstructor.hoverpad.bikeSpawnPoint.rotation;

                    hoverpadConstructor.hoverbike = target.GetComponent<global::Hoverbike>();
                    hoverpadConstructor.hoverbike.DockToHoverpad(hoverpadConstructor.hoverpad, true);
                    hoverpadConstructor.hoverbike.OnConstructionBegin();

                    CrafterLogic.NotifyCraftEnd(target.gameObject, TechType.Hoverbike);

                    var componentInChildren = target.GetComponentInChildren<VFXConstructing>();
                    if (componentInChildren != null)
                    {
                        componentInChildren.timeToConstruct = finishedTime;
                        componentInChildren.StartConstruction();
                        componentInChildren.informGameObject = hoverpadConstructor.gameObject;

                        hoverpadConstructor.hoverpad.SetBikeTriggersImmediate(false);
                        hoverpadConstructor.hoverpad.animator.SetBool("fabricate", true);
                    }
                }
                else
                {
                    hoverpadConstructor.hoverpad.dockedBike = target.GetComponent<global::Hoverbike>();
                    hoverpadConstructor.hoverpad.dockedBike.DockToHoverpad(hoverpadConstructor.hoverpad, true);
                    hoverpadConstructor.hoverpad.DockBike();
                }

                callBackAction?.Invoke(target);
            }
        }

        /**
         *
         * Async Hoverbike'yi park eder.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private static IEnumerator DockHoverbikeAsync(Hoverpad hoverpad, global::Hoverbike hoverbike, bool isMine, Func<bool> callback = null)
        {
            hoverpad.dockedBike = hoverbike;

            var pilotingCraft = hoverpad.dockedBike.GetPilotingCraft();
            hoverpad.dockedBike.DockToHoverpad(hoverpad, false);
            hoverpad.lerpBikeToAttach = true;
            hoverpad.startLerpTime = Time.time;
            hoverpad.startLerpPos = hoverbike.transform.position;
            hoverpad.startLerpRot = hoverbike.transform.rotation;
            hoverpad.sfx_park.Play();

            yield return new WaitForSecondsRealtime(hoverpad.dockDuration + 0.1f);

            if (hoverpad.dockedBike)
            {
                hoverpad.lerpBikeToAttach = false;

                if (isMine && pilotingCraft)
                {
                    hoverpad.dockedBike.animator.SetBool("park_bike", true);
                    hoverpad.dockedBike.ExitVehicle();
                    hoverpad.dockCinematic.StartCinematicMode(global::Player.main);
                    hoverpad.animator.SetBool("dockWithoutPlayer", false);
                }
                else
                {
                    if (callback == null)
                    {
                        hoverpad.animator.SetBool("dockWithoutPlayer", true);
                    }
                    else
                    {
                        hoverpad.animator.SetBool("dockWithoutPlayer", !callback.Invoke());
                    }
                }

                hoverpad.SetBikeTriggers(true);
                hoverpad.terminalGUI.SetScreen();
                hoverpad.terminalGUI.SetCustomizeable(hoverpad.dockedBike.colorNameControl);
                hoverpad.canShowroom = true;
            }
        }

        /**
         *
         * Hoverbike özellikleri uygular.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void ApplyHoverbikeComponent(GameObject gameObject, WorldEntityModel.Hoverbike component)
        {
            if (gameObject.TryGetComponent<global::Hoverbike>(out var hoverBike))
            {
                hoverBike.LazyInitialize();
                hoverBike.pickupable.isPickupable = true;

                Vehicle.ApplyModules(component.Modules, hoverBike.upgradeInput.equipment, TechType.Hoverbike);
                Vehicle.ApplyColorCustomizer(component.ColorCustomizer, hoverBike.colorNameControl);
                Vehicle.ApplyEnergyMixin(hoverBike.energyMixin, component.Charge, () => {
                    Vehicle.ApplyLights(hoverBike.toggleLights, component.IsLightActive);
                });
            }
        }

        /**
         *
         * Renkleri uygular.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void ApplyColorCustomizer(ZeroColorCustomizer colorCustomizer, ColorNameControl colorNameControl)
        {
            if (colorCustomizer?.BaseColor != null && colorNameControl != null)
            {
                if (colorNameControl.savedColors == null)
                {
                    colorNameControl.savedColors = (Vector3[])ColorNameControl.defaultColors.Clone();
                }

                var customizeable = colorNameControl as global::ICustomizeable;
                customizeable.SetName(colorCustomizer.Name);
                customizeable.SetColor(0, uGUI_ColorPicker.HSBFromColor(colorCustomizer.BaseColor.ToColor()), colorCustomizer.BaseColor.ToColor());
                customizeable.SetColor(1, uGUI_ColorPicker.HSBFromColor(colorCustomizer.StripeColor1.ToColor()), colorCustomizer.StripeColor1.ToColor());
                customizeable.SetColor(2, uGUI_ColorPicker.HSBFromColor(colorCustomizer.StripeColor2.ToColor()), colorCustomizer.StripeColor2.ToColor());
                customizeable.SetColor(3, uGUI_ColorPicker.HSBFromColor(colorCustomizer.NameColor.ToColor()), colorCustomizer.NameColor.ToColor());
            }
        }

        /**
         *
         * Modülleri uygular.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void ApplyModules(List<UpgradeConsoleItem> modules, Equipment equipment, TechType techType)
        {
            for (int i = 0; i < modules.Count; i++)
            {
                var item = modules.ElementAt(i);
                if (item.ModuleType != TechType.None)
                {
                    var slotId = GetModuleSlotId(i, techType);
                    if (!string.IsNullOrEmpty(slotId))
                    {
                        Entity.SpawnToQueue(slotId, item.ModuleType, item.ItemId, equipment);
                    }
                }
            }
        }

        /**
         *
         * Işıkları açar/kapatır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void ApplyLights(global::ToggleLights toggleLights, bool isActive)
        {
            ZeroGame.SetLightsActive(toggleLights, isActive);
        }

        /**
         *
         * Işıkları açar/kapatır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void ApplyLights(global::SeaTruckLights seaTruckLights, bool isActive)
        {
            ZeroGame.SetLightsActive(seaTruckLights, isActive);
        }

        /**
         *
         * Güç hücrelerini ayarlar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void ApplyPowerCells(string uniqueId, List<PowerCell> powerCells)
        {
            foreach (var powerCell in powerCells)
            {
                if (powerCell.Charge != -1f)
                {
                    var action = new ItemQueueAction();
                    action.OnEntitySpawned = OnPowerCellSpawned;
                    action.RegisterProperty("UniqueId"  , uniqueId);
                    action.RegisterProperty("PowerCell" , powerCell);

                    Entity.SpawnToQueue(powerCell.TechType, Network.Identifier.GenerateUniqueId(), new ZeroTransform(Vector3.down.ToZeroVector3(), Quaternion.identity.ToZeroQuaternion()), action);
                }
            }
        }

        /**
         *
         * Güç hücrelerini ayarlar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void ApplyStorageContainer(string uniqueId, Metadata.StorageContainer storageContainer)
        {
            if (storageContainer != null)
            {
                var container = Network.Identifier.GetComponentByGameObject<global::StorageContainer>(uniqueId);
                if (container)
                {
                    foreach (var item in storageContainer.Items)
                    {
                        if (item.Item == null)
                        {
                            Entity.SpawnToQueue(item.TechType, item.ItemId, container.container);
                        }
                        else
                        {
                            Entity.SpawnToQueue(item.Item, item.ItemId, container.container);
                        }
                    }
                }
            }
        }

        /**
         *
         * Araç sağlığını günceller.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void ApplyLiveMixin(global::LiveMixin liveMixin, float health)
        {
            liveMixin.health = health;
        }

        /**
         *
         * Batarya slotlarını ayarlar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void ApplyBatterySlotIds(GameObject gameObject, TechType techType, string firstPowerCellId, string secondPowerCellId)
        {
            foreach (var batterySlot in gameObject.GetComponentsInChildren<ChildObjectIdentifier>())
            {
                if ((techType == TechType.Exosuit && batterySlot.name.Contains("BatterySlot1")) || (techType == TechType.SeaTruck && batterySlot.name.Contains("BatterySlotLeft")))
                {
                    Network.Identifier.SetIdentityId(batterySlot.gameObject, firstPowerCellId);
                }
                else if ( (techType == TechType.Exosuit && batterySlot.name.Contains("BatterySlot2")) || (techType == TechType.SeaTruck && batterySlot.name.Contains("BatterySlotRight")))
                {
                    Network.Identifier.SetIdentityId(batterySlot.gameObject, secondPowerCellId);
                }
            }

            foreach (var batterySlot in gameObject.GetComponentsInChildren<GenericHandTarget>())
            {
                if ((techType == TechType.Exosuit && batterySlot.name.Contains("BatteryLeft")) || (techType == TechType.SeaTruck && batterySlot.name.Contains("BatteryInputLeft")))
                {
                    Network.Identifier.SetIdentityId(batterySlot.gameObject, ZeroGame.GetVehicleBatteryLabelUniqueId(firstPowerCellId));
                }
                else if ( (techType == TechType.Exosuit && batterySlot.name.Contains("BatteryRight")) || (techType == TechType.SeaTruck && batterySlot.name.Contains("BatteryInputRight")))
                {
                    Network.Identifier.SetIdentityId(batterySlot.gameObject, ZeroGame.GetVehicleBatteryLabelUniqueId(secondPowerCellId));
                }
            }
        }

        /**
         *
         * Nesne doğduğunda tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private static void OnPowerCellSpawned(ItemQueueProcess item, Pickupable pickupable, GameObject gameObject)
        {
            var powerCell = item.Action.GetProperty<PowerCell>("PowerCell");
            var vehicle   = Network.Identifier.GetGameObject(item.Action.GetProperty<string>("UniqueId"));

            EnergyMixin energyMixin = null;
            if (vehicle)
            {
                var exosuit = vehicle.GetComponent<global::Exosuit>();
                if (exosuit)
                {
                    foreach (var _item in exosuit.energyInterface.sources)
                    {
                        if (_item.storageRoot.gameObject.GetIdentityId() == powerCell.UniqueId)
                        {
                            energyMixin = _item;
                            break;
                        }
                    }
                }
                else
                {
                    foreach (var _item in vehicle.GetComponentsInChildren<global::BatterySource>())
                    {
                        if (_item.storageRoot.gameObject.GetIdentityId() == powerCell.UniqueId)
                        {
                            energyMixin = _item;
                            break;
                        }
                    }
                }

                if (energyMixin != null)
                {
                    var storedItem = energyMixin.batterySlot.storedItem;
                    if (storedItem != null)
                    {
                        energyMixin.batterySlot.RemoveItem();
                        World.DestroyGameObject(storedItem.item.gameObject);
                    }

                    energyMixin.batterySlot.AddItem(pickupable);
                    energyMixin.battery.charge = powerCell.Charge;
                }
            }

            if (energyMixin == null)
            {
                World.DestroyGameObject(gameObject);
            }
        }

        /**
         *
         * Module Slot numarasını döner.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private static string GetModuleSlotId(int slotId, TechType techType)
        {
            if (techType == TechType.Exosuit)
            {
                if (slotId == 4)
                {
                    return "ExosuitArmLeft";
                }

                if (slotId == 5)
                {
                    return "ExosuitArmRight";
                }

                return string.Format("ExosuitModule{0}", slotId + 1);
            }
            else if (techType == TechType.Hoverbike)
            {
                return string.Format("HoverbikeModule{0}", slotId + 1);
            }
            else if (techType == TechType.SeaTruck)
            {
                return string.Format("SeaTruckModule{0}", slotId + 1);
            }

            return null;
        }

        /**
         *
         * EnergyMixin özellikleri uygular.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void ApplyEnergyMixin(global::EnergyMixin energyMixin, float charge, Action callback = null)
        {
            CoroutineHost.StartCoroutine(ApplyEnergyMixinAsync(energyMixin, charge, callback));
        }

        /**
         *
         * EnergyMixin özellikleri ASYNC uygular.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private static IEnumerator ApplyEnergyMixinAsync(global::EnergyMixin energyMixin, float charge, Action callback)
        {
            if (energyMixin)
            {
                energyMixin.Initialize();

                yield return energyMixin.SpawnDefaultBatteryAsync();

                if (energyMixin)
                {
                    energyMixin.battery.charge = charge;

                    callback?.Invoke();
                }
            }
        }
    }
}