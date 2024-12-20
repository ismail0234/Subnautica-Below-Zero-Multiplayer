namespace Subnautica.Server.Storage
{
    using System;
    using System.IO;
    using System.Linq;

    using global::Story;

    using Subnautica.API.Enums;
    using Subnautica.API.Features;
    using Subnautica.Network.Core;
    using Subnautica.Network.Models.Storage.Story.StoryGoals;
    using Subnautica.Network.Structures;
    using Subnautica.Server.Abstracts;
    using Subnautica.Server.Core;

    using StoryStorage = Network.Models.Storage.Story;

    public class Story : BaseStorage
    {
        /**
         *
         * Technology sınıfını barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public StoryStorage.Story Storage { get; set; }

        /**
         *
         * Sınıf ayarlamalarını yapar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public override void Start(string serverId)
        {
            this.ServerId = serverId;
            this.FilePath = Paths.GetMultiplayerServerSavePath(this.ServerId, "Story.bin");
            this.InitializePath();
            this.Load();
        }

        /**
         *
         * Sunucu hikayeyi önbelleğe yükler.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public override void Load()
        {
            if (File.Exists(this.FilePath))
            {
                lock (this.ProcessLock)
                {
                    try
                    {
                        this.Storage = NetworkTools.Deserialize<StoryStorage.Story>(File.ReadAllBytes(this.FilePath));
                    }
                    catch (Exception e)
                    {
                        Log.Error($"Story.Load: {e}");
                    }
                }
            }
            else
            {
                this.Storage = new StoryStorage.Story();
                this.SaveToDisk();
            }

            if (Core.Server.DEBUG)
            {
                Log.Info($"Story Call Details: {this.Storage.CompletedCalls.Count}");
                Log.Info("---------------------------------------------------------------");
                foreach (var item in this.Storage.CompletedCalls)
                {
                    Log.Info(string.Format("CallId: {0}", item));
                }
                Log.Info("---------------------------------------------------------------");

                Log.Info($"Story Cinematic Details: {this.Storage.CompletedCinematics.Count}");
                Log.Info("---------------------------------------------------------------");
                foreach (var item in this.Storage.CompletedCinematics)
                {
                    Log.Info(string.Format("CinematicId: {0}", item));
                }
                Log.Info("---------------------------------------------------------------");

                Log.Info($"Story Signal Details: {this.Storage.Signals.Count}");
                Log.Info("---------------------------------------------------------------");
                foreach (var item in this.Storage.Signals)
                {
                    Log.Info(string.Format("UniqueId: {0}, SignalType: {1}", item.UniqueId, item.SignalType));
                }
                Log.Info("---------------------------------------------------------------");

                Log.Info($"Story Goal Details: {this.Storage.CompletedGoals.Count}");
                Log.Info("---------------------------------------------------------------");
                foreach (var item in this.Storage.CompletedGoals)
                {
                    Log.Info(string.Format("Key: {0}, GoalType: {1}, IsPlayMuted: {2}, FinishedTime: {3}", item.Key, item.GoalType, item.IsPlayMuted, item.FinishedTime));
                }
                Log.Info("---------------------------------------------------------------");
            }
        }

        /**
         *
         * Verileri diske yazar
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public override void SaveToDisk()
        {
            lock (this.ProcessLock)
            {
                this.WriteToDisk(this.Storage);
            }
        }

        /**
         *
         * Oynanabilir cinematik olup/olmadığına bakar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public bool IsPlayableCinematic(StoryCinematicType cinematicType)
        {
            lock (this.ProcessLock)
            {
                return !Server.Instance.Storages.Story.Storage.CompletedCinematics.Contains(cinematicType);
            }
        }

        /**
         *
         * Yeni bir hedef ekler.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public bool IsGoalComplete(string goalKey)
        {
            lock (this.ProcessLock)
            {
                return Server.Instance.Storages.Story.Storage.CompletedGoals.Any(q => q.Key == goalKey);
            }
        }

        /**
         *
         * Gelen çağrıyı ayarlar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void SetIncomingCall(string goalKey)
        {
            lock (this.ProcessLock)
            {
                Server.Instance.Storages.Story.Storage.IncomingCallGoalKey = goalKey;
            }
        }

        /**
         *
         * Oynanabilir cinematiği tamamlar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public bool CompleteCinematic(StoryCinematicType cinematicType)
        {
            lock (this.ProcessLock)
            {
                if (Server.Instance.Storages.Story.Storage.CompletedCinematics.Contains(cinematicType))
                {
                    return false;
                }

                Server.Instance.Storages.Story.Storage.CompletedCinematics.Add(cinematicType);
                return true;
            }
        }

        /**
         *
         * Oynanabilir cinematiği kaldırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void RemoveCinematic(StoryCinematicType cinematicType)
        {
            lock (this.ProcessLock)
            {
                Server.Instance.Storages.Story.Storage.CompletedCinematics.Remove(cinematicType);
            }
        }

        /**
         *
         * Yeni bir hedef ekler.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public bool CompleteCall(string callGoalKey)
        {
            lock (this.ProcessLock)
            {
                if (Server.Instance.Storages.Story.Storage.CompletedCalls.Contains(callGoalKey))
                {
                    return false;
                }

                Server.Instance.Storages.Story.Storage.CompletedCalls.Add(callGoalKey);
                Server.Instance.Storages.Story.Storage.IncomingCallGoalKey = null;

                return true;
            }
        }

        /**
         *
         * Yeni bir hedef ekler.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public bool CompleteGoal(string storyKey, global::Story.GoalType goalType = GoalType.Story, bool isPlayMuted = false)
        {
            lock (this.ProcessLock)
            {
                if (Server.Instance.Storages.Story.Storage.CompletedGoals.Any(q => q.Key == storyKey && q.GoalType == goalType && q.IsPlayMuted == isPlayMuted))
                {
                    return false;
                }

                return Server.Instance.Storages.Story.Storage.CompletedGoals.Add(new ZeroStoryGoal()
                {
                    Key          = storyKey,
                    GoalType     = goalType,
                    IsPlayMuted  = isPlayMuted,
                    FinishedTime = Server.Instance.Logices.World.GetServerTime(),
                });
            }
        }

        /**
         *
         * Hedefi kaldırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void RemoveGoal(string storyKey)
        {
            lock (this.ProcessLock)
            {
                Server.Instance.Storages.Story.Storage.CompletedGoals.RemoveWhere(q => q.Key == storyKey);
            }
        }

        /**
         *
         * Yeni bir sinyal ekler.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public ZeroStorySignal AddSignal(UnlockSignalData.SignalType signalType, ZeroVector3 targetPosition, string targetDescription, bool isRemoved = false)
        {
            lock (this.ProcessLock) 
            {
                if (Server.Instance.Storages.Story.Storage.Signals.Any(q => q.SignalType == signalType && q.TargetPosition == targetPosition && q.TargetDescription == targetDescription))
                {
                    return null;
                }

                var signal = new ZeroStorySignal()
                {
                    UniqueId          = Network.Identifier.GenerateUniqueId(),
                    SignalType        = signalType,
                    TargetPosition    = targetPosition,
                    TargetDescription = targetDescription,
                    IsRemoved         = isRemoved,
                };

                if (Server.Instance.Storages.Story.Storage.Signals.Add(signal))
                {
                    return signal;
                }

                return null;
            }
        }
    }
}
