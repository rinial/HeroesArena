namespace HeroesArena
{
    // Represents one cell on the map in game logic.
    public class Cell
    {
        public Coordinates Position { get; private set; }
        public BasicTile Tile { get; private set; }

        // TODO make it list if we need multiple objects on one cell.
        public BasicObject Object { get; set; }

        // TODO make it list of we need multiple units on one cell.
        public BasicUnit Unit { get; set; }

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
        // For cloning.
        public Cell(Cell cell) : this(cell.Position, cell.Tile, cell.Unit, cell.Object) { }
        #endregion

        #region Equals
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

            return Position == cell.Position && Tile == cell.Tile && Object == cell.Object && Unit == cell.Unit;
        }

        // For performance.
        public bool Equals(Cell cell)
        {
            if (cell == null)
            {
                return false;
            }

            return Position == cell.Position && Tile == cell.Tile && Object == cell.Object && Unit == cell.Unit;
        }

        // For Equals.
        public override int GetHashCode()
        {
            return Position.GetHashCode() ^ Tile.GetHashCode() ^ Object.GetHashCode() ^ Unit.GetHashCode();
        }
        #endregion

        // TODO some triggers should work from here probably, maybe not.
    }
}
