namespace Subnautica.API.Features
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;

    using Subnautica.API.Enums;
    using Subnautica.API.Extensions;
    using Subnautica.API.Features.PlayerUtility;
    using Subnautica.API.MonoBehaviours;
    using Subnautica.Network.Core.Components;
    using Subnautica.Network.Models.Server;
    using Subnautica.Network.Structures;
    using Subnautica.Server.Storage;

    using UnityEngine;

    public class ZeroPlayer
    {
        /**
         *
         * PlayerSignalName Değerini barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public const string PlayerSignalName = "MultiplayerPlayerSignal";

        /**
         *
         * DontUseThisMethod Değerini barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public const string DontUseThisMethod = "You can't use this method if you are the player! This method is only available for other players.";

        /**
         *
         * Oyuncu ben miyim?
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static bool IsPlayerMine(string uniqueId)
        {
            if (uniqueId.IsNull())
            {
                return false;
            }

            var player = ZeroPlayer.GetPlayerByUniqueId(uniqueId);
            return player != null && player.IsMine;
        }

        /**
         *
         * Oyuncu ben miyim?
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static bool IsPlayerMine(byte playerId)
        {
            var player = ZeroPlayer.GetPlayerById(playerId);
            return player != null && player.IsMine;
        }

        /**
         *
         * Sınıf ayarlamalarını yapar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public ZeroPlayer(string uniqueId, bool isLocalPlayer = false)
        {
            this.UniqueId = uniqueId;
            this.IsMine   = isLocalPlayer;

            if (isLocalPlayer)
            {
                this.PlayerObject = new GameObject(String.Format("MP_Client_{0}", Guid.NewGuid()));
                this.SetCurrentPlayer(this);
            }

            this.SetDefaultAnimationQueue();
            this.AddPlayerToList(this);
        }

        /**
         *
         * Oyuncuyu listeye ekler.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void AddPlayerToList(ZeroPlayer player)
        {
            Players.Add(player);
        }

        /**
         *
         * Mevcut oyuncu yoksa oluşturur ve Unique Id kimliğinden oyuncuyu döner.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static ZeroPlayer CreateOrGetPlayerByUniqueId(string uniqueId, byte playerId)
        {
            var player = Players.Where(q => q.UniqueId == uniqueId).FirstOrDefault() ?? new ZeroPlayer(uniqueId, false);
            player.PlayerId = playerId;
            return player;
        }

        /**
         *
         * Unique Id kimliğinden oyuncuyu döner.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static ZeroPlayer GetPlayerByUniqueId(string uniqueId)
        {
            if (uniqueId.IsNull())
            {
                return null;
            }

            return Players.Where(q => q.UniqueId == uniqueId).FirstOrDefault();
        }

        /**
         *
         * GameObject kimliğinden oyuncuyu döner.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static ZeroPlayer GetPlayerByGameObject(GameObject gameObject)
        {
            return Players.Where(q => q.PlayerModel == gameObject).FirstOrDefault();
        }

        /**
         *
         * Araç GameObject kimliğinden oyuncuyu döner.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static ZeroPlayer GetPlayerByVehicleGameObject(GameObject vehicleGameObject)
        {
            if (vehicleGameObject == null)
            {
                return null;
            }

            foreach (var player in ZeroPlayer.GetAllPlayers())
            {
                if (player.GetVehicle() == vehicleGameObject)
                {
                    return player;
                }
            }

            return null;
        }

        /**
         *
         * Player Id kimliğinden oyuncuyu döner.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static ZeroPlayer GetPlayerById(byte playerId)
        {
            if (playerId <= 0)
            {
                return null;
            }

            return Players.Where(q => q.PlayerId == playerId).FirstOrDefault();
        }

        /**
         *
         * Unique Id kimliğinden oyuncuyu döner.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static ZeroPlayer GetPlayerById(string playerId)
        {
            return Players.Where(q => q.UniqueId == playerId).FirstOrDefault();
        }

        /**
         *
         * Oyuncuları Döner.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static List<ZeroPlayer> GetPlayers()
        {
            return Players.Where(q => !q.IsMine).ToList();
        }

        /**
         *
         * Tüm Oyuncuları Döner.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static List<ZeroPlayer> GetAllPlayers()
        {
            return Players.ToList();
        }

        /**
         *
         * Aralıktaki oyuncuları Döner.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static PlayerRange GetPlayersByInRange(Vector3 sourcePosition, float range, bool inVehicle = false)
        {
            var playerRange = new PlayerRange();

            foreach (var player in ZeroPlayer.GetAllPlayers())
            {
                var distance = ZeroVector3.Distance(player.PlayerModel.transform.position, sourcePosition);
                if (distance > range)
                {
                    continue;
                }

                if (inVehicle && player.GetVehicle() == null)
                {
                    continue;
                }

                if (distance < playerRange.NearestPlayerDistance)
                {
                    playerRange.SetNearestPlayer(player, distance);
                }

                if (distance > playerRange.FarthestPlayerDistance)
                {
                    playerRange.SetFarthestPlayer(player, distance);
                }

                playerRange.AddPlayer(player, distance);
            }

            return playerRange;
        }

        /**
         *
         * Mevcut oyuncu ve bilgilerini yok eder.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void DisposeAll()
        {
            try
            {
                if (ZeroPlayer.CurrentPlayer?.PlayerObject)
                {
                    GameObject.DestroyImmediate(ZeroPlayer.CurrentPlayer.PlayerObject.gameObject);
                }

                ZeroPlayer.CurrentPlayer = null;
                ZeroPlayer.Players.Clear();

                global::Player.allowSaving = true;
            }
            catch (Exception e)
            {
                Log.Error($"ZeroPlayer.DisposeAll Exception: {e}");
            }
        }

        /**
         *
         * Oyuncu hızını değiştirir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void SetVelocity(Vector3 velocity)
        {
            if (!this.IsMine)
            {
                this.Velocity = velocity;
            }
        }

        /**
         *
         * Oyuncu UniqueId kimliğini günceller.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void SetUniqueId(string uniqueId)
        {
            this.UniqueId = uniqueId;
        }

        /**
         *
         * Hipnotize durumunu döner.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public bool IsHypnotized()
        {
            if (this.IsMine)
            {
                return this.Main.lilyPaddlerHypnosis.IsHypnotized();
            }

            return this.LastHypnotizeTime > Network.Session.GetWorldTime();
        }

        /**
         *
         * Oyuncu Animasyon kuyruğunu günceller.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void SetAnimationQueue(Dictionary<string, bool> animations)
        {
            if (this.IsMine)
            {
                throw new Exception(DontUseThisMethod);
            }
            else
            {
                if (animations != null)
                {
                    foreach (var animation in animations)
                    {
                        if (PlayerAnimationTypeExtensions.Animations.Contains(animation.Key))
                        {
                            this.Animations[animation.Key] = animation.Value;
                        }
                    }

                    this.AnimationQueue.Enqueue(this.Animations.ToDictionary(entry => entry.Key, entry => entry.Value));
                }
            }
        }

        /**
         *
         * Selfie modunu değiştirir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void SetSelfieMode(float selfieId)
        {
            if (this.IsMine)
            {
                this.Main.playerAnimator.SetBool("selfies", true);
                this.Main.playerAnimator.SetFloat("selfie_number", selfieId);
            }
            else
            {
                this.Animator?.SetBool("selfies", true);
                this.Animator?.SetFloat("selfie_number", selfieId);
            }
        }

        /**
         *
         * Oyuncu Animasyon kuyruğunu varsayılana çeker.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private void SetDefaultAnimationQueue()
        {
            if (!this.IsMine)
            {
                this.ClearAnimationQueue();
                this.SetAnimationQueue(PlayerAnimationTypeExtensions.Animations.ToDictionary(q => q, x => false));
            }
        }

        /**
         *
         * Oyuncu hızını döner.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public Vector3 GetVelocity()
        {
            if (this.IsMine)
            {
                return this.Main.rigidBody.velocity;
            }

            return this.Velocity;
        }

        /**
         *
         * Oyuncunun aracını döner.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public GameObject GetVehicle()
        {
            if (this.IsMine)
            {
                if (this.Main.mode != global::Player.Mode.LockedPiloting)
                {
                    return null;
                }

                var parentTransform = this.Main.transform.parent;
                if (parentTransform == null)
                {
                    return null;
                }

                var techType = parentTransform.gameObject.GetTechType();
                if (techType == TechType.SeaTruck)
                {
                    var seaTruckMotor = this.Main.GetComponentInParent<global::SeaTruckMotor>();
                    if (seaTruckMotor && seaTruckMotor.IsPiloted())
                    {
                        return seaTruckMotor.gameObject;
                    }
                }
                else if (techType == TechType.Exosuit)
                {
                    var exosuit = this.Main.GetComponentInParent<global::Vehicle>();
                    if (exosuit && exosuit.pilotId.IsNotNull())
                    {
                        return exosuit.gameObject;
                    }
                }
                else if (techType == TechType.Hoverbike)
                {
                    var hoverBike = this.Main.GetComponentInParent<global::Hoverbike>();
                    if (hoverBike && hoverBike.isPiloting)
                    {
                        return hoverBike.gameObject;
                    }
                }

                return null;
            }

            if (this.VehicleId <= 0)
            {
                return null;
            }

            var vehicle = Network.DynamicEntity.GetEntity(this.VehicleId);
            if (vehicle == null || !vehicle.TechType.IsVehicle())
            {
                return null;
            }

            return vehicle.GameObject;
        }

        /**
         *
         * Oyuncu boşlukta mı?
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public bool IsPlayerInVoid()
        {
            if (VoidLeviathansSpawner.main == null)
            {
                return false;
            }

            if (this.IsMine)
            {
                return VoidLeviathansSpawner.main.IsVoidBiome(this.Main.GetBiomeString());
            }

            return VoidLeviathansSpawner.main.IsVoidBiome(LargeWorld.main.GetBiome(this.PlayerModel.transform.position));
        }

        /**
         *
         * Oyuncu saldırıya uğrayabilir mi?
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public bool CanBeAttacked()
        {
            if (this.IsMine)
            {
                return this.Main.CanBeAttacked();
            }

            if (GameModeManager.HasNoCreatureAggression())
            {
                return false;
            }

            if (this.IsFrozen)
            {
                return false;
            }

            if (this.IsCinematicModeActive || this.CurrentSubRootId.IsNotNull())
            {
                return false;
            }

            if (this.CurrentInteriorId.IsNotNull())
            {
                return false;
            }

            return true;
        }

        /**
         *
         * Oyuncu hipnotize olabilir mi?
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public bool CanHypnotizePlayer()
        {
            if (this.IsMine)
            {
                return this.Main.CanBeAttacked() && this.Main.IsSwimming() && !this.Main.IsInside() && !this.IsHypnotized() && !this.Main.frozenMixin.IsFrozen() && !this.Main.cinematicModeActive;
            }

            if (!this.CanBeAttacked())
            {
                return false;
            }

            if (!this.IsUnderwater)
            {
                return false;
            }

            if (this.IsHypnotized())
            {
                return false;
            }

            return true;
        }

        /**
         *
         * Nesne'ye bakıyor muyum?
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public bool LooksAtMe(GameObject target, bool checkPhysics = true)
        {
            var direction = Vector3.Normalize(target.transform.position - this.PlayerModel.transform.position);
            var transform = this.IsMine ? MainCameraControl.main.transform : this.PlayerModel.transform;
            var forward   = this.IsMine ? MainCameraControl.main.transform.forward : this.CameraForward;

            if (Vector3.Dot(direction, forward) < 0.65)
            {
                return false;
            }

            if (checkPhysics)
            {
                int num = UWE.Utils.RaycastIntoSharedBuffer(transform.position, direction, Vector3.Distance(target.transform.position, transform.position), -1, QueryTriggerInteraction.Ignore);
                for (int index = 0; index < num; ++index)
                {
                    var raycastHit = UWE.Utils.sharedHitBuffer[index];
                    var gameObject = raycastHit.collider.attachedRigidbody == null ? raycastHit.collider.gameObject : raycastHit.collider.attachedRigidbody.gameObject;
                    if (gameObject != this.PlayerModel && gameObject != target)
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        /**
         *
         * Oyuncu modelini oluşturur.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public bool CreateModel(Vector3 position, Quaternion rotation)
        {
            if (this.IsMine)
            {
                return false;
            }

            if (this.IsCreatedModel)
            {
                return false;
            }

            var playerModel = GameObject.Find("player_view_female");
            if (playerModel == null)
            {
                return false;
            }

            this.Position = position;
            this.Rotation = rotation;

            this.PlayerModel = UnityEngine.Object.Instantiate<GameObject>(playerModel, this.Position, this.Rotation);
            this.PlayerModel.AddComponent<Rigidbody>().mass = global::Player.main.rigidBody.mass;
            this.PlayerModel.AddComponent<TechTag>().type   = TechType.Player;
            this.PlayerModel.layer = LayerID.Player;
            this.PlayerModel.name  = this.NickName;

            Network.Identifier.SetIdentityId(this.PlayerModel, this.UniqueId);

            this.Animator = this.PlayerModel.GetComponent<Animator>();

            this.RightHandItemTransform = this.PlayerModel.gameObject.transform.Find(GameIndex.PLAYER_ATTACH_IN_RIGHT_HAND);
            this.LeftHandItemTransform  = this.PlayerModel.gameObject.transform.Find(GameIndex.PLAYER_ATTACH_IN_LEFT_HAND);
            this.LeftHandItemTransform.gameObject.SetActive(false);

            Component[] components = this.PlayerModel.GetComponents(typeof(Component));

            for (int i = 0; i < components.Length; i++)
            {
                var castedToBehaviour = components[i] as MonoBehaviour;
                if (castedToBehaviour != null)
                {
                    castedToBehaviour.enabled = false;
                }
            }

            /*
            foreach (var component in this.PlayerModel.GetComponents<Component>())
            {
                MonoBehaviour castedToBehaviour = component as MonoBehaviour;
                if (castedToBehaviour != null)
                {
                    castedToBehaviour.enabled = false;
                }
            }*/

            foreach (Transform child in this.RightHandItemTransform)
            {
                if (!child.gameObject.name.Contains("attach1_"))
                {
                    UnityEngine.Object.Destroy(child.gameObject);
                }
            }

            this.CreateObstacle();
            this.CreateCapsuleCollider();
            this.CreatePingInstance();
            this.CreateEcoTarget();
            
            this.IsCreatedModel = true;
            this.PlayerModel.SetActive(true);

            return true;
        }

        /**
         *
         * Mevcut oyuncu yüklendiğinde tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void OnCurrentPlayerLoaded()
        {
            this.PlayerModel.SetIdentityId(this.UniqueId);
        }

        /**
         *
         * Selfie modunu sıfırlar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void ResetSelfieMode()
        {
            if (this.IsMine)
            {
                this.Main.playerAnimator.SetBool("selfies", false);
            }
            else
            {
                this.Animator?.SetBool("selfies", false);
            }
        }

        /**
         *
         * Oyuncunun bulunduğu su havuzunu döner.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public string GetCurrentWaterParkUniqueId()
        {
            if (this.IsMine)
            {
                if (this.Main.currentWaterPark == null)
                {
                    return null;
                }

                var baseDeconstructable = this.Main.currentWaterPark.GetBaseDeconstructable();
                if (baseDeconstructable == null)
                {
                    return null;
                }

                return baseDeconstructable.gameObject.GetIdentityId();
            }

            return null;
        }





















        /**
         *
         * İfadeleri sıfırlar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void ResetEmotes()
        {
            this.EmoteIndex = 0.0f;

            if (this.Animator)
            {
                this.Animator.SetFloat("FP_Emotes", 0.0f);
            }
        }

        /**
         *
         * Oyuncu animasyonlarını temizler.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void ResetAnimations()
        {
            if (this.Animator)
            {
                foreach (var animation in this.Animations)
                {
                    SafeAnimator.SetBool(this.Animator, animation.Key, false);
                }
            }

            this.ResetEmotes();
            this.ResetSelfieMode();
            this.ClearAnimationQueue();
        }

        /**
         *
         * LastHypnotizeTime değerini değiştirir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void SetLastHypnotizeTime(float time)
        {
            this.LastHypnotizeTime = time;
        }

        /**
         *
         * UsingRoomId değerini değiştirir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void SetUsingRoomId(string roomId)
        {
            this.UsingRoomId = roomId;
        }

        /**
         *
         * Hareket etmeyi aktifleştirir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void EnableMovement()
        {
            this.IsMovementActive = true;
        }

        /**
         *
         * Hareket etmeyi pasifleştirir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void DisableMovement()
        {
            this.IsMovementActive = false;
        }

        /**
         *
         * Cinematik modu aktifleştirir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void EnableCinematicMode()
        {
            this.IsCinematicModeActive = true;
        }

        /**
         *
         * Cinematik modu pasifleştirir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void DisableCinematicMode()
        {
            this.IsCinematicModeActive = false;
        }

        /**
         *
         * Donmayı aktifleştirir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void EnableFreeze(float time = -1f)
        {
            this.IsFrozen = true;
            
            if (this.FrozenOverlay)
            {
                this.FrozenOverlay.RemoveOverlay();
            }

            this.FrozenOverlay = this.PlayerModel.AddComponent<VFXOverlayMaterial>();
            this.FrozenOverlay.ApplyAndForgetOverlay(global::Player.main.frozenMixin.iceMaterial, "VFXOverlay: Frozen", Color.clear, time, this.GetRenderers(true));

            this.Animator.SetBool(AnimatorHashID.frozen, true);
            this.Animator.speed = 0.0f;
        }

        /**
         *
         * Donmayı pasifleştirir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void DisableFreeze()
        {
            this.IsFrozen = false;

            if (this.FrozenOverlay)
            {
                this.FrozenOverlay.RemoveOverlay();
            }

            this.Animator.SetBool(AnimatorHashID.frozen, false);
            this.Animator.speed = 1f;
        }

        /**
         *
         * Ebeveyne mesaj gönderir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void SendMessageToParent(string eventName, object value = null)
        {
            if (this.PlayerModel)
            {
                if (value == null)
                {
                    this.PlayerModel.SendMessageUpwards(eventName);
                }
                else
                {
                    this.PlayerModel.SendMessageUpwards(eventName, value);
                }
            }
        }

        /**
         *
         * Ebeveyni değiştirir
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void SetParent(Transform parent, bool resetPositions = false)
        {
            if (this.PlayerModel)
            {
                this.PlayerModel.transform.parent = parent;

                if (resetPositions)
                {
                    this.PlayerModel.transform.localPosition = Vector3.zero;
                    this.PlayerModel.transform.localRotation = Quaternion.identity;
                }
            }
        }

        /**
         *
         * Oyuncu Animasyon kuyruğunu varsayılana çeker.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void ClearAnimationQueue()
        {
            this.AnimationQueue.Clear();
        }

        /**
         *
         * Oyuncu elindeki eşyanın bileşenini günceller.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void SetHandItemComponent(NetworkPlayerItemComponent component)
        {
            this.HandItemComponent = component;
        }

        /**
         *
         * Oyuncu kamera açısını günceller.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void SetCameraPitch(float cameraPitch)
        {
            this.CameraPitch = cameraPitch;
        }

        /**
         *
         * Oyuncu kamera açısını günceller.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void SetCameraForward(Vector3 cameraForward)
        {
            this.CameraForward = cameraForward;
        }

        /**
         *
         * Animasyon hızını anında yapar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void InstantyAnimationMode()
        {
            if (this.Animator)
            {
                this.Animator.speed = 999f;
            }
        }

        /**
         *
         * Animasyon hızını normal yapar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void NormalAnimationMode()
        {
            if (this.Animator)
            {
                this.Animator.speed = 1f;
            }
        }
        
        /**
         *
         * Sınıfı mevcut oyuncu olarak günceller.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void SetCurrentPlayer(ZeroPlayer player)
        {
            CurrentPlayer = player;
        }

        /**
         *
         * Oyuncu adını değiştirir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void SetPlayerName(string nickName)
        {
            this.NickName = nickName;
        }

        /**
         *
         * Mevcut subroot id değerini değiştirir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void SetSubRootId(string subrootId)
        {
            this.CurrentSubRootId = subrootId;
        }

        /**
         *
         * Mevcut interior id değerini değiştirir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void SetInteriorId(string interiorId)
        {
            this.CurrentInteriorId = interiorId;
        }

        /**
         *
         * Mevcut yüzey türünü değiştirir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void SetSurfaceType(VFXSurfaceTypes surfaceType)
        {
            this.CurrentSurfaceType = surfaceType;
        }

        /**
         *
         * İlk kullanım animasyonunu çalıştırır/pasif hale getirir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void SetUsingToolFirstMode(bool isActive)
        {
            this.Animator?.SetBool("using_tool_first", isActive);
        }


        /**
         *
         * Animasyonun aktif olup olmadığına bakar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public bool IsAnimationActive(string animation)
        {
            return this.Animations.TryGetValue(animation, out bool value) && value;
        }

        /**
         *
         * Oyuncuyu gizler.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void Hide(bool instanty = true)
        {
            if (this.IsVisible && this.PlayerModel)
            {
                this.IsVisible = false;

                this.StopFading();

                if (instanty)
                {
                    this.PlayerModel.SetActive(this.IsVisible);
                }
                else
                {
                    this.StartFading(false);
                }
            }
        }

        /**
         *
         * Oyuncuyu gösterir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void Show(bool instanty = true)
        {
            if (!this.IsVisible && this.PlayerModel)
            {
                this.IsVisible = true;

                if (this.Position != null)
                {
                    this.PlayerModel.transform.position = this.Position;
                    this.PlayerModel.transform.rotation = this.Rotation;
                }

                this.StopFading();

                if (instanty)
                {
                    this.SetOpacity(1f);
                    this.PlayerModel.SetActive(this.IsVisible);
                }
                else
                {
                    this.StartFading(true);
                }
            }
        }

        /**
         *
         * Gizlenmeyi iptal eder.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private void StopFading()
        {
            if (this.FadeCoroutine != null)
            {
                UWE.CoroutineHost.StopCoroutine(this.FadeCoroutine);

                this.FadeCoroutine = null;
            }
        }

        /**
         *
         * Gizlenmeyi başlatır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private void StartFading(bool isShow)
        {
            this.FadeCoroutine = UWE.CoroutineHost.StartCoroutine(this.StartFadingAsync(this.FadeTime, isShow));
        }

        /**
         *
         * Gizlenmeyi başlatır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private IEnumerator StartFadingAsync(float fadeTime, bool isShow)
        {
            if (isShow)
            {
                this.PlayerModel.SetActive(true);
            }

            while (fadeTime > 0f)
            {
                fadeTime -= Time.unscaledDeltaTime;

                var opacity = Mathf.Clamp01(fadeTime / this.FadeTime);

                if (isShow)
                {
                    opacity = 1f - opacity;
                }

                this.SetOpacity(opacity);

                yield return UWE.CoroutineUtils.waitForNextFrame;
            }

            if (!isShow)
            {
                this.PlayerModel.SetActive(false);
            }
        }

        /**
         *
         * Oyuncu saydamlığını değiştirir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void SetOpacity(float opacity)
        {
            if (opacity <= 0.0001f)
            {
                opacity = 0.0001f;
            }

            if (opacity > 1f)
            {
                opacity = 1f;
            }

            foreach (var fadeRenderer in this.GetRenderers(false))
            {
                fadeRenderer.fadeAmount = opacity;
            }
        }


        /**
         *
         * Render'ları döner.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public Renderer[] GetRenderers(bool isOnlyPlayer)
        {
            if (isOnlyPlayer == false)
            {
                return this.PlayerModel.GetComponentsInChildren<Renderer>(true);
            }

            var renderers = new List<Renderer>();

            foreach (var item in this.PlayerModel.GetComponentsInChildren<Renderer>(true))
            {
                if (item.name.Contains("female_base_"))
                {
                    renderers.Add(item);
                }
            }

            return renderers.ToArray();
        }
        
        /**
         *
         * Bina yıkma oyuncu çarpışmalarını ayarlar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private bool CreateObstacle()
        {
            this.PlayerModel.AddComponent<PlayerObstacle>();
            return true;
        }

        /**
         *
         * Çarpışma detaylarını ayarlar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private bool CreateCapsuleCollider()
        {
            CapsuleCollider capsuleCollider = global::Player.mainCollider as CapsuleCollider;
            if (capsuleCollider == null)
            {
                return false;
            }

            var collider = this.PlayerModel.AddComponent<CapsuleCollider>();
            collider.center        = Vector3.up;
            collider.radius        = capsuleCollider.radius;
            collider.direction     = capsuleCollider.direction;
            collider.contactOffset = capsuleCollider.contactOffset;
            collider.isTrigger     = true;
            return true;
        }

        /**
         *
         * Oyuncu konum bildirim nesnesi oluşturur.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private void CreatePingInstance()
        {
            this.PingInstance = this.PlayerModel.AddComponent<PingInstance>();
            this.PingInstance.name    = ZeroPlayer.PlayerSignalName;
            this.PingInstance.origin  = this.PlayerModel.transform;
            this.PingInstance.minDist = 5f;
            this.PingInstance.range   = 1f;
            this.PingInstance.SetLabel(this.NickName);
          //  this.PingInstance.SetLabel("BOT Lily");
            this.PingInstance.SetType(PingType.Signal);
        }

        /**
         *
         * EcoTarget detaylarını ayarlar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private bool CreateEcoTarget()
        {
            if (global::Player.main.TryGetComponent<EcoTarget>(out var ecoTarget))
            {
                this.PlayerModel.AddComponent<EcoTarget>().SetTargetType(ecoTarget.GetTargetType());
                return true;
            }

            return true;
        }

        /**
         *
         * Oyuncu sınıfını yok eder.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static bool Destroy(string playerUniqueId)
        {
            var player = GetPlayerByUniqueId(playerUniqueId);

            return Destroy(player);
        }

        /**
         *
         * Oyuncu sınıfını yok eder.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static bool Destroy(ZeroPlayer player)
        {
            if (player == null)
            {
                return false;
            }

            player.IsDestroyed = true;

            if (player.PlayerObject != null)
            {
                GameObject.DestroyImmediate(player.PlayerObject);
            }

            if (player.PlayerModel != null)
            {
                GameObject.DestroyImmediate(player.PlayerModel);
            }

            Players.Remove(player);
            return true;
        }

        /**
         *
         * Oyuncu Modeli
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private GameObject _PlayerModel;

        /**
         *
         * Mevcut Oyuncu
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private global::Player _Main;

        /**
         *
         * FreecamController Değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private FreecamController _FreeCamController;

        /**
         *
         * Oyuncu Modeli
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public GameObject PlayerModel
        {
            get
            {
                if (this.IsMine)
                {
                    return global::Player.main.gameObject;
                }

                return this._PlayerModel;
            }
            set
            {
                if (this.IsMine)
                {
                    throw new Exception("Don't, Not Possible!");
                }

                this._PlayerModel = value;
            }
        }

        /**
         *
         * Mevcut Oyuncu
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public global::Player Main 
        { 
            get
            {
                if (this.IsMine == false)
                {
                    return null;
                }

                if (this._Main == null)
                {
                    this._Main = global::Player.main;
                }

                return this._Main;
            }
        }

        /**
         *
         * Mevcut Oyuncu
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public FreecamController FreecamController
        { 
            get
            {
                if (this.IsMine == false)
                {
                    return null;
                }

                if (this._FreeCamController == null && MainCameraControl.main)
                {
                    this._FreeCamController = MainCameraControl.main.GetComponent<FreecamController>();
                }

                return this._FreeCamController;
            }
        }

        /**
         *
         * Oyuncu Hızını barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public Vector3 Velocity { private get; set; }































        /**
         *
         * Oyuncuların listesini tutar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private static HashSet<ZeroPlayer> Players { get; set; } = new HashSet<ZeroPlayer>();

        /**
         *
         * Mevcut Oyuncu
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static ZeroPlayer CurrentPlayer { get; set; } = null;

        /**
         *
         * Sağ El Transform Nesnesi
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public Transform RightHandItemTransform { get; set; } = null;

        /**
         *
         * Sol Transform Nesnesi
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public Transform LeftHandItemTransform { get; set; } = null;

        /**
         *
         * FrozenOverlay Nesnesi
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private VFXOverlayMaterial FrozenOverlay  { get; set; }

        /**
         *
         * Oyuncu Id
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public byte PlayerId { get; set; }

        /**
         *
         * Benzersiz Oyuncu Id
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public string UniqueId { get; set; }

        /**
         *
         * EmoteIndex Değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public float EmoteIndex { get; set; }

        /**
         *
         * Kullanıcının Ben olup olmadığını döner.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public bool IsMine { get; set; }

        /**
         *
         * Oyuncu gizlilik durumu
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private bool IsVisible { get; set; } = true;

        /**
         *
         * Su altında olup olmadığını barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public bool IsUnderwater { get; set; }

        /**
         *
         * Su yüzeyine yakın olup olmadığını barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public bool IsOnSurface { get; set; }

        /**
         *
         * SeaTruck içinde olup olmadığını barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public bool IsInSeaTruck { get; set; }

        /**
         *
         * Oyuncu Id Kimliği
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public string CurrentServerId { get; set; }

        /**
         *
         * Mevcut SubRoot Id
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public string CurrentSubRootId { get; set; }

        /**
         *
         * Mevcut Interior Id
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public string CurrentInteriorId { get; set; }

        /**
         *
         * CurrentSurfaceType Değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public VFXSurfaceTypes CurrentSurfaceType { get; set; }

        /**
         *
         * Oyuncu donmuş mu?
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public bool IsFrozen { get; private set; }

        /**
         *
         * Sinematik Mod Aktiflik Durumu
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public bool IsCinematicModeActive { get; private set; }

        /**
         *
         * Hikaye Sinematik Mod Aktiflik Durumu
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public bool IsStoryCinematicModeActive { get; set; }

        /**
         *
         * Oyuncu nesnesi
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public GameObject PlayerObject { get; set; }

        /**
         *
         * Animator Sınıfı
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public Animator Animator { get; set; }

        /**
         *
         * Oyuncu Sinyali
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public PingInstance PingInstance { get; set; }

        /**
         *
         * Oyuncu Posizyonu
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public Vector3 Position { get; set; } = new Vector3();

        /**
         *
         * Oyuncu Açısı
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public Quaternion Rotation { get; set; } = new Quaternion();

        /**
         *
         * Eldeki teknoloji türü
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public TechType TechTypeInHand { get; set; }

        /**
         *
         * Model Oluşturuldu mu?
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public bool IsCreatedModel { get; set; } = false;

        /**
         *
         * Yok edildi mi?
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public bool IsDestroyed { get; set; } = false;

        /**
         *
         * Oyuncu Adı
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public string NickName { get; set; }

        /**
         *
         * Araç ID numarası
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public ushort VehicleId { get; set; }

        /**
         *
         * Araç Türü
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public TechType VehicleType { get; set; }

        /**
         *
         * Oyuncu Araç Posizyonu
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public Vector3 VehiclePosition { get; set; } = new Vector3();

        /**
         *
         * Oyuncu Araç Açısı
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public Quaternion VehicleRotation { get; set; } = new Quaternion();

        /**
         *
         * Oyuncu Araç Bileşeni
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public VehicleUpdateComponent VehicleComponent { get; set; }

        /**
         *
         * Oyuncu elindeki eşya Bileşeni
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public NetworkPlayerItemComponent HandItemComponent { get; set; }

        /**
         *
         * Oyuncu Sağ elindeki eşya açısı
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public Quaternion RightHandItemRotation { get; set; }

        /**
         *
         * Oyuncu Sol elindeki eşya açısı
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public Quaternion LeftHandItemRotation { get; set; }

        /**
         *
         * Oyuncu kamera açısını barındırır
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public float CameraPitch { get; set; }

        /**
         *
         * Oyuncu kamera yönünü barındırır
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public Vector3 CameraForward { get; set; }

        /**
         *
         * IsPrecursorArm değerini barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public bool IsPrecursorArm { get; set; }

        /**
         *
         * Araç rıhtıma yanaştırılıyor mu?
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public bool IsVehicleDocking { get; set; }

        /**
         *
         * Şuanki cinematic Id
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public string CurrentCinematicUniqueId { get; set; }

        /**
         *
         * Kullanılan Oda Numarası
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public string UsingRoomId { get; set; }

        /**
         *
         * Hareket Etme Aktif mi?
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public bool IsMovementActive { get; set; } = true;

        /**
         *
         * FadeTime değerini barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public float FadeTime { get; private set; } = 1f;

        /**
         *
         * LastHypnotizeTime değerini barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public float LastHypnotizeTime { get; private set; }

        /**
         *
         * FadeCoroutine değerini barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private Coroutine FadeCoroutine { get; set; }

        /**
         *
         * Oyuncu Animasyon durumlarını barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public Dictionary<string, bool> Animations { get; private set; } = new Dictionary<string, bool>();

        /**
         *
         * Oyuncu Animasyon kuyruğunu barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public Queue<Dictionary<string, bool>> AnimationQueue { get; private set; } = new Queue<Dictionary<string, bool>>();

        /**
         *
         * Ekipmanları barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public List<TechType> Equipments { get; set; } = new List<TechType>()
        {
            TechType.None,
            TechType.None,
            TechType.None,
            TechType.None,
            TechType.None,
        };
    }
}
