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

        #region Equals
        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }

            // If obj can not be cast to BasicObject.
            BasicObject ob = obj as BasicObject;
            if (ob == null)
            {
                return false;
            }

            return Cell == ob.Cell;
        }

        // For performance.
        public bool Equals(BasicObject ob)
        {
            if (ob == null)
            {
                return false;
            }

            return Cell == ob.Cell;
        }

        // For Equals.
        public override int GetHashCode()
        {
            return Cell.GetHashCode();
        }
        #endregion

        // TODO
    }
}
