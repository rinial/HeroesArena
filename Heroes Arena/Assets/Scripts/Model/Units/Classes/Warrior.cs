using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HeroesArena
{
    // Represents Rogue class.
    public class Warrior : Class
    {
        // Some class parameters.
        private Parameter<int> _healthPoints = new Parameter<int>(16);
        private Parameter<int> _actionPoints = new Parameter<int>(11);
        private int _basicMoveCost = 3;
        private int _basicAttackCost = 4;
        private Damage _basicAttackDamage = new Damage(7);
        private int _basicAttackMaxRange = 1;
        private int _basicHookCost = 8;
        private int _basicHookMaxRange = 3;


        // Constructor.
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
