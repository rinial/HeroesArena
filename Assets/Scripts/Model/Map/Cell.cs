/*
 * Copyright © 2017 Fazli Jan, Oleg Ivanov
 * The project is licensed under the MIT License.
 */

using System;

namespace HeroesArena
{
    // Represents one cell on the map in game logic.
    public class Cell : ICloneable
    {
        // Position of cell on map.
        public Coordinates Position { get; private set; }
        // Tile of the cell.
        public BasicTile Tile { get; set; }
        // TODO make it list if we need multiple objects on one cell.
        // Object on the cell.
        private BasicObject _object;
        public BasicObject Object
        {
            get
            {
                return _object;
            }
            set
            {
                _object = value;
                if (value != null)
                    OnCellEnter += value.OnObjectUse;
            }
        }
        // Unit on the cell.
        private BasicUnit _unit;
        public BasicUnit Unit
        {
            get
            {
                return _unit;
            }
            set
            {
                _unit = value;
                if(_unit != null)
                    OnCellEnter(value);
            }
        }

        // Called when unit enters cell.
        public event System.Action<BasicUnit> OnCellEnter = delegate { };

        #region DistanceMethods
        // Counts distance from this cell to target cell.
        public int Distance(Cell target)
        {
            return Position.Distance(target.Position);
        }
        // Counts distance from A cell to B cell.
        public static int Distance(Cell a, Cell b)
        {
            return Coordinates.Distance(a.Position, b.Position);
        }
        #endregion

        #region DirectionMethods
        // Gets direction from this cell to target cell.
        public Direction GetDirection(Cell target)
        {
            return Position.GetDirection(target.Position);
        }
        // Gets direction from A cell to B cell.
        public static Direction GetDirection(Cell a, Cell b)
        {
            return Coordinates.GetDirection(a.Position, b.Position);
        }
        #endregion

        // Returns true is the cell is occupied, false otherwise.
        public bool IsOccupied(bool onUnwalkable = false, bool onUnits = false, bool onObjects = false)
        {
            return !((onUnwalkable || Tile.Walkable) && (onObjects || Object == null) && (onUnits || Unit == null));
        }

        #region Constructors
        // Constructors.
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
            if (Object == null)
                return Unit == null
                    ? Position.GetHashCode() ^ Tile.GetHashCode()
                    : Position.GetHashCode() ^ Tile.GetHashCode() ^ Unit.GetHashCode();
            else
                return Unit == null
                    ? Position.GetHashCode() ^ Tile.GetHashCode() ^ Object.GetHashCode()
                    : Position.GetHashCode() ^ Tile.GetHashCode() ^ Object.GetHashCode() ^ Unit.GetHashCode();
        }
        #endregion
    }
}
