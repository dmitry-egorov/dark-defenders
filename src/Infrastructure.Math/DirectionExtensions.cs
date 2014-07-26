using System;

namespace Infrastructure.Math
{
    public static class DirectionExtensions
    {
        public static Direction Other(this Direction direction)
        {
            switch (direction)
            {
                case Direction.Left:
                    return Direction.Right;
                case Direction.Right:
                    return Direction.Left;
                default:
                    throw new ArgumentOutOfRangeException("direction");
            }
        }

        public static int GetXIncrement(this Direction direction)
        {
            switch (direction)
            {
                case Direction.Left:
                    return -1;
                case Direction.Right:
                    return 1;
                default:
                    throw new ArgumentOutOfRangeException("direction");
            }
        }

        public static AxisDirection ToAxisDirection(this Direction direction)
        {
            switch (direction)
            {
                case Direction.Left:
                    return AxisDirection.Negative;
                case Direction.Right:
                    return AxisDirection.Positive;
                default:
                    throw new ArgumentOutOfRangeException("direction");
            }
        }
    }
}