namespace Subnautica.API.Features.Creatures.MonoBehaviours.Shared
{
    using UnityEngine;

    public class MultiplayerMeleeAttack : BaseMultiplayerCreature
    {
        /**
         *
         * MeleeAttack sınıfını barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private global::MeleeAttack MeleeAttack { get; set; }

        /**
         *
         * Sınıf uyanırken tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void Awake()
        {
            this.MeleeAttack = this.GetComponent<global::MeleeAttack>();
        }

        /**
         *
         * Yakın dövüş saldırısını başlatır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public bool StartMeleeAttack(GameObject target)
        {
            if (target == null)
            {
                return false;
            }

            this.MeleeAttack.timeLastBite = Time.time;

            Vector3 position = target.transform.position;

            if (target.TryGetComponent<Rigidbody>(out var rb))
            {
                position = rb.ClosestPointOnBounds(this.MeleeAttack.mouth.transform.position);
            }

            if (this.MeleeAttack.damageFX)
            {
                UnityEngine.Object.Instantiate<GameObject>(this.MeleeAttack.damageFX, position, this.MeleeAttack.damageFX.transform.rotation);
            }

            if (this.MeleeAttack.attackSound)
            {
                global::Utils.PlayEnvSound(this.MeleeAttack.attackSound, position);
            }

            if (this.MeleeAttack.biteSound)
            {
                this.MeleeAttack.biteSound.Play();
            }

            this.MeleeAttack.creature.Aggression.Add(-this.MeleeAttack.biteAggressionDecrement);

            if (this.MultiplayerCreature.CreatureItem.IsMine())
            {
                var liveMixin = target.GetComponent<global::LiveMixin>();
                if (liveMixin)
                {
                    if (liveMixin.IsAlive())
                    {
                        liveMixin.TakeDamage(this.MeleeAttack.GetBiteDamage(target), dealer: this.gameObject);
                        liveMixin.NotifyCreatureDeathsOfCreatureAttack();
                    }
                }
            }
            
            this.MeleeAttack.gameObject.SendMessage("OnMeleeAttack", target, SendMessageOptions.DontRequireReceiver);
            return true;
        }
    }
}
