namespace HeroesArena
{
    // Represents parameters of unit.
    public class UnitParameters
    {
        // Type of this object.
        public ClassTag Class;
        // Facing of unit.
        public Direction Facing;

        #region Constructors
        // Constructors. UNetWeaver needs basic constructor.
        public UnitParameters()
        {
            Class = ClassTag.None;
            Facing = Direction.Down;
        }
        public UnitParameters(ClassTag clas, Direction facing)
        {
            Class = clas;
            Facing = facing;
        }
        #endregion
    }
}
