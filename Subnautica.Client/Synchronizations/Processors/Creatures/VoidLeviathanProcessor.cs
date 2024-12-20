namespace Subnautica.Client.Synchronizations.Processors.Creatures
{
    using System.Collections;
    using System.Collections.Generic;

    using Subnautica.API.Extensions;
    using Subnautica.API.Features;
    using Subnautica.API.Features.Creatures.Datas;
    using Subnautica.Client.Abstracts.Processors;
    using Subnautica.Events.EventArgs;
    using Subnautica.Network.Core.Components;
    using Subnautica.Network.Structures;

    using UnityEngine;

    public class VoidLeviathanProcessor : WorldCreatureProcessor
    {
        /**
         *
         * VoidData değerini barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private static BaseCreatureData VoidData { get; set; }

        /**
         *
         * Targets değerini barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private static Dictionary<string, string> Targets = new Dictionary<string, string>();

        /**
        *
        * Gelen veriyi işler
        *
        * @author Ismail <ismaiil_0234@hotmail.com>
        *
        */
        public override bool OnDataReceived(NetworkCreatureComponent networkPacket, byte requesterId, double processTime, TechType creatureType, ushort creatureId)
        {
            return true;
        }

        /**
         *
         * Sınıf başlatılırken tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public override void OnStart()
        {
            Targets.Clear();
            VoidData = TechType.GhostLeviathan.GetCreatureData();
        }

        /**
         *
         * VoidLeviathan Update döngüsü
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private static IEnumerator VoidBehaviourUpdate(global::VoidLeviathan voidLeviathan, string creatureId)
        {
            Targets[creatureId] = null;

            while (true)
            {
                if (voidLeviathan.liveMixin.IsAlive())
                {
                    var player = GetRandomFreePlayer(creatureId, voidLeviathan.transform.position, VoidData.VisibilityLongDistance * VoidData.VisibilityLongDistance);
                    if (player != null)
                    {
                        var vehicle = player.GetVehicle();
                        if (vehicle != null)
                        {
                            voidLeviathan.lastTarget.SetLockedTarget(vehicle);
                        }
                        else
                        {
                            voidLeviathan.lastTarget.SetLockedTarget(player.PlayerModel);
                        }

                        voidLeviathan.Aggression.FullOn();
                    }
                    else
                    {
                        var vector3 = voidLeviathan.transform.position - Vector3.zero;
                        var targetPosition = voidLeviathan.transform.position + vector3 * voidLeviathan.maxDistanceToPlayer;
                        targetPosition.y = Mathf.Min(targetPosition.y, -50f);

                        voidLeviathan.swimBehaviour.SwimTo(targetPosition, 30f);
                        voidLeviathan.lastTarget.UnlockTarget();
                    }

                    Targets[creatureId] = player != null ? player.UniqueId : null;

                    voidLeviathan.updateBehaviour = player != null;
                    voidLeviathan.AllowCreatureUpdates(voidLeviathan.updateBehaviour);
                    voidLeviathan.SetAggressionLevel(Targets.Count > 1 ? 2 : 1);

                    yield return new WaitForSeconds(voidLeviathan.updateBehaviourRate);
                }
                else
                {
                    break;
                }
            }
        }

        /**
         *
         * Hedef alınmayan boşta bir oyuncu döner.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private static ZeroPlayer GetRandomFreePlayer(string creatureId, Vector3 creaturePosition, float maxDistance)
        {
            if (Targets.TryGetValue(creatureId, out var playerId) && playerId.IsNotNull())
            {
                var player = ZeroPlayer.GetPlayerById(playerId);
                if (player != null && player.IsPlayerInVoid() && ZeroVector3.Distance(player.PlayerModel.transform.position, creaturePosition) <= maxDistance)
                {
                    return player;
                }
            }

            foreach (var player in ZeroPlayer.GetAllPlayers())
            {
                if (Targets.ContainsValue(player.UniqueId))
                {
                    continue;
                }

                if (player.IsPlayerInVoid() && ZeroVector3.Distance(player.PlayerModel.transform.position, creaturePosition) <= maxDistance)
                {
                    return player;
                }
            }

            foreach (var player in ZeroPlayer.GetAllPlayers())
            {
                if (Targets.ContainsValue(player.UniqueId))
                {
                    if (player.IsPlayerInVoid() && ZeroVector3.Distance(player.PlayerModel.transform.position, creaturePosition) <= maxDistance)
                    {
                        return player;
                    }
                }
            }

            return null;
        }

        /**
         *
         * Yaratık etkinleştiğinde tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void OnCreatureEnabled(CreatureEnabledEventArgs ev)
        {
            if (Network.IsMultiplayerActive && ev.TechType == TechType.GhostLeviathan)
            {
                if (ev.Instance.TryGetComponent<global::VoidLeviathan>(out var component))
                {
                    if (component.voidBehaviourRoutine != null)
                    {
                        component.StopCoroutine(component.voidBehaviourRoutine);
                        component.voidBehaviourRoutine = null;
                    }

                    component.voidBehaviourRoutine = component.StartCoroutine(VoidBehaviourUpdate(component, ev.UniqueId));
                }
            }
        }

        /**
         *
         * Yaratık pasifleştiğinde tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void OnCreatureDisabled(CreatureDisabledEventArgs ev)
        {
            if (Network.IsMultiplayerActive && ev.TechType == TechType.GhostLeviathan)
            {
                Targets.Remove(ev.UniqueId);
            }
        }
    }
}