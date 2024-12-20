namespace Subnautica.Events.Patches.Fixes.Vehicle
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection.Emit;

    using HarmonyLib;

    using Subnautica.API.Features;
    using Subnautica.API.Extensions;

    using UnityEngine;

    using UWE;

    [HarmonyPatch]
    public class VehicleKill
    {
        /**
         *
         * Yok edilen araçları barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private static HashSet<string> DestroyedVehicles { get; set; } = new HashSet<string>();

        /**
         *
         * Araç yok edilme vfx'ini çalıştırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private static bool SpawnDeathVFX(string uniqueId, GameObject deathVFX, Vector3 position, Quaternion rotation)
        {
            if (!Network.IsMultiplayerActive)
            {
                UnityEngine.Object.Instantiate<GameObject>(deathVFX, position, rotation);
                return true;
            }

            if (DestroyedVehicles.Contains(uniqueId))
            {
                return false;
            }

            DestroyedVehicles.Add(uniqueId);

            var gameObject = UnityEngine.Object.Instantiate<GameObject>(deathVFX, position, rotation);
            if (gameObject)
            {
                if (gameObject.TryGetComponent<VFXDestroyAfterSeconds>(out var vfx))
                {
                    vfx.enabled = false;

                    CoroutineHost.StartCoroutine(VFXAutoRemove(gameObject, vfx.lifeTime));
                }
                else if (gameObject.TryGetComponent<DestroyAfterSeconds>(out var vfx2))
                {
                    vfx2.CancelInvoke();
                    vfx2.enabled = false;

                    CoroutineHost.StartCoroutine(VFXAutoRemove(gameObject, vfx2.destroyAfterDuraiton));
                }
            }

            return true;
        }

        /**
         *
         * VFX'i belirli bir süre sonra yok eder.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private static IEnumerator VFXAutoRemove(GameObject gameObject, float lifeTime)
        {
            var worldForces = gameObject.GetComponentsInChildren<Rigidbody>();
            if (worldForces?.Length > 0)
            {
                while (lifeTime > 0f)
                {
                    lifeTime -= Time.fixedUnscaledDeltaTime * Time.timeScale;

                    yield return CoroutineUtils.waitForFixedUpdate;
                }

                foreach (var item in worldForces)
                {
                    if (item)
                    {
                        GameObject.Destroy(item.gameObject);
                    }
                }

                if (gameObject)
                {
                    GameObject.Destroy(gameObject);
                }
            }
        }

        /**
         *
         * Ana Menüye dönünce veriler temizlenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [HarmonyPostfix]
        [HarmonyPatch(typeof(uGUI_MainMenu), nameof(uGUI_MainMenu.Awake))]
        private static void uGUI_MainMenu_Awake()
        {
            DestroyedVehicles.Clear();
        }


        /**
         *
         * Hoverbike Patlama işlemini yamalar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [HarmonyPostfix]
        [HarmonyPatch(typeof(global::Hoverbike), nameof(global::Hoverbike.KillAsync))]
        private static IEnumerator Hoverbike_KillAsync(IEnumerator values, global::Hoverbike __instance)
        {
            if (Network.IsMultiplayerActive)
            {
                if (__instance.isPiloting)
                {
                    __instance.ExitVehicle();
                }

                if (__instance.deathVFX)
                {
                    SpawnDeathVFX(__instance.gameObject.GetIdentityId(), __instance.deathVFX, __instance.transform.position, __instance.transform.rotation);
                }

                __instance.sfx_explode.Play();
            }
            else
            {
                yield return values;
            }
        }

        /**
         *
         * Penguin Patlama işlemini yamalar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [HarmonyPrefix]
        [HarmonyPatch(typeof(global::SpyPenguin), nameof(global::SpyPenguin.OnKill))]
        private static bool SpyPenguin_OnKill(global::SpyPenguin __instance)
        {
            if (Network.IsMultiplayerActive)
            {
                if (__instance.isUsingPenguin)
                {
                    __instance.DisablePenguinCam();
                }

                SpyPenguinRemoteManager.main.UnregisterPenguin(__instance);

                if (__instance.destroyedPenguinPrefab)
                {
                    SpawnDeathVFX(__instance.gameObject.GetIdentityId(), __instance.destroyedPenguinPrefab, __instance.transform.position, __instance.transform.rotation);
                }

                return false;
            }

            return true;
        }

        /**
         *
         * Exosuit Patlama işlemini yamalar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [HarmonyPrefix]
        [HarmonyPatch(typeof(global::Vehicle), nameof(global::Vehicle.OnKill))]
        private static bool Exosuit_OnKill(global::Vehicle __instance)
        {
            if (Network.IsMultiplayerActive)
            {
                if (global::Player.main.currentMountedVehicle == __instance)
                {
                    __instance.OnPilotModeEnd();

                    global::Player.main.ToNormalMode(false);
                    global::Player.main.transform.parent = null;
                    global::Player.main.transform.localScale = Vector3.one;
                }

                if (__instance.destructionEffect)
                {
                    SpawnDeathVFX(__instance.gameObject.GetIdentityId(), __instance.destructionEffect, __instance.transform.position, __instance.transform.rotation);
                }

                return false;
            }

            return true;
        }

        /**
         *
         * SeaTruck/Module Patlama işlemini yamalar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void SeaTruckSegment_OnKill_Destruction(global::SeaTruckSegment seaTruckSegment)
        {
            if (seaTruckSegment.destructionEffect)
            {
                SpawnDeathVFX(seaTruckSegment.gameObject.GetIdentityId(), seaTruckSegment.destructionEffect, seaTruckSegment.transform.position, seaTruckSegment.transform.rotation);
            }
        }

        /**
         *
         * SeaTruckSegment işlemini yamalar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [HarmonyTranspiler]
        [HarmonyPatch(typeof(global::SeaTruckSegment), nameof(global::SeaTruckSegment.OnKill))]
        private static IEnumerable<CodeInstruction> SeaTruckSegment_OnKill(IEnumerable<CodeInstruction> instructions)
        {
            var codes = instructions.ToList();
            var index = codes.FindIndex(q => q.opcode == OpCodes.Call && q.operand.ToString().Contains("Destroy"));

            if (index > -1)
            {
                codes.RemoveRange(index, 1);
                codes.InsertRange(index, new CodeInstruction[] {
                    new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(VehicleKill), nameof(VehicleKill.SeaTruckSegment_OnKill_Destroy), new System.Type[] { typeof(GameObject) }))
                });
            }

            index = codes.FindLastIndex(q => q.opcode == OpCodes.Callvirt && q.operand.ToString().Contains("ExitCurrentInterior"));

            if (index > -1)
            {
                codes.RemoveRange(index - 1, 2);
                codes.InsertRange(index - 1, new CodeInstruction[] {
                    new CodeInstruction(OpCodes.Callvirt, AccessTools.Method(typeof(VehicleKill), nameof(VehicleKill.SeaTruckSegment_OnKill_ExitInterior)))
                });
            }

            index = codes.FindIndex(q => q.opcode == OpCodes.Ldfld && q.operand.ToString().Contains("destructionEffect"));

            if (index > -1)
            {
                codes.RemoveRange(index, 18);
                codes.InsertRange(index, new CodeInstruction[] {
                     new CodeInstruction(OpCodes.Ldarg_0),
                     new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(VehicleKill), nameof(VehicleKill.SeaTruckSegment_OnKill_Destruction), new Type[] { typeof(global::SeaTruckSegment) }))
                 });
            }

            return codes.AsEnumerable();
        }

        /**
         *
         * SeaTruckSegment yok etme işlemini yamalar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void SeaTruckSegment_OnKill_Destroy(GameObject gameObject)
        {
            if (!Network.IsMultiplayerActive)
            {
                GameObject.Destroy(gameObject);
            }
        }

        /**
         *
         * SeaTruckSegment/ExitInterior işlemini yamalar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void SeaTruckSegment_OnKill_ExitInterior()
        {
            global::Player.main.ExitCurrentInterior();
        }
    }
}
