namespace Subnautica.Client.Multiplayer.Constructing.BaseGhostExtensions
{
    using Subnautica.API.Extensions;
    using Subnautica.API.Features;
    using Subnautica.API.MonoBehaviours;
    using Subnautica.Network.Models.Construction;

    public static class BaseAddCellGhost
    {
        /**
         *
         * Yapının konumunu ayarlar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static bool UpdateMultiplayerPlacement(this global::BaseAddCellGhost baseGhost, bool updatePlacement, out bool positionFound, out bool geometryChanged, BaseAddCellGhostComponent component)
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
                        baseGhost.ghostBase.SetCell(Int3.zero, baseGhost.GetMultiplayerCellType());

                        if (component.BelowFaceType == Base.FaceType.Hole)
                        {
                            baseGhost.ghostBase.SetFaceType(new Base.Face(Int3.zero, Base.Direction.Below), Base.FaceType.Hole);
                        }

                        if (component.AboveFaceType == Base.FaceType.Hole)
                        {
                            baseGhost.ghostBase.SetFaceType(new Base.Face(Int3.zero, Base.Direction.Above), Base.FaceType.Hole);
                        }

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
                if (baseGhost.prevTargetBase != null)
                {
                    baseGhost.SetupGhost();
                    
                    geometryChanged = true;
                }

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
        public static void UpdateMultiplayerRotation(this global::BaseAddCellGhost baseGhost, ref bool geometryChanged)
        {
            if (baseGhost.TryGetComponent<BaseGhostRotationComponent>(out var component))
            {
                if (baseGhost.cellTypes.Count < 2)
                {
                    return;
                }

                var cellType = baseGhost.GetMultiplayerCellType();
                var cellSize = Base.CellSize[(uint) cellType];
                if (cellSize != baseGhost.ghostBase.GetSize())
                {
                    baseGhost.ghostBase.ClearGeometry();
                    baseGhost.ghostBase.SetSize(cellSize);
                }

                baseGhost.ghostBase.ClearCell(Int3.zero);
                baseGhost.ghostBase.SetCell(Int3.zero, cellType);
                baseGhost.RebuildGhostGeometry();

                geometryChanged = true;
            }
        }

        /**
         *
         * Çok oyunculu hücre türünü döner.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static Base.CellType GetMultiplayerCellType(this global::BaseAddCellGhost baseGhost)
        {
            if (baseGhost.TryGetComponent<BaseGhostRotationComponent>(out var component))
            {
                return baseGhost.cellTypes[component.LastRotation % baseGhost.cellTypes.Count];
            }

            return Base.CellType.Empty;
        }
    }
}

