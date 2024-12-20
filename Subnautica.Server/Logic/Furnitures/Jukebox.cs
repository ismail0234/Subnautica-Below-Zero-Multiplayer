namespace Subnautica.Server.Logic.Furnitures
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;

    using Subnautica.API.Enums;
    using Subnautica.API.Extensions;
    using Subnautica.Server.Abstracts;
    using Subnautica.Server.Core;
    using Subnautica.Server.Extensions;
    using UnityEngine;

    using ServerModel      = Subnautica.Network.Models.Server;
    using WorldEntityModel = Subnautica.Network.Models.WorldEntity.DynamicEntityComponents;

    public class Jukebox : BaseLogic
    {
        /**
         *
         * Timing nesnesini barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public API.Features.StopwatchItem Timing { get; set; } = new API.Features.StopwatchItem(2000f);

        /**
         *
         * Müzik Metinleri
         *
         * @author Ismail  <ismaiil_0234@hotmail.com>
         *
         */
        private readonly Dictionary<string, string> MusicLabels = new Dictionary<string, string>()
        {
          { "event:/jukebox/jukebox_one"                  , "Subnautica - Jukebox One" },
          { "event:/jukebox/jukebox_deepdive"             , "TryHardNinja - Deep Dive (feat. Zach Boucher)" },
          { "event:/jukebox/jukebox_survive"              , "Divide Music - Survive" },
          { "event:/jukebox/jukebox_diepeacefully"        , "First Sun - Die Peacefully" },
          { "event:/jukebox/jukebox_dontholdyourbreath"   , "JT Music - Don't Hold Your Breath" },
          { "event:/jukebox/jukebox_takethedive"          , "JT Music - Take the Dive" },
          { "event:/jukebox/jukebox_deepblue"             , "Miracle of Sound - Deep Blue" },
          { "event:/jukebox/jukebox_divingintoodeep"      , "NerdOut - Diving In Too Deep" },
          { "event:/jukebox/jukebox_subnauticstimulus"    , "Rockit Gaming - Subnautic Stimulus" },
          { "event:/jukebox/jukebox_riteofthedeep"        , "Steve Pardo - Rite of the Deep" }
        };

        /**
         *
         * Müzik Uzunlukları
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private readonly Dictionary<string, uint> MusicLengths = new Dictionary<string, uint>()
        {
            { "event:/jukebox/jukebox_one"                  , 237714 },
            { "event:/jukebox/jukebox_survive"              , 198577 },
            { "event:/jukebox/jukebox_diepeacefully"        , 180415 }, 
            { "event:/jukebox/jukebox_divingintoodeep"      , 195813 }, 
            { "event:/jukebox/jukebox_deepdive"             , 213846 }, 
            { "event:/jukebox/jukebox_deepblue"             , 240902 }, 
            { "event:/jukebox/jukebox_subnauticstimulus"    , 200468 }, 
            { "event:/jukebox/jukebox_dontholdyourbreath"   , 255692 }, 
            { "event:/jukebox/jukebox_takethedive"          , 300000 }, 
            { "event:/jukebox/jukebox_riteofthedeep"        , 187071 }, 
        };

        /**
         *
         * StopwatchMusicTime barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private Stopwatch StopwatchMusicTime { get; set; } = new Stopwatch();
        /**
         *
         * MusicData verisini barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public Network.Models.Metadata.Jukebox CurrentMusic { get; set; }

        /**
         *
         * Kullanıcılara veri gönderme durumnu aktif eder.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private bool IsSendTrigger { get; set; }

        /**
         *
         * CurrentJukeboxId değerini barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private string CurrentJukeboxId { get; set; }

        /**
         *
         * Sınıfı başlatır
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public override void OnStart()
        {
            if (!Core.Server.Instance.Storages.World.Storage.JukeboxDisks.Contains("event:/jukebox/jukebox_one"))
            {
                Core.Server.Instance.Storages.World.Storage.JukeboxDisks.Add("event:/jukebox/jukebox_one");
            }

            this.Reset();
            this.SortPlaylist();
        }

        /**
         *
         * Zamanlayıcının geri dönüş methodu
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public override void OnFixedUpdate(float fixedDeltaTime)
        {
            if (this.CurrentMusic == null)
            {
                return;
            }

            if (this.CurrentJukeboxId.IsNull())
            {
                this.Reset();
                return;
            }

            var constructionUniqueId = this.GetConstructionUniqueId();
            if (constructionUniqueId == null)
            {
                this.Reset();
                return;
            }

            if (this.CurrentMusic.IsStoped)
            {
                this.CurrentMusic.CurrentPlayingTrack = null;
            }

            if (this.CurrentMusic.IsPaused || this.CurrentMusic.IsStoped)
            {
                if (this.StopwatchMusicTime.IsRunning)
                {
                    this.StopwatchMusicTime.Stop();
                }
            }
            else
            {
                if (!this.StopwatchMusicTime.IsRunning)
                {
                    this.StopwatchMusicTime.Start();
                }
            }

            if (this.IsPlaying() && this.Timing.IsFinished() && fixedDeltaTime != 0f)
            {
                this.Timing.Restart();

                var jukebox = API.Features.Network.Identifier.GetComponentByGameObject<global::JukeboxInstance>(constructionUniqueId);
                if (jukebox)
                {

                    float requiredPower = (fixedDeltaTime + 2f) * 0.1f * this.CurrentMusic.Volume;

                    if (!this.ConsumePower(jukebox, requiredPower))
                    {
                        this.StopwatchMusicTime.Reset();
                        this.CurrentMusic.CurrentPlayingTrack = null;
                        this.CurrentMusic.IsStoped = true;
                        this.CurrentMusic.IsPaused = false;
                        this.IsSendTrigger = true;
                    }
                }
            }

            if (!this.CurrentMusic.IsStoped && !this.CurrentMusic.IsPaused && this.CurrentMusic.CurrentPlayingTrack.IsNull())
            {
                this.ChangeMusic(true, true);
            }
            else if (this.CurrentMusic.IsNext)
            {
                this.ChangeMusic(true, true);
            }
            else if (this.CurrentMusic.IsPrevious)
            {
                this.ChangeMusic(false, true);
            }
            else if (this.GetMusicLength() > 0 && this.GetCurrentPosition() >= this.GetMusicLength())
            {
                this.ChangeMusic(true, false);
            }

            if (this.IsSendTrigger)
            {
                this.SendMusicToClients();
            }
        }

        /**
         *
         * Gücü tüketir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private bool ConsumePower(global::JukeboxInstance jukebox, float requiredPower)
        {
            if (jukebox.gameObject.GetComponentInParent<global::SeaTruckSegment>())
            {
                var entity = Core.Server.Instance.Storages.World.GetDynamicEntity(this.CurrentJukeboxId);
                if (entity == null)
                {
                    return false;
                }

                var seaTruck = entity.GetSeaTruckHeadModule();
                if (seaTruck == null)
                {
                    return false;
                }

                var component = seaTruck.Component.GetComponent<WorldEntityModel.SeaTruck>();
                if (component == null)
                {
                    return false;
                }

                return Server.Instance.Logices.VehicleEnergyTransmission.ConsumeEnergy(component.PowerCells, requiredPower);
            }
            else if (jukebox._baseComp)
            {
                if (jukebox._baseComp.IsPowered(jukebox.transform.position) && jukebox._powerRelay && Core.Server.Instance.Logices.PowerConsumer.HasPower(jukebox._powerRelay, requiredPower))
                {
                    Core.Server.Instance.Logices.PowerConsumer.ConsumePower(jukebox._powerRelay, requiredPower, out float _);
                    return true;
                }
            }

            return false;
        }


        /**
         *
         * Jukebox kutu idsini döner.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public string GetConstructionUniqueId()
        {
            var construction = Core.Server.Instance.Storages.Construction.GetConstruction(this.CurrentJukeboxId);
            if (construction != null)
            {
                return construction.UniqueId;
            }

            var entity = Core.Server.Instance.Storages.World.GetDynamicEntity(this.CurrentJukeboxId);
            if (entity != null)
            {
                return entity.UniqueId;
            }
            
            return null;
        }

        /**
         *
         * Gelen veriyi işler
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void OnDataReceived(AuthorizationProfile profile, string uniqueId, API.Features.CustomProperty music)
        {
            this.CheckJukeboxChanged(uniqueId);

            var type  = music.GetKey<JukeboxProcessType>();
            switch (type)
            {
                case JukeboxProcessType.IsPrevious:
                        this.CurrentMusic.IsPrevious = music.GetValue<bool>();
                    break;
                case JukeboxProcessType.IsNext:
                        this.CurrentMusic.IsNext = music.GetValue<bool>();
                    break;
                case JukeboxProcessType.IsPaused:
                        this.SetPaused(music.GetValue<bool>());
                    break;
                case JukeboxProcessType.IsStoped:
                        this.SetStoped(music.GetValue<bool>());
                    break;
                case JukeboxProcessType.IsShuffled:
                        this.CurrentMusic.IsShuffled = music.GetValue<bool>();
                    break;
                case JukeboxProcessType.RepeatMode:
                        this.CurrentMusic.RepeatMode = music.GetValue<global::Jukebox.Repeat>();
                    break;
                case JukeboxProcessType.Position:
                        this.SetPosition(music.GetValue<float>(), true);
                    break;
                case JukeboxProcessType.Volume:
                        this.CurrentMusic.Volume = music.GetValue<float>();
                    break;
            }

            if (type == JukeboxProcessType.Volume)
            {
                this.SendMusicToClients(profile);
            }
            else
            {
                this.IsSendTrigger = true;
            }

            this.OnUpdate(0f);
        }

        /**
         *
         * Şarkıyı değiştirir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void ChangeMusic(bool forward = true, bool isIgnoreRepeat = false)
        {
            this.CurrentMusic.CurrentPlayingTrack = this.GetNextMusic(this.CurrentMusic.RepeatMode, this.CurrentMusic.CurrentPlayingTrack, forward, isIgnoreRepeat, this.CurrentMusic.IsShuffled);
            this.CurrentMusic.IsNext     = false;
            this.CurrentMusic.IsPrevious = false;
            this.CurrentMusic.IsStoped   = false;
            this.CurrentMusic.IsPaused   = false;
            this.SetPosition(0f);

            if (this.CurrentMusic.CurrentPlayingTrack.IsNull())
            {
                this.SetStoped(true);
            }

            this.IsSendTrigger = true;
        }

        /**
         *
         * Şarkıyı duraklatır/devam ettirir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void SetPaused(bool isPaused)
        {
            this.CurrentMusic.IsPaused = isPaused;

            if (!this.CurrentMusic.IsPaused)
            {
                this.CurrentMusic.IsStoped = false;
            }
        }

        /**
         *
         * Şarkıyı durdurur.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void SetStoped(bool isStoped)
        {
            this.CurrentMusic.IsStoped = isStoped;

            if (isStoped)
            {
                this.CurrentMusic.IsPaused = true;
                this.StopwatchMusicTime.Reset();
            }
        }

        /**
         *
         * Şarkıyı ilerletir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void SetPosition(float position, bool restart = false)
        {
            this.CurrentMusic.Position = position;
            this.StopwatchMusicTime.Reset();
        }

        /**
         *
         * Şarkıyı Çalıyor mu?.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public bool IsPlaying()
        {
            return !string.IsNullOrEmpty(this.CurrentMusic.CurrentPlayingTrack) && !this.CurrentMusic.IsStoped && !this.CurrentMusic.IsPaused;
        }

        /**
         *
         * Oyunculara şarkı verilerini gönderir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void SendMusicToClients(AuthorizationProfile profile = null)
        {
            this.IsSendTrigger = false;

            ServerModel.MetadataComponentArgs request = new ServerModel.MetadataComponentArgs()
            {
                UniqueId  = this.CurrentJukeboxId,
                TechType  = TechType.Jukebox,
                Component = this.GetCurrentMetadata(),
            };

            if (profile != null)
            {
                profile.SendPacketToOtherClients(request);
            }
            else
            {
                Core.Server.SendPacketToAllClient(request);
            }
        }

        /**
         *
         * Mevcut metadata verisini döner.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public Network.Models.Metadata.Jukebox GetCurrentMetadata()
        {
            return new Network.Models.Metadata.Jukebox()
            {
                CurrentPlayingTrack = this.CurrentMusic.CurrentPlayingTrack,
                IsPaused            = this.CurrentMusic.IsPaused,
                IsStoped            = this.CurrentMusic.IsStoped,
                IsNext              = this.CurrentMusic.IsNext,
                IsPrevious          = this.CurrentMusic.IsPrevious,
                RepeatMode          = this.CurrentMusic.RepeatMode,
                IsShuffled          = this.CurrentMusic.IsShuffled,
                Position            = this.GetCurrentPosition() / this.GetMusicLength(),
                Length              = this.GetOriginalMusicLength(),
                Volume              = this.CurrentMusic.Volume,
            };
        }

        /**
         *
         * Müzik kutusu değiştiğinde her şeyi sıfırlar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private void CheckJukeboxChanged(string uniqueId)
        {
            if (uniqueId != this.CurrentJukeboxId)
            {
                this.Reset();
                this.CurrentJukeboxId = uniqueId;
            }
        }

        /**
         *
         * Her şeyi sıfırlar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private void Reset()
        {
            this.CurrentJukeboxId      = null;
            this.CurrentMusic          = new Network.Models.Metadata.Jukebox();
            this.CurrentMusic.Volume   = 1f;
            this.CurrentMusic.IsStoped = true;
            this.StopwatchMusicTime.Reset();
        }
        /**
         *
         * Sonraki veya önceki şarkıyı döner.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private string GetNextMusic(global::Jukebox.Repeat repeatMode, string currentTrack, bool forward, bool isIgnoreRepeat = false, bool shuffle = false)
        {
            if (string.IsNullOrEmpty(currentTrack))
            {
                return MusicLabels.First().Key;
            }

            if (!isIgnoreRepeat)
            {
                if (repeatMode == global::Jukebox.Repeat.Off)
                {
                    return null;
                }

                if (repeatMode == global::Jukebox.Repeat.Track)
                {
                    return currentTrack;
                }
            }

            return this.GetInternalNextMusic(currentTrack, forward, shuffle);
        }

        /**
         *
         * İç Fonksiyon - Sonraki veya önceki şarkıyı döner.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private string GetInternalNextMusic(string currentTrack, bool forward, bool shuffle = false)
        {
            int diskCount = Core.Server.Instance.Storages.World.Storage.JukeboxDisks.Count;
            if (diskCount == 1)
            {
                return currentTrack;
            }

            if (shuffle)
            {
                int currentIndex = Core.Server.Instance.Storages.World.Storage.JukeboxDisks.FindIndex(q => q == currentTrack);
                int randomIndex  = currentIndex;

                do
                {
                    randomIndex = API.Features.Tools.Random.Next(0, diskCount);
                } while (currentIndex == randomIndex);

                return Core.Server.Instance.Storages.World.Storage.JukeboxDisks.ElementAt(randomIndex);
            }

            int foundedIndex = Core.Server.Instance.Storages.World.Storage.JukeboxDisks.IndexOf(currentTrack);
            if (foundedIndex < 0)
            {
                return currentTrack;
            }

            if (forward)
            {
                foundedIndex++;
                if (foundedIndex >= diskCount)
                {
                    foundedIndex = 0;
                }
            }
            else
            {
                foundedIndex--;
                if (foundedIndex < 0)
                {
                    foundedIndex = diskCount - 1;
                }
            }

            return Core.Server.Instance.Storages.World.Storage.JukeboxDisks.ElementAt(foundedIndex);
        }

        /**
         *
         * Şarkının mevcut süresini döner.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private float GetCurrentPosition()
        {
            return (this.CurrentMusic.Position * this.GetMusicLength()) + (this.StopwatchMusicTime.ElapsedMilliseconds / 1000f);
        }

        /**
         *
         * Şarkı uzunluğunu döner.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private float GetMusicLength()
        {
            if (this.CurrentMusic.CurrentPlayingTrack != null && this.MusicLengths.TryGetValue(this.CurrentMusic.CurrentPlayingTrack, out uint length))
            {
                return (float)length / 1000f;
            }

            return 0f;
        }

        /**
         *
         * Şarkı uzunluğunu döner.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private uint GetOriginalMusicLength()
        {
            if (this.CurrentMusic.CurrentPlayingTrack != null && this.MusicLengths.TryGetValue(this.CurrentMusic.CurrentPlayingTrack, out uint length))
            {
                return length;
            }

            return 0;
        }

        /**
         *
         * Şarkı listesini sıralar
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void SortPlaylist()
        {
            Core.Server.Instance.Storages.World.Storage.JukeboxDisks.Sort(new Comparison<string>(this.PlaylistComparer));
        }

        /**
         *
         * Zamanlayıcının geri dönüş methodu
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private int PlaylistComparer(string strA, string strB)
        {
            this.MusicLabels.TryGetValue(strA, out strA);
            this.MusicLabels.TryGetValue(strB, out strB);

            return string.Compare(strA, strB, StringComparison.OrdinalIgnoreCase);
        }
    }
}
