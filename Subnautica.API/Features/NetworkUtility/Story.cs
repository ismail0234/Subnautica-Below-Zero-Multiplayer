namespace Subnautica.API.Features.NetworkUtility
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using global::Story;

    using Subnautica.API.Enums;
    using Subnautica.API.Extensions;
    using Subnautica.Network.Structures;

    using UnityEngine;
    using UnityEngine.EventSystems;

    public class Story
    {
        /**
         *
         * Triggers nesnesini barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public List<StoryTriggerItem> Triggers { get; set; } = new List<StoryTriggerItem>()
        {
            /**
             *
             * Belirtilen hedefi tetiklemez ve yakınında tüm oyuncular toplandığında tetiklenir. (Örneğin bir kapı engeli var ise kapıyı kaldırmak için.)
             *
             */
            new StoryTriggerItem("OnEnterSanctuary"        , GoalType.Story, new ZeroVector3(-579.8f, -201.4f, -474.9f), 150f, true, true),
            new StoryTriggerItem("UnlockArcticSpiresCache" , GoalType.Story, new ZeroVector3(-1017.7f, 19.1f, 722.8f)  , 100f, true, true),
            new StoryTriggerItem("UnlockCrystalCastleCache", GoalType.Story, new ZeroVector3(499.8f, -832.8f, -701.5f) , 150f, true, true),
            new StoryTriggerItem("UnlockDeepPadsCache"     , GoalType.Story, new ZeroVector3(546.7f, -621.1f, -1095.0f), 100f, true, true),
            new StoryTriggerItem("ShieldBaseDoorUnlocked"  , GoalType.Story, new ZeroVector3(-53.5f, 29.2f, 561.1f)    ,  80f, true, true),
            
            /**
             *
             * Bir hedef tetiklendiğinde ona bağlı alt hedefi tetikler.
             *
             */
            new StoryTriggerItem("StartApproachingCube", GoalType.Story, subTarget: new StoryTriggerItem("Log_Cine_MeetAl_P3_MainChamber", GoalType.PDA)),

            /**
             *
             * Sadece yakında oyuncular yok iken uyarı vermek ve işlemleri engellemek için. 
             *
             */
            new StoryTriggerItem(StoryCinematicType.StoryPrecursorComputerTerminal.ToString(), new ZeroVector3(-666.3f, -191.4f, -339.3f),  100f, false),
            new StoryTriggerItem(StoryCinematicType.StoryMarg1.ToString()                    , new ZeroVector3(-212.4f, 39.3f, -767.6f)  ,  100f, false, isCustomDoor: true, isTrigger: true),
            new StoryTriggerItem(StoryCinematicType.StoryMarg2.ToString()                    , new ZeroVector3(82.5f, -376.2f, -910.4f)  ,  50f , false, precondition: "FirstEncounterStart", isInBase: true),
            new StoryTriggerItem(StoryCinematicType.StoryMarg3.ToString()                    , new ZeroVector3(994.9f, 30.8f, -880.8f)   ,  25f , false),
            new StoryTriggerItem(StoryCinematicType.StoryBuildAlanTerminal.ToString()        , new ZeroVector3(1297.9f, -951.6f, -324.8f),  400f, false, precondition: "PrecursorPartsCollected"),
            new StoryTriggerItem(StoryCinematicType.StoryAlanTransfer.ToString()             , new ZeroVector3(1312.3f, -952.6f, -328.5f),  900f, false, precondition: "OnPrecursorNPCFabricated"),
            new StoryTriggerItem(StoryCinematicType.StoryFrozenCreatureSample.ToString()     , new ZeroVector3(-1633.1f, 40.4f, -699.1f) ,  400f, false, isTriggerActive: false),
            new StoryTriggerItem(StoryCinematicType.StoryFrozenCreatureInject.ToString()     , new ZeroVector3(-1630.0f, 39.6f, -699.0f) ,  400f, false, isTriggerActive: false),
            new StoryTriggerItem(StoryCinematicType.StoryShieldBaseInnerGate.ToString()      , new ZeroVector3(-53.5f, 30.1f, 601.2f)    ,  100f, false, isCustomDoor: true, isTrigger: true, precondition: "OnPrecursorNPCDone"),
            new StoryTriggerItem(StoryCinematicType.StoryShieldBaseEndGate.ToString()        , new ZeroVector3(1152.1f, 119.6f, 2396.6f) ,  100f, false, isCustomDoor: true, isTrigger: true),
            new StoryTriggerItem(StoryCinematicType.StoryEndGameRepairPillar1.ToString()     , new ZeroVector3(1154.0f, 109.9f, 2472.1f) ,   -1f, false),
            new StoryTriggerItem(StoryCinematicType.StoryEndGameRepairPillar2.ToString()     , new ZeroVector3(1127.7f, 109.9f, 2471.1f) ,   -1f, false),
            new StoryTriggerItem(StoryCinematicType.StoryEndGameReturnArms.ToString()        , new ZeroVector3(1150.4f, 119.8f, 2434.6f) ,  100f, false, precondition: "EndgameRepairsComplete"),
            new StoryTriggerItem(StoryCinematicType.StoryEndGameEnterShip.ToString()         , new ZeroVector3(1149.0f, 110.2f, 2457.8f) ,   14f, false, precondition: "EndgameRepairsComplete"),
            new StoryTriggerItem(StoryCinematicType.StoryEndGameGoToHomeWorld.ToString()     , new ZeroVector3(1149.5f, 120.2f, 2451.3f) ,   15f, false, precondition: "EndGameEnterShip"),
        };

        /**
         *
         * Sinematik listesini barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public List<string> StoryCinematics = new List<string>()
        {
            "IntroCinematics",
            "Mobile_Extractor_anim",
            "marge_intro_cin",
            "marge_base1_cin",
            "marge_greenhouse_cin",
            "sanctuary_cube_cin",
            "Precursor_fabricator_room_anim",
            "NoReturnTimeline",
            "Greetings",
            "RepairPillar2",
            "RepairPillar1",
            "ReturnArmsRight",
            "ReturnArmsLeft",
            "EnterShip",
            "Takeoff",
        };

        /**
         *
         * Hikaye nesnesini döner.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public StoryTriggerItem GetTriggerItem(StoryCinematicType cinematicType)
        {
            return this.GetTriggerItem(cinematicType.ToString());
        }

        /**
         *
         * Hikaye nesnesini döner.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public StoryTriggerItem GetTriggerItem(string itemId)
        {
            return this.Triggers.FirstOrDefault(q => q.GoalKey == itemId);
        }

        /**
         *
         * Beklenen oyuncu sayısını gösterir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void ShowWaitingPlayerMessage(byte playerCount, byte maxPlayer)
        {
            var hint = uGUI_FeedbackCollector.main.hint;

            if (hint.rootRT.sizeDelta.x != 500)
            {
                hint.rootRT.sizeDelta = new Vector2(500, hint.rootRT.rect.height);
            }

            uGUI_FeedbackCollector.main.hint.SetText(ZeroLanguage.GetStoryWaitingPlayers(playerCount, maxPlayer), TextAnchor.MiddleCenter);
            uGUI_FeedbackCollector.main.hint.Show(2f);
        }


        /**
         *
         * Beklenen oyuncu sayısını ekran ortasında gösterir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public bool ShowWaitingForPlayersMessage(StoryCinematicType cinematicType)
        {
            var trigger = Network.Story.GetTriggerItem(cinematicType);
            if (trigger == null)
            {
                return false;
            }

            byte playerCount = 1;
            byte maxPlayer = Convert.ToByte(ZeroPlayer.GetAllPlayers().Count());

            foreach (var player in ZeroPlayer.GetPlayers())
            {
                if (trigger.Position.Distance(player.Position.ToZeroVector3()) < trigger.TriggerRange)
                {
                    playerCount++;
                }
            }

            if (playerCount >= maxPlayer)
            {
                return true;
            }

            Interact.ShowDenyMessage(ZeroLanguage.GetStoryWaitingPlayers(playerCount, maxPlayer), false);
            return false;
        }

        /**
         *
         * Beklenen oyuncu sayısını gizler.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void HideWaitingPlayerMessage()
        {
            uGUI_FeedbackCollector.main.hint.Hide();
        }

        /**
         *
         * Hedef sessiz tamamlandı listesine ekler.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void MuteFutureStoryGoal(string goalKey)
        {
            StoryGoalManager.main.mutedStoryGoals.Add(goalKey);
        }

        /**
         *
         * Hedefi tamamlar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void GoalExecute(string key, GoalType goalType, bool isStoryGoalMuted = false)
        {
            var isGoalComplete = StoryGoalManager.main.OnGoalComplete(key);
            var isGoalMuted    = StoryGoalManager.main.IsStoryGoalMuted(key) || isStoryGoalMuted;
            switch (goalType)
            {
                case GoalType.PDA:
                    PDALog.Add(key, !isGoalMuted);
                    break;
                case GoalType.Radio:
                    if (isGoalComplete)
                    {
                        StoryGoalManager.main.AddPendingRadioMessage(key);
                    }

                    break;
                case GoalType.Encyclopedia:
                    {
                        if (PDAEncyclopedia.GetEntryData(key, out var entryData))
                        {
                            PDAEncyclopedia.Add(key, !isGoalMuted, postNotification: true);
                            if (entryData.hidden)
                            {
                                PDAEncyclopedia.Reveal(key, !isGoalMuted);
                            }
                        }

                        break;
                    }
                case GoalType.Call:
                    if (isGoalMuted)
                    {
                        if (PDACalls.TryGet(key, out var callData))
                        {
                            StoryGoalManager.main.OnGoalComplete(callData.voiceMail);
                            PDALog.Add(callData.voiceMail, playSound: false);
                        }
                    }
                    else
                    {
                        uGUI_PopupNotification.main.IncomingCall(key);
                    }

                    break;
            }

            var trigger = this.Triggers.FirstOrDefault(q => q.GoalKey == key);
            if (trigger != null)
            {
                foreach (var subTrigger in trigger.SubTriggers)
                {
                    this.GoalExecute(subTrigger.GoalKey, subTrigger.GoalType);
                }
            }
        }

        /**
         *
         * İlgili cinematiği başlatır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public bool StartCinematicMode(string uniqueId)
        {
            var gameObject = Network.Identifier.GetGameObject(uniqueId);
            if (gameObject == null)
            {
                return false;
            }

            var cinematic = gameObject.GetComponentInChildren<CinematicModeTriggerBase>(true);
            if (cinematic == null)
            {
                Log.Error("Cinematic Not Started - 2: " + uniqueId);
                return false;
            }

            if (!cinematic.gameObject.activeSelf)
            {
                cinematic.gameObject.SetActive(true);

                Log.Error("Cinematic Not Active - 3: " + uniqueId);
            }

            Log.Info("[DEBUG] CINEMATIC STARTING.. " + cinematic.gameObject?.name + ", v: " + cinematic.cinematicController?.playInVr + ", v2: " + VRGameOptions.GetVrAnimationMode() + ", active: " + cinematic.cinematicController?.cinematicModeActive + ", end: " + cinematic.cinematicController?.enforceCinematicModeEnd);

            try
            {
                cinematic.quickSlot = global::Inventory.main.quickSlots.activeSlot;

                global::Inventory.main.ReturnHeld();

                cinematic.timeUsingStarted = Time.time;
                cinematic.cinematicController.director?.Stop();
                cinematic.cinematicController.StartCinematicMode(global::Player.main);
                cinematic.OnStartCinematicMode();

                if (cinematic.secureInventory)
                {
                    global::Inventory.main.SecureItems(true);
                }

                if (cinematic.onCinematicStart != null)
                {
                    Log.Info("[DEBUG] CINEMATIC STARTED.. " + cinematic.gameObject?.name);
                    cinematic.onCinematicStart.Invoke(new CinematicModeEventData(EventSystem.current)
                    {
                        player = global::Player.main
                    });
                }
                
                cinematic.cinematicController.informGameObject = cinematic.gameObject;
            }
            catch (Exception ex)
            {
                Log.Error($"StartCinematicMode: {ex}");
            }

            return true;
        }

        /**
         *
         * Verileri temizler.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void Dispose()
        {

        }
    }

    public class StoryTriggerItem
    {
        /**
         *
         * Precondition değerini barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public string Precondition { get; set; }

        /**
         *
         * GoalKey değerini barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public string GoalKey { get; set; }
        
        /**
         *
         * GoalType değerini barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public global::Story.GoalType GoalType { get; set; }

        /**
         *
         * Position değerini barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public ZeroVector3 Position { get; set; }

        /**
         *
         * IsDoorway değerini barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public bool IsDoorway { get; set; }

        /**
         *
         * TriggerRange değerini barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public float TriggerRange { get; set; }

        /**
         *
         * IsTrigger değerini barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public bool IsTrigger { get; set; }

        /**
         *
         * IsTriggerActive değerini barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public bool IsTriggerActive { get; set; }

        /**
         *
         * IsInBase değerini barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public bool IsInBase { get; set; }

        /**
         *
         * IsActive değerini barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public bool IsActive { get; set; }

        /**
         *
         * IsCustomDoor değerini barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public bool IsCustomDoor { get; set; }

        /**
         *
         * SubTriggers değerini barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public List<StoryTriggerItem> SubTriggers { get; set; } = new List<StoryTriggerItem>();

        /**
         *
         * Sınıf ayarlamalarını yapar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public StoryTriggerItem()
        {
        }

        /**
         *
         * Sınıf ayarlamalarını yapar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public StoryTriggerItem(string goalKey, global::Story.GoalType goalType, ZeroVector3 position = null, float triggerRange = 0f, bool isDoorway = false, bool isTrigger = false, StoryTriggerItem subTarget = null)
        {
            this.GoalKey         = goalKey;
            this.GoalType        = goalType;
            this.Position        = position;
            this.IsDoorway       = isDoorway;
            this.TriggerRange    = triggerRange;
            this.IsTrigger       = isTrigger;
            this.IsTriggerActive = true;

            if (subTarget != null)
            {
                this.SubTriggers.Add(subTarget);
            }
        }

        /**
         *
         * Sınıf ayarlamalarını yapar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public StoryTriggerItem(string goalKey, ZeroVector3 position = null, float triggerRange = 0f, bool isDoorway = false, bool isInBase = false, string precondition = null, bool isCustomDoor = false, bool isTrigger = false, bool isTriggerActive = true)
        {
            this.GoalKey         = goalKey;
            this.Position        = position;
            this.IsDoorway       = isDoorway;
            this.TriggerRange    = triggerRange;
            this.IsTrigger       = isTrigger;
            this.IsTriggerActive = isTriggerActive;
            this.IsActive        = true;
            this.IsInBase        = isInBase;
            this.Precondition    = precondition;
            this.IsCustomDoor    = isCustomDoor;
        }

        /**
         *
         * Sınıfı klonlar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public StoryTriggerItem Clone()
        {
            return new StoryTriggerItem()
            {
                Precondition    = this.Precondition,
                GoalKey         = this.GoalKey,
                GoalType        = this.GoalType,
                Position        = this.Position,
                IsDoorway       = this.IsDoorway,
                TriggerRange    = this.TriggerRange,
                IsTrigger       = this.IsTrigger,
                IsTriggerActive = this.IsTriggerActive,
                IsInBase        = this.IsInBase,
                IsActive        = this.IsActive,
                IsCustomDoor    = this.IsCustomDoor,
                SubTriggers     = this.SubTriggers.ToList(),
            };
        }
    }
}