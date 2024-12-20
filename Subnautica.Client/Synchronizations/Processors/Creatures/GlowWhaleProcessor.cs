namespace Subnautica.Client.Synchronizations.Processors.Creatures
{
    using Subnautica.API.Enums.Creatures;
    using Subnautica.API.Extensions;
    using Subnautica.API.Features;
    using Subnautica.API.Features.Creatures;
    using Subnautica.Client.Abstracts.Processors;
    using Subnautica.Client.Core;
    using Subnautica.Client.Extensions;
    using Subnautica.Events.EventArgs;
    using Subnautica.Network.Core.Components;

    using CreatureModel = Subnautica.Network.Models.Creatures;
    using ServerModel   = Subnautica.Network.Models.Server;

    public class GlowWhaleProcessor : WorldCreatureProcessor
    {
        /**
         *
         * Gelen veriyi işler
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public override bool OnDataReceived(NetworkCreatureComponent networkPacket, byte requesterId, double processTime, TechType creatureType, ushort creatureId)
        {
            var component = networkPacket.GetComponent<CreatureModel.GlowWhale>();
            if (component == null)
            {
                return false;
            }

            var action = new CreatureQueueAction();
            action.OnProcessCompleted = this.OnCreatureProcessCompleted;
            action.RegisterProperty("RequesterId"   , requesterId);
            action.RegisterProperty("ProcessTime"   , processTime);
            action.RegisterProperty("IsRideStart"   , component.IsRideStart);
            action.RegisterProperty("IsRideEnd"     , component.IsRideEnd);
            action.RegisterProperty("IsEyeInteract" , component.IsEyeInteract);
            action.RegisterProperty("SFXType"       , component.SFXType);

            Network.Creatures.ProcessToQueue(creatureId, action);
            return true;
        }

        /**
         *
         * İşlem tamamlandığında tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private void OnCreatureProcessCompleted(MultiplayerCreature creature, CreatureQueueItem item)
        {
            var isRideStart   = item.Action.GetProperty<bool>("IsRideStart");
            var isRideEnd     = item.Action.GetProperty<bool>("IsRideEnd");
            var isEyeInteract = item.Action.GetProperty<bool>("IsEyeInteract");
            var processTime   = item.Action.GetProperty<double>("ProcessTime");
            var sfxType       = item.Action.GetProperty<GlowWhaleSFXType>("SFXType");

            if (sfxType != GlowWhaleSFXType.None)
            {
                var sfxManager = creature.GameObject.GetComponentInChildren<global::GlowWhaleSFXManager>();
                if (sfxManager)
                {
                    using (EventBlocker.Create(TechType.GlowWhale))
                    {
                        switch (sfxType)
                        {
                            case GlowWhaleSFXType.WhaleBreach: sfxManager.OnWhaleBreach(); break;
                            case GlowWhaleSFXType.WhaleDive: sfxManager.OnWhaleDive(); break;
                            case GlowWhaleSFXType.GulpAnimation: sfxManager.OnGulpAnimation(); break;
                            case GlowWhaleSFXType.BreathAnimation: sfxManager.OnBreathAnimation(); break;
                        }
                    }
                }
            }
            else
            {
                var player = ZeroPlayer.GetPlayerById(item.Action.GetProperty<byte>("RequesterId"));
                if (player != null)
                {
                    if (isRideStart)
                    {
                        if (player.IsMine)
                        {
                            player.OnHandClickGlowWhaleRideStart(creature.CreatureItem.Id.ToCreatureStringId());
                        }
                        else
                        {
                            creature.CreatureItem.SetBusy(true);

                            player.StartGlowWhaleRideCinematic(creature.CreatureItem.Id.ToCreatureStringId());
                        }
                    }
                    else if (isRideEnd)
                    {
                        if (!player.IsMine)
                        {
                            player.StopGlowWhaleRideCinematic(creature.CreatureItem.Id.ToCreatureStringId());
                        }

                    }
                    else if (isEyeInteract)
                    {
                        if (player.IsMine)
                        {
                            player.OnHandClickGlowWhaleEyeCinematicStart(creature.CreatureItem.Id.ToCreatureStringId());
                        }
                        else
                        {
                            if (processTime + 4 >= Network.Session.GetWorldTime())
                            {
                                creature.CreatureItem.SetBusy(true);

                                player.StartGlowWhaleEyeCinematic(creature.CreatureItem.Id.ToCreatureStringId());
                            }
                        }
                    }
                }
            }
        }

        /**
         *
         * Balina göz animasyonu başlarken tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void OnGlowWhaleEyeCinematicStarting(GlowWhaleEyeCinematicStartingEventArgs ev)
        {
            ev.IsAllowed = false;

            if (!Network.HandTarget.IsBlocked(ev.UniqueId))
            {
                GlowWhaleProcessor.SendPacketToServer(ev.UniqueId.ToCreatureId(), isEyeInteract: true);
            }
        }

        /**
         *
         * Balina sürme başlarken tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void OnGlowWhaleRideStarting(GlowWhaleRideStartingEventArgs ev)
        {
            ev.IsAllowed = false;

            if (!Network.HandTarget.IsBlocked(ev.UniqueId))
            {
                GlowWhaleProcessor.SendPacketToServer(ev.UniqueId.ToCreatureId(), isRideStart: true);
            }
        }

        /**
         *
         * Balina sürme sona erdiğinde tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void OnGlowWhaleRideStoped(GlowWhaleRideStopedEventArgs ev)
        {
           GlowWhaleProcessor.SendPacketToServer(ev.UniqueId.ToCreatureId(), isRideEnd: true);
        }

        /**
         *
         * Balina SFX tetiklendiğine çalışır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void OnGlowWhaleSFXTriggered(GlowWhaleSFXTriggeredEventArgs ev)
        {
            if (ev.UniqueId.IsMultiplayerCreature())
            {
                GlowWhaleProcessor.SendPacketToServer(ev.UniqueId.ToCreatureId(), sfxType: ev.SFXType);
            }
        }

        /**
         *
         * Sunucuya veri gönderir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private static void SendPacketToServer(ushort creatureId, bool isRideStart = false, bool isRideEnd = false, bool isEyeInteract = false, GlowWhaleSFXType sfxType = GlowWhaleSFXType.None)
        {
            ServerModel.CreatureProcessArgs request = new ServerModel.CreatureProcessArgs()
            {
                CreatureId = creatureId,
                Component  = new CreatureModel.GlowWhale(isRideStart, isRideEnd, isEyeInteract, sfxType)
            };

            NetworkClient.SendPacket(request);
        }
    }
}
