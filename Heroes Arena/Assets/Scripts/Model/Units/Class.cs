using System.Collections.Generic;

namespace HeroesArena
{
    // Represents generic class of unit.
    public abstract class Class
    {
        // This class's tag.
        public ClassTag Tag { get; protected set; }

        // Unit that has this class.
        public readonly BasicUnit Unit;

        // Unit's health points.
        public Parameter<int> HealthPoints;
        // Unit's action points.
        public Parameter<int> ActionPoints;

        // Stores unit's actions gained from class while providing convinient access to them.
        public Dictionary<ActionTag, Action> Actions = new Dictionary<ActionTag, Action>();

        // Adds action to the Actions.
        public void AddAction(Action action)
        {
            // If action with the same tag already exists.
            if (action == null || Actions.ContainsKey(action.Tag))
                return;

            Actions[action.Tag] = action;
        }

        // Gets new class from class tag.
        public static Class GetNewClass(ClassTag tag, BasicUnit unit)
        {
            Class clas;
            switch (tag)
            {
                case ClassTag.Rogue:
                    clas = new Rogue(unit);
                    break;
                case ClassTag.Wizard:
                    clas = new Wizard(unit);
                    break;
                case ClassTag.Warrior:
                    clas = new Warrior(unit);
                    break;
                default:
                    clas = null;
                    break;
            }
            return clas;
        }

        // Constructor.
        protected Class(BasicUnit unit)
        {
            Unit = unit;
        }

        #region Equals
        // Equality override.
        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }

            // If obj can not be cast to Class.
            Class clas = obj as Class;
            if (clas == null)
            {
                return false;
            }

            // We don't check if Unit.Equals(clas.Unit) to avoid recursion. We also ignore actions.
            return Tag.Equals(clas.Tag) && HealthPoints.Equals(clas.HealthPoints) && ActionPoints.Equals(clas.ActionPoints);
        }

        // For performance.
        public bool Equals(Class clas)
        {
            if (clas == null)
            {
                return false;
            }

            // We don't check if Unit.Equals(clas.Unit) to avoid recursion. We also ignore actions.
            return Tag.Equals(clas.Tag) && HealthPoints.Equals(clas.HealthPoints) && ActionPoints.Equals(clas.ActionPoints);
        }

        // For Equals.
        public override int GetHashCode()
        {
            // We don't add Unit.GetHashCode() to avoid recursion. We also ignore actions.
            return Tag.GetHashCode() ^ HealthPoints.GetHashCode() ^ ActionPoints.GetHashCode();
        }
        #endregion
    }
}