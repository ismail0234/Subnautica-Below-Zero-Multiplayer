namespace Subnautica.Client.MonoBehaviours.World
{
    using System.Collections.Generic;

    using Subnautica.API.Extensions;
    using Subnautica.API.Features;
    using Subnautica.Network.Models.Server;

    using UnityEngine;

    public class MultiplayerScannerRoom : MonoBehaviour
    {
        /**
         *
         * Değişiklik yapıldı mı?
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private bool IsChanged { get; set; }

        /**
         *
         * Harita odasını barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private MapRoomFunctionality MapRoom { get; set; }

        /**
         *
         * ResourceTracker barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private uGUI_ResourceTracker ResourceTracker { get; set; }

        /**
         *
         * Nesneleri barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private List<BaseMapRoomTransmissionItem> Items { get; set; } = new List<BaseMapRoomTransmissionItem>();

        /**
         *
         * Timing barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private static StopwatchItem Timing { get; set; } = new StopwatchItem(1000f);

        /**
         *
         * Sınıf başlatılırken tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void Awake()
        {
            if (this.TryGetComponent<MapRoomFunctionality>(out var mapRoom))
            {
                this.MapRoom = mapRoom;
            }

            if (this.MapRoom.mapBlipRoot == null)
            {
                this.MapRoom.mapBlipRoot = new GameObject("MapBlipRoot");
                this.MapRoom.mapBlipRoot.transform.SetParent(mapRoom.wireFrameWorld, false);
            }

            this.ResourceTracker = GameObject.FindObjectOfType<uGUI_ResourceTracker>();
        }

        /**
         *
         * Nesneleri günceller.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void SetItems(List<BaseMapRoomTransmissionItem> items)
        {
            this.Items.Clear();
            this.Items.AddRange(items);

            this.IsChanged = true;
        }

        /**
         *
         * Her sabit karede tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void FixedUpdate()
        {
            if (this.IsChanged)
            {
                this.IsChanged = false;

                this.UpdateBlips(this.MapRoom.mapBlipRoot.transform.position);
                this.UpdateCameraAndPlayerBlips(this.MapRoom.mapBlipRoot.transform.position);
            }

            if (Timing.IsFinished())
            {
                Timing.Restart();

                this.UpdateResourceNodes();
            }
        }

        /**
         *
         * Nesne noktalarını günceller.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private void UpdateBlips(Vector3 rootPosition)
        {
            for (int index = 0; index < this.Items.Count; index++)
            {
                var item     = this.Items[index];
                var position = (item.Position.ToVector3() - rootPosition) * this.MapRoom.mapScale;

                if (index >= this.MapRoom.mapBlips.Count)
                {
                    GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.MapRoom.blipPrefab, position, Quaternion.identity);
                    gameObject.transform.SetParent(this.MapRoom.mapBlipRoot.transform, false);

                    this.MapRoom.mapBlips.Add(gameObject);
                }

                this.MapRoom.mapBlips[index].transform.localPosition = position;
                this.MapRoom.mapBlips[index].SetActive(true);
            }

            for (int index = this.Items.Count; index < this.MapRoom.mapBlips.Count; index++)
            {
                this.MapRoom.mapBlips[index].SetActive(false);
            }
        }

        /**
         *
         * Kamera noktalarını günceller.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private void UpdateCameraAndPlayerBlips(Vector3 rootPosition)
        {
            var scanRange = this.MapRoom.scanRange * this.MapRoom.scanRange;

            var counter = 0;
            for (int index = 0; index < MapRoomCamera.cameras.Count; index++)
            {
                var camera = MapRoomCamera.cameras[index];
                if (camera.pickupAble.attached)
                {
                    continue;
                }

                if ((this.MapRoom.wireFrameWorld.position - camera.transform.position).sqrMagnitude > scanRange)
                {
                    continue;
                }

                var cameraPosition = (camera.transform.position - rootPosition) * this.MapRoom.mapScale;
                if (counter >= this.MapRoom.cameraBlips.Count)
                {
                    MapRoomCameraBlip mapRoomCameraBlip = UnityEngine.Object.Instantiate<MapRoomCameraBlip>(this.MapRoom.cameraBlipPrefab, cameraPosition, Quaternion.identity);
                    mapRoomCameraBlip.transform.SetParent(this.MapRoom.cameraBlipRoot.transform, false);

                    this.MapRoom.cameraBlips.Add(mapRoomCameraBlip);
                }

                this.MapRoom.cameraBlips[counter].transform.localPosition = cameraPosition;
                this.MapRoom.cameraBlips[counter].gameObject.SetActive(true);
                this.MapRoom.cameraBlips[counter].cameraName.text = global::Language.main.GetFormat<int>("MapRoomCameraInfoScreen", camera.GetCameraNumber());

                counter++;
            }

            foreach (var player in ZeroPlayer.GetAllPlayers())
            {
                if (player.IsMine)
                {
                    continue;
                }

                if ((this.MapRoom.wireFrameWorld.position - player.Position).sqrMagnitude > scanRange)
                {
                    continue;
                }

                var playerPosition = (player.Position - rootPosition) * this.MapRoom.mapScale;
                if (counter >= this.MapRoom.cameraBlips.Count)
                {
                    MapRoomCameraBlip mapRoomCameraBlip = UnityEngine.Object.Instantiate<MapRoomCameraBlip>(this.MapRoom.cameraBlipPrefab, playerPosition, Quaternion.identity);
                    mapRoomCameraBlip.transform.SetParent(this.MapRoom.cameraBlipRoot.transform, false);

                    this.MapRoom.cameraBlips.Add(mapRoomCameraBlip);
                }

                this.MapRoom.cameraBlips[counter].transform.localPosition = playerPosition;
                this.MapRoom.cameraBlips[counter].gameObject.SetActive(true);
                this.MapRoom.cameraBlips[counter].cameraName.text = player.NickName;

                counter++;
            }

            for (int index = counter; index < this.MapRoom.cameraBlips.Count; ++index)
            {
                this.MapRoom.cameraBlips[index].gameObject.SetActive(false);
            }
        }

        /**
         *
         * Kaynakları günceller.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private void UpdateResourceNodes()
        {
            this.ResourceTracker.nodes.Clear();
            this.ResourceTracker.mapRooms.Clear();

            if (this.ResourceTracker.showGUI)
            {
                MapRoomScreen screen = uGUI_CameraDrone.main.GetScreen();
                if (screen != null)
                {
                    this.ResourceTracker.mapRooms.Add(screen.mapRoomFunctionality);
                }
                else
                {
                    MapRoomFunctionality.GetMapRoomsInRange(MainCamera.camera.transform.position, 500f, this.ResourceTracker.mapRooms);
                }

                foreach (var mapRoom in this.ResourceTracker.mapRooms)
                {
                    var activeTechType = mapRoom.GetActiveTechType();
                    if (activeTechType != TechType.None)
                    {
                        foreach (var item in mapRoom.gameObject.EnsureComponent<MultiplayerScannerRoom>().Items)
                        {
                            this.ResourceTracker.nodes.Add(new ResourceTrackerDatabase.ResourceInfo()
                            {
                                uniqueId = item.UniqueId,
                                position = item.Position.ToVector3(),
                                techType = activeTechType
                            });
                        }
                    }
                }
            }
        }
    }
}
