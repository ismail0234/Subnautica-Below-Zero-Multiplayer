namespace Subnautica.Events.Patches.Events.Game
{
    using System;
    using System.Collections;

    using HarmonyLib;

    using Subnautica.API.Features;
    using Subnautica.Events.EventArgs;

    using UnityEngine;

    using UWE;

    [HarmonyPatch(typeof(global::SupplyDropManager), nameof(global::SupplyDropManager.PerformDropInterpolated))]
    public static class LifepodInterpolation
    {
        private static IEnumerator Postfix(IEnumerator values, global::SupplyDropManager __instance, GameObject dropObject)
        {
            if (!Network.IsMultiplayerActive)
            {
                yield return values;
            }
            else 
            {
                LifepodInterpolationEventArgs args = new LifepodInterpolationEventArgs(dropObject);
                
                try
                {
                    Handlers.Game.OnLifepodInterpolation(args);
                }
                catch (Exception e)
                {
                    Log.Error($"LifepodInterpolation.Prefix: {e}\n{e.StackTrace}");
                }

                if (args.IsAllowed)
                {
                    yield return values;
                }
                else 
                {
                    if (args.IsCompleted)
                    {
                        var lifepodDrop = dropObject.GetComponent<LifepodDrop>();
                        var dropZone    = dropObject.GetComponent<LifepodDrop>().GetDropZone();

                        dropObject.transform.position = dropZone.destination;
                        dropObject.transform.rotation = args.Rotation;

                        lifepodDrop.dropComplete = true;
                        lifepodDrop.Start();
                    }
                    else
                    {
                        var dropTransform = dropObject.transform;
                        var supplyDrop    = dropObject.GetComponent<LifepodDrop>();
                        var rigidBody     = dropObject.GetComponent<Rigidbody>();
                        var component     = dropObject.GetComponent<WorldForces>();
                        var dropZone      = supplyDrop.GetDropZone();
                        var sourceData    = supplyDrop.GetDropData();
                        var dropPath      = new SupplyDropPath();

                        dropTransform.rotation = args.Rotation;

                        Debug.LogFormat("DROP:  interpolating to {0}", (object)dropZone.destination.ToString());

                        supplyDrop.OnDropBegin();
                        sourceData.triggerOnDropBegin.Trigger();

                        __instance.CalculateDropPath(dropPath, component, dropZone.destination, __instance.startHeight);

                        UWE.Utils.SetIsKinematicAndUpdateInterpolation(rigidBody, true);
                        component.handleWind = false;

                        var prevPos  = dropTransform.position;
                        var pos      = dropTransform.position;
                        var leftTime = DayNightCycle.main.timePassedAsFloat - args.StartedTime;

                        while (leftTime < 0)
                        {
                            yield return CoroutineUtils.waitForNextFrame;

                            leftTime = DayNightCycle.main.timePassedAsFloat - args.StartedTime;
                        }

                        var isTriggered = false;

                        while (__instance.GetInterpolatedPosition(dropPath, leftTime, out pos))
                        {
                            if (supplyDrop.IsDropSuspended())
                            {
                                yield return CoroutineUtils.waitForNextFrame;
                            }
                            else
                            {
                                prevPos = dropTransform.position;
                                dropTransform.position = pos;
                                leftTime = DayNightCycle.main.timePassedAsFloat - args.StartedTime;

                                if (pos.y < 0.0 && prevPos.y >= 0.0)
                                {
                                    isTriggered = true;

                                    supplyDrop.OnWaterCollision(pos);
                                }

                                yield return CoroutineUtils.waitForNextFrame;
                            }
                        }

                        if (!isTriggered)
                        {
                            supplyDrop.OnWaterCollision(pos);
                        }

                        if (pos.y > 0.0)
                        {
                            supplyDrop.OnGroundCollision(pos);
                        }

                        sourceData.triggerOnLanding.Trigger();

                        dropTransform.position = dropZone.destination;

                        if (sourceData.isPhysicalDrop)
                        {
                            Vector3 savedVelocity = (dropTransform.position - prevPos) / Time.deltaTime;
                            float checkIfSettledTime = Time.time + 0.5f;
                            while (true)
                            {
                                bool flag = LargeWorldStreamer.main.IsRangeActiveAndBuilt(new UnityEngine.Bounds(dropTransform.position, Vector3.zero));
                                if (!rigidBody.isKinematic && !flag)
                                {
                                    savedVelocity = rigidBody.velocity;
                                    UWE.Utils.SetIsKinematicAndUpdateInterpolation(rigidBody, true);
                                }

                                if (rigidBody.isKinematic & flag)
                                {
                                    UWE.Utils.SetIsKinematicAndUpdateInterpolation(rigidBody, false);
                                    rigidBody.velocity = savedVelocity;
                                    checkIfSettledTime = Time.time + 0.5f;
                                }
                                if (rigidBody.isKinematic || Time.time <= checkIfSettledTime || rigidBody.velocity.sqrMagnitude >= 0.00999999977648258)
                                {
                                    yield return CoroutineUtils.waitForNextFrame;
                                }
                                else
                                {
                                    break;
                                }
                            }
                        }

                        UWE.Utils.SetIsKinematicAndUpdateInterpolation(rigidBody, true);
                        supplyDrop.OnSettled();
                    }

                    Debug.Log("DROP: ** DONE **");
                }
            }
        }
    }
}