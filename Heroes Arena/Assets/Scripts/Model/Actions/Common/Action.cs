using System.Collections.Generic;
using UnityEngine;

namespace HeroesArena
{
    // Represents generic action that can be executed.
    public abstract class Action
    {
        // Actions executer.
        public IExecuter Executer { get; private set; }

        // Cost in action points.
        public int Cost { get; private set; }

        // This action's tag.
        public ActionTag Tag { get; protected set; }

        // Checks if cell is in action range.
        public bool InRange(Cell cell, Map map)
        {
            return InRange(cell.Position, map);
        }
        public abstract bool InRange(Coordinates pos, Map map);

        // Returns all cells in range.
        public abstract List<Cell> AllInRange(Map map);

        // Returns selected cells for target. Normally just returns target or empty list back, unless action affects some area.
        public virtual List<Cell> SelectedArea(Coordinates target, Map map)
        {
            List<Cell> area = new List<Cell>();
            if (InRange(target, map) && map.Cells.ContainsKey(target))
                area.Add(map.Cells[target]);
            return area;
        }

        // Action execution.
        public abstract bool Execute(List<Cell> targets = null, Map map = null);
        public bool Execute(Cell target, Map map = null)
        {
            List<Cell> cells = new List<Cell>();
            cells.Add(target);
            return Execute(cells, map);
        }
        public bool Execute(Map map = null)
        {
            List<Cell> cells = null;
            return Execute(cells, map);
        }

        // Constructor.
        protected Action(IExecuter executer = null, int cost = 0)
        {
            Executer = executer;
            Cost = cost;
        }
    }
}