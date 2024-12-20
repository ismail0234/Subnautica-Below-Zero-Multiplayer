namespace Subnautica.Client.Synchronizations.Processors.General
{
    using System.Collections;
    using System.Linq;

    using Subnautica.API.Features;
    using Subnautica.Client.Abstracts;
    using Subnautica.Client.Core;
    using Subnautica.Client.Synchronizations.InitialSync;
    using Subnautica.Events.EventArgs;
    using Subnautica.Network.Models.Core;

    using UnityEngine;

    using UWE;

    using ServerModel = Subnautica.Network.Models.Server;

    public class IntroProcessor : NormalProcessor
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
            var packet = networkPacket.GetPacket<ServerModel.IntroStartArgs>();
            if (packet == null)
            {
                return true;
            }

            if (packet.IsFinished)
            {
                Network.Session.Current.SupplyDrops.RemoveAll(q => q.Key == packet.SupplyDrop.Key);
                Network.Session.Current.SupplyDrops.Add(packet.SupplyDrop);

                WorldProcessor.SetDayNightCycle(packet.ServerTime);
                LifepodProcessor.ForceSupplyDrop(packet.SupplyDrop.Key);
            }
            else
            {
                Network.Session.Current.IsFirstLogin = false;
            }

            return true;
        }

        /**
         *
         * Intro kontrolü yapılırken tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void OnIntroChecking(IntroCheckingEventArgs ev)
        {
            if (Network.Session.Current.IsFirstLogin && GameModeManager.GetOption<bool>(GameOption.Story))
            {
                ev.IsAllowed     = false;
                ev.WaitingMethod = IntroProcessor.IntroCheckingAsync();
            }
            else
            {
                if (!Network.Session.Current.SupplyDrops.Any(q => q.Key == API.Constants.SupplyDrop.Lifepod))
                {
                    IntroProcessor.SendPacketToServer(true);
                }

                global::Utils.SetContinueMode(true);
            }
        }

        /**
         *
         * Intro kontrolü yapılırken tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private static IEnumerator IntroCheckingAsync()
        {
            while (!LightmappedPrefabs.main || LightmappedPrefabs.main.IsWaitingOnLoads() || uGUI.main.loading.IsLoading || PAXTerrainController.main.isWorking)
            {
                yield return null;
            }

            var data = IntroVignette.main.player.GetGameData(SaveLoadManager.main.storyVersion);
            if (data)
            {
                IntroVignette.main.player.SetPosition(data.storyStartLocation.position, Quaternion.Euler(data.storyStartLocation.rotation));

                uGUI.main.intro.coroutine = CoroutineHost.StartCoroutine(IntroProcessor.InitalizeIntroAsync(UnityEngine.Object.Instantiate<ExpansionIntroManager>(data.introManagerPrefab), uGUI.main.intro));
                InputHandlerStack.main.Push(uGUI.main.intro);
            }
        }

        /**
         *
         * Introyu hazırlıklarını yapar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private static IEnumerator InitalizeIntroAsync(ExpansionIntroManager introManager, uGUI_ExpansionIntro gui)
        {
            IntroVignette.isIntroActive = true;

            if (FPSInputModule.current)
            {
                FPSInputModule.current.lockPauseMenu = true;
            }

            gui.fader.SetState(true);
            global::Player.main.playerController.inputEnabled = false;
            gui.PauseGameTime();

            yield return new WaitForSecondsRealtime(0.5f);

            MainMenuMusic.Stop();
            introManager.TriggerStartScreenAudio();
            gui.mainText.SetText("");

            yield return new WaitForSecondsRealtime(2f);

            while (!LargeWorldStreamer.main || !LargeWorldStreamer.main.IsReady() || !LargeWorldStreamer.main.IsWorldSettled())
            {
                yield return new WaitForSecondsRealtime(1f);
            }

            VRLoadingOverlay.Hide();

            if (Network.IsHost)
            {
                gui.mainText.SetState(true);

                while (!GameInput.GetButtonDown(GameInput.Button.UICancel))
                {
                    gui.mainText.SetText(ZeroLanguage.Get("GAME_INTRO_PLAYERS_CONNECTED").Replace("{playerCount}", ZeroPlayer.GetAllPlayers().Count.ToString()) + "\n" + ZeroLanguage.Get("GAME_INTRO_SERVER_START_DESCRIPTION").Replace("{key}", "ESC") + "\n\n"+  ZeroLanguage.Get("GAME_INVITE_CODE") + "\n" + Network.InviteCode.GetInviteCode());
                    yield return null;
                }

                IntroProcessor.SendPacketToServer();

                while (Network.Session.Current.IsFirstLogin)
                {
                    yield return CoroutineUtils.waitForNextFrame;
                }
            }
            else
            {
                if (Network.Session.Current.IsFirstLogin)
                {
                    gui.mainText.SetState(true);

                    while (Network.Session.Current.IsFirstLogin)
                    {
                        gui.mainText.SetText(ZeroLanguage.Get("GAME_INTRO_PLAYERS_CONNECTED").Replace("{playerCount}", ZeroPlayer.GetAllPlayers().Count.ToString()) + "\n" + ZeroLanguage.Get("GAME_INTRO_SERVER_OWNER_WAITING"));
                        yield return new WaitForSecondsRealtime(0.25f);
                    }
                }
            }

            if (FPSInputModule.current)
            {
                FPSInputModule.current.lockPauseMenu = false;
            }

            yield return introManager.Play(global::Player.main, gui);

            IntroVignette.isIntroActive = false;

            gui.ResumeGameTime();
            gui.StopCoroutine(gui.coroutine);
            gui.coroutine = null;
            gui.StartCoroutine(gui.ControlsHints());

            IntroVignette.main.OnDone();

            IntroProcessor.SendPacketToServer(true);
        }

        /**
         *
         * Sunucuya Veri Gönderir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void SendPacketToServer(bool isFinished = false)
        {
            ServerModel.IntroStartArgs request = new ServerModel.IntroStartArgs()
            {
                IsFinished = isFinished,
            };

            NetworkClient.SendPacket(request);
        }
    }
}