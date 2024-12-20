namespace Subnautica.API.Features.Creatures.MonoBehaviours.Shared
{
    using Subnautica.API.Enums;
    using Subnautica.API.Extensions;

    using UnityEngine;

    public class MultiplayerAttackLastTarget : BaseMultiplayerCreature
    {
        /**
         *
         * AttackLastTarget değerini barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public global::AttackLastTarget AttackLastTarget { get; private set; }

        /**
         *
         * Sınıf uyanırken tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void Awake()
        {
            this.AttackLastTarget = this.GetComponent<global::AttackLastTarget>();
        }

        /**
         *
         * Yaratığı hedef konuma doğru saldırısını başlatır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void ForceAttackTarget(GameObject target)
        {
            if (this.MultiplayerCreature.CreatureItem.IsMine())
            {
                using (EventBlocker.Create(ProcessType.CreatureAttackLastTarget))
                {
                    this.AttackLastTarget.creature.StopPrevAction();

                    if (target)
                    {
                        this.AttackLastTarget.ForceAttackTarget(target);
                    }
                }
            }
            else 
            {
                this.AttackLastTarget.StartAttackSoundAndAnimation();
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
            this.StopAttackSoundAndAnimation();
        }

        /**
         *
         * Pasif olduğunda tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void OnDisable()
        {
            this.StopAttackSoundAndAnimation();
        }

        /**
         *
         * Ses ve animasyonları durdurur.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void StopAttackSoundAndAnimation(bool onlyAnimationAndSounds = false)
        {
            if (this.AttackLastTarget)
            {
                this.AttackLastTarget.StopAttackSoundAndAnimation();

                if (!onlyAnimationAndSounds)
                {
                    this.AttackLastTarget.StopAttack();
                }
            }
        }
    }
}
