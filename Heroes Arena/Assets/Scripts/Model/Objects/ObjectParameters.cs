namespace HeroesArena
{
    // Represents parameters of object.
    public class ObjectParameters
    {
        // Type of this object.
        public ObjectType Type;

        #region Constructors
        // Constructors. UNetWeaver needs basic constructor.
        public ObjectParameters()
        {
            Type = ObjectType.None;
        }
        public ObjectParameters(ObjectType type)
        {
            Type = type;
        }
        #endregion
    }
}
