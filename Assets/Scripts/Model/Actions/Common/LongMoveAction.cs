/*
 * Copyright © 2017 Fazli Jan, Oleg Ivanov
 * The project is licensed under the MIT License.
 */

using System.Collections.Generic;
using System.Linq;

namespace HeroesArena
{
    // Represents a long move consisting of several basic moves of any unit.
    public class LongMoveAction : Action
    {
        // Basic move.
        public MoveAction BasicMove { get; private set; }

        // Checks if position is in action range.
        public override bool InRange(Coordinates pos, Map map)
        {
            Route route = null;
            return InRange(pos, map, ref route);
        }
        public bool InRange(Coordinates pos, Map map, ref Route route)
        {
            if (map == null || map.Cells == null || !map.Cells.ContainsKey(pos))
                return false;
            BasicUnit unit = Executer as BasicUnit;
            int minimumDistance = unit.Cell.Position.Distance(pos);
            if (minimumDistance > PossibleDistance || minimumDistance < 1)
                return false;
            route = map.GetRoute(map.Cells[unit.Cell.Position], map.Cells[pos], PossibleDistance);
            if (route == null)
                return false;
            return true;
        }

        // Returns all cells in range.
        public override List<Cell> AllInRange(Map map)
        {
            if (map == null || map.Cells == null || Executer == null)
                return new List<Cell>();

            BasicUnit unit = Executer as BasicUnit;
            
            Cell center = map.Cells[unit.Cell.Position];
            List<Cell> cells = map.GetCellsInRange(center, PossibleDistance).Keys.ToList();
            cells.Remove(center);
            return cells;
        }

        // Returns selected cells for target.
        public override List<Cell> SelectedArea(Coordinates target, Map map)
        {
            Route route = new Route();
            if (!InRange(target, map, ref route))
                return new List<Cell>();
            return route.Cells.GetRange(1, route.Length);
        }

        // Returns possible distance of move.
        public int PossibleDistance
        {
            get
            {
                BasicUnit unit = Executer as BasicUnit;
                return BasicMove.Cost == 0 ? int.MaxValue : unit.ActionPoints.Current / BasicMove.Cost;
            }
        }

        // Move execution.
        public override bool Execute(List<Cell> targets, Map map)
        {
            // If no target.
            if (targets == null)
                return false;

            // If more than one target.
            if (targets.Count != 1)
                return false;

            Cell cell = targets[0];

            // If no executer or cell to move to.
            if (Executer == null || cell == null)
                return false;

            Route route = null;

            // If cell is not in range.
            if (!InRange(cell.Position, map, ref route))
                return false;

            // If there is no route to move.
            if (route == null)
                return false;

            BasicUnit unit = Executer as BasicUnit;;

            for (int i = 1; i < route.Length + 1; ++i)
            {
                if (unit.IsAlive)
                    BasicMove.Execute(route.Cells[i], map);
                else
                    break;
            }

            return true;
        }

        // Constructor.
        public LongMoveAction(MoveAction basicMove) : base(basicMove.Executer, 0)
        {
            Tag = ActionTag.LongMove;
            BasicMove = basicMove;
        }
    }
}