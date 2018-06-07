/*
 * Copyright © 2017 Fazli Jan, Oleg Ivanov
 * The project is licensed under the MIT License.
 */

using System;

namespace HeroesArena
{
    // Represents generic comparable parameter with current and maximum values.
    public class Parameter<T> : ICloneable where T : IComparable
    {
        // Current value.
        private T _current;
        public T Current
        {
            get
            {
                return
                    _current;
            }
            set
            {
                // If maximum value is not set, sets it as current value.
                if (_maximum == null)
                {
                    _current = value;
                    _maximum = value;
                }
                // If current value is being set higher than maximum value, sets it to maximum value instead.
                _current = value.CompareTo(Maximum) > 0 ? _maximum : value;
            }
        }

        // Maximum value.
        private T _maximum;
        public T Maximum
        {
            get
            {
                return _maximum;
            }
            set
            {
                _maximum = value;
                // If current value is not set or if it is higher than maximum value, sets it to maximum value.
                if (_current == null || _maximum.CompareTo(_current) < 0)
                    _current = _maximum;
            }
        }

        // Resets current value to maximum.
        public void Reset()
        {
            Current = Maximum;
        }

        #region Constructors
        // Constructors.
        public Parameter(T maximum)
        {
            Maximum = maximum;
            Current = maximum;
        }
        public Parameter(T maximum, T current)
        {
            Maximum = maximum;
            Current = current;
        }
        #endregion

        // For cloning.
        public object Clone()
        {
            return new Parameter<T>(Maximum, Current);
        }

        #region Equals
        // Equality override.
        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }

            // If obj can not be cast to Parameter<T>.
            Parameter<T> par = obj as Parameter<T>;
            if (par == null)
            {
                return false;
            }

            return Current.Equals(par.Current) && Maximum.Equals(par.Maximum);
        }

        // For performance.
        public bool Equals(Parameter<T> par)
        {
            if (par == null)
            {
                return false;
            }

            return Current.Equals(par.Current) && Maximum.Equals(par.Maximum);
        }

        // For Equals.
        public override int GetHashCode()
        {
            return Current.GetHashCode() ^ Maximum.GetHashCode();
        }
        #endregion
    }
}
