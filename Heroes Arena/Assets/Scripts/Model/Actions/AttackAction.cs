using System.Collections.Generic;

namespace HeroesArena
{
    // Represents a simple attack of any unit.
    public class AttackAction : Action
    {
        // Minimum distance of this action.
        public int MinRange { get; private set; }
        // Maximum distance of this action.
        public int MaxRange { get; private set; }
        // Damage of the attack.
        public Damage Damage { get; private set; }

        // Checks if position is in action range.
        public override bool InRange(Coordinates pos, Map map)
        {
            if (map == null || map.Cells == null || !map.Cells.ContainsKey(pos))
                return false;
            BasicUnit unit = Executer as BasicUnit;
            int distance = unit.Cell.Position.Distance(pos);
            if (distance > MaxRange || distance < MinRange)
                return false;
            return true;
        }

        // Attack execution.
        public override bool Execute(List<Cell> targets, Map map)
        {
            // If no damage.
            if (Damage == null)
                return false;

            // If no target.
            if (targets == null)
                return false;

            // If more than one target.
            if (targets.Count != 1)
                return false;

            Cell cell = targets[0];

            // If no executer or cell to attack.
            if (Executer == null || cell == null)
                return false;

            // If no target to attack.
            IDamageable target = cell.Unit;
            if (target == null)
                return false;
            
            BasicUnit unit = Executer as BasicUnit;

            // If cell is not in range.
            if (!InRange(cell, map))
                return false;

            // If not enough action points.
            if (!unit.UseActionPoints(Cost))
                return false;

            unit.UpdateFacing(cell);
            
            unit.DealDamage(target, Damage);

            return true;
        }

        // Constructor.
        public AttackAction(BasicUnit executer, int cost = 0, Damage damage = null, int maxRange = 1, int minRange = 1) : base(executer, cost)
        {
            Tag = ActionTag.Attack;
            Damage = damage;
            MaxRange = maxRange;
            MinRange = minRange;
        }
    }
}