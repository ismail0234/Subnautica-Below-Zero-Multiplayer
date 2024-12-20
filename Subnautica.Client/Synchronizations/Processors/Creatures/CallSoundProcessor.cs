namespace Subnautica.Client.Synchronizations.Processors.Creatures
{
    using FMOD.Studio;

    using Subnautica.API.Enums;
    using Subnautica.API.Extensions;
    using Subnautica.API.Features;
    using Subnautica.API.Features.Creatures;
    using Subnautica.Client.Abstracts;
    using Subnautica.Client.Core;
    using Subnautica.Events.EventArgs;
    using Subnautica.Network.Models.Core;

    using ServerModel = Subnautica.Network.Models.Server;
    using System.Collections;

    public class CallSoundProcessor : NormalProcessor
    {
        /**
        *
        * Gelen veriyi işler
        *
        * @author Ismail <ismaiil_0234@hotmail.com>
        *
        */
        public override bool OnDataReceived(NetworkPacket networkPacket)
        {
            var packet = networkPacket.GetPacket<ServerModel.CreatureCallArgs>();
            if (packet == null)
            {
                return false;
            }

            var action = new CreatureQueueAction();
            action.OnProcessCompleted = this.OnCreatureProcessCompleted;
            action.RegisterProperty("CallId"   , packet.CallId);
            action.RegisterProperty("Animation", packet.Animation);

            Network.Creatures.ProcessToQueue(packet.CreatureId, action);
            return true;
        }

        /**
         *
         * İşlem tamamlandığında tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void OnCreatureProcessCompleted(MultiplayerCreature creature, CreatureQueueItem item)
        {
            var callId    = item.Action.GetProperty<byte>("CallId");
            var animation = item.Action.GetProperty<string>("Animation");

            foreach (var component in creature.GameObject.GetComponentsInChildren<global::CreatureCallSound>())
            {
                if (component.animation == animation)
                {
                    UWE.CoroutineHost.StartCoroutine(this.TriggerCallAsync(component, callId));
                    break;
                }
            }
        }

        /**
         *
         * Yaratık çağrı sesini tetikler.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private IEnumerator TriggerCallAsync(global::CreatureCallSound component, byte callVariant)
        {
            component.sound.Play();

            if (callVariant > 0 && FMODUWE.IsValidParameterId(component.sfxCallVariantParamIndex))
            {
                component.sound.SetParameterValue(component.sfxCallVariantParamIndex, callVariant);
            }

            yield return null;

            component.sound.GetEventInstance().getPlaybackState(out var state);

            if (state != PLAYBACK_STATE.STOPPED && component.animator != null)
            {
                component.animator.SetTrigger(component.animation);

                if (callVariant > 0 && component.animation.IsNotNull())
                {
                    component.animator.SetInteger(component.animCallVariantParameter, callVariant);
                }
            }
        }

        /**
         *
         * Yaratık bazı sesler oynarken tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void OnCallSoundTriggering(CreatureCallSoundTriggeringEventArgs ev)
        {
            if (ev.UniqueId.IsMultiplayerCreature())
            {
                ev.IsAllowed = false;

                CallSoundProcessor.SendPacketToServer(ev.UniqueId.ToCreatureId(), ev.CallId, ev.Animation);
            }
        }

        /**
         *
         * Sunucuya veri gönderir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private static void SendPacketToServer(ushort creatureId, byte callId, string animation)
        {
            ServerModel.CreatureCallArgs request = new ServerModel.CreatureCallArgs()
            {
                CreatureId = creatureId,
                CallId     = callId,
                Animation  = animation
            };

            NetworkClient.SendPacket(request);
        }
    }
}