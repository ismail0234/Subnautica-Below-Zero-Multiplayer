namespace Subnautica.Client.Multiplayer.Constructing.BaseGhostExtensions
{
    using Subnautica.API.Extensions;
    using Subnautica.API.Features;
    using Subnautica.API.MonoBehaviours;
    using Subnautica.Network.Models.Construction;

    public static class BaseAddMapRoomGhost
    {
        /**
         *
         * Yapının konumunu ayarlar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static bool UpdateMultiplayerPlacement(this global::BaseAddMapRoomGhost baseGhost, bool updatePlacement, out bool positionFound, out bool geometryChanged, BaseAddMapRoomGhostComponent component)
        {
            positionFound = false;
            geometryChanged = false;

            if (component == null)
            {
                baseGhost.targetBase = null;
                return false;
            }

            baseGhost.UpdateMultiplayerRotation(ref geometryChanged);

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
        public static void UpdateMultiplayerRotation(this global::BaseAddMapRoomGhost baseGhost, ref bool geometryChanged)
        {
            baseGhost.ghostBase.SetCell(Int3.zero, baseGhost.GetMultiplayerCellType());
            baseGhost.RebuildGhostGeometry();

            geometryChanged = true;
        }        

        /**
         *
         * Çok oyunculu döndürme işlemini uygular.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static Base.CellType GetMultiplayerCellType(this global::BaseAddMapRoomGhost baseGhost)
        {
            if (baseGhost.TryGetComponent<BaseGhostRotationComponent>(out var component))
            {
                return baseGhost.cellTypes[component.LastRotation % baseGhost.cellTypes.Length];
            }

            return Base.CellType.Empty;
        }        
    }
}
