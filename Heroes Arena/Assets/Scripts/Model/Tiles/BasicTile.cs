using System;

namespace HeroesArena
{
    // TODO maybe change it to enum later if tiles are just pictures with no mechanicsю
    // Represents one generiс tile in game logic.
    public class BasicTile : ICloneable
    {
        public readonly TileType Type;

        public bool Walkable { get { return Type != TileType.Wall && Type != TileType.WallLow; } }

        // Constructors.
        public BasicTile()
        {
            Type = TileType.Ground;
        }
        public BasicTile(TileType type)
        {
            Type = type;
        }

        // For cloning.
        public object Clone()
        {
            return new BasicTile(Type);
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

            return Type.Equals(tile.Type);
        }

        // For performance.
        public bool Equals(BasicTile tile)
        {
            if (tile == null)
            {
                return false;
            }

            return Type.Equals(tile.Type);
        }

        // For Equals.
        public override int GetHashCode()
        {
            return Type.GetHashCode();
        }
        #endregion
    }
}