namespace Subnautica.Client.Multiplayer.Constructing.BaseGhostExtensions
{
    using Subnautica.API.Extensions;
    using Subnautica.API.Features;
    using Subnautica.Network.Models.Construction;

    public static class BaseAddBulkheadGhost
    {
        /**
         *
         * Yapının konumunu ayarlar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static bool UpdateMultiplayerPlacement(this global::BaseAddBulkheadGhost baseGhost, bool updatePlacement, out bool positionFound, out bool geometryChanged, BaseAddBulkheadGhostComponent component)
        {
            positionFound   = false;
            geometryChanged = false;

            if (updatePlacement == false || component == null || component.TargetBaseId.IsNull() || component.FaceStart.Cell == null)
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

            var faceCell = component.FaceStart.Cell.ToInt3();
            var normCell = baseGhost.targetBase.NormalizeCell(faceCell);

            if (!baseGhost.face.HasValue || baseGhost.face.Value.cell != faceCell || baseGhost.face.Value.direction != component.FaceStart.Direction)
            {
                var targetCell = baseGhost.targetBase.GetCell(normCell);
                var cellSize   = Base.CellSize[(int)targetCell];

                if (baseGhost.ghostBase.Shape.ToInt3() != cellSize)
                {
                    baseGhost.ghostBase.SetSize(cellSize);
                    baseGhost.ghostBase.AllocateMasks();
                }

                baseGhost.ghostBase.CopyFrom(baseGhost.targetBase, new Int3.Bounds(normCell, normCell + cellSize - 1), normCell * -1);

                var face2 = new Base.Face(faceCell - normCell, component.FaceStart.Direction);
                baseGhost.ghostBase.SetFaceType(face2, Base.FaceType.BulkheadClosed);
                baseGhost.ghostBase.ClearMasks();
                baseGhost.ghostBase.SetFaceMask(face2, true);
                baseGhost.RebuildGhostGeometry();
                baseGhost.face = new Base.Face(faceCell, component.FaceStart.Direction);

                geometryChanged = true;
            }

            baseGhost.targetOffset = faceCell;

            var constructableBase = baseGhost.GetComponentInParent<ConstructableBase>();
            constructableBase.transform.position = baseGhost.targetBase.GridToWorld(normCell);
            constructableBase.transform.rotation = baseGhost.targetBase.transform.rotation;

            positionFound = true;
            return true;
        }
    }
}