using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace HeroesArena
{
    // Represents one generiс unit in game logic.
    public class BasicUnit : ICloneable, IExecuter, IDamageDealer, IDamageable
    {
        // Cell containing this unit.
        public Cell Cell;
        // Direction where unit is looking.
        public Direction Facing { get; private set; }
        // Unit's health points.
        public Parameter<int> HealthPoints
        {
            get { return Class.HealthPoints; }
            set { Class.HealthPoints = value; }
        }
        // Unit's action points.
        public Parameter<int> ActionPoints
        {
            get { return Class.ActionPoints; }
            set { Class.ActionPoints = value; }
        }
        // Unit's class.
        public Class Class { get; private set; }
        // Stores unit's actions while providing convinient access to them.
        public Dictionary<ActionTag, Action> Actions = new Dictionary<ActionTag, Action>();

        // Adds action to the Actions.
        public void AddAction(Action action)
        {
            // If action with the same tag already exists.
            if (action == null || Actions.ContainsKey(action.Tag))
                return;

            Actions[action.Tag] = action;
        }
        public void AddActions(Dictionary<ActionTag, Action> actions)
        {
            foreach (Action action in actions.Values)
            {
                AddAction(action);
            }
        }

        // Uses action points and returns true if everything was ok.
        public bool UseActionPoints(int cost)
        {
            // If there are not enough action points.
            if (cost < 0 || ActionPoints.Current < cost)
                return false;

            ActionPoints.Current -= cost;
            return true;
        }

        // Deals damage to target.
        public void DealDamage(IDamageable target, Damage damage)
        {
            target.TakeDamage(this, damage);
        }
        // Takes damage from source.
        public void TakeDamage(IDamageDealer source, Damage damage)
        {
            HealthPoints.Current -= damage.Amount;
        }
        // Gets healed.
        public void Heal(int amount)
        {
            HealthPoints.Current += amount;
        }

        // Updates unit's facing.
        public void UpdateFacing(Cell cell)
        {
            Facing = Cell.GetDirection(cell);
        }

        // Called when turn for player controlling this unit starts.
        public void TurnStart()
        {
            ActionPoints.Reset();
        }

        // Constructor.
        public BasicUnit(Cell cell, Direction facing, ClassTag clas)
        {
            Cell = cell;
            if (cell != null)
                Cell.Unit = this;
            Facing = facing;

            Class = Class.GetNewClass(clas, this);

            // TODO magic number
            // Every unit has move action.
            MoveAction basicMove = new MoveAction(this, 3);
            AddAction(basicMove);
            AddAction(new LongMoveAction(basicMove));
        }

        // For cloning.
        public object Clone()
        {
            // We don't clone Cell to avoid recursion.
            BasicUnit unit = new BasicUnit(null, Facing, Class.Tag);
            unit.HealthPoints = (Parameter<int>)HealthPoints.Clone();
            unit.ActionPoints = (Parameter<int>)ActionPoints.Clone();
            return unit;
        }

        #region Equals
        // Equality override.
        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }

            // If obj can not be cast to BasicUnit.
            BasicUnit unit = obj as BasicUnit;
            if (unit == null)
            {
                return false;
            }

            // We don't check if Cell.Equals(unit.Cell) to avoid recursion. We also ignore actions.
            return Facing.Equals(unit.Facing) && Class.Equals(unit.Class);
        }

        // For performance.
        public bool Equals(BasicUnit unit)
        {
            if (unit == null)
            {
                return false;
            }

            // We don't check if Cell.Equals(unit.Cell) to avoid recursion. We also ignore actions.
            return Facing.Equals(unit.Facing) && Class.Equals(unit.Class);
        }

        // For Equals.
        public override int GetHashCode()
        {
            // We don't add Cell.GetHashCode() to avoid recursion. We also ignore actions.
            return Facing.GetHashCode() ^ Class.GetHashCode();
        }
        #endregion
    }
}
