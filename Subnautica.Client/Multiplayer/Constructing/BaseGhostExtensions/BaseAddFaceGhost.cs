namespace Subnautica.Client.Multiplayer.Constructing.BaseGhostExtensions
{
    using Subnautica.API.Extensions;
    using Subnautica.API.Features;
    using Subnautica.Network.Models.Construction;

    public static class BaseAddFaceGhost
    {
        /**
         *
         * Yapının konumunu ayarlar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static bool UpdateMultiplayerPlacement(this global::BaseAddFaceGhost baseGhost, bool updatePlacement, out bool positionFound, out bool geometryChanged, BaseAddFaceGhostComponent component)
        {
            positionFound   = false;
            geometryChanged = false;

            if (updatePlacement == false || component == null || component.TargetBaseId.IsNull() || component.Face.Cell == null)
            {
                baseGhost.targetBase = null;

                geometryChanged = baseGhost.SetupInvalid();
                return false;
            }

            baseGhost.targetBase = Network.Identifier.GetComponentByGameObject<global::Base>(component.TargetBaseId);
            if (baseGhost.targetBase == null)
            {
                geometryChanged = baseGhost.SetupInvalid();
                return false;
            }

            baseGhost.targetBase.SetPlacementGhost(baseGhost);

            var faceCell = component.Face.Cell.ToInt3();
            var normCell = baseGhost.targetBase.NormalizeCell(faceCell);

            baseGhost.targetOffset = normCell;

            var cell     = baseGhost.targetBase.GetCell(normCell);
            var cellSize = Base.CellSize[(int)cell];

            var sourceRange = Int3.Bounds.Union(new Int3.Bounds(faceCell, faceCell), new Int3.Bounds(normCell, normCell + cellSize - 1));
            geometryChanged = baseGhost.UpdateSize(sourceRange.size);

            if (baseGhost.ghostBase.GetComponentInChildren<IBaseAccessoryGeometry>() != null)
            {
                geometryChanged = true;
            }

            var newFace = new Base.Face(faceCell - baseGhost.targetBase.GetAnchor(), component.Face.Direction);
            if (!baseGhost.anchoredFace.HasValue || baseGhost.anchoredFace.Value != newFace)
            {
                baseGhost.anchoredFace = new Base.Face?(newFace);
                baseGhost.ghostBase.CopyFrom(baseGhost.targetBase, sourceRange, sourceRange.mins * -1);
                baseGhost.ghostBase.ClearMasks();

                var face3 = new Base.Face(faceCell - normCell, component.Face.Direction);
                baseGhost.ghostBase.SetFaceMask(face3, true);
                baseGhost.ghostBase.SetFaceType(face3, baseGhost.faceType);
                baseGhost.RebuildGhostGeometry();

                geometryChanged = true;
            }

            var constructableBase = baseGhost.GetComponentInParent<ConstructableBase>();
            constructableBase.transform.position = baseGhost.targetBase.GridToWorld(normCell);
            constructableBase.transform.rotation = baseGhost.targetBase.transform.rotation;
            
            positionFound = true;
            return true;
        }
    }
}