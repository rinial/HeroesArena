/*
 * Copyright © 2017 Fazli Jan, Oleg Ivanov
 * The project is licensed under the MIT License.
 */

namespace HeroesArena
{
    // Represents the Warrior class.
    public class Warrior : Class
    {
        // Class parameters
        private Parameter<int> _healthPoints = new Parameter<int>(16);
        private Parameter<int> _actionPoints = new Parameter<int>(11);
        private const int _basicMoveCost = 3;
        private const int _basicAttackCost = 4;
        private Damage _basicAttackDamage = new Damage(7);
        private const int _basicAttackMaxRange = 1;
        private const int _basicHookCost = 8;
        private const int _basicHookMaxRange = 3;

        public Warrior(BasicUnit unit) : base(unit)
        {
            Tag = ClassTag.Warrior;
            HealthPoints = _healthPoints;
            ActionPoints = _actionPoints;

            // basic actions
            AddAction(new AttackAction(Unit, _basicAttackCost, _basicAttackDamage, _basicAttackMaxRange));

            MoveAction basicMove = new MoveAction(Unit, _basicMoveCost);
            AddAction(basicMove);
            AddAction(new LongMoveAction(basicMove));

            // class specific actions
            AddAction(new HookAction(Unit, _basicHookCost, _basicAttackDamage, _basicHookMaxRange));
        }
    }
}
