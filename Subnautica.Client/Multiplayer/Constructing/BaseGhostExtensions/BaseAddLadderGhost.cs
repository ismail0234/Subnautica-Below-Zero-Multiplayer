namespace Subnautica.Client.Multiplayer.Constructing.BaseGhostExtensions
{
    using Subnautica.API.Extensions;
    using Subnautica.API.Features;
    using Subnautica.Network.Models.Construction;

    public static class BaseAddLadderGhost
    {
        /**
         *
         * Yapının konumunu ayarlar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static bool UpdateMultiplayerPlacement(this global::BaseAddLadderGhost baseGhost, bool updatePlacement, out bool positionFound, out bool geometryChanged, BaseAddLadderGhostComponent component)
        {
            positionFound = false;
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

            if (component.FaceStart.Direction == Base.Direction.Below)
            {
                var face2 = component.FaceStart;

                component.FaceStart = component.FaceEnd;
                component.FaceEnd = face2;
            }

            var faceStartCell = component.FaceStart.Cell.ToInt3();
            var faceEndCell   = component.FaceEnd.Cell.ToInt3();

            var normCell = baseGhost.targetBase.NormalizeCell(faceStartCell);
            var newCell  = baseGhost.targetBase.GetCell(normCell);
            var cellSize = Base.CellSize[(int)newCell];

            var sourceRange = Int3.Bounds.Union(new Int3.Bounds(faceStartCell, faceEndCell), new Int3.Bounds(normCell, normCell + cellSize - 1));
            var size = sourceRange.size;

            geometryChanged = baseGhost.UpdateSize(size);

            if (baseGhost.isDirty || baseGhost.targetOffset != faceStartCell)
            {
                baseGhost.ghostBase.CopyFrom(baseGhost.targetBase, sourceRange, sourceRange.mins * -1);

                var face2 = new global::Base.Face(faceStartCell - sourceRange.mins, component.FaceStart.Direction);
                var face3 = new global::Base.Face(faceEndCell - sourceRange.mins, component.FaceEnd.Direction);
                baseGhost.ghostBase.ClearMasks();
                baseGhost.ghostBase.SetFaceMask(face2, true);
                baseGhost.ghostBase.SetFaceMask(face3, true);
                baseGhost.ghostBase.SetFaceType(face2, Base.FaceType.Ladder);
                baseGhost.ghostBase.SetFaceType(face3, Base.FaceType.Ladder);

                for (int index = 1; index < face3.cell.y; ++index)
                {
                    var cell2 = face3.cell;
                    cell2.y = index;

                    var face4 = new Base.Face(cell2, global::BaseAddLadderGhost.ladderFaceDir);
                    baseGhost.ghostBase.SetFaceMask(face4, true);
                    baseGhost.ghostBase.SetFaceType(face4, Base.FaceType.Ladder);
                }

                baseGhost.RebuildGhostGeometry();
                baseGhost.isDirty = false;

                geometryChanged = true;
            }

            baseGhost.targetOffset = faceStartCell;

            var constructableBase = baseGhost.GetComponentInParent<ConstructableBase>();
            constructableBase.transform.position = baseGhost.targetBase.GridToWorld(sourceRange.mins);
            constructableBase.transform.rotation = baseGhost.targetBase.transform.rotation;

            positionFound = true;
            return true;
        }
    }
}