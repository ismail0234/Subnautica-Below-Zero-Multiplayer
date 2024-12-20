namespace Subnautica.API.Features.Creatures.MonoBehaviours.Shared
{
    public class MultiplayerAggressiveToPilotingVehicle : BaseMultiplayerCreature
    {
        /**
         *
         * AggressiveToPilotingVehicle sınıfını barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private global::AggressiveToPilotingVehicle AggressiveToPilotingVehicle { get; set; }

        /**
         *
         * Sınıf uyanırken tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void Awake()
        {
            this.AggressiveToPilotingVehicle = this.GetComponent<global::AggressiveToPilotingVehicle>();
        }

        /**
         *
         * Aktifleşirken tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void OnEnable()
        {
            this.StopUpdateAggression();

            if (this.MultiplayerCreature.CreatureItem.IsMine())
            {
                this.StartUpdateAggression();
            }
        }

        /**
         *
         * Sahiplik değiştiğinde tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void OnChangedOwnership()
        {
            this.StopUpdateAggression();

            if (this.MultiplayerCreature.CreatureItem.IsMine())
            {
                this.StartUpdateAggression();
            }
        }

        /**
         *
         * Pasif olurken tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void OnDisable()
        {
            this.StopUpdateAggression();
        }

        /**
         *
         * Agresif güncellemeyi durdurur.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private void StopUpdateAggression()
        {
            this.CancelInvoke("MultiplayerUpdateAggression");
        }

        /**
         *
         * Agresif güncellemeyi başlatır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private void StartUpdateAggression()
        {
            this.InvokeRepeating("MultiplayerUpdateAggression", global::UnityEngine.Random.value * this.AggressiveToPilotingVehicle.updateAggressionInterval, this.AggressiveToPilotingVehicle.updateAggressionInterval);
        }

        /**
         *
         * Agresif güncellemeyi uygular.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private void MultiplayerUpdateAggression()
        {
            var playerInRange = ZeroPlayer.GetPlayersByInRange(this.transform.position, this.AggressiveToPilotingVehicle.range * this.AggressiveToPilotingVehicle.range, true);
            if (playerInRange.IsExistsPlayer())
            {
                this.AggressiveToPilotingVehicle.lastTarget.SetTarget(playerInRange.NearestPlayer.GetVehicle(), this.AggressiveToPilotingVehicle.targetPriority);
                this.AggressiveToPilotingVehicle.creature.Aggression.Add(this.AggressiveToPilotingVehicle.aggressionPerSecond * this.AggressiveToPilotingVehicle.updateAggressionInterval);
            }
        }
    }
}
