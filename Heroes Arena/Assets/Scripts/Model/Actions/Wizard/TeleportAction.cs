using System.Collections.Generic;
using UnityEngine;

namespace HeroesArena
{
    // Represents the skill of the Wizard class.
    // Teleports the hero on the random vacant cell on the map.
    public class TeleportAction : Action
    {
        // Checks if position is in action range.
        public override bool InRange(Coordinates pos, Map map)
        {
            if (map == null || map.Cells == null || !map.Cells.ContainsKey(pos) || map.Cells[pos] == null || map.Cells[pos].Tile == null || !map.Cells[pos].Tile.Walkable)
                return false;
            BasicUnit unit = Executer as BasicUnit;
            return unit.Cell.Position.Equals(pos);
        }

        // Returns all cells in range.
        public override List<Cell> AllInRange(Map map)
        {
            if (map == null || map.Cells == null || Executer == null)
                return new List<Cell>();

            BasicUnit unit = Executer as BasicUnit;

            Cell center = map.Cells[unit.Cell.Position];
            return map.GetSelfCell(center);
        }

        // Skill execution.
        public override bool Execute(List<Cell> targets, Map map)
        {
            Debug.Log(targets.Count + " ");
            // If no target.
            if (targets == null)
                return false;

            // If more than one target.
            if (targets.Count != 1)
                return false;

            Cell cell = targets[0];
            Debug.Log(cell);

            // If no executer or cell to teleport.
            if (Executer == null || cell == null)
                return false;

            BasicUnit unit = Executer as BasicUnit;

            // If target is not a wizard on the same cell.
            // TODO nullexception
            if (unit == null || unit.Cell == null || cell.Unit == null || !(cell.Unit.Class is Wizard && cell.Equals(unit.Cell)))
                return false;

            // If cell is not in range.
            if (!InRange(cell, map))
                return false;

            // If not enough action points.
            if (!unit.UseActionPoints(Cost))
                return false;

            // finds random unoccupied cell to teleport the wizard to
            var targetCell = map.GetRandomUnoccupiedCell();

            unit.UpdateFacing(cell);

            unit.Cell.Unit = null;
            unit.Cell = targetCell;
            targetCell.Unit = unit;

            return true;
        }

        // Constructor.
        public TeleportAction(BasicUnit executer, int cost = 0) : base(executer, cost)
        {
            Tag = ActionTag.Teleport;
        }
    }
}