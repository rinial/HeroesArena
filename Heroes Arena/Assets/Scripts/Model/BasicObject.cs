namespace HeroesArena
{
	// Represents one generiс object in game logic.
	public class BasicObject
	{
        public Cell Cell { get; private set; }

        public BasicObject(Cell cell = null)
        {
            Cell = cell;
            cell.Object = this;
        }

        // TODO
    }
}
