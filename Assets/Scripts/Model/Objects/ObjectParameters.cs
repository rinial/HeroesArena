/*
 * Copyright © 2017 Fazli Jan, Oleg Ivanov
 * The project is licensed under the MIT License.
 */

namespace HeroesArena
{
    // Represents parameters of an object.
    public class ObjectParameters
    {
        // Type of an object.
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
