/*
 * Copyright © 2017 Fazli Jan, Oleg Ivanov
 * The project is licensed under the MIT License.
 */

using System;
using System.Collections.Generic;
using UnityEngine;

namespace HeroesArena
{
    // Represents coordinates on the grid in game logic.
    public class Coordinates : ICloneable
    {
        // Coordinates.
        public readonly int X, Y;

        public List<Coordinates> GetClose()
        {
            List<Coordinates> list = new List<Coordinates>();
            list.AddRange(new[]
            {
                new Coordinates(X - 1, Y),
                new Coordinates(X + 1, Y),
                new Coordinates(X, Y - 1),
                new Coordinates(X, Y + 1)
            });
            return list;
        }

        #region DistanceMethods
        // Counts distance from this point to target point.
        public int Distance(Coordinates target)
        {
            return Distance(this, target);
        }
        // Counts distance from A point to B point.
        public static int Distance(Coordinates a, Coordinates b)
        {
            return Math.Abs(b.X - a.X) + Math.Abs(b.Y - a.Y);
        }
        #endregion

        #region DirectionMethods
        // Gets direction from this point to target point.
        public Direction GetDirection(Coordinates target)
        {
            return GetDirection(this, target);
        }
        // Gets direction from A point to B point.
        public static Direction GetDirection(Coordinates a, Coordinates b)
        {
            int dX = b.X - a.X;
            int dY = b.Y - a.Y;
            return dY <= dX && dY <= -dX ? Direction.Down
                 : dY >= dX && dY >= -dX ? Direction.Up
                 : dX > dY ? Direction.Right
                 : Direction.Left;
        }

        // Returns a vector of the length 1 to the given direction.
        public static Vector2 GetVectorByDirection(Direction dir)
        {
            var v = new Vector2();
            switch (dir) {
                case Direction.Up:
                    v.Set(0, 1);
                    break;
                case Direction.Down:
                    v.Set(0, -1);
                    break;
                case Direction.Left:
                    v.Set(-1, 0);
                    break;
                case Direction.Right:
                    v.Set(1, 0);
                    break;
            }
            return v;
        }
        #endregion

        #region Constructors
        // Constructors. UNetWeaver needs basic constructor.
        public Coordinates()
        {
            X = 0;
            Y = 0;
        }
        public Coordinates(int x = 0, int y = 0)
        {
            X = x;
            Y = y;
        }
        #endregion

        // For cloning.
        public object Clone()
        {
            return new Coordinates(X, Y);
        }

        #region Equals
        // Equality override.
        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }

            // If obj can not be cast to Coordinates.
            Coordinates coords = obj as Coordinates;
            if (coords == null)
            {
                return false;
            }

            return (X == coords.X) && (Y == coords.Y);
        }

        // For performance.
        public bool Equals(Coordinates coords)
        {
            if (coords == null)
            {
                return false;
            }

            return (X == coords.X) && (Y == coords.Y);
        }

        // For Equals.
        public override int GetHashCode()
        {
            return X ^ Y;
        }
        #endregion
    }
}
