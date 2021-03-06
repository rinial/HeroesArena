﻿/*
 * Copyright © 2017 Fazli Jan, Oleg Ivanov
 * The project is licensed under the MIT License.
 */

namespace HeroesArena
{
    // Represents the Rogue class.
    public class Rogue : Class
    {
        // Class parameters
        private Parameter<int> _healthPoints = new Parameter<int>(18);
        private Parameter<int> _actionPoints = new Parameter<int>(11);
        private int _basicMoveCost = 2;
        private int _basicAttackCost = 3;
        private Damage _basicAttackDamage = new Damage(3);
        private int _basicAttackMaxRange = 3;
        private int _basicSpikesTrapCost = 8;
        private int _basicSpikesTrapMaxRange = 2;

        public Rogue(BasicUnit unit) : base(unit)
        {
            Tag = ClassTag.Rogue;
            HealthPoints = _healthPoints;
            ActionPoints = _actionPoints;

            // basic actions
            AddAction(new AttackAction(Unit, _basicAttackCost, _basicAttackDamage, _basicAttackMaxRange));

            MoveAction basicMove = new MoveAction(Unit, _basicMoveCost);
            AddAction(basicMove);
            AddAction(new LongMoveAction(basicMove));

            // class specific actions
            AddAction(new SpikesTrapAction(Unit, _basicSpikesTrapCost, _basicSpikesTrapMaxRange));
        }
    }
}
