using System.Collections.Generic;

namespace HeroesArena
{
    // Represents map as a set of cells with their tiles, objects and units in game logic.
    public class Map
    {
        public Dictionary<Coordinates, Cell> Cells { get; private set; }

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
        // For cloning.
        public Map(Map map)
        {
            Cells = new Dictionary<Coordinates, Cell>();
            foreach (Cell cell in map.Cells.Values)
            {
                Cells[cell.Position] = new Cell(cell);
            }
        }

        // TODO
    }
}
