namespace Subnautica.Client.Synchronizations.InitialSync
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;

    using Subnautica.API.Extensions;
    using Subnautica.API.Features;
    using Subnautica.Client.MonoBehaviours.Construction;
    using Subnautica.Network.Models.Storage.World.Childrens;
    using Subnautica.Network.Structures;

    using UWE;

    public class BaseProcessor
    {
        /**
         *
         * Üs verilerini ayarlar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static IEnumerator InitialBases()
        {
            yield return CoroutineUtils.waitForNextFrame;

            try
            {
                if (Network.Session.Current.Bases != null)
                {
                    foreach (var item in Network.Session.Current.Bases)
                    {
                        if (item.BaseColor != null)
                        {
                            SetBaseColor(item.BaseId, item.Name, item.BaseColor, item.StripeColor1, item.StripeColor2, item.NameColor);
                        }

                        var baseComponent = Network.Identifier.GetComponentByGameObject<global::Base>(item.BaseId);
                        if (baseComponent)
                        {
                            SetDisablePowers(baseComponent, item.DisablePowers);
                            SetMinimapPositions(baseComponent, item.MinimapPositions);
                            SetLeakers(baseComponent, item.Leakers);
                            SetCellWaterLevel(baseComponent, item.CellWaterLevels);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Log.Error(string.Format("[InitialBases] Exception: {0}", e));
            }
        }

        /**
         *
         * Üs rengini değiştirir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private static void SetBaseColor(string baseId, string name, ZeroColor baseColor, ZeroColor stripeColor1, ZeroColor stripeColor2, ZeroColor nameColor)
        {
            var customizeable = Network.Identifier.GetComponentByGameObject<ICustomizeable>(baseId);
            if (customizeable != null)
            {
                customizeable.SetName(name);
                customizeable.SetColor(0, uGUI_ColorPicker.HSBFromColor(baseColor.ToColor()), baseColor.ToColor());
                customizeable.SetColor(1, uGUI_ColorPicker.HSBFromColor(stripeColor1.ToColor()), stripeColor1.ToColor());
                customizeable.SetColor(2, uGUI_ColorPicker.HSBFromColor(stripeColor2.ToColor()), stripeColor2.ToColor());
                customizeable.SetColor(3, uGUI_ColorPicker.HSBFromColor(nameColor.ToColor()), nameColor.ToColor());
            }
        }

        /**
         *
         * Üs harita konumlarını değiştirir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private static void SetMinimapPositions(global::Base baseComponent, Dictionary<string, ZeroVector3> minimapPositions)
        {
            foreach (var controlRoom in baseComponent.GetComponentsInChildren<global::BaseControlRoom>())
            {
                controlRoom.mapDirty = true;

                if (minimapPositions.Count > 0)
                {
                    var uniqueId = Network.Identifier.GetIdentityId(controlRoom.gameObject, false);
                    if (uniqueId.IsNotNull())
                    {
                        var minimap = minimapPositions.FirstOrDefault(q => q.Key == uniqueId);
                        if (minimap.Key == null)
                        {
                            continue;
                        }

                        controlRoom.minimapBase.transform.localPosition = minimap.Value.ToVector3();
                    }
                }
            }
        }

        /**
         *
         * Üs kapalı ışıkları ayarlar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private static void SetDisablePowers(global::Base baseComponent, HashSet<ZeroInt3> disablePowers)
        {
            for (int cellIndex = 0; cellIndex < baseComponent.cellLighting.Length; cellIndex++)
            {
                if (baseComponent.cellLighting[cellIndex] && !baseComponent.cellLighting[cellIndex].cellPowered)
                {
                    baseComponent.SetPowered(baseComponent.GetCellPointFromIndex(cellIndex), true);
                }
            }

            foreach (var cell in disablePowers)
            {
                var cellIndex = baseComponent.GetCellIndex(cell.ToInt3());
                if (baseComponent.cellLighting[cellIndex])
                {
                    baseComponent.SetPowered(cell.ToInt3(), false);
                }
            }
        }

        /**
         *
         * Üs'deki sızıntıları senkronlar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private static void SetLeakers(global::Base baseComponent, HashSet<Leaker> leakers)
        {
            if (leakers.Count > 0)
            {
                foreach (var leaker in leakers)
                {
                    var baseDeconstructable = Network.Identifier.GetComponentByGameObject<global::BaseDeconstructable>(leaker.UniqueId);
                    if (baseDeconstructable == null)
                    {
                        continue;
                    }

                    var leakable = baseDeconstructable.GetComponentInParent<global::Leakable>();
                    if (leakable == null)
                    {
                        continue;
                    }

                    foreach (var point in leaker.Points)
                    {
                        var leakPoint = leakable.unusedLeakPoints.FirstOrDefault(q => q.transform.position.ToZeroVector3() == point);
                        if (leakPoint)
                        {
                            leakPoint.pointActive = true;

                            leakable.leakingLeakPoints.Add(leakPoint);
                            leakable.unusedLeakPoints.Remove(leakPoint);
                        }
                    }
                }
            }
        }

        /**
         *
         * Üs'deki su seviyelerini senkronlar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private static void SetCellWaterLevel(global::Base baseComponent, Dictionary<ushort, float> cellWaterLevels)
        {
            if (cellWaterLevels.Count > 0)
            {
                var baseHullStrength = baseComponent.gameObject.EnsureComponent<BuilderBaseHullStrength>();
                if (baseHullStrength)
                {
                    foreach (var waterLevel in cellWaterLevels)
                    {
                        baseHullStrength.SetCellWaterLevel(waterLevel.Key, waterLevel.Value);
                    }
                }
            }
        }
    }
}
