using System.Runtime.Remoting.Metadata.W3cXsd2001;

namespace HeroesArena
{
    // Represents one generiс unit in game logic.
    public class BasicUnit
    {
        public Cell Cell { get; private set; }
        public Direction Facing { get; private set; }

        // TODO delete later, this is just for tests,
        // moves unit to another cell.
        public void Move(Cell cell)
        {
            if (cell.Unit == null)
            {
                // TODO maybe add some choise of facing here in cases like moving 45 degrees up right.
                Facing = DirectionOfMovement(Cell, cell);

                cell.Unit = this;
                Cell.Unit = null;
                Cell = cell;
            }
        }

        // TODO this is definitely not the right place for these methods, move.
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

        public BasicUnit(Cell cell = null, Direction facing = Direction.Down)
        {
            Cell = cell;
            Facing = facing;
            cell.Unit = this;
        }

        #region Equals
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

            return Cell == unit.Cell && Facing == unit.Facing;
        }

        // For performance.
        public bool Equals(BasicUnit unit)
        {
            if (unit == null)
            {
                return false;
            }

            return Cell == unit.Cell && Facing == unit.Facing;
        }

        // For Equals.
        public override int GetHashCode()
        {
            return Cell.GetHashCode() ^ Facing.GetHashCode();
        }
        #endregion

        // TODO
    }
}
