using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HeroesArena
{
    // Represents Rogue class.
    public class Warrior : Class
    {
        // Some class parameters.
        private Parameter<int> _healthPoints = new Parameter<int>(13);
        private Parameter<int> _actionPoints = new Parameter<int>(100);
        private int _basicMoveCost = 3;
        private int _basicAttackCost = 5;
        private Damage _basicAttackDamage = new Damage(5);
        private int _basicAttackMaxRange = 1;

        // Constructor.
        public Warrior(BasicUnit unit) : base(unit)
        {
            Tag = ClassTag.Warrior;
            HealthPoints = _healthPoints;
            ActionPoints = _actionPoints;

            AddAction(new AttackAction(Unit, _basicAttackCost, _basicAttackDamage, _basicAttackMaxRange));

            MoveAction basicMove = new MoveAction(Unit, _basicMoveCost);
            AddAction(basicMove);
            AddAction(new LongMoveAction(basicMove));
        }
    }
}
