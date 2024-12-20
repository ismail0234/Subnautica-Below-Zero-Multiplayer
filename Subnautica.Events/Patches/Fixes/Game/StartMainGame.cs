namespace Subnautica.Events.Patches.Fixes.Game
{
    using HarmonyLib;

    using Subnautica.API.Features;
    using Subnautica.Events.EventArgs;

    using System;
    using System.Collections;

    using UnityEngine;
    using UnityEngine.ResourceManagement.AsyncOperations;
    using UnityEngine.ResourceManagement.ResourceProviders;
    using UnityEngine.SceneManagement;

    using UWE;

    [HarmonyPatch(typeof(global::MainGameController), nameof(global::MainGameController.StartGame))]
    public static class StartMainGame
    {
        /**
         *
         * Fonksiyon ön ekini yamalar
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private static IEnumerator Postfix(IEnumerator values, global::MainGameController __instance)
        {
            if (Network.IsMultiplayerActive)
            {
                WaitScreen.ManualWaitItem waitItem = WaitScreen.Add("Root");

                yield return global::Utils.EnsureLootCubeCreated();

                __instance.skipHeartbeat = true;

                Physics.autoSyncTransforms = false;
                Physics2D.autoSimulation = false;

                __instance.detailedMemoryLog = Environment.GetEnvironmentVariable("SN_DETAILED_MEMLOG") == "1";

                if (__instance.detailedMemoryLog && !Debug.isDebugBuild)
                {
                    Debug.LogWarning("SN_DETAILED_MEMLOG was set, but this is not a debug/dev build. So the detailed mem readings will all be 0.");
                }

                float num = 60f;
                string environmentVariable = Environment.GetEnvironmentVariable("SN_HEARTBEAT_PERIOD_S");
                if (!string.IsNullOrEmpty(environmentVariable))
                {
                    num = float.Parse(environmentVariable);
                }

                __instance.InvokeRepeating("DoHeartbeat", 0.0f, num);
                waitItem.SetProgress(0.1f);

                for (int i = 0; i < __instance.additionalScenes.Length; ++i)
                {
                    string additionalScene = __instance.additionalScenes[i];
                    AsyncOperationHandle<SceneInstance> asyncOperationHandle = AddressablesUtility.LoadSceneAsync(additionalScene, LoadSceneMode.Additive);
                    WaitScreen.AsyncOperationItem sceneWaitItem = WaitScreen.Add("Scene" + additionalScene, asyncOperationHandle);
                    yield return asyncOperationHandle;
                    WaitScreen.Remove(sceneWaitItem);
                    sceneWaitItem = null;
                }

                waitItem.SetProgress(0.2f);

                while (LightmappedPrefabs.main.IsWaitingOnLoads())
                {
                    yield return CoroutineUtils.waitForNextFrame;
                }

                __instance.SetInitialPlayerPosition();

                waitItem.SetProgress(0.4f);

                PAXTerrainController main2 = PAXTerrainController.main;
                if (main2 != null)
                {
                    yield return main2.Initialize();
                }

                while (!LargeWorldStreamer.main || !LargeWorldStreamer.main.IsWorldSettled())
                {
                    yield return CoroutineUtils.waitForNextFrame;
                }

                waitItem.SetProgress(0.8f);

                __instance.PerformGarbageAndAssetCollection();

                waitItem.SetProgress(0.9f);

                yield return WorldLoadedEvent();

                waitItem.SetProgress(1f);

                WaitScreen.Remove(waitItem);

                Application.backgroundLoadingPriority = ThreadPriority.Normal;
                __instance.UpdateFixedTimestep();

                DevConsole.RegisterConsoleCommand(__instance, "collect");
                DevConsole.RegisterConsoleCommand(__instance, "endsession");

                // VR AKTİF İSE TETİKLENİR.
                // VRUtil.OnRecenter += __instance.ResetOrientation;

                MainGameController.OnGameStarted?.Invoke();

                World.SetLoaded(true);
            }
            else
            {
                yield return values;
            }
        }

        /**
         *
         * Dünya yüklendikten sonra tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private static IEnumerator WorldLoadedEvent()
        {
            WorldLoadedEventArgs args = new WorldLoadedEventArgs();

            Handlers.Game.OnWorldLoaded(args);

            if (args.WaitingMethods != null)
            {
                foreach (var waitingMethod in args.WaitingMethods)
                {
                    yield return waitingMethod;
                }

                args.WaitingMethods = null;
            }
        }
    }
}



