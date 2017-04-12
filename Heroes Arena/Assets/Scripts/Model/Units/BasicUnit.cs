using System;
using UnityEngine;

namespace HeroesArena
{
    // Represents one generiс unit in game logic.
    public class BasicUnit : ICloneable, IDamageDealer, IDamageable
    {
        // Cell containing this unit.
        public Cell Cell;
        // Direction where unit is looking.
        public Direction Facing { get; private set; }
        // Unit's health points.
        public Parameter<int> HealthPoints { get; private set; }
        // Unit's action points.
        public Parameter<int> ActionPoints { get; private set; }

        // Moves unit to another cell.
        public void Move(Cell cell)
        {
            if (cell.Unit == null)
            {
                // TODO add some choise of facing here in cases like moving 45 degrees up right.
                Facing = DirectionOfMovement(Cell, cell);

                cell.Unit = this;
                Cell.Unit = null;
                Cell = cell;
            }
            else if (cell.Unit != this)
            {
                // TODO this is just an attack test that is not supposed to be here. Also magic number.
                DealDamage(cell.Unit, new Damage(2));
            }
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

        #region Direction Methods
        // TODO this is probably not the right place for these methods, move.
        // Gets the facing direction after movement.
        public static Direction DirectionOfMovement(int dX, int dY)
        {
            return dY <= dX && dY <= -dX ? Direction.Down
                 : dY >= dX && dY >= -dX ? Direction.Up
                 : dX > dY ? Direction.Right
                 : Direction.Left;
        }
        public static Direction DirectionOfMovement(Coordinates start, Coordinates end)
        {
            int dX = end.X - start.X;
            int dY = end.Y - start.Y;
            return DirectionOfMovement(dX, dY);
        }
        public static Direction DirectionOfMovement(Cell start, Cell end)
        {
            return DirectionOfMovement(start.Position, end.Position);
        }
        #endregion

        // Constructor.
        public BasicUnit(Cell cell, Direction facing, Parameter<int> healthPoints, Parameter<int> actionPoints)
        {
            Cell = cell;
            if (cell != null)
                Cell.Unit = this;
            Facing = facing;
            HealthPoints = healthPoints;
            ActionPoints = actionPoints;
        }

        // For cloning.
        public object Clone()
        {
            // We don't clone Cell to avoid recursion.
            return new BasicUnit(null, Facing, (Parameter<int>)HealthPoints.Clone(), (Parameter<int>)ActionPoints.Clone());
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

            // We don't check if Cell.Equals(unit.Cell) to avoid recursion.
            return Facing.Equals(unit.Facing) && HealthPoints.Equals(unit.HealthPoints) && ActionPoints.Equals(unit.ActionPoints);
        }

        // For performance.
        public bool Equals(BasicUnit unit)
        {
            if (unit == null)
            {
                return false;
            }

            // We don't check if Cell.Equals(unit.Cell) to avoid recursion.
            return Facing.Equals(unit.Facing) && HealthPoints.Equals(unit.HealthPoints) && ActionPoints.Equals(unit.ActionPoints);
        }

        // For Equals.
        public override int GetHashCode()
        {
            // We don't add Cell.GetHashCode() to avoid recursion.
            return Facing.GetHashCode() ^ HealthPoints.GetHashCode() ^ ActionPoints.GetHashCode();
        }
        #endregion
    }
}
