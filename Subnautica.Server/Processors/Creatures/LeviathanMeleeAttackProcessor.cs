namespace Subnautica.Server.Processors.Creatures
{
    using System.Collections.Generic;

    using Subnautica.API.Enums;
    using Subnautica.API.Extensions;
    using Subnautica.Network.Models.Core;
    using Subnautica.Network.Structures;
    using Subnautica.Server.Abstracts.Processors;
    using Subnautica.Server.Core;
    using Subnautica.Server.Extensions;
    using Subnautica.Server.Logic;

    using ServerModel = Subnautica.Network.Models.Server;

    public class LeviathanMeleeAttackProcessor : NormalProcessor
    {
        /**
         *
         * Araçların saldırıya uğrama zamanını barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private Dictionary<string, double> VehicleLastAttackTimes = new Dictionary<string, double>();

        /**
         *
         * Gelen veriyi işler
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public override bool OnExecute(AuthorizationProfile profile, NetworkPacket networkPacket)
        {
            var packet = networkPacket.GetPacket<ServerModel.CreatureLeviathanMeleeAttackArgs>();
            if (packet == null)
            {
                return this.SendEmptyPacketErrorLog(networkPacket);
            }

            if (!Server.Instance.Logices.CreatureWatcher.TryGetCreature(packet.CreatureId, out var creature))
            {
                return false;
            }

            if (creature.LiveMixin.IsDead)
            {
                return false;
            }

            if (creature.GetActionType() == ProcessType.CreatureLeviathanMeleeAttack)
            {
                return false;
            }

            var targetPlayerId = packet.Target.GetTargetOwnerId(profile.PlayerId);
            if (targetPlayerId <= 0)
            {
                return false;
            }

            var damage = this.CalculateDamage(packet.BiteDamage, packet.Target);
            if (damage <= 0f)
            {
                return false;
            }

            var animationTime = this.GetAttackAnimationTime(creature.TechType, packet.Target.Type);
            if (animationTime <= 0f)
            {
                return false;
            }

            var targetPlayer = Server.Instance.GetPlayer(targetPlayerId);
            if (targetPlayer == null || !targetPlayer.IsFullConnected)
            {
                return false;
            }

            packet.ProcessTime = Server.Instance.Logices.World.GetServerTime();

            if (packet.Target.IsCreature())
            {
                if (Server.Instance.Logices.CreatureWatcher.ImmediatelyTrigger())
                {
                    Server.Instance.Logices.CreatureWatcher.TriggerAction(packet);
                }
            }
            else if (packet.Target.IsPlayer())
            {
                if (targetPlayer.Health - damage <= 0f)
                {
                    packet.Target.Kill();
                }

                if (packet.Target.IsDead)
                {
                    if (profile.IsUnderAttack())
                    {
                        return false;
                    }

                    profile.SetUnderAttack(animationTime * 2f);
                }

                creature.SetAction(packet, targetPlayer.PlayerId);

                if (Server.Instance.Logices.CreatureWatcher.ImmediatelyTrigger())
                {
                    Server.Instance.Logices.CreatureWatcher.TriggerAction(packet);
                    Server.Instance.Logices.CreatureWatcher.ClearAction(creature, animationTime);
                }
            }
            else if (packet.Target.IsVehicle())
            {
                if (this.CanGrabVehicle(packet.Target.TargetId))
                {
                    this.UpdateVehicleAttackTime(packet.Target.TargetId, animationTime);

                    creature.SetAction(packet, targetPlayer.PlayerId);

                    if (Server.Instance.Logices.CreatureWatcher.ImmediatelyTrigger())
                    {
                        Server.Instance.Logices.CreatureWatcher.TriggerAction(packet);
                        Server.Instance.Logices.CreatureWatcher.ClearAction(creature, animationTime);
                    }
                }              
            }

            return true;
        }

        /**
         *
         * Oyuncu animasyon süresini döner.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private float GetAttackAnimationTime(TechType attackerType, TechType targetType)
        {
            if (attackerType == TechType.Chelicerate)
            {
                if (targetType.IsPlayer())
                {
                    return Interact.CreatureCheliceratePlayerAttack;
                }

                if (targetType.IsVehicle())
                {
                    return Interact.CreatureChelicerateVehicleAttack;
                }

                if (targetType.IsCreature())
                {
                    return 0.1f;
                }
            }
            
            if (attackerType == TechType.ShadowLeviathan)
            {
                if (targetType.IsPlayer())
                {
                    return Interact.ShadowLeviathanPlayerAttack;
                }

                if (targetType.IsVehicle())
                {
                    return Interact.ShadowLeviathanVehicleAttack;
                }

                if (targetType.IsCreature())
                {
                    return 0.1f;
                }
            }

            return 0f;
        }

        /**
         *
         * Yaratık tarafından araç yakalanabilir mi?
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private bool CanGrabVehicle(string vehicleId)
        {
            if (this.VehicleLastAttackTimes.TryGetValue(vehicleId, out var lastTime))
            {
                return lastTime < Server.Instance.Logices.World.GetServerTimeAsDouble();
            }

            return true;
        }

        /**
         *
         * Araç saldırı altında zamanını günceller.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private void UpdateVehicleAttackTime(string vehicleId, float animationTime)
        {
            this.VehicleLastAttackTimes[vehicleId] = Server.Instance.Logices.World.GetServerTimeAsDouble() + (animationTime * 2.5);
        }

        /**
         *
         * Hasarı hesaplar
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private float CalculateDamage(float damage, ZeroLastTarget target)
        {
            var damageTakenModifier = GameModeManager.GetDamageTakenModifier(target.Type, false);
            if (damageTakenModifier <= 0f) 
            {
                return 0f;
            }

            damage *= damageTakenModifier * DamageSystem.damageMultiplier;
            return damage;
        }
    }
}
