using System.Collections.Generic;

namespace HeroesArena
{
    // Represents Rogue class.
    public class Rogue : Class
    {
        // Some class parameters.
        private Parameter<int> _healthPoints = new Parameter<int>(10);
        private Parameter<int> _actionPoints = new Parameter<int>(100);
        private int _basicMoveCost = 2;
        private int _basicAttackCost = 3;
        private Damage _basicAttackDamage = new Damage(3);
        private int _basicAttackMaxRange = 3;

        // Constructor.
        public Rogue(BasicUnit unit) : base(unit)
        {
            Tag = ClassTag.Rogue;
            HealthPoints = _healthPoints;
            ActionPoints = _actionPoints;

            AddAction(new AttackAction(Unit, _basicAttackCost, _basicAttackDamage, _basicAttackMaxRange));

            MoveAction basicMove = new MoveAction(Unit, _basicMoveCost);
            AddAction(basicMove);
            AddAction(new LongMoveAction(basicMove));
        }
    }
}
