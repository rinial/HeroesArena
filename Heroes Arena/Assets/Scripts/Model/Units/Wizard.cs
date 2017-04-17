using System.Collections.Generic;

namespace HeroesArena
{
    // Represents Rogue class.
    public class Wizard : Class
    {
        // Some class parameters.
        private Parameter<int> _healthPoints = new Parameter<int>(7);
        private Parameter<int> _actionPoints = new Parameter<int>(100);
        private int _basicMoveCost = 4;
        private int _basicAttackCost = 4;
        private Damage _basicAttackDamage = new Damage(4);
        private int _basicAttackMaxRange = 5;

        // Constructor.
        public Wizard(BasicUnit unit) : base(unit)
        {
            Tag = ClassTag.Wizard;
            HealthPoints = _healthPoints;
            ActionPoints = _actionPoints;

            AddAction(new AttackAction(Unit, _basicAttackCost, _basicAttackDamage, _basicAttackMaxRange));

            MoveAction basicMove = new MoveAction(Unit, _basicMoveCost);
            AddAction(basicMove);
            AddAction(new LongMoveAction(basicMove));
        }
    }
}
