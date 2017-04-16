using System;
using System.Collections.Generic;

namespace HeroesArena
{
    // Represents map as a set of cells with their tiles, objects and units in game logic.
    public class Map : ICloneable
    {
        // Stores cells and provides easy position-cell access.
        public Dictionary<Coordinates, Cell> Cells { get; private set; }

        // Gets all cells achievable from center.
        public Dictionary<Cell, int> GetCellsInRange(Cell center, int range)
        {
            if (!Cells.ContainsKey(center.Position) || Cells[center.Position] != center)
                return null;

            Dictionary<Cell, int> cellsWithDistance = new Dictionary<Cell, int>();

            cellsWithDistance[center] = 0;
            if(range > 0)
                UpdateCellsWithDistance(center, 1, range, ref cellsWithDistance);

            return cellsWithDistance;
        }
        private void UpdateCellsWithDistance(Cell cell, int currentRange, int maxRange, ref Dictionary<Cell, int> cellsWithDistance)
        {
            List<Coordinates> closePositions = cell.Position.GetClose();
            foreach (Coordinates position in closePositions)
            {
                if (!Cells.ContainsKey(position))
                    continue;
                Cell closeCell = Cells[position];
                if ((!cellsWithDistance.ContainsKey(closeCell) || cellsWithDistance[closeCell] > currentRange) && closeCell.Tile.Walkable && closeCell.Unit == null)
                {
                    cellsWithDistance[closeCell] = currentRange;
                    if (currentRange + 1 <= maxRange)
                        UpdateCellsWithDistance(closeCell, currentRange + 1, maxRange, ref cellsWithDistance);
                }
            }
        }

        // TODO this should have a better algorithm.
        // Gets route from A cell to B cell with specified step distance.
        public Route GetRoute(Cell a, Cell b, int range = int.MaxValue)
        {
            if (!Cells.ContainsKey(a.Position) || !Cells.ContainsKey(b.Position) || Cells[a.Position] != a || Cells[b.Position] != b)
                return null;

            Dictionary<Cell, int> cellsWithDistance = GetCellsInRange(a, range);

            if (!cellsWithDistance.ContainsKey(b))
                return null;

            List<Cell> reverseRoute = new List<Cell>();
            reverseRoute.Add(b);

            Cell temp = b;
            while (temp != a)
            {
                int distance = cellsWithDistance[temp];
                List<Coordinates> closePositions = temp.Position.GetClose();
                foreach (Coordinates position in closePositions)
                {
                    if (!Cells.ContainsKey(position))
                        continue;
                    Cell closeCell = Cells[position];
                    if (!cellsWithDistance.ContainsKey(closeCell))
                        continue;

                    int closeDistance = cellsWithDistance[closeCell];
                    if (closeDistance < distance)
                    {
                        distance = closeDistance;
                        temp = closeCell;
                        reverseRoute.Add(temp);
                    }
                }
            }
            
            reverseRoute.Reverse();
            return new Route(reverseRoute);
        }

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
