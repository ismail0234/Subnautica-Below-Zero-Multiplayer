namespace Subnautica.Client.Modules
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text.RegularExpressions;

    using Subnautica.API.Features;
    using Subnautica.API.Extensions;
    using Subnautica.API.Features.Helper;
    using Subnautica.Client.Core;
    using Subnautica.Client.Modules.MultiplayerMainMenuModule;
    using Subnautica.Events.EventArgs;

    using UnityEngine;

    using UWE;

    public class MultiplayerMainMenu
    {
        /**
         *
         * Sahne yüklendiğinde tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void OnSceneLoaded(SceneLoadedEventArgs ev)
        {
            IsClicked = false;

            if (ev.Scene.name == "XMenu")
            {
                if (Settings.IsBepinexInstalled)
                {
                    CoroutineHost.StartCoroutine(SendAutoBepinexWarn());
                }
                else
                {
                    InitializeMultiplayerMenu();
                }
            }
        }

        /**
         *
         * Bepinex uyarı mesajını gösterir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static IEnumerator SendAutoBepinexWarn()
        {
            yield return new WaitForSecondsRealtime(2f);

            uGUI.main.confirmation.Show(ZeroLanguage.Get("GAME_BEPINEX_DETECTED"), null, null);
        }

        /**
         *
         * Çok oyunculu menü ayarlarını yapar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void InitializeMultiplayerMenu()
        {
            CacheSinglePlayerSaveGames();

            UserInterfaceElements.SinglePlayerButtonAddEvent(OnSinglePlayerButtonClick);
            UserInterfaceElements.CreateSidebarButton(ZeroLanguage.Get("GAME_MULTIPLAYER"), OnSidebarMultiplayerButtonClick);

            var baseGroup         = UserInterfaceElements.CreateMultiplayerBaseContent(MULTIPLAYER_BASE_GROUP_NAME, ZeroLanguage.Get("GAME_MULTIPLAYER"), ZeroLanguage.Get("GAME_MULTIPLAYER_HOST_GAME"), ZeroLanguage.Get("GAME_MULTIPLAYER_JOIN_GAME"), OnHostGameButtonClick, OnJoinGameButtonClick);
            var hostGameGroup     = UserInterfaceElements.CreateHostBaseContent(MULTIPLAYER_HOST_GROUP_NAME, ZeroLanguage.Get("GAME_MULTIPLAYER_HOST_GAME"), OnHostCreateServerButtonClick);
            var joinGameGroup     = UserInterfaceElements.CreateJoinBaseContent(MULTIPLAYER_JOIN_GROUP_NAME, ZeroLanguage.Get("GAME_MULTIPLAYER_JOIN_GAME"), ZeroLanguage.Get("GAME_ADD_SERVER"), OnAddServerButtonClick);
            var addServerGroup    = UserInterfaceElements.CreateAddServerGroup(MULTIPLAYER_JOIN_ADD_SERVER_GROUP_NAME, OnAddServerSaveButtonClick);
            var createServerGroup = UserInterfaceElements.CreateServerHostGroup(MULTIPLAYER_HOST_CREATE_SERVER_GROUP_NAME, OnCreateServerHostClick);
            
            MainMenuRightSide.main.groups.Add(baseGroup.GetComponent<MainMenuGroup>());
            MainMenuRightSide.main.groups.Add(hostGameGroup.GetComponent<MainMenuGroup>());
            MainMenuRightSide.main.groups.Add(joinGameGroup.GetComponent<MainMenuGroup>());
            MainMenuRightSide.main.groups.Add(addServerGroup.GetComponent<MainMenuGroup>());
            MainMenuRightSide.main.groups.Add(createServerGroup.GetComponent<MainMenuGroup>());
        }

        /**
         *
         * Singleplayer butonuna basıldığında tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void OnSinglePlayerButtonClick()
        {
            SaveLoadManager.main.gameInfoCache.Clear();

            foreach (var item in SinglePlayerGameSaves)
            {
                SaveLoadManager.main.gameInfoCache[item.Key] = item.Value;
            }

            MainMenuRightSide.main.OpenGroup("SavedGames");
        }

        /**
         *
         * Sidebar çok oyunculu butonuna basınca tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void OnSidebarMultiplayerButtonClick()
        {
            MainMenuRightSide.main.OpenGroup(MULTIPLAYER_BASE_GROUP_NAME);
        }

        /**
         *
         * Host Game Butonuna basınca tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void OnHostGameButtonClick()
        {
            SaveLoadManager.main.gameInfoCache.Clear();

            foreach (var item in NetworkServer.GetHostServerList())
            {
                SaveLoadManager.GameInfo info = new SaveLoadManager.GameInfo();
                info.Initialize(0, 0, SaveLoadManager.defaultStoryVersion, item.Id, null, item.GetGameMode(), null);

                SaveLoadManager.main.gameInfoCache[item.Id] = info;
            }

            MainMenuRightSide.main.OpenGroup(MULTIPLAYER_HOST_GROUP_NAME);
        }

        /**
         *
         * Add server Butonuna basınca tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void OnAddServerButtonClick()
        {
            var serverInviteCode = UserInterfaceElements.GetInputText("GAME_INVITE_CODE").Trim();
            if (serverInviteCode.IsNull())
            {
                UserInterfaceElements.SetInputErrorMessage("GAME_INVITE_CODE", ZeroLanguage.Get("GAME_INVITE_CODE_EMPTY_ERROR"));
                return;
            }

            if (serverInviteCode.Length == 6)
            {
                UserInterfaceElements.ClearInputText("GAME_INVITE_CODE");

                UWE.CoroutineHost.StartCoroutine(Network.InviteCode.JoinServerAsync(serverInviteCode, (LobbyJoinServerResponseFormat response) =>
                {
                    NetworkClient.Connect(response.ServerIp, response.ServerPort);
                }));
            }
            else
            {
                NetworkClient.Connect(serverInviteCode, 666, false);
            }
        }

        /**
         *
         * Create server Butonuna basınca tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void OnHostCreateServerButtonClick()
        {
            MainMenuRightSide.main.OpenGroup(MULTIPLAYER_HOST_CREATE_SERVER_GROUP_NAME);
        }

        /**
         *
         * Join Game Butonuna basınca tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void OnJoinGameButtonClick()
        {
            SaveLoadManager.main.gameInfoCache.Clear();

            foreach (var item in NetworkServer.GetLocalServerList())
            {
                SaveLoadManager.GameInfo info = new SaveLoadManager.GameInfo();
                info.Initialize(0, 0, SaveLoadManager.defaultStoryVersion, item.Id, null, GameModePresetId.Survival, null);

                SaveLoadManager.main.gameInfoCache[item.Id] = info;
            };

            MainMenuRightSide.main.OpenGroup(MULTIPLAYER_JOIN_GROUP_NAME);
        }

        /**
         *
         * Singleplayer verilerini önbelleğe alır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void CacheSinglePlayerSaveGames()
        {
            SinglePlayerGameSaves = new Dictionary<string, SaveLoadManager.GameInfo>(SaveLoadManager.main.gameInfoCache);
        }

        /**
         *
         * Ana menü kayıtlı oyunları sil iptal onay butonu tetiklenmesi.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void OnMenuSaveCancelDeleteButtonClicking(MenuSaveCancelDeleteButtonClickingEventArgs ev)
        {
            ev.IsAllowed = CancelDeleteSave();

            if (!ev.IsAllowed)
            {
                ev.IsRunAnimation = true;
            }
        }

        /**
         *
         * Ana menü kayıtlı oyunları sil butonu tetiklenmesi.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void OnMenuSaveDeleteButtonClicking(MenuSaveDeleteButtonClickingEventArgs ev)
        {
            ev.IsAllowed = DeleteSave(ev.SessionId);

            if (!ev.IsAllowed)
            {
                ev.IsRunAnimation = true;
            }
        }

        /**
         *
         * Ana menü kayıtlı oyun buton bilgileri tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void OnMenuSaveUpdateLoadedButtonState(MenuSaveUpdateLoadedButtonStateEventArgs ev)
        {
            UpdateLoadSaveButtonState(ev.Button);
        }

        /**
         *
         * Ana menü kayıtlı oyunu başlat tetiklemesi
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void OnMenuSaveLoadButtonClicking(MenuSaveLoadButtonClickingEventArgs ev)
        {
            ev.IsAllowed = LoadSave(ev.SessionId);
        }

        /**
         *
         * Kayıt dosyası tıklandığında tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static bool LoadSave(string sessionId)
        {
            if (UserInterfaceElements.IsSinglePlayerMenuActive)
            {
                return true;
            }

            if (UserInterfaceElements.IsHostGroupActive)
            {
                var server = NetworkServer.GetHostServerList().Where(q => q.Id == sessionId).FirstOrDefault();
                if (server == null)
                {
                    ErrorMessage.AddMessage(ZeroLanguage.Get("GAME_NOT_FOUND_SERVER"));
                    return false;
                }
     
                if (NetworkServer.IsConnecting())
                {
                    ErrorMessage.AddMessage(ZeroLanguage.Get("GAME_SERVER_ALREADY_CONNECTING"));
                    return false;
                }

                if (NetworkServer.IsConnected())
                {
                    ErrorMessage.AddMessage(ZeroLanguage.Get("GAME_SERVER_ALREADY_CONNECTED"));
                    return false;
                }

                if (IsClicked)
                {
                    return false;
                }

                IsClicked = true;

                UWE.CoroutineHost.StartCoroutine(Network.InviteCode.CreateServerAsync((LobbyCreateServerResponse response) =>
                {
                    NetworkServer.StartServer(server.Id, Tools.GetLoggedId());
                    NetworkClient.Connect(response.ServerIp, response.ServerPort);
                }, () => {
                    IsClicked = false;
                }));
            }
            
            return false;
        }

        /**
         *
         * Kayıt dosyası onay kabul reddedildiğinde tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static bool CancelDeleteSave()
        {
            if (UserInterfaceElements.IsSinglePlayerMenuActive)
            {
                return true;
            }

            return false;
        }

        /**
         *
         * Kayıt dosyası onay kabul edildiğinde tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static bool DeleteSave(string sessionId)
        {
            if (UserInterfaceElements.IsSinglePlayerMenuActive)
            {
                return true;
            }

            if (UserInterfaceElements.IsHostGroupActive)
            {
                var server = NetworkServer.GetHostServerList().Where(q => q.Id == sessionId).FirstOrDefault();
                if (server != null)
                {
                    string serverPath = Paths.GetMultiplayerServerSavePath(server.Id);
                    if (Directory.Exists(serverPath))
                    {
                        Directory.Delete(serverPath, true);
                    }
                }
            }
            else
            {
                var serverList = NetworkServer.GetLocalServerList();
                if (serverList.Count <= 0)
                {
                    return false;
                }

                var server = serverList.Where(q => q.Id == sessionId).FirstOrDefault();
                if (server != null)
                {
                    serverList.Remove(server);
                    NetworkServer.SaveLocalServerList(serverList);
                }
            }

            return false;
        }

        /**
         *
         * Kayıt kutularının detaylarını doldurur.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void UpdateLoadSaveButtonState(MainMenuLoadButton lb)
        {
            if (UserInterfaceElements.IsHostGroupActive)
            {
                var server = NetworkServer.GetHostServerList().Where(q => q.Id == lb.sessionId).FirstOrDefault();
                if (server != null)
                {
                    lb.saveGameLengthText.text = Tools.GetSizeByTextFormat(Tools.GetFolderSize(Paths.GetMultiplayerServerSavePath(server.Id)));
                    lb.saveGameTimeText.text   = Tools.GetDateByTextFormat(server.CreationDate);
                }
            }
            else
            {
                var server = NetworkServer.GetLocalServerList().Where(q => q.Id == lb.sessionId).FirstOrDefault();
                if (server != null)
                {
                    lb.saveGameLengthText.text = String.Format("{0}:{1}", server.IpAddress, server.Port);
                    lb.saveGameTimeText.text   = server.Name;
                    lb.saveGameModeText.text   = "";
                }
            }
        }

        /**
         *
         * Add server save Butonuna basınca tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void OnAddServerSaveButtonClick()
        {
            var serverName = UserInterfaceElements.GetInputText("GAME_SERVER_NAME").Trim();
            var serverIp   = UserInterfaceElements.GetInputText("GAME_SERVER_IP").Trim();

            if (string.IsNullOrEmpty(serverName))
            {
                UserInterfaceElements.SetInputErrorMessage("GAME_SERVER_IP", ZeroLanguage.Get("GAME_SERVER_NAME_EMPTY_ERROR"));
                return;
            }

            if (serverName.Length < 3 || serverName.Length > 64)
            {
                UserInterfaceElements.SetInputErrorMessage("GAME_SERVER_IP", ZeroLanguage.Get("GAME_SERVER_NAME_LENGTH_ERROR"));
                return;
            }

            if (string.IsNullOrEmpty(serverIp) || !Regex.Match(serverIp, @"\b\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3}\b").Success)
            {
                UserInterfaceElements.SetInputErrorMessage("GAME_SERVER_IP", ZeroLanguage.Get("GAME_SERVER_IP_INVALID_ERROR"));
                return;
            }

            var serverList = NetworkServer.GetLocalServerList();
            if (serverList.Where(q => q.IpAddress == serverIp).Any())
            {
                UserInterfaceElements.SetInputErrorMessage("GAME_SERVER_IP", ZeroLanguage.Get("GAME_SERVER_EXIST"));
                return;
            }

            serverList.Add(new LocalServerItem()
            {
                Id        = Guid.NewGuid().ToString(),
                IpAddress = serverIp,
                Port      = NetworkServer.DefaultPort,
                Name      = serverName,
            });

            NetworkServer.SaveLocalServerList(serverList);

            UserInterfaceElements.ClearInputText("GAME_SERVER_NAME");
            UserInterfaceElements.ClearInputText("GAME_SERVER_IP");

            OnJoinGameButtonClick();
        }

        /**
         *
         * Survival/Hardcore game v.s server oluştur Butonuna basınca tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void OnCreateServerHostClick(GameModePresetId gameModeId)
        {
            if (IsClicked == false)
            {
                IsClicked = true;

                UWE.CoroutineHost.StartCoroutine(Network.InviteCode.CreateServerAsync((LobbyCreateServerResponse response) =>
                {
                    var serverId = NetworkServer.CreateNewServer(gameModeId);

                    if (NetworkServer.StartServer(serverId, Tools.GetLoggedId()))
                    {
                        NetworkClient.Connect(response.ServerIp, response.ServerPort);
                    }
                    else
                    {
                        OnHostGameButtonClick();
                    }
                }, () => {
                    IsClicked = false;
                }));
            }
        }

        /**
         *
         * Grup Anahtarları
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public const string MULTIPLAYER_BASE_GROUP_NAME = "MultiplayerBase";
        public const string MULTIPLAYER_HOST_GROUP_NAME = "MultiplayerHostBase";
        public const string MULTIPLAYER_JOIN_GROUP_NAME = "MultiplayerJoinBase";
        public const string MULTIPLAYER_JOIN_ADD_SERVER_GROUP_NAME    = "MultiplayerJoinAddServerBase";
        public const string MULTIPLAYER_HOST_CREATE_SERVER_GROUP_NAME = "MultiplayerHostCreateServerBase";

        /**
         *
         * SinglePlayer verilerini barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static Dictionary<string, SaveLoadManager.GameInfo> SinglePlayerGameSaves { get; set; }

        /**
         *
         *  IsClicked Değerini barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static bool IsClicked { get; set; } = false;
    }
}