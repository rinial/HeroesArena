using System;

namespace HeroesArena
{
    // TODO maybe change it to enum later if tiles are just pictures with no mechanicsю
    // Represents one generiс tile in game logic.
    public class BasicTile : ICloneable
    {
        // TODO tile parameters should be here.

        // For cloning.
        public object Clone()
        {
            return new BasicTile();
        }

        #region Equals
        // Equality override.
        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }

            // If obj can not be cast to BasicTile.
            BasicTile tile = obj as BasicTile;
            if (tile == null)
            {
                return false;
            }

            return true;
        }

        // For performance.
        public bool Equals(BasicTile tile)
        {
            if (tile == null)
            {
                return false;
            }

            return true;
        }

        // For Equals.
        public override int GetHashCode()
        {
            return 0;
        }
        #endregion
    }
}