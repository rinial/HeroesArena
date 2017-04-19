using System;

namespace HeroesArena
{
	// Represents one generiс object in game logic.
	public class BasicObject : ICloneable
	{
        // Cell containing this object.
	    public Cell Cell;

        // Type of the object.
        public readonly ObjectType Type;

        // Called upon using object.
        public System.Action<BasicUnit> OnObjectUse;

        // Constructor.
        public BasicObject(Cell cell = null, ObjectType type = ObjectType.HealthPotion)
        {
            Cell = cell;
            Type = type;

            // TODO this shouldnt be here. Also magic numbers.
            switch (type)
            {
                case ObjectType.HealthPotion:
                    OnObjectUse = unit =>
                    {
                        if (unit != null)
                        {
                            unit.Heal(5);
                            Cell.OnCellEnter -= OnObjectUse;
                            Cell.Object = null;
                        }
                    };
                    break;
                case ObjectType.Spikes:
                    OnObjectUse = unit =>
                    {
                        if (unit != null)
                        {
                            unit.TakeDamage(null, new Damage(4));
                        }
                    };
                    break;
                case ObjectType.Corpse:
                    OnObjectUse = unit => { };
                    break;
            }

            if (cell != null)
            {
                cell.Object = this;
            }
        }

        // For cloning.
	    public object Clone()
	    {
            // We don't clone Cell to avoid recursion.
            return new BasicObject(null, Type);
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
            return Type == ob.Type;
        }

        // For performance.
        public bool Equals(BasicObject ob)
        {
            if (ob == null)
            {
                return false;
            }

            // We don't check if Cell.Equals(ob.Cell) to avoid recusrion.
            return Type == ob.Type;
        }

        // For Equals.
        public override int GetHashCode()
        {
            // We don't add Cell.GetHashCode() to avoid recursion.
            return Type.GetHashCode();
        }
        #endregion
    }
}
