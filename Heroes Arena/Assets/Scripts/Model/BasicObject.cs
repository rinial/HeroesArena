using System;

namespace HeroesArena
{
	// Represents one generiс object in game logic.
	public class BasicObject : ICloneable
	{
        // Cell containing this object.
	    public Cell Cell;

        // Some triggers should be called from here like OnObjectUse.

        // Constructor.
        public BasicObject(Cell cell = null)
        {
            Cell = cell;
            if(cell != null)
                cell.Object = this;
        }

        // For cloning.
	    public object Clone()
	    {
            // We don't clone Cell to avoid recursion.
            return new BasicObject(null);
	    }

        #region Equals
        // Equality override.
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

            // We don't check if Cell.Equals(ob.Cell) to avoid recusrion.
            return true;
        }

        // For performance.
        public bool Equals(BasicObject ob)
        {
            if (ob == null)
            {
                return false;
            }

            // We don't check if Cell.Equals(ob.Cell) to avoid recusrion.
            return true;
        }

        // For Equals.
        public override int GetHashCode()
        {
            // We don't add Cell.GetHashCode() to avoid recursion.
            return 0;
        }
        #endregion
    }
}
