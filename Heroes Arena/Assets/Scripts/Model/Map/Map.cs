using System;
using System.Collections.Generic;
using System.Linq;
using Random = UnityEngine.Random;

namespace HeroesArena
{
    // Represents map as a set of cells with their tiles, objects and units in game logic.
    public class Map : ICloneable
    {
        // Stores cells and provides easy position-cell access.
        public Dictionary<Coordinates, Cell> Cells { get; private set; }

        // Returns a random unoccupied cell of the map.
        public Cell GetRandomUnoccupiedCell()
        {
            Cell cell;

            do cell = Cells.ElementAt(Random.Range(0, Cells.Count)).Value;
            while (cell.IsOccupied);

            return cell;
        }

        public List<BasicUnit> GetUnits()
        {
            List<BasicUnit> units = new List<BasicUnit>();
            List<Cell> cells = Cells.Values.ToList();
            for (int i = 0; i < cells.Count; ++i)
            {
                if(cells[i].Unit != null)
                    units.Add(cells[i].Unit);
            }
            return units;
        }

        // Gets all cells achievable from center.
        public Dictionary<Cell, int> GetCellsInRange(Cell center, int range)
        {
            if (!Cells.ContainsKey(center.Position) || Cells[center.Position] != center)
                return null;

            Dictionary<Cell, int> cellsWithDistance = new Dictionary<Cell, int>();

            List<Cell> visibleCells = GetVisibleCells(center, range);

            cellsWithDistance[center] = 0;
            if (range > 0)
                UpdateCellsWithDistance(center, 1, range, ref cellsWithDistance, visibleCells);

            return cellsWithDistance;
        }
        private void UpdateCellsWithDistance(Cell cell, int currentRange, int maxRange, ref Dictionary<Cell, int> cellsWithDistance, List<Cell> visibleCells)
        {
            List<Coordinates> closePositions = cell.Position.GetClose();
            foreach (Coordinates position in closePositions)
            {
                if (!Cells.ContainsKey(position))
                    continue;
                Cell closeCell = Cells[position];
                if (!visibleCells.Contains(closeCell))
                    continue;
                if ((!cellsWithDistance.ContainsKey(closeCell) || cellsWithDistance[closeCell] > currentRange) && closeCell.Tile.Walkable && closeCell.Unit == null)
                {
                    cellsWithDistance[closeCell] = currentRange;
                    if (currentRange + 1 <= maxRange)
                        UpdateCellsWithDistance(closeCell, currentRange + 1, maxRange, ref cellsWithDistance, visibleCells);
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

        #region Visibility methods
        // Gets all cells visible from center.
        public List<Cell> GetVisibleCells(Cell center, int maxRange = int.MaxValue)
        {
            if (!Cells.ContainsKey(center.Position) || Cells[center.Position] != center)
                return null;

            List<Cell> visibleCells = new List<Cell>();

            foreach (Cell cell in Cells.Values)
                if (!visibleCells.Contains(cell))
                    SeenInLine(center.Position, cell.Position, ref visibleCells, maxRange);

            return visibleCells;
        }
        // Returns true if target can be seen.
        public bool CanBeSeen(Coordinates from, Coordinates target, int maxRange = int.MaxValue)
        {
            if (!Cells.ContainsKey(from) || !Cells.ContainsKey(target))
                return false;

            List<Cell> visibleCells = GetVisibleCells(Cells[from], maxRange);

            return visibleCells.Contains(Cells[target]);
        }
        private void SeenInLine(Coordinates from, Coordinates target, ref List<Cell> visibleCells, int maxRange = int.MaxValue)
        {
            if (!Cells.ContainsKey(from) || !Cells.ContainsKey(target))
                return;

            if (!visibleCells.Contains(Cells[from]))
                visibleCells.Add(Cells[from]);

            double startX = from.X;
            double startY = from.Y;
            double endX = target.X;
            double endY = target.Y;

            SeenInLine(startX, startY, endX, endY, ref visibleCells, maxRange);
        }
        private void SeenInLine(double startX, double startY, double endX, double endY, ref List<Cell> visibleCells, int maxRange = int.MaxValue)
        {
            double dX = endX - startX;
            double dY = endY - startY;
            double newX = NextGridIntersectionX(startX, startY, endX, endY);
            double newY = newX != startX
                ? startY + (newX - startX) * dY / dX
                : (OnGrid(startY)
                    ? (startY + (dY > 0 ? 1 : -1))
                    : (dY > 0 ? Math.Ceiling(startY + 0.5) - 0.5 : Math.Floor(startY - 0.5) + 0.5));

            if ((Math.Abs(endX - newX) <= 0.5 && Math.Abs(endY - newY) <= 0.5))
            {
                Cell cell = Cells[new Coordinates((int)endX, (int)endY)];
                if (!visibleCells.Contains(cell) && visibleCells[0].Distance(cell) <= maxRange)
                    visibleCells.Add(cell);
                return;
            }
            if (Math.Abs(newX - startX) > Math.Abs(dX) || Math.Abs(newY - startY) > Math.Abs(dY))
                return;

            int end = 0;

            if (OnGrid(newX))
            {
                if (OnGrid(newY))
                {
                    if (!CheckPos((int)(newX + 0.5), (int)(newY + 0.5), ref visibleCells, maxRange))
                        end += (dX > 0 && dY > 0) ? 2 : 1;

                    if (!CheckPos((int)(newX + 0.5), (int)(newY - 0.5), ref visibleCells, maxRange))
                        end += (dX > 0 && dY < 0) ? 2 : 1;

                    if (!CheckPos((int)(newX - 0.5), (int)(newY + 0.5), ref visibleCells, maxRange))
                        end += (dX < 0 && dY > 0) ? 2 : 1;

                    if (!CheckPos((int)(newX - 0.5), (int)(newY - 0.5), ref visibleCells, maxRange))
                        end += (dX < 0 && dY < 0) ? 2 : 1;
                }
                else
                {
                    if (!CheckPos((int)(newX + 0.5), (int)Math.Floor(newY + 0.5), ref visibleCells, maxRange))
                        end = 2;

                    if (!CheckPos((int)(newX - 0.5), (int)Math.Floor(newY + 0.5), ref visibleCells, maxRange))
                        end = 2;
                }
            }
            else if (OnGrid(newY))
            {
                if (!CheckPos((int)Math.Floor(newX + 0.5), (int)(newY + 0.5), ref visibleCells, maxRange))
                    end = 2;

                if (!CheckPos((int)Math.Floor(newX + 0.5), (int)(newY - 0.5), ref visibleCells, maxRange))
                    end = 2;
            }

            if (end > 0)
                return;

            SeenInLine(newX, newY, endX, endY, ref visibleCells, maxRange);
        }
        private bool CheckPos(int posX, int posY, ref List<Cell> visibleCells, int maxRange)
        {
            Coordinates pos = new Coordinates(posX, posY);
            if (!Cells.ContainsKey(pos))
                return true;
            if (!Cells[pos].Tile.Walkable)
            {
                if (!visibleCells.Contains(Cells[pos]) && visibleCells[0].Distance(Cells[pos]) <= maxRange)
                    visibleCells.Add(Cells[pos]);
                return false;
            }
            return true;
        }
        private static bool OnGrid(double coord)
        {
            return Math.Floor(coord + 0.5d) == coord + 0.5d;
        }
        private static double NextGridIntersectionX(double startX, double startY, double endX, double endY)
        {
            double dX;
            if (endX > startX)
            {
                dX = Math.Ceiling(startX + 0.5d) - startX - 0.5d;
                dX = dX > 0d ? dX : 1d;
            }
            else if (endX < startX)
            {
                dX = Math.Floor(startX - 0.5d) - startX + 0.5d;
                dX = dX < 0d ? dX : -1d;
            }
            else
                return startX;

            double dY;
            if (endY > startY)
            {
                dY = Math.Ceiling(startY + 0.5d) - startY - 0.5d;
                dY = dY > 0d ? dY : 1d;
            }
            else if (endY < startY)
            {
                dY = Math.Floor(startY - 0.5d) - startY + 0.5d;
                dY = dY < 0d ? dY : -1d;
            }
            else
                return startX + dX;

            double temp = dY * (endX - startX) / (endY - startY);

            if (Math.Abs(dX) < Math.Abs(temp))
                return startX + dX;
            return startX + temp;
        }
        #endregion

        #region Constructors
        // Construstors.
        public Map()
        {
            Cells = null;
        }
        public Map(Dictionary<Coordinates, Cell> cells)
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
        public Map(MapParameters mapParam)
        {
            Cells = new Dictionary<Coordinates, Cell>();
            for (int i = 0; i < mapParam.Count; ++i)
            {
                Cell cell = new Cell(mapParam.Positions[i], mapParam.Tiles[i]);
                ObjectParameters objectParams = mapParam.Objects[i];
                cell.Object = objectParams.Type == ObjectType.None ? null : new BasicObject(cell, objectParams.Type);
                UnitParameters unitParams = mapParam.Units[i];
                cell.Unit = unitParams.Class == ClassTag.None ? null : new BasicUnit(cell, unitParams.Facing, unitParams.Class);
                Cells[mapParam.Positions[i]] = cell;
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
