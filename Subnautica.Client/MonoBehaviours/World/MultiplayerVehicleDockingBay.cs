namespace Subnautica.Client.MonoBehaviours.World
{
    using Subnautica.API.Enums;
    using Subnautica.API.Extensions;
    using Subnautica.API.Features;
    using Subnautica.Client.Extensions;
    using Subnautica.Network.Structures;

    using UnityEngine;

    public class MultiplayerVehicleDockingBay : MonoBehaviour
    {
        /**
         *
         * BackSeaTruckSegment nesnesini barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private global::SeaTruckSegment BackSeaTruckSegment { get; set; }

        /**
         *
         * VehicleDockingBay nesnesini barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public VehicleDockingBay VehicleDockingBay { get; set; }

        /**
         *
         * ExpansionManager nesnesini barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public MultiplayerExpansionManager ExpansionManager { get; set; }

        /**
         *
         * ManuelCinematicPlayerId nesnesini barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private byte ManuelCinematicPlayerId { get; set; } = 0;

        /**
         *
         * TimeDockingStarted nesnesini barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private double TimeDockingStarted { get; set; }

        /**
         *
         * DockPlayer nesnesini barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private bool DockPlayer { get; set; }

        /**
         *
         * BackModulePosition nesnesini barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private ZeroVector3 BackModulePosition { get; set; }

        /**
         *
         * IsDocking nesnesini barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private bool IsDocking { get; set; }

        /**
         *
         * TailId nesnesini barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public string TailId { get; set; }

        /**
         *
         * InterpolationTime nesnesini barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public double InterpolationTime { get; set; } = 1;

        /**
         *
         * Sınıf ayarlamalarını yapar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void Awake()
        {
            if (this.TryGetComponent<VehicleDockingBay>(out var vehicleDockingBay))
            {
                this.VehicleDockingBay = vehicleDockingBay;
            }

            this.ExpansionManager = new MultiplayerExpansionManager(this);
            this.ExpansionManager.OnAwake();
        }

        /**
         *
         * Manuel Dock Oyuncu id'sini değiştirir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void SetManuelDockingPlayerId(byte playerId)
        {
            this.ManuelCinematicPlayerId = playerId;
        }

        /**
         *
         * Kuyruğu değiştirir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void SetDockingTail(string tailId)
        {
            if (tailId.IsNotNull() && Network.Session.Current.SeaTruckConnections != null)
            {
                Network.Session.Current.SeaTruckConnections.Remove(tailId);
            }

            this.TailId = tailId;
        }

        /**
         *
         * Kenetlenme başlangıç zamanını değiştirir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void SetDockingStartTime(double time)
        {
            this.TimeDockingStarted = time;
        }

        /**
         *
         * Oyuncu kenetlenme durumunu ayarlar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void SetDockPlayer(bool isDockPlayer)
        {
            this.DockPlayer = isDockPlayer;
        }

        /**
         *
         * Oyuncu kenetlenme durumunu ayarlar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void SetBackModulePosition(ZeroVector3 backModulePosition)
        {
            this.BackModulePosition = backModulePosition;
        }

        /**
         *
         * Rıhtıma yanaşma işlemini başlatır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void StartDocking(string vehicleId)
        {
            var vehicle = Network.Identifier.GetGameObject(vehicleId);
            if (vehicle && vehicle.TryGetComponent<Dockable>(out var dockable))
            {
                this.VehicleDockingBay.interpolatingDockable = dockable;
                this.VehicleDockingBay.startPosition         = this.VehicleDockingBay.interpolatingDockable.transform.position;
                this.VehicleDockingBay.startRotation         = this.VehicleDockingBay.interpolatingDockable.transform.rotation;

                if (this.DockPlayer)
                {
                    this.VehicleDockingBay.dockPlayer = dockable.GetPlayer();
                }
                else
                {
                    this.VehicleDockingBay.dockPlayer = false;
                }

                if (!this.VehicleDockingBay.dockPlayer && !this.ExpansionManager.IsActive())
                {
                    this.PlayerAutoClimb(global::Player.main, this.VehicleDockingBay, vehicleId);
                }

                if (dockable.truckSegment && !this.VehicleDockingBay.dockPlayer)
                {
                    dockable.truckSegment.player = null;
                    dockable.truckSegment.PropagatePlayer();
                }

                if (!this.ExpansionManager.IsActive() && dockable.truckSegment && dockable.truckSegment.isRearConnected)
                {
                    this.SetBackSeaTruckSegment(dockable.truckSegment.rearConnection.GetConnection().truckSegment);
                }

                using (EventBlocker.Create(ProcessType.SeaTruckConnection))
                {
                    dockable.OnDockingStart(this.VehicleDockingBay.disableDockableCollisionInProcess, !this.ExpansionManager.IsActive());
                }

                if (this.BackSeaTruckSegment && this.BackModulePosition != null)
                {
                    this.UndockSeaTruckModule(this.BackSeaTruckSegment, this.BackModulePosition);
                    this.SetBackSeaTruckSegment(null);
                }

                if (dockable.truckSegment && !this.VehicleDockingBay.MoonpoolExpansionEnabled())
                {
                    dockable.truckSegment.player = null;
                }

                if (this.ExpansionManager.IsActive())
                {
                    this.ExpansionManager.PrepDocking(dockable);
                }

                this.IsDocking = true;
                this.UpdateDocking();

                if (this.ExpansionManager.IsActive() && this.TimeDockingStarted == 0f)
                {
                    this.ExpansionManager.OnHandleLoading();
                }
            }
            else
            {
                this.Reset();
            }
        }

        /**
          *
          * Rıhtımdan ayrılma cinematiğini başlatır.
          *
          * @author Ismail <ismaiil_0234@hotmail.com>
          *
          */
        public void StartUndocking(byte playerId, bool isLeft)
        {
            if (this.ExpansionManager.IsActive())
            {
                this.ExpansionManager.StartUndocking(ZeroPlayer.IsPlayerMine(playerId));
            }
            else
            {
                if (this.IsDocking)
                {
                    this.VehicleDockingBay.interpolationTime = 0f;
                    this.UpdateDocking();
                }

                var player = ZeroPlayer.GetPlayerById(playerId);
                if (player != null)
                {
                    if (player.IsMine)
                    {
                        player.OnHandClickMoonpoolUndocking(this.GetMoonpoolId(), isLeft);
                    }
                    else
                    {
                        player.StartMooonpoolUndockingCinematic(this.GetMoonpoolId(), isLeft);
                    }
                }
                else
                {
                    this.Undock();
                } 
            }
        }

        /**
          *
          * Rıhtımdan ayrılma işlemini anında tamamlar.
          *
          * @author Ismail <ismaiil_0234@hotmail.com>
          *
          */
        public bool Undock(bool isMine = false)
        {
            if (this.VehicleDockingBay.dockedObject == null)
            {
                return false;
            }

            this.VehicleDockingBay.interpolatingDockable            = null;
            this.VehicleDockingBay.dockedObject.timeUndocked        = Time.time;
            this.VehicleDockingBay.dockedObject.rb.detectCollisions = true;
            this.VehicleDockingBay.dockedObject.isInTransition      = false;

            if (this.VehicleDockingBay.dockedObject.TryGetComponent<SeaTruckSegment>(out var seaTruck))
            {
                seaTruck.SetDockedCollisionEnabled(true);
                seaTruck.UpdatePingInstance();

                if (seaTruck.crushDamage)
                {
                    seaTruck.crushDamage.enabled = true;
                }

                if (seaTruck.seatruckLights)
                {
                    seaTruck.seatruckLights.SetSuppressionState(false);
                }

                if (isMine)
                {
                    seaTruck.Enter(global::Player.main);
                    seaTruck.motor.SetPiloting(true);
                }
            }

            SkyEnvironmentChanged.Broadcast(this.VehicleDockingBay.dockedObject.gameObject, null, onlyDynamic: true);

            if (this.VehicleDockingBay.dockedObject.vehicle)
            {
                this.VehicleDockingBay.dockedObject.vehicle.docked = false;
            }

            this.VehicleDockingBay.dockedObject.bay = null;
            this.VehicleDockingBay.dockedObject.transform.parent = null;
            this.VehicleDockingBay.dockedObject = null;
            return true;
        }

        /**
          *
          * Geç güncelleme işlemi
          *
          * @author Ismail <ismaiil_0234@hotmail.com>
          *
          */
        public void LateUpdate()
        {
            if (this.VehicleDockingBay)
            {
                this.UpdateDocking();
            }
        }

        /**
         *
         * Kenetleme ve animasyonları ayarlar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void UpdateDocking()
        {
            var dockable = this.GetDockableObject();
            var techType = this.GetVehicleType(dockable);
            if (dockable)
            {
                var interpfraction = this.GetInterpfraction();

                this.VehicleDockingBay.UpdateDockedPosition(dockable, interpfraction);

                if (this.VehicleDockingBay.interpolatingDockable != null && interpfraction == 1.0f)
                {
                    this.VehicleDockingBay.Dock(dockable);
                    this.VehicleDockingBay.interpolatingDockable = null;
                    this.VehicleDockingBay.interpolationTime = 1f;
                    this.IsDocking = false;

                    if (!this.ExpansionManager.IsActive())
                    {
                        if (this.ManuelCinematicPlayerId != 0)
                        {
                            if (!this.StartManuelCinematicMode(techType))
                            {
                                this.StartAutoCinematicMode(techType, null);
                            }
                        }
                        else
                        {
                            this.StartAutoCinematicMode(techType, this.GetPlayer());
                        }
                    }
                }
            }

            if (this.VehicleDockingBay.seaTruckRedockLock.IsLocked())
            {
                this.VehicleDockingBay.seaTruckRedockLock.CheckIfDockShouldUnlock(this.GetComponent<BoxCollider>(), this.VehicleDockingBay.docklockColliderScalar);
            }

            var nearby = (this.VehicleDockingBay.nearbyDockable != null || this.VehicleDockingBay.interpolatingDockable != null) && this.VehicleDockingBay.powerConsumer.IsPowered();

            SafeAnimator.SetBool(this.VehicleDockingBay.animator, "vehicle_nearby", nearby);
            SafeAnimator.SetBool(this.VehicleDockingBay.animator, "seamoth_docked", this.VehicleDockingBay.docked_param && techType == TechType.Seamoth || techType == TechType.SeaTruck);
            SafeAnimator.SetBool(this.VehicleDockingBay.animator, "exosuit_docked", this.VehicleDockingBay.docked_param && techType == TechType.Exosuit);
            
            if (this.ExpansionManager.IsActive())
            {
                this.ExpansionManager.OnUpdate(nearby);
            }
        }

        /**
         *
         * SeaTruck arka modülünü ayırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private bool UndockSeaTruckModule(global::SeaTruckSegment seaTruckSegment, ZeroVector3 position)
        {
            var backModuleId = seaTruckSegment.gameObject.GetIdentityId();
            if (backModuleId.IsNull())
            {
                return false;
            }

            var entity = Network.DynamicEntity.GetEntity(backModuleId);
            if (entity == null)
            {
                return false;
            }

            entity.SetParent(null);
            entity.SetPosition(position);

            seaTruckSegment.transform.position = position.ToVector3();
            seaTruckSegment.UpdateKinematicState();
            return true;
        }

        /**
         *
         * Oyuncu otomatik tırmanmayı başlatır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private bool PlayerAutoClimb(global::Player player, global::VehicleDockingBay vehicleDockingBay, string vehicleId)
        {
            if (player.currentInterior != null && player.currentInterior is global::SeaTruckSegment seaTruckSegment)
            {
                if (player.gameObject.transform.parent)
                {
                    var bed = player.gameObject.transform.parent.gameObject.GetComponentInChildren<global::Bed>(true);
                    if (bed)
                    {
                        if (bed.currentPlayer != null)
                        {
                            bed.OnPlayerDeath(bed.currentPlayer);

                            player.playerAnimator.Rebind();
                        }
                        else if (bed.cinematicController && bed.cinematicController.cinematicModeActive)
                        {
                            bed.cinematicController.BreakCinematic();
                        }
                        else if (bed.currentStandUpCinematicController && bed.currentStandUpCinematicController.cinematicModeActive)
                        {
                            bed.currentStandUpCinematicController.BreakCinematic();
                        }
                    }
                }

                if (vehicleId == player.currentInterior.GetGameObject()?.GetIdentityId())
                {
                    seaTruckSegment.Exit();

                    if (vehicleDockingBay.transform.parent?.parent?.gameObject)
                    {
                        foreach (var item in vehicleDockingBay.transform.parent.parent.GetComponentsInChildren<global::UseableDiveHatch>())
                        {
                            item.OnHandClick(player.guiHand);
                            return true;
                        }
                    }
                }
            }

            return false;
        }

        /**
         *
         * Otomatik cinematic modunu başlatır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private void StartAutoCinematicMode(TechType techType, global::Player player)
        {
            if (techType == TechType.Exosuit)
            {
                this.VehicleDockingBay.exosuitDockPlayerCinematic.StartCinematicMode(player);
            }
            else
            {
                this.VehicleDockingBay.dockPlayerCinematic.StartCinematicMode(player);
            }
        }

        /**
         *
         * Manuel cinematic modunu başlatır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private bool StartManuelCinematicMode(TechType techType)
        {
            var player = ZeroPlayer.GetPlayerById(this.ManuelCinematicPlayerId);
            if (player != null)
            {
                player.StartMooonpoolDockingCinematic(this.GetMoonpoolId(), techType);
            }

            this.SetManuelDockingPlayerId(0);

            return player != null;
        }

        /**
         *
         * Moonpool Id döner.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private string GetMoonpoolId()
        {
            return this.gameObject.GetComponentInParent<BaseDeconstructable>().gameObject.GetIdentityId();
        }

        /**
         *
         * Araç türünü döner.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private TechType GetVehicleType(Dockable dockable)
        {
            if (dockable?.gameObject == null)
            {
                return TechType.None;
            }

            return CraftData.GetTechType(dockable.gameObject);
        }

        /**
         *
         * Oyuncuyu döner.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private global::Player GetPlayer()
        {
            if (this.VehicleDockingBay.dockPlayer)
            {
                var player = global::Player.main;
                player.transform.parent = null;
                player.SetCurrentSub(this.VehicleDockingBay.GetSubRoot());
                player.ToNormalMode(false);
                return player;
            }

            return null;
        }

        /**
         *
         * Kırınım oranını döner.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public float GetInterpfraction()
        {
            if (this.TimeDockingStarted != 0)
            {
                return Mathf.Clamp01((float) ((Network.Session.GetWorldTime() - this.TimeDockingStarted) / this.InterpolationTime));
            }

            return 1f;
        }

        /**
         *
         * Kenetlenen aracı döner.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public Dockable GetDockableObject()
        {
            if (this.VehicleDockingBay.dockedObject)
            {
                return this.VehicleDockingBay.dockedObject;
            }

            return this.VehicleDockingBay.interpolatingDockable;
        }

        /**
         *
         * Arka seatruck segmenti barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private void SetBackSeaTruckSegment(global::SeaTruckSegment seaTruckSegment)
        {
            this.BackSeaTruckSegment = seaTruckSegment;
        }

        /**
         *
         * Verileri sıfırlar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private void Reset()
        {
            this.TailId     = null;
            this.DockPlayer = false;
            this.TimeDockingStarted  = 0f;
            this.BackSeaTruckSegment = null;
            this.BackModulePosition  = null;
            this.ManuelCinematicPlayerId = 0;
        }
    }
}
