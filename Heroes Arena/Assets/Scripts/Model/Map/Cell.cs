using System;

namespace HeroesArena
{
    // Represents one cell on the map in game logic.
    public class Cell : ICloneable
    {
        // Position of cell on map.
        public Coordinates Position { get; private set; }
        // Tile of the cell.
        public BasicTile Tile { get; private set; }
        // TODO make it list if we need multiple objects on one cell.
        // Object on the cell.
        public BasicObject Object;
        // Unit on the cell.
        public BasicUnit Unit;

        // TODO some triggers should be called from here like OnCellEnter.

        #region Constructors
        // Constructors. Every cell MUST have assigned position and tile.
        public Cell(BasicTile tile, BasicUnit unit = null, BasicObject obj = null)
        {
            Position = new Coordinates();
            Tile = tile;
            Unit = unit;
            Object = obj;
        }
        public Cell(Coordinates position, BasicTile tile, BasicUnit unit = null, BasicObject obj = null) : this(tile, unit, obj)
        {
            Position = position;
        }
        #endregion

        // For cloning.
        public object Clone()
        {
            Cell cell = new Cell((Coordinates)Position.Clone(), (BasicTile)Tile.Clone(), Unit == null ? null : (BasicUnit)Unit.Clone(), Object == null ? null : (BasicObject)Object.Clone());

            // We don't clone Cell in Unit and Object to avoid recursion and need this instead.
            if (cell.Unit != null)
                cell.Unit.Cell = cell;
            if (cell.Object != null)
                cell.Object.Cell = cell;

            return cell;
        }

        #region Equals
        // Equality override.
        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }

            // If obj can not be cast to Cell.
            Cell cell = obj as Cell;
            if (cell == null)
            {
                return false;
            }

            return Position.Equals(cell.Position)
                && Tile.Equals(cell.Tile)
                && ((Object == null && cell.Object == null) || ((Object != null && cell.Object != null) && Object.Equals(cell.Object)))
                && ((Unit == null && cell.Unit == null) || ((Unit != null && cell.Unit != null) && Unit.Equals(cell.Unit)));
        }

        // For performance.
        public bool Equals(Cell cell)
        {
            if (cell == null)
            {
                return false;
            }

            return Position.Equals(cell.Position)
                && Tile.Equals(cell.Tile)
                && ((Object == null && cell.Object == null) || ((Object != null && cell.Object != null) && Object.Equals(cell.Object)))
                && ((Unit == null && cell.Unit == null) || ((Unit != null && cell.Unit != null) && Unit.Equals(cell.Unit)));
        }

        // For Equals.
        public override int GetHashCode()
        {
            return Position.GetHashCode() ^ Tile.GetHashCode() ^ Object.GetHashCode() ^ Unit.GetHashCode();
        }
        #endregion
    }
}
