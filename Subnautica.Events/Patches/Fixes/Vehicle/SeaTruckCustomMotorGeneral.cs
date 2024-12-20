namespace Subnautica.Events.Patches.Fixes.Vehicle
{
    using System;
    using System.Collections;

    using HarmonyLib;

    using Subnautica.API.Extensions;
    using Subnautica.API.Features;
    using Subnautica.Events.EventArgs;

    using UnityEngine;

    [HarmonyPatch]
    public class SeaTruckCustomMotorGeneral
    {
        [HarmonyPrefix]
        [HarmonyPatch(typeof(GroundMotor), nameof(GroundMotor.UpdateFunction))]
        private static bool GroundMotor_UpdateFunction(global::GroundMotor __instance)
        {
            if (Network.IsMultiplayerActive && Network.Session.IsInSeaTruck)
            {
                __instance.playerController.SetEnabled(false);
                return false;
            }

            return true;
        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(global::SeaTruckSegment), nameof(global::SeaTruckSegment.EnterHatch))]
        private static bool SeaTruckSegment_EnterHatch(global::SeaTruckSegment __instance, global::Player player)
        {
            if (Network.IsMultiplayerActive)
            {
                __instance.Enter(player);
                Utils.PlayFMODAsset(__instance.enterSound, player.transform);
                return false;
            }

            return true;
        }      

        [HarmonyPrefix]
        [HarmonyPatch(typeof(global::SeaTruckSegment), nameof(global::SeaTruckSegment.CanAnimate))]
        private static bool SeaTruckSegment_CanAnimate()
        {
            return !Network.IsMultiplayerActive;
        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(global::SeaTruckSegment), nameof(global::SeaTruckSegment.IsWalkable))]
        private static bool SeaTruckSegment_IsWalkable(ref bool __result)
        {
            if (!Network.IsMultiplayerActive)
            {
                return true;
            }

            __result = true;
            return false;
        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(global::GroundMotor), nameof(global::GroundMotor.IsGroundedTest))]
        private static bool GroundMotor_IsGroundedTest(ref bool __result)
        {
            if (!Network.IsMultiplayerActive || !Network.Session.IsInSeaTruck)
            {
                return true;
            }

            __result = true;
            return false;
        }
//as
        [HarmonyPrefix]
        [HarmonyPatch(typeof(global::ArmsController), nameof(global::ArmsController.SetPlayerSpeedParameters))]
        private static bool ArmsController_SetPlayerSpeedParameters(global::SeaTruckSegment __instance)
        {
            return Network.IsMultiplayerActive && Network.Session.IsInSeaTruck ? false : true;
        }

        [HarmonyPostfix]
        [HarmonyPatch(typeof(global::SeaTruckSegment), nameof(global::SeaTruckSegment.Enter))]
        private static void SeaTruckSegment_Enter(global::SeaTruckSegment __instance)
        {
            if (Network.IsMultiplayerActive && !Tools.IsInStackTrace("OnUndockingComplete"))
            {
                var expansionManager = __instance.GetFirstSegment()?.GetDockedMoonpoolExpansion();
                if (expansionManager == null || Tools.IsInStackTrace("StartUndocking"))
                {
                    ChangeSeaTruckMode(__instance, true);
                }
            }
        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(global::Player), nameof(global::Player.EnterInterior))]
        private static void Player_EnterInterior(global::Player __instance, IInteriorSpace interior)
        {
            if (Network.IsMultiplayerActive && !Tools.IsInStackTrace("OnUndockingComplete") && interior != __instance.currentInterior && interior != null && interior is global::SeaTruckSegment seaTruckSegment)
            {
                var expansionManager = seaTruckSegment.GetFirstSegment()?.GetDockedMoonpoolExpansion();
                if (expansionManager == null)
                {
                    ChangeSeaTruckMode(seaTruckSegment, true);
                }
            }
        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(global::SeaTruckSegment), nameof(global::SeaTruckSegment.Exit))]
        private static void SeaTruckSegment_Exit(global::SeaTruckSegment __instance)
        {
            if (Network.IsMultiplayerActive)
            {
                ChangeSeaTruckMode(__instance, false);
            }
        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(global::MoonpoolExpansionManager), nameof(global::MoonpoolExpansionManager.Update))]
        private static void MoonpoolExpansionManager_Exit(global::MoonpoolExpansionManager __instance)
        {
            if (Network.IsMultiplayerActive)
            {
                if (__instance.IsOccupied() && __instance.inGetUpAnimation &&  !__instance.dockedHead.motor.IsBusyAnimating())
                {
                    ChangeSeaTruckMode(__instance.dockedHead, false);
                }
            }
        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(global::SeaTruckMotor), nameof(global::SeaTruckMotor.SetPiloting))]
        private static void SeaTruckMotor_SetPilotxxing(global::SeaTruckMotor __instance, bool piloting)
        {
            if (Network.IsMultiplayerActive && !Tools.IsInStackTrace("OnUndockingComplete"))
            {
                ChangeSeaTruckMode(__instance.truckSegment, piloting, !piloting);
            }
        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(global::SeaTruckTeleporter), nameof(global::SeaTruckTeleporter.PlayTeleportAnimation))]
        private static void SeaTruckTeleporter_PlayTeleportAnimation(global::SeaTruckTeleporter __instance)
        {
            if (Network.IsMultiplayerActive)
            {
                UWE.CoroutineHost.StartCoroutine(SeaTruckCustomMotorGeneral.TeleportationEndCheck(__instance.truckSegment));
            }
        }

        [HarmonyPostfix]
        [HarmonyPatch(typeof(global::PlayerCinematicController), nameof(global::PlayerCinematicController.EndCinematicMode))]
        private static void PlayerCinematicController_EndCinematicMode(global::PlayerCinematicController __instance)
        {
            if (Network.IsMultiplayerActive && global::Player.main && global::Player.main.gameObject.TryGetComponent<SeaTruckCustomMotor>(out var motor))
            {
                motor.OnPlayerCinematicModeEnd(__instance);
            }
        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(global::SeaTruckAnimation), nameof(global::SeaTruckAnimation.OnPlayerCinematicModeEnd))]
        private static void SeaTruckAnimation_OnPlayerCinematicModeEnd(global::SeaTruckAnimation __instance)
        {
            if (Network.IsMultiplayerActive)
            {
                if (__instance.currentAnimation == SeaTruckAnimation.Animation.EndPilot)
                {
                    ChangeSeaTruckMode(__instance.GetComponentInParent<global::SeaTruckSegment>(), true);
                }
            }
        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(global::Player), nameof(global::Player.GetQuickSlotsParent))]
        private static bool Player_GetQuickSlotsParent(global::Player __instance, ref IQuickSlots __result)
        {
            if (!Network.IsMultiplayerActive || !Network.Session.IsInSeaTruck || global::Player.main.inSeatruckPilotingChair)
            {
                return true;
            }

            __result = __instance.GetComponentInParent<QuickSlots>();
            return false;
        }

        [HarmonyPostfix]
        [HarmonyPatch(typeof(global::SeaTruckSegment), nameof(global::SeaTruckSegment.OnKill))]
        private static void SeaTruckSegment_OnKill(global::SeaTruckSegment __instance)
        {
            if (Network.IsMultiplayerActive)
            {
                var uniqueId = __instance.gameObject.GetIdentityId();
                if (uniqueId.IsNotNull())
                {
                    global::Player.main.gameObject.EnsureComponent<SeaTruckCustomMotor>().OnCustomKill(uniqueId);
                }
            }
        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(global::SeaTruckMotor), nameof(global::SeaTruckMotor.StartPiloting))]
        private static void SeaTruckMotor_StartPiloting(global::SeaTruckMotor __instance)
        {
            if (Network.IsMultiplayerActive)
            {
                SeaTruckCustomMotorGeneral.ChangeSeaTruckMode(__instance.truckSegment, false);

                __instance.truckSegment.rb.SetNonKinematic();
            }
        }

        /**
         *
         * SeaTruck modunu değiştirir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void ChangeSeaTruckMode(global::SeaTruckSegment seaTruck, bool isInSeaTruck, bool isKeepParent = false)
        {
            Network.Session.IsInSeaTruck = isInSeaTruck;

            if (Network.Session.IsInSeaTruck)
            {
                if (!global::Player.main.inSeatruckPilotingChair)
                {
                    global::Player.main.gameObject.EnsureComponent<SeaTruckCustomMotor>().SetSeaTruckSegment(seaTruck).SetActive(true, isKeepParent: isKeepParent);
                }
            }
            else
            {
                global::Player.main.gameObject.EnsureComponent<SeaTruckCustomMotor>().SetSeaTruckSegment(null).SetActive(false, isKeepParent: isKeepParent);
            }
        }

        /**
         *
         * Işınlanma tamamlanma durumunu kontrol eder.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private static IEnumerator TeleportationEndCheck(global::SeaTruckSegment seaTruckSegment)
        {
            var timing = new StopwatchItem(6000f);
            timing.Restart();

            while (!timing.IsFinished())
            {
                yield return UWE.CoroutineUtils.waitForNextFrame;

                if (!global::Player.main.IsAlive())
                {
                    yield break;
                }

                if (!global::Player.main.cinematicModeActive)
                {
                    if (seaTruckSegment)
                    {
                        seaTruckSegment.Enter(global::Player.main);
                    }

                    yield break;
                }
            }
        }
    }

    public class SeaTruckCustomMotor : MonoBehaviour
    {
        /**
         *
         * Oyuncu sınıfını barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public global::Player Player { get; set; }

        /**
         *
         * Ana Kamerayı sınıfını barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public global::MainCameraControl MainCamera { get; set; }

        /**
         *
         * Mevcut SeaTruckSegment sınıfını barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public global::SeaTruckSegment SeaTruckSegment { get; set; }

        /**
         *
         * Mevcut yön.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public Vector3 Direction = Vector3.zero;

        /**
         *
         * Yumuşatılmış yön.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public Vector3 SmoothedDirection = Vector3.zero;

        /**
         *
         * Yeniden doğma zamanlayıcısını barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public StopwatchItem RespawnTiming { get; set; } = new StopwatchItem(2000f);

        /**
         *
         * En son üzerinde bulunan ilk modül id
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public string LastFirstSegmentModuleId { get; set; }

        /**
         *
         * Hızı barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public float Speed { get; set; } = 4f;

        /**
         *
         * Hızı barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public float SprintSpeed { get; set; } = 6f;

        /**
         *
         * IsActive durumu.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public bool IsActive { get; set; } = false;

        /**
         *
         * Sınıf ayarlamalarını yapar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void Awake()
        {
            this.enabled = false;

            this.Player     = global::Player.main;
            this.MainCamera = global::MainCameraControl.main;
            this.Player.playerRespawnEvent.AddHandler(this.gameObject, new UWE.Event<global::Player>.HandleFunction(this.OnRespawn));
        }

        /**
         *
         * Araç patlayınca tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void OnCustomKill(string vehicleId)
        {
            if (this.IsActive && this.SeaTruckSegment)
            {
                var uniqueId = this.SeaTruckSegment.gameObject.GetIdentityId();
                if (uniqueId.IsNotNull() && vehicleId == uniqueId)
                {
                    this.SetSeaTruckSegment(null);
                    this.SetActive(false);

                    Network.Session.IsInSeaTruck = false;
                }
                else
                {
                    this.UpdateSeaTruckModule();
                }
            }
        }

        /**
         *
         * Oyuncu doğduğunda tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void OnRespawn(global::Player p)
        {
            UWE.CoroutineHost.StartCoroutine(this.OnRespawnAsync());
        }

        /**
         *
         * Cinematik bittiğinde tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void OnPlayerCinematicModeEnd(PlayerCinematicController sender)
        {
            if (this.IsActive && Network.Session.IsInSeaTruck && sender.name.Contains("seatruck_module_sleeper_anim"))
            {
                UWE.CoroutineHost.StartCoroutine(this.CheckPlayerKinematic());
            }
        }

        /**
         *
         * SeaTruckSegment değerini değiştirir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public SeaTruckCustomMotor SetSeaTruckSegment(global::SeaTruckSegment seaTruckSegment)
        {
            this.SeaTruckSegment = seaTruckSegment;
            return this;
        }

        /**
         *
         * Her karede tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void Update()
        {
            if (!this.Player.inSeatruckPilotingChair)
            {
                if (FPSInputModule.current.lockMovement)
                {
                    this.Direction = Vector3.zero;
                }
                else
                {
                    this.Direction = GameInput.GetMoveDirection() * this.GetSpeed();
                    this.Direction.Set(this.Direction.x, 0f, this.Direction.z);
                    this.Direction = this.MainCamera.viewModel.transform.TransformDirection(this.Direction);

                    this.MovePlayer(this.Direction);

                    this.Player.transform.localPosition = new Vector3(this.Player.transform.localPosition.x, 0.01f, this.Player.transform.localPosition.z);
                }

                this.SetPlayerSpeedParameters();
            }
        }

        /**
         *
         * Oyuncuyu hareket ettirir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private void MovePlayer(Vector3 direction)
        {
            this.Player.groundMotor.controller.enabled = true;
            this.Player.groundMotor.controller.Move(direction * Time.deltaTime);
            this.Player.groundMotor.controller.enabled = false;
        }

        /**
         *
         * Oyuncu Hızını döner.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private float GetSpeed()
        {
            return GameInput.GetIsRunning() ? this.SprintSpeed : this.Speed;
        }

        /**
         *
         * Her sabit karede tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void FixedUpdate()
        {
            if (this.Player.transform.parent && !this.Player.inSeatruckPilotingChair)
            {
                if (this.Player.rigidBody.isKinematic)
                {
                    this.SetActive(true, true);
                }

                this.UpdateSeaTruckModule();
            }
        }

        /**
         *
         * Oyuncunun mevcut modülünü değiştirir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private bool UpdateSeaTruckModule()
        {
            if (!Physics.Raycast(this.Player.transform.position, this.MainCamera.viewModel.transform.TransformDirection(Vector3.down), out var hit, 3f))
            {
                return false;
            }

            var seaTruckSegment = hit.collider.gameObject.GetComponentInParent<global::SeaTruckSegment>();
            if (seaTruckSegment == false)
            {
                return false;
            }

            var firstSegment = seaTruckSegment.GetFirstSegment();
            if (firstSegment == null)
            {
                return false;
            }

            var moduleId = firstSegment.gameObject.GetIdentityId();
            
            if (this.SeaTruckSegment && firstSegment != this.SeaTruckSegment && firstSegment.isMainCab && this.SeaTruckSegment.isMainCab)
            {
                return false;
            }

            this.SeaTruckSegment = seaTruckSegment;

            if (this.LastFirstSegmentModuleId != moduleId)
            {
                this.UpdateVehicleInterior(firstSegment, true);

                UWE.CoroutineHost.StartCoroutine(this.CheckPlayerSegmentPosition());
            }

            return true;
        }

        /**
         *
         * Bağlanan araç içerisindeki oyuncu konumunu senkronlar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private IEnumerator CheckPlayerSegmentPosition()
        {
            while (this.IsUpdateDocking(this.SeaTruckSegment))
            {
                yield return UWE.CoroutineUtils.waitForNextFrame;

                if (this.SeaTruckSegment)
                {
                    this.Player.transform.localPosition = this.SeaTruckSegment.GetFirstSegment().transform.InverseTransformPoint(this.SeaTruckSegment.enterPosition.transform.position);
                }
            }
        }

        /**
         *
         * Oyuncu kinematic durumunu senkronlar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private IEnumerator CheckPlayerKinematic()
        {
            var timing = new StopwatchItem(150f);

            while (!timing.IsFinished())
            {
                if (this.Player.rigidBody.isKinematic)
                {
                    this.SetActive(true, isForce: true, isKeepPosition: true);
                }

                yield return UWE.CoroutineUtils.waitForNextFrame;
            }
        }

        /**
         *
         * Modülün bağlanma durumunu kontrol eder.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private bool IsUpdateDocking(global::SeaTruckSegment currentSegment)
        {
            if (currentSegment.updateDockedPosition)
            {
                return true;
            }

            var seaTruckSegment = currentSegment;
            while (seaTruckSegment)
            {
                if (!seaTruckSegment.isFrontConnected)
                {
                    return seaTruckSegment.updateDockedPosition;
                }

                seaTruckSegment = seaTruckSegment.frontConnection.GetConnection().truckSegment;

                if (seaTruckSegment.updateDockedPosition)
                {
                    return true;
                }
            }

            return false;
        }

        /**
         *
         * YÜrüme/Hız/Açı ayarlamalarını yapar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private void SetPlayerSpeedParameters()
        {
            var relativeVelocity = this.MainCamera.viewModel.transform.InverseTransformDirection(this.Player.groundMotor.controller.velocity);

            this.SmoothedDirection = Vector3.Slerp(this.SmoothedDirection, relativeVelocity, this.GetSpeed() * Time.deltaTime);

            this.Player.armsController.animator.SetFloat(AnimatorHashID.move_speed, this.SmoothedDirection.magnitude);
            this.Player.armsController.animator.SetFloat(AnimatorHashID.move_speed_x, this.SmoothedDirection.x);
            this.Player.armsController.animator.SetFloat(AnimatorHashID.move_speed_y, this.SmoothedDirection.y);
            this.Player.armsController.animator.SetFloat(AnimatorHashID.move_speed_z, this.SmoothedDirection.z);

            SafeAnimator.SetFloat(this.Player.armsController.animator, "view_pitch", this.MainCamera.GetCameraPitch());

            var yaw = this.MainCamera.cameraOffsetTransform.localEulerAngles.y;
            UWE.Utils.MakeAngleInCCWBounds(ref yaw, -180f, 180f);
            this.Player.armsController.animator.SetFloat("view_yaw", yaw);

            if (Time.deltaTime <= 0.0f || this.MainCamera.viewModel == null)
            {
                return;
            }

            var angleY = this.MainCamera.viewModel.eulerAngles.y;

            this.Player.armsController.animator.SetFloat("view_turn", Mathf.DeltaAngle(this.Player.armsController.previousYAngle, angleY) / Time.deltaTime, this.Player.armsController.turnAnimationDampTime, Time.deltaTime);
            this.Player.armsController.previousYAngle = angleY;
        }

        /**
         *
         * Araç İç mekan id'sini günceller.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private void UpdateVehicleInterior(global::SeaTruckSegment firstSegment, bool callEvent = false)
        {
            this.LastFirstSegmentModuleId = firstSegment.gameObject.GetIdentityId();
            
            this.Player.transform.SetParent(firstSegment.transform);

            if (callEvent)
            {
                try
                {
                    VehicleInteriorToggleEventArgs args = new VehicleInteriorToggleEventArgs(this.LastFirstSegmentModuleId, true);

                    Subnautica.Events.Handlers.Vehicle.OnInteriorToggle(args);
                }
                catch (Exception e)
                {
                    Log.Error($"CustomMotor.UpdateSeaTruckModule: {e}\n{e.StackTrace}");
                }               
            }
        }

        /**
         *
         * Async Oyuncu doğduğunda tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private IEnumerator OnRespawnAsync()
        {
            this.RespawnTiming.Restart();

            while (!this.RespawnTiming.IsFinished())
            {
                yield return UWE.CoroutineUtils.waitForNextFrame;

                if (this.Player.playerController.enabled)
                {
                    if (this.Player.currentInterior is global::SeaTruckSegment seaTruckSegment)
                    {
                        this.SetSeaTruckSegment(seaTruckSegment);
                        this.SetActive(true, true);

                        yield break;
                    }
                }
            }
        }
        
        /**
         *
         * Aktiflik durumunu değiştirir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public bool SetActive(bool isActive, bool isForce = false, bool isKeepParent = false, bool isKeepPosition = false)
        {
            if (!isForce && this.IsActive == isActive)
            {
                return false;
            }

            this.enabled = this.IsActive = Physics.autoSyncTransforms = isActive;

            if (isActive)
            {
                var firstSegment = this.SeaTruckSegment.GetFirstSegment();
                firstSegment.rb.SetKinematic();

                this.UpdateVehicleInterior(firstSegment, isForce);

                this.Player.playerController.SetEnabled(false);

                if (isKeepPosition)
                {
                    this.Player.transform.localPosition = firstSegment.transform.InverseTransformPoint(this.Player.transform.position);
                }
                else
                {
                    this.Player.transform.localPosition = firstSegment.transform.InverseTransformPoint(this.SeaTruckSegment.enterPosition.transform.position);
                }

                this.Player.rigidBody.detectCollisions = true;
                this.Player.rigidBody.SetNonKinematic(true);
                this.Player.GetComponent<CapsuleCollider>().enabled = true;

                this.Player.groundMotor.grounded = true;
                this.Player.groundMotor.movement.velocity = Vector3.zero;
            }
            else
            {
                this.LastFirstSegmentModuleId = null;

                this.Player.playerController.SetEnabled(true);

                if (!isKeepParent)
                {
                    this.Player.transform.SetParent(null);
                }

                this.Player.rigidBody.detectCollisions = false;
                this.Player.rigidBody.SetKinematic(true);
                this.Player.GetComponent<CapsuleCollider>().enabled = false;
            }

            return true;
        }
    }
}