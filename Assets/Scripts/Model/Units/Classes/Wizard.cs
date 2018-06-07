/*
 * Copyright © 2017 Fazli Jan, Oleg Ivanov
 * The project is licensed under the MIT License.
 */

namespace HeroesArena
{
    // Represents the Wizard class.
    public class Wizard : Class
    {
        // Class parameters.
        private Parameter<int> _healthPoints = new Parameter<int>(12);
        private Parameter<int> _actionPoints = new Parameter<int>(12);
        private const int _basicMoveCost = 4;
        private const int _basicAttackCost = 3;
        private Damage _basicAttackDamage = new Damage(2);
        private const int _basicAttackMaxRange = 5;
        private const int _basicTeleportCost = 8;

        public Wizard(BasicUnit unit) : base(unit)
        {
            Tag = ClassTag.Wizard;
            HealthPoints = _healthPoints;
            ActionPoints = _actionPoints;

            // basic actions
            AddAction(new AttackAction(Unit, _basicAttackCost, _basicAttackDamage, _basicAttackMaxRange));

            MoveAction basicMove = new MoveAction(Unit, _basicMoveCost);
            AddAction(basicMove);
            AddAction(new LongMoveAction(basicMove));

            // class specific actions
            AddAction(new TeleportAction(Unit, _basicTeleportCost));
        }
    }
}
