using System;

namespace HeroesArena
{
    // Represents coordinates on the grid in game logic.
    public class Coordinates : ICloneable
    {
        // Coordinates.
        public readonly int X, Y;

        #region Constructors
        // Constructors. UNetWeaver needs basic constructor.
        public Coordinates()
        {
            X = 0;
            Y = 0;
        }
        public Coordinates(int x = 0, int y = 0)
        {
            X = x;
            Y = y;
        }
        #endregion

        // For cloning.
        public object Clone()
        {
            return new Coordinates(X, Y);
        }

        #region Equals
        // Equality override.
        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }

            // If obj can not be cast to Coordinates.
            Coordinates coords = obj as Coordinates;
            if (coords == null)
            {
                return false;
            }

            return (X == coords.X) && (Y == coords.Y);
        }

        // For performance.
        public bool Equals(Coordinates coords)
        {
            if (coords == null)
            {
                return false;
            }

            return (X == coords.X) && (Y == coords.Y);
        }

        // For Equals.
        public override int GetHashCode()
        {
            return X ^ Y;
        }
        #endregion
    }
}
