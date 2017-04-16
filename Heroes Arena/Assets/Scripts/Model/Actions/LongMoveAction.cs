using System;
using System.Collections.Generic;

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
            int distance = route.Length;
            if (distance > PossibleDistance || distance < 1)
                return false;
            return true;
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

            for (int i = 1; i < route.Length + 1; ++i)
                BasicMove.Execute(route.Cells[i], map);

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