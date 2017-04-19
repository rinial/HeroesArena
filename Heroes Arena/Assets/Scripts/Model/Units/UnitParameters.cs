namespace HeroesArena
{
    // Represents parameters of unit.
    public class UnitParameters
    {
        // Type of this object.
        public ClassTag Class;
        // Facing of unit.
        public Direction Facing;

        public int RandomSeed;

        #region Constructors
        // Constructors. UNetWeaver needs basic constructor.
        public UnitParameters()
        {
            Class = ClassTag.None;
            Facing = Direction.Down;

            RandomSeed = System.Guid.NewGuid().GetHashCode();
        }
        public UnitParameters(ClassTag clas, Direction facing = Direction.Down)
        {
            Class = clas;
            Facing = facing;

            RandomSeed = System.Guid.NewGuid().GetHashCode();
        }
        #endregion
    }
}
