namespace Subnautica.Client.Multiplayer.Constructing.BaseGhostExtensions
{
    using Subnautica.API.Extensions;
    using Subnautica.API.Features;
    using Subnautica.API.MonoBehaviours;
    using Subnautica.Network.Models.Construction;

    public static class BaseAddCorridorGhost
    {
        /**
         *
         * Yapının konumunu ayarlar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static bool UpdateMultiplayerPlacement(this global::BaseAddCorridorGhost baseGhost, bool updatePlacement, out bool positionFound, out bool geometryChanged, BaseAddCorridorGhostComponent component)
        {
            positionFound   = false;
            geometryChanged = false;

            if (component == null)
            {
                baseGhost.targetBase = null;
                return false;
            }

            baseGhost.UpdateMultiplayerRotation(ref geometryChanged);
            baseGhost.prevTargetBase = baseGhost.targetBase;

            var targetOffset = component.TargetOffset.ToInt3();

            if (component.TargetBaseId.IsNotNull())
            {
                baseGhost.targetBase = Network.Identifier.GetComponentByGameObject<global::Base>(component.TargetBaseId);
                if (baseGhost.targetBase)
                {
                    baseGhost.targetBase.SetPlacementGhost(baseGhost);

                    if (baseGhost.targetOffset != targetOffset)
                    {
                        baseGhost.targetOffset = targetOffset;
                        baseGhost.RebuildGhostGeometry();
                        
                        geometryChanged = true;
                    }

                    var constructableBase = baseGhost.GetComponentInParent<ConstructableBase>();
                    constructableBase.transform.position = baseGhost.targetBase.GridToWorld(targetOffset);
                    constructableBase.transform.rotation = baseGhost.targetBase.transform.rotation;

                    positionFound = true;
                }
            }
            else
            {
                baseGhost.targetBase   = null;
                baseGhost.targetOffset = targetOffset;
            }

            return true;
        }


        /**
         *
         * Çok oyunculu döndürme işlemini uygular.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void UpdateMultiplayerRotation(this global::BaseAddCorridorGhost baseGhost, ref bool geometryChanged)
        {
            baseGhost.corridorType = baseGhost.CalculateMultiplayerCorridorType();
            baseGhost.ghostBase.SetCorridor(Int3.zero, baseGhost.corridorType, baseGhost.isGlass);
            baseGhost.RebuildGhostGeometry();

            geometryChanged = true;
        }

        /**
         *
         * Çok oyunculu koridor türünü döner.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static int CalculateMultiplayerCorridorType(this global::BaseAddCorridorGhost baseGhost)
        {
            int num = 0;

            if (baseGhost.TryGetComponent<BaseGhostRotationComponent>(out var component))
            {
                for (int i = 0; i < 4; i++)
                {
                    if (global::BaseAddCorridorGhost.Shapes[(int) baseGhost.corridor, i])
                    {
                        num |= 1 << (int)Base.HorizontalDirections[(i + component.LastRotation) % 4];
                    }
                }
            }

            return num;
        }
    }
}

