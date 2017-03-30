namespace HeroesArena
{
	// Represents one generiс unit in game logic.
	public class BasicUnit
	{
        public Cell Cell { get; private set; }

        // TODO delete later, this is just for tests,
        // moves unit to another cell.
        public void Move(Cell cell)
        {
            if (cell.Unit == null)
            {
                cell.Unit = this;
                Cell.Unit = null;
                Cell = cell;
            }
        }

	    public BasicUnit( Cell cell = null )
	    {
	        Cell = cell;
	    }

	    // TODO
    }
}
