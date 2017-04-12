using System;
using System.Collections.Generic;

namespace HeroesArena
{
    // Represents map as a set of cells with their tiles, objects and units in game logic.
    public class Map : ICloneable
    {
        // Stores cells and provides easy position-cell access.
        public Dictionary<Coordinates, Cell> Cells { get; private set; }

        // TODO some methods for route search.

        #region Constructors
        // Construstors.
        public Map(Dictionary<Coordinates, Cell> cells = null)
        {
            Cells = cells;
        }
        public Map(List<Cell> cells)
        {
            Cells = new Dictionary<Coordinates, Cell>();
            foreach (Cell cell in cells)
            {
                Cells[cell.Position] = cell;
            }
        }
        #endregion

        // For cloning.
        public object Clone()
        {
            Dictionary<Coordinates, Cell> cells = new Dictionary<Coordinates, Cell>();
            foreach (Cell cell in Cells.Values)
            {
                cells[(Coordinates)cell.Position.Clone()] = (Cell)cell.Clone();
            }
            return new Map(cells);
        }
    }
}
