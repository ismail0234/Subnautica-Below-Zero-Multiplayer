namespace Subnautica.Events.Patches.Fixes.Creatures.Actions
{
    using HarmonyLib;

    using Subnautica.API.Features;
 
    using UnityEngine;

    [HarmonyPatch(typeof(global::FlyAwayWhenScared), nameof(global::FlyAwayWhenScared.OnFearTriggerEnter))]
    public class FlyAwayWhenScared
    {
        private static bool Prefix(global::FlyAwayWhenScared __instance, Collider collider)
        {
            if (!Network.IsMultiplayerActive || !__instance.enabled)
            {
                return true;
            }

            var isTriggered = false;
            var gameObject  = collider.attachedRigidbody ? collider.attachedRigidbody.gameObject : collider.gameObject;

            if (gameObject == global::Player.main.gameObject)
            {
                isTriggered = !global::Player.main.IsInsideWalkable() && !GameModeManager.HasNoCreatureAggression();
            }
            else if (gameObject.GetComponent<Creature>() != null)
            {
                isTriggered = global::CreatureData.GetBehaviourType(gameObject) == BehaviourType.Shark;
            }
            else
            {
                var player = ZeroPlayer.GetPlayerByGameObject(gameObject);
                if (player != null)
                {
                    isTriggered = player.CanBeAttacked();
                }
            }

            if (isTriggered)
            {
                __instance.creatureFear.SetTarget(gameObject);
                __instance.creature.Scared.Add(1f);
                __instance.creature.TryStartAction(__instance);
            }

            return false;
        }
    }
}
