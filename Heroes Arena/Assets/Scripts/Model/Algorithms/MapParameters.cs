using System;
using System.Collections.Generic;

namespace HeroesArena
{
    // Represents parameters of map.
    public class MapParameters
    {
        // Positions of tiles.
        public Coordinates[] Positions;
        // Tiles.
        public BasicTile[] Tiles;
        // Objects.
        public ObjectParameters[] Objects;
        // Units.
        public UnitParameters[] Units;

        // Number of tiles.
        public int Count {
            get
            {
                if (Positions == null) return 0;
                return Positions.Length;
            }
        }

        #region Constructors
        // Constructors. UNetWeaver needs basic constructor.
        public MapParameters()
        {
            Positions = null;
            Tiles = null;
            Objects = null;
            Units = null;
        }
        public MapParameters(List<Cell> cells)
        {
            Positions = new Coordinates[cells.Count];
            Tiles = new BasicTile[cells.Count];
            Objects = new ObjectParameters[cells.Count];
            Units = new UnitParameters[cells.Count];
            for (int i = 0; i < cells.Count; ++i)
            {
                Positions[i] = cells[i].Position;
                Tiles[i] = cells[i].Tile;
                if (cells[i].Object != null)
                    Objects[i] = new ObjectParameters(cells[i].Object.Type);
                else
                    Objects[i] = new ObjectParameters();
                if (cells[i].Unit != null)
                    Units[i] = new UnitParameters(cells[i].Unit.Class.Tag, cells[i].Unit.Facing);
                else
                    Units[i] = new UnitParameters();
            }
        }
        #endregion
    }
}
