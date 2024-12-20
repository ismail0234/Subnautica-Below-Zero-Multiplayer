namespace Subnautica.API.Extensions
{
    using System.Collections.Generic;
    using System.IO;

    using Subnautica.API.Features;
    using Subnautica.Network.Models.WorldEntity.DynamicEntityComponents.Shared;
    using Subnautica.Network.Structures;

    using UnityEngine;

    using WorldEntityModel = Subnautica.Network.Models.WorldEntity.DynamicEntityComponents;

    public static class GameExtensions
    {
       /**
         *
         * Nesnenin id'sini döner.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static string GetIdentityId(this GameObject gameObject, bool autoAdd = false)
        {
            return Network.Identifier.GetIdentityId(gameObject, autoAdd);
        }

       /**
         *
         * Nesnenin id'sini değiştirir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void SetIdentityId(this GameObject gameObject, string uniqueId)
        {
            Network.Identifier.SetIdentityId(gameObject, uniqueId);
        }

       /**
         *
         * Oyun modunu döner.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */        
        public static GameModePresetId GetButtonGameModeId(this GameObject gameObject)
        {
            foreach (var item in gameObject.GetComponentsInChildren<TranslationLiveUpdate>())
            {
                if (item.name.Contains("ModeTitle"))
                {
                    if (item.translationKey.Contains("Survival"))
                    {
                        return GameModePresetId.Survival;
                    }
                    
                    if (item.translationKey.Contains("Freedom"))
                    {
                        return GameModePresetId.Freedom;
                    }
                    
                    if (item.translationKey.Contains("Hardcore"))
                    {
                        return GameModePresetId.Hardcore;
                    }
                    
                    if (item.translationKey.Contains("Creative"))
                    {
                        return GameModePresetId.Creative;
                    }
                    
                    if (item.translationKey.Contains("CustomGameMode"))
                    {
                        return GameModePresetId.Custom;
                    }
                }   
            }

            return GameModePresetId.Custom;
        }

        /**
         *
         * ColorCustomizer özellikleri döner.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void CopyFrom(this global::ICustomizeable customizeable, ZeroColorCustomizer customizer)
        {
            customizeable.SetName(customizer.Name);
            customizeable.SetColor(0, uGUI_ColorPicker.HSBFromColor(customizer.BaseColor.ToColor()), customizer.BaseColor.ToColor());
            customizeable.SetColor(1, uGUI_ColorPicker.HSBFromColor(customizer.StripeColor1.ToColor()), customizer.StripeColor1.ToColor());
            customizeable.SetColor(2, uGUI_ColorPicker.HSBFromColor(customizer.StripeColor2.ToColor()), customizer.StripeColor2.ToColor());
            customizeable.SetColor(3, uGUI_ColorPicker.HSBFromColor(customizer.NameColor.ToColor()), customizer.NameColor.ToColor());
        }

        /**
         *
         * ColorCustomizer özellikleri döner.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void CopyFrom(this global::SubNameInput subNameInput, ZeroColorCustomizer customizer)
        {
            subNameInput.SetName(customizer.Name);
            subNameInput.SetColor(0, customizer.BaseColor.ToColor());
            subNameInput.SetColor(1, customizer.StripeColor1.ToColor());
            subNameInput.SetColor(2, customizer.StripeColor2.ToColor());
            subNameInput.SetColor(3, customizer.NameColor.ToColor());
        }

        /**
         *
         * ColorCustomizer özellikleri döner.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void CopyFrom(this global::ColorNameControl toColorNameControl, global::ColorNameControl fromColorNameControl)
        {
            toColorNameControl.savedColors = fromColorNameControl.savedColors;
            toColorNameControl.ApplyColors();
        }

        /**
         *
         * Hoverbike özellikleri döner.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static WorldEntityModel.Hoverbike ToHoverbikeComponent(this global::Hoverbike hoverBike)
        {
            return new WorldEntityModel.Hoverbike()
            {
                Modules         = hoverBike.modules.ToUpgradeConsoleItems(new string[1] { global::Hoverbike.slot1ID }),
                Charge          = hoverBike.energyMixin.battery == null ? (GameModeManager.GetOption<bool>(GameOption.TechnologyRequiresPower) ? 0.0f : 1.701412E+38f) : hoverBike.energyMixin.battery.charge,
                LiveMixin       = hoverBike.liveMixin.ToZeroLiveMixin(),
                IsLightActive   = hoverBike.toggleLights.lightsActive,
                ColorCustomizer = hoverBike.colorNameControl.ToZeroColorCustomer(),
            };
        }

        /**
         *
         * Hoverbike özellikleri döner.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static WorldEntityModel.MapRoomCamera ToMapRoomCameraComponent(this global::Pickupable pickupable)
        {
            return new WorldEntityModel.MapRoomCamera()
            {
                Battery        = pickupable.GetComponent<global::EnergyMixin>().ToPowerCell(),
                LiveMixin      = pickupable.GetComponent<global::LiveMixin>().ToZeroLiveMixin(),
                IsLightEnabled = pickupable.GetComponent<global::MapRoomCamera>().lightsParent.activeInHierarchy,
            };
        }

        /**
         *
         * ZeroColorCustomizer nesnesine dönüştürür.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static ZeroColorCustomizer ToZeroColorCustomer(this ColorNameControl colorNameControl)
        {
            return new ZeroColorCustomizer()
            {
                Name         = colorNameControl.savedName,
                BaseColor    = uGUI_ColorPicker.HSBToColor(colorNameControl.savedColors[0]).ToZeroColor(),
                StripeColor1 = uGUI_ColorPicker.HSBToColor(colorNameControl.savedColors[1]).ToZeroColor(),
                StripeColor2 = uGUI_ColorPicker.HSBToColor(colorNameControl.savedColors[2]).ToZeroColor(),
                NameColor    = uGUI_ColorPicker.HSBToColor(colorNameControl.savedColors[3]).ToZeroColor(),
            };
        }

        /**
         *
         * LiveMixin nesnesine dönüştürür.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static WorldEntityModel.Shared.LiveMixin ToZeroLiveMixin(this global::LiveMixin liveMixin)
        {
            return new WorldEntityModel.Shared.LiveMixin(liveMixin.health, liveMixin.maxHealth);
        }

        /**
         *
         * Powercell nesnesine dönüştürür.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static WorldEntityModel.Shared.PowerCell ToPowerCell(this global::EnergyMixin energyMixin)
        {
            return new WorldEntityModel.Shared.PowerCell()
            {
                UniqueId = Network.Identifier.GenerateUniqueId(),
                Charge   = energyMixin.charge,
                Capacity = energyMixin.capacity,
            };
        }

        /**
         *
         * UpgradeConsole nesnelerine dönüştürür.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static List<UpgradeConsoleItem> ToUpgradeConsoleItems(this global::Equipment equipment, string[] slotIds)
        {
            var modules = new List<UpgradeConsoleItem>();

            for (int i = 0; i < slotIds.Length; i++)
            {
                modules.Add(new UpgradeConsoleItem());

                var module = equipment.GetItemInSlot(slotIds[i]);
                if (module?.item != null)
                {
                    modules[i].ItemId = Network.Identifier.GetIdentityId(module.item.gameObject);
                    modules[i].ModuleType = module.item.GetTechType();
                }
            }

            return modules;
        }

        /**
         *
         * Güç hücrelerine dönüştürür.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void SetHealth(this global::Drillable drillable, float newHealth, bool isRespawnable = false, bool isSpawnFx = false)
        {
            if (drillable.renderers == null)
            {
                drillable.renderers = drillable.GetComponentsInChildren<MeshRenderer>();
            }

            if (drillable.health == null)
            {
                drillable.health = new float[drillable.renderers.Length];
            }

            if (newHealth == -1f || (newHealth == 0f && isRespawnable))
            {

                drillable.Restore();
            }
            else
            {
                var leftHealth = newHealth;

                for (int i = drillable.health.Length - 1; i >= 0; i--)
                {
                    if (leftHealth >= 200f)
                    {
                        drillable.health[i] = 200f;

                        leftHealth -= 200f;
                    }
                    else
                    {
                        drillable.health[i] = leftHealth;

                        leftHealth -= leftHealth;
                    }

                    if (drillable.health[i] > 0f)
                    {
                        if (!drillable.renderers[i].gameObject.activeSelf)
                        {
                            drillable.renderers[i].gameObject.SetActive(true);
                        }
                    }
                    else
                    {
                        if (drillable.renderers[i].gameObject.activeSelf)
                        {
                            drillable.renderers[i].gameObject.SetActive(false);
                        }
                    }
                }

                if (isSpawnFx)
                {
                    if (newHealth > 0f)
                    {
                        drillable.SpawnFX(drillable.breakFX, drillable.transform.position);
                    }
                    else
                    {
                        drillable.SpawnFX(drillable.breakAllFX, drillable.transform.position);
                    }
                }
            }
        }

        /**
         *
         * Çok Oyunculu resim çerçevesi seçer.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static bool MultiplayerSelectImage(this global::PictureFrame pictureFrame, string filename, byte[] imageData)
        {
            if (filename.IsNull())
            {
                return false;
            }

            Directory.CreateDirectory(Paths.GetMultiplayerClientRemoteScreenshotsPath(ZeroPlayer.CurrentPlayer.CurrentServerId));

            string filePath = Paths.GetMultiplayerClientRemoteScreenshotsPath(ZeroPlayer.CurrentPlayer.CurrentServerId, filename);
            if (imageData != null && imageData.Length > 0)
            {
                if (!File.Exists(filePath))
                {
                    File.WriteAllBytes(filePath, imageData);
                }
            }

            string thumbnailPath = Paths.GetMultiplayerClientRemoteScreenshotsThumbnailPath(ZeroPlayer.CurrentPlayer.CurrentServerId, filename);
            if (!File.Exists(thumbnailPath))
            {
                ZeroGame.SaveThumbnailToFile(thumbnailPath, File.ReadAllBytes(filePath));
            }

            using (EventBlocker.Create(TechType.PictureFrame))
            {
                pictureFrame.SelectImage(filename);

                if (!ScreenshotManager.thumbnailingQueue.Contains(filename) && !ScreenshotManager.screenshotDatabase.ContainsKey(filename))
                {
                    ScreenshotManager.screenshotDatabase.Add(filename, ZeroGame.LoadThumbnailFromFile(thumbnailPath));
                }
            }

            return true;
        }

        /**
         *
         * Çok Oyunculu muzik değiştirir
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static bool ChangeMusic(this global::JukeboxInstance jukebox, string currentPlayingTrack, bool isPaused, global::Jukebox.Repeat repeatMode, bool isShuffled, float volume, float position, uint length)
        {
            using (EventBlocker.Create(TechType.Jukebox))
            {
                if (currentPlayingTrack.IsNull())
                {
                    jukebox.imagePlayPause.sprite = jukebox.spritePlay;
                    global::Jukebox.Stop();
                }
                else
                {
                    if (jukebox.file != currentPlayingTrack)
                    {
                        jukebox.file = currentPlayingTrack;
                        global::Jukebox.Play(jukebox);
                    }
                    else
                    {
                        jukebox.file = currentPlayingTrack;
                        jukebox.OnButtonPlayPause();
                    }

                    // Paused
                    if (isPaused != global::Jukebox.paused)
                    {
                        global::Jukebox.paused = isPaused;
                    }

                    // Repeat
                    if (repeatMode != jukebox.repeat)
                    {
                        jukebox.repeat = repeatMode;
                        jukebox.imageRepeat.sprite = jukebox.spritesRepeat[(int) repeatMode];
                        global::Jukebox.repeat = repeatMode;
                    }

                    // Shuffle
                    if (isShuffled != jukebox.shuffle)
                    {
                        jukebox.shuffle = isShuffled;
                        jukebox.imageShuffle.sprite = isShuffled ? jukebox.spriteShuffleOn : jukebox.spriteShuffleOff;
                        global::Jukebox.shuffle = isShuffled;
                    }

                    // Volume
                    if (volume != jukebox.volume)
                    {
                        jukebox.volume = volume;
                        jukebox.UpdateVolumeSlider();
                        jukebox.textVolume.text = global::Language.main.GetFormat<float>("JukeboxVolumePercent", volume);
                        global::Jukebox.volume = volume;
                    }

                    // Position
                    float difference = (position - jukebox._position) * (float) (global::Jukebox.length / 1000f);
                    if (difference == 0 || difference > 1.5f || difference < 1.5f)
                    {
                        jukebox._position = position / length;
                        jukebox.UpdatePositionSlider();
                        global::Jukebox.position = (uint)((double)position * (double)length);
                    }
                }
            }

            return true;
        }

        /**
         *
         * WaterPark döner.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */  
        public static global::WaterPark GetBaseWaterPark(this global::BaseDeconstructable baseDeconstructable)
        {
            if (baseDeconstructable && baseDeconstructable.TryGetComponent<global::WaterParkGeometry>(out var waterParkGeometry))
            {
                return waterParkGeometry.GetModule();
            }

            return null;
        }

        /**
         *
         * WaterPark için BaseDeconstructable döner.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static BaseDeconstructable GetBaseDeconstructable(this global::WaterPark waterPark)
        {
            if (waterPark.hostBase == null)
            {
                return null;
            }

            foreach (var component in waterPark.hostBase.GetComponentsInChildren<BaseDeconstructable>(true))
            {
                if (component.recipe == TechType.BaseWaterPark && component.moduleFace.HasValue)
                {
                    if (component.moduleFace.Value.cell == waterPark.moduleFace.cell && component.moduleFace.Value.direction == waterPark.moduleFace.direction)
                    {
                        return component;
                    }
                }
            }

            return null;
        }

        /**
         *
         * Maproom için BaseDeconstructable döner.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static BaseDeconstructable GetBaseDeconstructable(this MapRoomFunctionality mapRoom)
        {
            var baseComp = mapRoom.GetComponentInParent<global::Base>();
            if (baseComp == null)
            {
                return null;
            }

            var cell = baseComp.NormalizeCell(baseComp.WorldToGrid(mapRoom.transform.position));

            foreach (var component in baseComp.GetComponentsInChildren<BaseDeconstructable>())
            {
                if (component.name == "BaseMapRoom" || component.name == "BaseMapRoom(Clone)")
                {
                    if (baseComp.NormalizeCell(baseComp.WorldToGrid(component.transform.position)) == cell)
                    {
                        return component;
                    }
                }
            }

            return null;
        }

        /**
         *
         * BaseDeconstructable için MapRoomFunctionality döner.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */    
        public static MapRoomFunctionality GetMapRoomFunctionality(this BaseDeconstructable baseDeconstructable)
        {
            return baseDeconstructable.deconstructedBase.GetMapRoomFunctionalityForCell(baseDeconstructable.bounds.mins);
        }

        /**
         *
         * Ghost crafter döner.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */    
        public static global::GhostCrafter GetGhostCrafter(this GameObject gameObject)
        {
            if (gameObject == null)
            {
                return null;
            }
            
            if (gameObject.TryGetComponent<global::BaseDeconstructable>(out var baseDeconstructable) && baseDeconstructable.recipe == TechType.BaseMapRoom)
            {
                return baseDeconstructable.GetMapRoomFunctionality()?.gameObject?.GetComponentInChildren<global::GhostCrafter>();
            }

            return gameObject.GetComponentInChildren<global::GhostCrafter>();
        }

        /**
         *
         * Sinematiği atlar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */    
        public static void SkipCinematic(this global::PlayerCinematicController cinematic)
        {
            if (cinematic)
            {
                cinematic.OnPlayerCinematicModeEnd();
                cinematic.BreakCinematic();
            }
        }

        /**
         *
         * Oyuncunun pilotluk durumunu döner.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */    
        public static bool IsPiloted(this global::SeaTruckSegment seaTruckSegment)
        {
            if (seaTruckSegment.motor.IsPiloted())
            {
                return true;
            }

            if (global::Player.main.transform.parent && global::Player.main.transform.parent.transform == seaTruckSegment.motor.pilotPosition)
            {
                return true;
            }

            return false;
        }

        /**
         *
         * Ghost crafter döner.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */    
        public static global::MoonpoolExpansionManager GetDockedMoonpoolExpansion(this global::SeaTruckSegment seaTruckSegment, bool checkHead = false)
        {
            if (seaTruckSegment == null || seaTruckSegment.transform.parent == null)
            {
                return null;
            }

            foreach (var item in seaTruckSegment.transform.parent.GetComponentsInChildren<global::MoonpoolExpansionManager>())
            {
                if (item.tail == seaTruckSegment)
                {
                    return item;
                }

                if (checkHead && item.dockedHead == seaTruckSegment)
                {
                    return item;
                }
            }

            return null;
        }

        /**
         *
         * Oyuncunun altındaki nesneyi döner.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static T GetUnderGameObject<T>(this global::Player player)
        {
            if (Physics.Raycast(player.transform.position, MainCameraControl.main.viewModel.transform.TransformDirection(Vector3.down), out var hit, 3f))
            {
                return hit.collider.gameObject.GetComponentInParent<T>();
            }

            return default;
        }

        /**
         *
         * Oyuncunun hareketini dondurur.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void DisableMovement(this global::Player player)
        {
            FPSInputModule.current.lockMovement = true;
        }

        /**
         *
         * Oyuncunun hareketini açar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void EnableMovement(this global::Player player)
        {
            FPSInputModule.current.lockMovement = false;
        }
    }
}