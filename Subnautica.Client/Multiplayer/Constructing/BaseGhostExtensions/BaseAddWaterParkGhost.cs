namespace Subnautica.Client.Multiplayer.Constructing.BaseGhostExtensions
{
    using Subnautica.API.Extensions;
    using Subnautica.API.Features;
    using Subnautica.API.MonoBehaviours;
    using Subnautica.Network.Models.Construction;

    public static class BaseAddWaterParkGhost
    {
        /**
         *
         * Yapının konumunu ayarlar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static bool UpdateMultiplayerPlacement(this global::BaseAddWaterPark baseGhost, bool updatePlacement, out bool positionFound, out bool geometryChanged, BaseAddWaterParkGhostComponent component)
        {
            positionFound   = false;
            geometryChanged = false;

            if (component == null || component.TargetBaseId.IsNull() || component.FaceStart == null)
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

            baseGhost.targetOffset = normCell;

            var targetCellType = baseGhost.targetBase.GetCell(normCell);
            var targetSize     = Base.CellSize[(int) targetCellType];
            var targetFace     = new Base.Face(faceCell - baseGhost.targetBase.GetAnchor(), component.FaceStart.Direction);

            if (!baseGhost.anchoredFace.HasValue || baseGhost.anchoredFace.Value != targetFace)
            {   
                baseGhost.anchoredFace = new Base.Face?(targetFace);

                geometryChanged = baseGhost.UpdateSize(targetSize);

                baseGhost.ghostBase.CopyFrom(baseGhost.targetBase, new Int3.Bounds(normCell, normCell + targetSize - 1), normCell * -1);
                baseGhost.ghostBase.ClearMasks();

                var newCell = faceCell - normCell;

                switch (targetCellType)
                {
                    case Base.CellType.Room:

                        var newFace = new Base.Face(newCell, component.FaceStart.Direction);

                        for (int index = 0; index < 2; ++index)
                        {
                            baseGhost.ghostBase.SetFaceType(newFace, Base.FaceType.WaterPark);
                            baseGhost.ghostBase.SetFaceMask(newFace, true);

                            newFace.direction = Base.OppositeDirections[(int) newFace.direction];
                        }

                        foreach (var horizontalDirection in Base.HorizontalDirections)
                        {
                            newFace.direction = horizontalDirection;

                            baseGhost.ghostBase.SetFaceType(newFace, Base.FaceType.Solid);
                            baseGhost.ghostBase.SetFaceMask(newFace, true);
                        }

                        break;
                    case Base.CellType.LargeRoom:
                    case Base.CellType.LargeRoomRotated:

                        var newFace2 = new Base.Face();
                        int index1   = targetCellType == Base.CellType.LargeRoom ? 0 : 2;

                        for (int index2 = 0; index2 < 2; ++index2)
                        {
                            newFace2.cell = newCell;
                            newFace2.cell[index1] += index2;

                            foreach (var horizontalDirection in Base.HorizontalDirections)
                            {
                                if (targetCellType == Base.CellType.LargeRoomRotated)
                                {
                                    if (index2 == 0 && horizontalDirection == Base.Direction.North || index2 == 1 && horizontalDirection == Base.Direction.South)
                                    {
                                        continue;
                                    }
                                }
                                else if (index2 == 0 && horizontalDirection == Base.Direction.East || index2 == 1 && horizontalDirection == Base.Direction.West)
                                {
                                    continue;
                                }
                                
                                newFace2.direction = horizontalDirection;

                                baseGhost.ghostBase.SetFaceMask(newFace2, true);
                                baseGhost.ghostBase.SetFaceType(newFace2, Base.FaceType.Solid);
                            }

                            newFace2.direction = component.FaceStart.Direction;

                            for (int index3 = 0; index3 < 2; ++index3)
                            {
                                baseGhost.ghostBase.SetFaceType(newFace2, Base.FaceType.WaterPark);
                                baseGhost.ghostBase.SetFaceMask(newFace2, true);

                                newFace2.direction = Base.OppositeDirections[(int) newFace2.direction];
                            }
                        }

                        break;
                }

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
