using System;

namespace HeroesArena
{
    // TODO maybe change it to enum later if tiles are just pictures with no mechanicsю
    // Represents one generiс tile in game logic.
    public class BasicTile : ICloneable
    {
        public readonly TileType Type;
        public readonly bool Walkable;

        // Constructor.
        public BasicTile(TileType type, bool walkable = true)
        {
            Type = type;
            Walkable = walkable;
        }

        // For cloning.
        public object Clone()
        {
            return new BasicTile(Type, Walkable);
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

            return Type.Equals(tile.Type) && Walkable.Equals(tile.Walkable);
        }

        // For performance.
        public bool Equals(BasicTile tile)
        {
            if (tile == null)
            {
                return false;
            }

            return Type.Equals(tile.Type) && Walkable.Equals(tile.Walkable);
        }

        // For Equals.
        public override int GetHashCode()
        {
            return Type.GetHashCode() ^ Walkable.GetHashCode();
        }
        #endregion
    }
}