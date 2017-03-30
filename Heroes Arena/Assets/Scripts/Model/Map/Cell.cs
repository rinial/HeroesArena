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

        // TODO some triggers should work from here probably, maybe not.
    }
}
