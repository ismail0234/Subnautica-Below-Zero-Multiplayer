namespace Subnautica.Server.Logic
{
    using System.Collections.Generic;
    using System.Linq;

    using Subnautica.API.Enums;
    using Subnautica.API.Extensions;
    using Subnautica.API.Features;
    using Subnautica.API.Features.NetworkUtility;
    using Subnautica.Network.Models.Server;
    using Subnautica.Network.Models.Storage.Story.Components;
    using Subnautica.Network.Structures;
    using Subnautica.Server.Abstracts;
    using Subnautica.Server.Core;

    public class StoryTrigger : BaseLogic
    {
        /**
         *
         * Timing nesnesini barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public StopwatchItem Timing { get; set; } = new StopwatchItem(1000f);

        /**
         *
         * Triggers nesnesini barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public List<StoryTriggerItem> Triggers { get; set; } = new List<StoryTriggerItem>();

        /**
         *
         * Oyun Sonu hedefleri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private List<string> EndGameGoals { get; set; } = new List<string>()
        {
            "OnEndGameBegin",
            "EndofRepairPillar2",
            "EndofRepairPillar1",
            "EndgameRepairsComplete",
            "EndGameEnterShip",
            "EndGame5GateOpen",
            "EndGame5TeleportBegin",
            "EndGame5TeleportEnd",
            "EndGame5Homeworld",
            "ShowGameCredits",
        };

        /**
         *
         * Oyun Sonu Cinematicleri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private List<StoryCinematicType> EndGameCinematics { get; set; } = new List<StoryCinematicType>()
        {
            StoryCinematicType.StoryShieldBaseEndGate,
            StoryCinematicType.StoryEndGameAlanFirstMeet,
            StoryCinematicType.StoryEndGameRepairPillar1,
            StoryCinematicType.StoryEndGameRepairPillar2,
            StoryCinematicType.StoryEndGameReturnArms,
            StoryCinematicType.StoryEndGameEnterShip,
            StoryCinematicType.StoryEndGameGoToHomeWorld,
        };

        /**
         *
         * Sınıfı başlatır
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public override void OnStart()
        {
            this.ResetEndGame();

            foreach (var trigger in Network.Story.Triggers)
            {
                if (Server.Instance.Storages.Story.IsGoalComplete(trigger.GoalKey) || Server.Instance.Storages.Story.Storage.CompletedTriggers.Contains(trigger.GoalKey))
                {
                    continue;
                }

                this.Triggers.Add(trigger.Clone());
            }

            this.AddCustomDoorway(StoryCinematicType.StoryMarg1              , new ZeroQuaternion(0.0f, -0.2f, 0.0f, 1.0f), new ZeroVector3(1.1f, 1.2f, 1.0f));
            this.AddCustomDoorway(StoryCinematicType.StoryShieldBaseInnerGate, new ZeroQuaternion(0.0f, 0.0f, 0.0f, 1.0f) , new ZeroVector3(1.1f, 1.2f, 1.0f));
            this.AddCustomDoorway(StoryCinematicType.StoryShieldBaseEndGate  , new ZeroQuaternion(0.0f, 0.0f, 0.0f, 1.0f) , new ZeroVector3(1.1f, 1.6f, 1.0f));
        }

        /**
         *
         * Tetikleyicinin varlığını kontrol eder.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public bool IsTriggerExists(string goalKey)
        {
            return this.Triggers.Any(q => q.GoalKey == goalKey && q.IsTrigger);
        }

        /**
         *
         * Her sabit tick'de tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public override void OnFixedUpdate(float fixedDeltaTime)
        {
            if (API.Features.World.IsLoaded && this.Timing.IsFinished())
            {
                this.Timing.Restart();

                foreach (var trigger in this.Triggers)
                {
                    if (!trigger.IsActive || !trigger.IsTriggerActive || trigger.Position == null || trigger.TriggerRange == -1f)
                    {
                        continue;
                    }

                    if (!this.IsPreconditionComplete(trigger.GoalKey))
                    {
                        continue;
                    }
                    
                    var playerCount = this.GetClosestPlayerCount(trigger.Position, trigger.TriggerRange, trigger.IsInBase);
                    var maxPlayer = Server.Instance.GetPlayerCount();

                    if (playerCount >= maxPlayer)
                    {
                        if (trigger.IsTrigger)
                        {
                            this.Trigger(trigger);
                        }
                    }
                    else
                    {
                        foreach (var player in Server.Instance.GetPlayers())
                        {
                            if (player.IsFullConnected && this.IsPlayerInBase(player, trigger.IsInBase) && trigger.Position.Distance(player.Position) < trigger.TriggerRange && playerCount > 0)
                            {
                                this.SendClosestPlayerCount(player, playerCount, maxPlayer);
                            }
                        }
                    }
                }
            }
        }

        /**
         *
         * Hikayeyi tetikler.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void Trigger(StoryTriggerItem trigger)
        {
            this.CompleteTrigger(trigger.GoalKey);

            if (Server.Instance.Storages.Story.CompleteGoal(trigger.GoalKey, trigger.GoalType, false))
            {
                StoryTriggerArgs packet = new StoryTriggerArgs()
                {
                    GoalKey     = trigger.GoalKey,
                    GoalType    = trigger.GoalType,
                    IsTrigger   = true,
                    TriggerTime = Server.Instance.Logices.World.GetServerTime() + 0.25f,
                };

                foreach (var player in Server.Instance.GetPlayers())
                {
                    player.SendPacket(packet);
                }
            }
        }

        /**
         *
         * Oyuncu sayılarını iletir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void SendClosestPlayerCount(AuthorizationProfile player, byte playerCount, byte maxPlayer)
        {
            StoryTriggerArgs packet = new StoryTriggerArgs()
            {
                IsTrigger      = true,
                PlayerCount    = playerCount,
                MaxPlayerCount = maxPlayer
            };

            player.SendPacket(packet);
        }

        /**
         *
         * Hikayeyi tetikler.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public bool AddCheckTrigger(string storyKey)
        {
            var trigger = this.Triggers.FirstOrDefault(q => q.GoalKey == storyKey);
            if (trigger == null || trigger.IsActive)
            {
                return false;
            }

            if (Server.Instance.Storages.Story.IsGoalComplete(storyKey))
            {
                return false;
            }

            trigger.IsActive = true;
            return true;
        }

        /**
         *
         * Tetikleyici pasif yapar. tetikler.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public bool CompleteTrigger(string storyKey)
        {
            var trigger = this.Triggers.FirstOrDefault(q => q.GoalKey == storyKey);
            if (trigger == null)
            {
                return false;
            }

            trigger.IsActive = false;

            Server.Instance.Storages.Story.Storage.CompletedTriggers.Add(storyKey);

            if (trigger.IsCustomDoor)
            {
                var door = Server.Instance.Storages.Story.Storage.CustomDoorways.FirstOrDefault(q => q.UniqueId == storyKey);
                if (door != null)
                {
                    door.IsActive = false;
                }
            }

            return true;
        }

        /**
         *
         * Ön koşul tamamlandı mı?
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public bool IsPreconditionComplete(string goalKey)
        {
            var trigger = this.Triggers.FirstOrDefault(q => q.GoalKey == goalKey);
            if (trigger == null)
            {
                return false;
            }

            return trigger.Precondition.IsNull() || Server.Instance.Storages.Story.IsGoalComplete(trigger.Precondition);
        }

        /**
         *
         * Tetiklemenin aktif olup olmadığına bakar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public bool IsTriggerActive(string goalKey)
        {
            var trigger = this.Triggers.FirstOrDefault(q => q.GoalKey == goalKey);
            if (trigger == null)
            {
                return false;
            }

            return trigger.IsActive;
        }

        /**
         *
         * Cinematic tamamlanmış mı?
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public bool IsCinematicFinished(string goalKey)
        {
            return Server.Instance.Storages.Story.Storage.CompletedTriggers.Contains(goalKey);
        }

        /**
         *
         * Hedefin yakınındaki oyuncuları kontrol eder ve tamamlanabilirliği döner.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public bool IsCompleteableCinematic(string goalKey)
        {
            if (goalKey == "None")
            {
                return false;
            }

            if (!this.IsTriggerActive(goalKey))
            {
                return false;
            }

            if (!this.IsPreconditionComplete(goalKey))
            {
                return false;
            }

            var closestPlayerCount = this.GetClosestPlayerCount(goalKey);
            return closestPlayerCount > 0 && closestPlayerCount >= Server.Instance.GetPlayerCount();
        }

        /**
         *
         * Hedefin yakınındaki oyuncu sayısını döner.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public byte GetClosestPlayerCount(string goalKey)
        {
            var trigger = this.Triggers.FirstOrDefault(q => q.GoalKey == goalKey);
            if (trigger == null)
            {
                return 0;
            }

            return this.GetClosestPlayerCount(trigger.Position, trigger.TriggerRange, trigger.IsInBase);
        }

        /**
         *
         * Hedefin yakınındaki oyuncu sayısını döner.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public byte GetClosestPlayerCount(ZeroVector3 position, float range, bool isInBaseCheck)
        {
            if (range == -1f)
            {
                return Server.Instance.GetPlayerCount();
            }

            byte closestPlayerCount = 0;

            foreach (var player in Server.Instance.GetPlayers())
            {
                if (player.IsFullConnected && this.IsPlayerInBase(player, isInBaseCheck) && position.Distance(player.Position) < range)
                {
                    closestPlayerCount++;
                }
            }

            return closestPlayerCount;
        }

        /**
         *
         * Oyuncu üs içerisinde mi?
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private bool IsPlayerInBase(AuthorizationProfile player, bool isInBaseCheck = false)
        {
            if (!isInBaseCheck)
            {
                return true;
            }

            return !string.IsNullOrEmpty(player.SubrootId) || !string.IsNullOrEmpty(player.InteriorId);
        }

        /**
         *
         * Özel kapıları ekler.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private bool AddCustomDoorway(StoryCinematicType cinematicType, ZeroQuaternion rotation, ZeroVector3 scale)
        {
            if (Server.Instance.Storages.Story.Storage.CustomDoorways.Any(q => q.UniqueId == cinematicType.ToString()))
            {
                return false;
            }

            var trigger = this.Triggers.FirstOrDefault(q => q.GoalKey == cinematicType.ToString());
            if (trigger == null)
            {
                return false;
            }

            Server.Instance.Storages.Story.Storage.CustomDoorways.Add(new CustomDoorwayComponent(cinematicType.ToString(), trigger.Position, rotation, scale, true));
            return true;
        }

        /**
         *
         * Oyun sonu ayarlarını sıfırlar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void ResetEndGame()
        {
            foreach (var goalKey in this.EndGameGoals)
            {
                Server.Instance.Storages.Story.RemoveGoal(goalKey);
            }

            foreach (var cinematicType in this.EndGameCinematics)
            {
                Server.Instance.Storages.Story.Storage.CompletedTriggers.Remove(cinematicType.ToString());
                Server.Instance.Storages.Story.RemoveGoal(cinematicType.ToString());
                Server.Instance.Storages.Story.RemoveCinematic(cinematicType);

                var customDoor = Server.Instance.Storages.Story.Storage.CustomDoorways.FirstOrDefault(q => q.UniqueId == cinematicType.ToString());
                if (customDoor != null)
                {
                    customDoor.IsActive = true;
                }
            }
        }
    }
}