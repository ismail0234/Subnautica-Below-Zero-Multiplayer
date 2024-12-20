namespace Subnautica.Client.MonoBehaviours.World
{
    using System;
    using System.Collections;
    using System.Collections.Generic;

    using Subnautica.API.Enums;
    using Subnautica.API.Extensions;
    using Subnautica.API.Features;
    using Subnautica.Client.Modules;
    using Subnautica.Network.Structures;

    using UnityEngine;

    using UWE;

    public class MultiplayerExpansionManager
    {
        /**
         *
         * DockingBay nesnesini barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public MultiplayerVehicleDockingBay DockingBay { get; set; }

        /**
         *
         * ExpansionManager nesnesini barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public MoonpoolExpansionManager ExpansionManager { get; set; }

        /**
         *
         * ExpansionManager nesnesini barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public global::Player Player { get; set; }

        /**
         *
         * Sınıf ayarlamalarını yapar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public MultiplayerExpansionManager(MultiplayerVehicleDockingBay dockingBay)
        {
            if (dockingBay)
            {
                this.Player           = global::Player.main;
                this.DockingBay       = dockingBay;
                this.ExpansionManager = dockingBay.VehicleDockingBay.expansionManager;
            }
        }

        /**
         *
         * Sınıf ayarlamalarını yapar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void OnAwake()
        {

        }

        /**
         *
         * Dünya yüklendikten sonra aracı hemen demirler.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void OnHandleLoading(bool isLoadingWorld = false)
        {
            if (isLoadingWorld)
            {
                if (this.DockingBay.TailId.IsNotNull())
                {
                    var tailModule = Network.Identifier.GetComponentByGameObject<global::SeaTruckSegment>(this.DockingBay.TailId, true);
                    if (tailModule)
                    {
                        this.DockTail(tailModule, true);
                    }
                }

                this.DockingBay.SetDockingTail(null);
            }
            else
            {
                this.ExpansionManager.timelineDirector.MultiplayerPlay();
                this.ExpansionManager.timelineDirector.time = this.ExpansionManager.timelineDirector.playableAsset.duration;
                this.ExpansionManager.isFullyDocked = false;
                this.ExpansionManager.OnDockingTimelineCompleted();
            }
        }

        /**
         *
         * Her karede tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void OnUpdate(bool nearby)
        {
            this.ExpansionManager.UpdateArmsAnim(nearby);
        }

        /**
         *
         * Expansion -> Seatruck kuyruk kenetlenme işlemi tamamlanırken tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void OnDockTail(global::SeaTruckSegment newTail)
        {
            this.DockTail(newTail);
        }

        /**
         *
         * Expansion -> Seatruck kuyruk ayrılma işlemi tamamlanırken tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void OnUndockTail(bool withEjection)
        {
            this.UndockTail(withEjection, false);
        }

        /**
         *
         * Modülü kenetler.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public bool DockTail(global::SeaTruckSegment newTail, bool isConnection = false, bool teleport = false)
        {
            if (newTail == null)
            {
                return false;
            }

            this.ExpansionManager.tail = newTail;
            this.ExpansionManager.tail.relay.dontConnectToRelays = false;
            this.ExpansionManager.tail.relay.AddInboundPower(this.ExpansionManager.bay.GetPowerSource());
            this.ExpansionManager.tail.rb.SetKinematic();
            this.ExpansionManager.HideSealGlass(true);

            if (!this.ExpansionManager.tail.isFrontConnected)
            {
                this.ExpansionManager.tail.UpdatePowerRelay();
            }

            using (EventBlocker.Create(ProcessType.SeaTruckConnection))
            {
                this.ExpansionManager.tail.frontConnection.Disconnect(); 
            }

            this.ExpansionManager.armsAnimator.SetBool(AnimatorHashID.plunger_override_retract, false);
            this.ExpansionManager.tail.motor.OnPilotModeChanged += new Action<SeaTruckSegment, bool>(this.ExpansionManager.OnPilotingChanged);
            this.ExpansionManager.tail.OnEnteredSeatruck.AddHandler(this.ExpansionManager, new Event<SeaTruckSegment>.HandleFunction(this.ExpansionManager.OnEnteredTail));
            this.ExpansionManager.defaultTailEnteringStandalone = this.ExpansionManager.tail.allowEnteringStandalone;
            this.ExpansionManager.tail.allowEnteringStandalone  = true;

            using (EventBlocker.Create(ProcessType.SeaTruckConnection))
            {
                this.ExpansionManager.moduleConnection.SetConnectedTo(this.ExpansionManager.tail.frontConnection);
            }

            this.UpdateTailColors();

            if (teleport)
            {
                this.ExpansionManager.tail.transform.position = this.ExpansionManager.tailDockingPosition.position;
                this.ExpansionManager.tail.transform.rotation = this.ExpansionManager.tailDockingPosition.rotation;

                if (this.ExpansionManager.moduleConnection.connection && this.ExpansionManager.moduleConnection.updateDockedPosition)
                {
                    this.ExpansionManager.moduleConnection.connection.truckSegment.transform.rotation = this.ExpansionManager.moduleConnection.connectionPoint.rotation;
                    this.ExpansionManager.moduleConnection.connection.truckSegment.transform.position = this.ExpansionManager.moduleConnection.connectionPoint.position - this.ExpansionManager.moduleConnection.connection.truckSegment.frontConnection.connectionPoint.position + this.ExpansionManager.moduleConnection.connection.truckSegment.transform.position;
                    this.ExpansionManager.moduleConnection.Update();
                }
            }

            if (isConnection)
            {
                if (this.Player.transform.parent && this.Player.transform.parent.TryGetComponent<global::SeaTruckSegment>(out var seaTruckSegment))
                {
                    var firstSegment = seaTruckSegment.GetFirstSegment();
                    if (firstSegment)
                    {
                        firstSegment.player = this.Player;
                        firstSegment.Exit(skipTeleport: true, newInterior: this.ExpansionManager.interior);
                    }
                }
            }

            return true;
        }

        /**
         *
         * Modül demirlemesini çözer.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public bool UndockTail(bool withEjection = false, bool supressEvent = true)
        {
            this.PlayerDisableMovementPlatform();

            if (supressEvent == false)
            {
                this.ExpansionManager.moduleConnection.Disconnect();
                return false;
            }

            if (this.ExpansionManager.tail)
            {
                this.ExpansionManager.tail.allowEnteringStandalone = this.ExpansionManager.defaultTailEnteringStandalone;
                this.ExpansionManager.tail.relay.RemoveInboundPower(this.ExpansionManager.bay.GetPowerSource());
                this.ExpansionManager.tail.relay.dontConnectToRelays = true;
                this.ExpansionManager.tail.UpdatePowerRelay();
                this.ExpansionManager.tail.motor.OnPilotModeChanged -= new Action<SeaTruckSegment, bool>(this.ExpansionManager.OnPilotingChanged);
                this.ExpansionManager.tail.OnEnteredSeatruck.RemoveHandler(this.ExpansionManager);

                using (EventBlocker.Create(ProcessType.SeaTruckConnection))
                {
                    this.ExpansionManager.moduleConnection.Disconnect();
                }

                if (this.ExpansionManager.isFullyDocked)
                {
                    this.ExpansionManager.HideSealGlass(false);
                }

                if (withEjection)
                {
                    this.ExpansionManager.tail.rb.AddForce(-this.ExpansionManager.tail.transform.forward.normalized * (this.ExpansionManager.exitForce * 0.9f), ForceMode.Impulse);
                }

                var expansionTail = this.ExpansionManager.tail;

                this.ExpansionManager.tail = null;

                if (expansionTail)
                {
                    this.UndockPlayerAutoTeleport(expansionTail);
                }
            }

            return true;
        }

        /**
         *
         * Docking hazırlığını yapar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public bool PrepDocking(Dockable dockable)
        {
            this.ExpansionManager.dockedHead = dockable.truckSegment;
            if (this.ExpansionManager.dockedHead == null)
            {
                return false;
            }

            var connection = this.ExpansionManager.dockedHead.rearConnection.GetConnection();
            if (connection != null)
            {
                this.ExpansionManager.tail = connection.truckSegment;
            }

            this.DockPlayerAutoTeleport();

            this.ExpansionManager.isFullyDocked = false;
            this.ExpansionManager.exitTrigger.SetActive(false);
            this.ExpansionManager.entranceTriggerDocked.SetActive(true);
            this.ExpansionManager.ResetCabinCarrierPosition();

            if (this.Player.transform.parent && this.Player.transform.parent.TryGetComponent<global::SeaTruckSegment>(out var seaTruckSegment))
            {
                var firstSegment = seaTruckSegment.GetFirstSegment();
                if (firstSegment && firstSegment == this.ExpansionManager.dockedHead)
                {
                    this.Player.DisableMovement();

                    UWE.CoroutineHost.StartCoroutine(this.UnlockMovement());
                }
            }

            return true;
        }

        /**
         *
         * Kuyruk renklerini günceller.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void UpdateTailColors()
        {
            if (this.ExpansionManager.dockedHead && this.ExpansionManager.tail)
            {
                this.ExpansionManager.tail.colorNameControl.CopyFrom(this.ExpansionManager.dockedHead.colorNameControl);

                var rearConnection = this.ExpansionManager.tail.rearConnection?.GetConnection();
                while (rearConnection)
                {
                    rearConnection.truckSegment.colorNameControl.CopyFrom(this.ExpansionManager.dockedHead.colorNameControl);
                    rearConnection = rearConnection.truckSegment.rearConnection?.GetConnection();
                }
            }
        }

        /**
         *
         * Rıhtımdan ayrılmayı başlatır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void StartUndocking(bool isMine)
        {
            this.PlayerDisableMovementPlatform();

            this.ExpansionManager.isFullyDocked = false;
            this.ExpansionManager.StartUndocking();

            if (this.ExpansionManager.tail && this.ExpansionManager.tail.frontConnection)
            {
                this.ExpansionManager.tail.frontConnection.closedGo.SetActive(true);
                this.ExpansionManager.tail.frontConnection.openedGo.SetActive(false);
            }

            if (isMine)
            {
                this.Player.SetCurrentSub(null);

                if (this.DockingBay.VehicleDockingBay.dockedObject.TryGetComponent<global::SeaTruckMotor>(out var seaTruckMotor))
                {
                    seaTruckMotor.StartPiloting();

                    if (seaTruckMotor.seatruckanimation)
                    {
                        seaTruckMotor.seatruckanimation.currentAnimation = SeaTruckAnimation.Animation.BeginPilot;
                    }
                }
            }
            else
            {
                this.UndockPlayerAutoTeleport();

                if (this.ExpansionManager.IsOccupied())
                {
                    this.Player.UnfreezeStats();
                }
            }
        }

        /**
         *
         * Expansion -> Seatruck yanaşma tamamlandığında tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public bool OnDockingTimelineCompleted()
        {
            this.ExpansionManager.inCriticalAnimation = false;
            this.ExpansionManager.latestRepairTick = Time.time;
            this.ExpansionManager.isRepairing = true;
            this.ExpansionManager.dockedHead.rearConnection.closedGo.SetActive(false);
            this.ExpansionManager.moduleConnection.enabled = true;
            this.ExpansionManager.UpdateDockedOnlyColliders(true);
            this.ExpansionManager.UpdateUndockedOnlyColliders(false);

            if (this.ExpansionManager.tail)
            {
                this.ExpansionManager.tail.SetPlayer(null);
                this.ExpansionManager.tail.PropagatePlayer();  
            }
            else
            {
                this.ExpansionManager.HideSealGlass(false);
            }

            if (this.ExpansionManager.dockedHead.IsPiloted())
            {
                if (this.Player.IsFrozenStats())
                {
                    this.Player.UnfreezeStats();
                }

                this.ExpansionManager.Invoke("StartGetupSequence", 0.0f);
            }
            else
            {
                this.ExpansionManager.isFullyDocked = true;
            }

            this.ExpansionManager.SetDockingRoomFlooding(true);
            return true;
        }

        /**
         *
         * Expansion -> Seatruck ayrılma tamamlandığında tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public bool OnUndockingTimelineCompleted()
        {
            var seaTruckMotor = this.ExpansionManager.dockedHead.motor;
            if (seaTruckMotor == null)
            {
                return false;
            }

            this.ExpansionManager.inCriticalAnimation = false;
            this.ExpansionManager.dockedHead.DeprioRearExits(true);
            this.ExpansionManager.HideSealGlass(false);
            this.ExpansionManager.moduleConnection.doorCollision.enabled = false;

            if (this.ExpansionManager.tail)
            {
                var seaTruckSegment = this.ExpansionManager.tail;

                this.UndockTail();

                using (EventBlocker.Create(ProcessType.SeaTruckConnection))
                {
                    seaTruckSegment.frontConnection.SetConnectedTo(this.ExpansionManager.dockedHead.rearConnection);
                }
            }

            if (this.ExpansionManager.dockedHead.wheelTriggerCollider != null)
            {
                this.ExpansionManager.dockedHead.wheelTriggerCollider.enabled = true;
            }

            this.ExpansionManager.exitingTruck = this.ExpansionManager.dockedHead;
            this.ExpansionManager.arrowsMaterial.SetColor(ShaderPropertyID._Color, this.ExpansionManager.backingUpArrowsColor);
            this.ExpansionManager.arrowsMaterial.SetVector(ShaderPropertyID._MainTex2_Speed, this.ExpansionManager.backingUpArrowsSpeed);
            this.ExpansionManager.thrustEndTime = Time.fixedTime + this.ExpansionManager.maxExitThrustTime;
            this.ExpansionManager.dockedHead = null;
            this.ExpansionManager.Terminal.UpdateCameras();

            if (seaTruckMotor.truckSegment.IsPiloted())
            {
                this.Player.UnfreezeStats();
                this.Player.SetCurrentSub(null);

                this.DockingBay.Undock(true);
            }
            else
            {
                this.DockingBay.Undock(false);
                this.DockPlayerAutoTeleport();
            }

            return true;
        }

        /**
         *
         * Oyuncuyu otomatik olarak moonpool dışına ışınlar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void DockPlayerAutoTeleport(bool ignoreCheck = false)
        {
            if (ignoreCheck || (this.Player.IsUnderwater() && this.IsPlayerInMoonpoolExpansion()))
            {
                this.Player.transform.position = this.GetOutsideSpawnPosition();
            }
        }

        /**
         *
         * Oyuncunun hareket platformunu devre dışı bırakır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void PlayerDisableMovementPlatform()
        {
            if (this.Player.groundMotor.movingPlatform.activePlatform && this.ExpansionManager.tail)
            {
                var firstSegment = this.Player.groundMotor.movingPlatform.activePlatform.GetComponentInParent<global::SeaTruckSegment>()?.GetFirstSegment();
                if (firstSegment == this.ExpansionManager.tail.GetFirstSegment() || firstSegment == this.ExpansionManager.dockedHead.GetFirstSegment())
                {
                    this.Player.groundMotor.OnTeleport();
                }                
            }
        }

        /**
         *
         * Oyuncu güvenli bir yere ışınlar ya da araca bindirir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public bool UndockPlayerAutoTeleport(global::SeaTruckSegment undockTail = null)
        {
            if (this.Player.IsUnderwater())
            {
                if (this.IsPlayerInMoonpoolExpansion() && undockTail == null)
                {
                    this.DockPlayerAutoTeleport(true);
                }
            }
            else
            {
                var seaTruckSegment = this.Player.GetUnderGameObject<global::SeaTruckSegment>()?.GetFirstSegment();
                if (seaTruckSegment == null)
                {
                    if (SleepScreen.Instance.UniqueId.IsNotNull() && (SleepScreen.Instance.IsSleepingStarted || SleepScreen.Instance.IsEnabled))
                    {
                        var bed = SleepScreen.Instance.GetBed();
                        if (bed)
                        {
                            seaTruckSegment = bed.GetComponentInParent<global::SeaTruckSegment>()?.GetFirstSegment();
                        }
                    }
                }

                if (seaTruckSegment)
                { 
                    if (undockTail)
                    {
                        if (seaTruckSegment == undockTail)
                        {
                            this.Player.SetCurrentSub(null);
                            undockTail.Enter(this.Player);
                        }
                    }
                    else if (seaTruckSegment == this.ExpansionManager.dockedHead)
                    {
                        this.Player.SetCurrentSub(null);
                        seaTruckSegment.Enter(this.Player);
                    }
                    else if (seaTruckSegment == this.ExpansionManager.tail)
                    {
                        this.Player.SetCurrentSub(null);
                        seaTruckSegment.Enter(this.Player);
                    }

                    return false;
                }

                if (this.IsPlayerInMoonpoolExpansion() && undockTail == null)
                {
                    this.Player.SetPosition(this.GetTerminalSpawnPosition());
                    return false;
                }
            }

            return false;
        }

        /**
         *
         * Oyuncu hareket kısıtlamasını kaldırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private IEnumerator UnlockMovement()
        {
            yield return new WaitForSecondsRealtime(7.5f);

            var seaTruckSegment = this.Player.GetUnderGameObject<global::SeaTruckSegment>()?.GetFirstSegment();
            if (seaTruckSegment)
            {
                seaTruckSegment.player = this.Player;
                seaTruckSegment.Exit(skipTeleport: true, newInterior: this.ExpansionManager.interior);
            }

            this.Player.EnableMovement();
        }

        /**
         *
         * ExpansionManager aktifliğini döner.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public bool IsActive()
        {
            return this.ExpansionManager != null;
        }

        /**
         *
         * Kuyruk aktifliğini döner.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public bool IsTailOccupied()
        {
            return this.ExpansionManager.tail;
        }
        
        /**
         *
         * Oyuncu moonpool expansion bölgesi içerisinde mi?
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public bool IsPlayerInMoonpoolExpansion()
        {
            if (!this.IsActive())
            {
                return false;
            }

            var tailPosition     = this.ExpansionManager.tailDockingPosition.transform.position;
            var tailForward      = this.ExpansionManager.tailDockingPosition.transform.forward;
            var seaTruckPosition = this.ExpansionManager.seatruckDockingPosition.transform.position;

            var sphereRadius       = 0.5f;
            var localScaleBig      = new Vector3(7f, 7f, 7f);
            var localScaleSmall    = new Vector3(5f, 5f, 5f);
            var tailCenterPosition = new Vector3(tailPosition.x, tailPosition.y, tailPosition.z);

            var positions = new List<KeyValuePair<Vector3, Vector3>>();
            positions.Add(new KeyValuePair<Vector3, Vector3>(tailCenterPosition, localScaleBig));

            for (int i = 0; i < 3; i++)
            {
                tailCenterPosition.x -= tailForward.x * (localScaleBig.x / 2f);
                tailCenterPosition.y -= tailForward.y * (localScaleBig.x / 2f);
                tailCenterPosition.z -= tailForward.z * (localScaleBig.x / 2f);

                positions.Add(new KeyValuePair<Vector3, Vector3>(tailCenterPosition, localScaleBig));
            }

            tailCenterPosition = new Vector3(tailPosition.x, tailPosition.y, tailPosition.z);

            for (int i = 0; i < 2; i++)
            {
                tailCenterPosition.x += tailForward.x * (localScaleBig.x / 2f);
                tailCenterPosition.y += tailForward.y * (localScaleBig.x / 2f);
                tailCenterPosition.z += tailForward.z * (localScaleBig.x / 2f);

                positions.Add(new KeyValuePair<Vector3, Vector3>(tailCenterPosition, localScaleBig));
            }

            positions.Add(new KeyValuePair<Vector3, Vector3>(seaTruckPosition, localScaleSmall));

            foreach (var item in positions)
            {
                float actualradius = sphereRadius * item.Value.x;

                if (ZeroVector3.Distance(this.Player.transform.position, item.Key) < actualradius * actualradius)
                {
                    return true;
                }
            }

            return false;
        }

        /**
         *
         * Terminal doğma konumunu döner.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public Vector3 GetTerminalSpawnPosition()
        {
            return this.ExpansionManager.Terminal.transform.position + (this.ExpansionManager.Terminal.transform.right * 1.5f);
        }

        /**
         *
         * Dışarıdaki doğma konumunu döner.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public Vector3 GetOutsideSpawnPosition()
        {
            var position = new Vector3(this.ExpansionManager.tailDockingPosition.transform.position.x, this.ExpansionManager.tailDockingPosition.transform.position.y, this.ExpansionManager.tailDockingPosition.transform.position.z);
            position.x += -this.ExpansionManager.tailDockingPosition.transform.right.x * 6f;
            position.y += -this.ExpansionManager.tailDockingPosition.transform.right.y * 6f;
            position.z += -this.ExpansionManager.tailDockingPosition.transform.right.z * 6f;

            position.x += this.ExpansionManager.tailDockingPosition.transform.forward.x * 5f;
            position.y += this.ExpansionManager.tailDockingPosition.transform.forward.y * 5f;
            position.z += this.ExpansionManager.tailDockingPosition.transform.forward.z * 5f;

            return position;
        }
    }
}