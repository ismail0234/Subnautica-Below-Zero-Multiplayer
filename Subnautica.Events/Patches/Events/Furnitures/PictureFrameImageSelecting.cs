namespace Subnautica.Events.Patches.Events.Furnitures
{
    using System;
    using System.Diagnostics;
    using System.IO;

    using HarmonyLib;

    using Subnautica.API.Extensions;
    using Subnautica.API.Features;
    using Subnautica.Events.EventArgs;
    using Subnautica.Events.Patches.Fixes.Interact;

    using UnityEngine;

    [HarmonyPatch(typeof(global::PictureFrame), nameof(global::PictureFrame.SelectImage))]
    public static class PictureFrameImageSelecting
    {
        /**
         *
         * Fonksiyonu yamalar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private static bool Prefix(global::PictureFrame __instance, string image)
        {
            if (!Network.IsMultiplayerActive)
            {
                return true;
            }

            if (EventBlocker.IsEventBlocked(TechType.PictureFrame))
            {
                return true;
            }
            
            if (string.Equals(__instance.fileName, image, StringComparison.Ordinal))
            {
                return false;
            }

            var filePath = ScreenshotManager.Combine(ScreenshotManager.savePath, image);
            if (!File.Exists(filePath))
            {
                return false;
            }

            var uniqueId = PictureFrame.GetUniqueId(__instance);
            if (uniqueId.IsNull())
            {
                return false;
            }

            var imageData = GetImageData(filePath);
            if (imageData == null)
            {
                return false;
            }

            if (__instance.GetComponentInParent<Constructable>())
            {
                try
                {
                    PictureFrameImageSelectingEventArgs args = new PictureFrameImageSelectingEventArgs(uniqueId, image, imageData);

                    Handlers.Furnitures.OnPictureFrameImageSelecting(args);

                    return args.IsAllowed;
                }
                catch (Exception e)
                {
                    Log.Error($"Furnitures.PictureFrameImageSelecting: {e}\n{e.StackTrace}");
                    return true;
                }
            }
            else
            {
                try
                {
                    SeaTruckPictureFrameImageSelectingEventArgs args = new SeaTruckPictureFrameImageSelectingEventArgs(uniqueId, image, imageData);

                    Handlers.Vehicle.OnSeaTruckPictureFrameImageSelecting(args);

                    return args.IsAllowed;
                }
                catch (Exception e)
                {
                    Log.Error($"Furnitures.SeaTruckPictureFrameImageSelecting: {e}\n{e.StackTrace}");
                    return true;
                }

            }
        }

        /**
         *
         * Resim verisini döner.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private static byte[] GetImageData(string filePath)
        {
            var imageData = File.ReadAllBytes(filePath);
            if (imageData == null || imageData.Length <= 0)
            {
                return null;
            }

            Texture2D texture = ScreenshotManager.CreateScreenshotTextureFromBytesAndReEncodeIfMaximumQuality(filePath, imageData);

            if (texture.height > 1024)
            {
                float num = (float)texture.height / (float)texture.width;

                TextureScale.Scale(texture, 1024, Mathf.FloorToInt(num * (float)1024));

                imageData = texture.EncodeToJPG();

                UnityEngine.Object.Destroy(texture);
            }

            return imageData;
        }
    }

    public class TextureScale
    {
        /**
         *
         * Değerleri barındırır.
         *
         */
        private static Color[] texColors;
        private static Color[] newColors;
        private static int w;
        private static float ratioX;
        private static float ratioY;
        private static int w2;

        /**
         *
         * Ölçeklendirir.
         *
         */
        public static void Scale(Texture2D tex, int newWidth, int newHeight)
        {
            texColors = tex.GetPixels();
            newColors = new Color[newWidth * newHeight];
            ratioX = 1.0f / ((float)newWidth / (tex.width - 1));
            ratioY = 1.0f / ((float)newHeight / (tex.height - 1));
            w = tex.width;
            w2 = newWidth;

            BilinearScale(0, newHeight);

            tex.Resize(newWidth, newHeight);
            tex.SetPixels(newColors);
            tex.Apply();
        }

        /**
         *
         * Ölçeklendirir.
         *
         */
        private static void BilinearScale(int start, int end)
        {
            for (var y = start; y < end; y++)
            {
                int yFloor = (int)Mathf.Floor(y * ratioY);
                var y1 = yFloor * w;
                var y2 = (yFloor + 1) * w;
                var yw = y * w2;

                for (var x = 0; x < w2; x++)
                {
                    int xFloor = (int)Mathf.Floor(x * ratioX);
                    var xLerp  = x * ratioX - xFloor;
                    newColors[yw + x] = ColorLerpUnclamped(ColorLerpUnclamped(texColors[y1 + xFloor], texColors[y1 + xFloor + 1], xLerp), ColorLerpUnclamped(texColors[y2 + xFloor], texColors[y2 + xFloor + 1], xLerp), y * ratioY - yFloor);
                }
            }
        }

        /**
         *
         * Renk döner.
         *
         */
        private static Color ColorLerpUnclamped(Color c1, Color c2, float value)
        {
            return new Color(c1.r + (c2.r - c1.r) * value, c1.g + (c2.g - c1.g) * value, c1.b + (c2.b - c1.b) * value, c1.a + (c2.a - c1.a) * value);
        }
    }
}
