using System;

namespace Infrastructure.Math
{
    public static class DirectionExtensions
    {
        public static HorizontalDirection Other(this HorizontalDirection direction)
        {
            switch (direction)
            {
                case HorizontalDirection.Left:
                    return HorizontalDirection.Right;
                case HorizontalDirection.Right:
                    return HorizontalDirection.Left;
                default:
                    throw new ArgumentOutOfRangeException("direction");
            }
        }

        public static Direction ToDirection(this HorizontalDirection direction)
        {
            switch (direction)
            {
                case HorizontalDirection.Left:
                    return Direction.Left;
                case HorizontalDirection.Right:
                    return Direction.Right;
                default:
                    throw new ArgumentOutOfRangeException("direction");
            }
        }

        public static AxisDirection ToAxisDirection(this HorizontalDirection direction)
        {
            switch (direction)
            {
                case HorizontalDirection.Left:
                    return Math.AxisDirection.Negative;
                case HorizontalDirection.Right:
                    return Math.AxisDirection.Positive;
                default:
                    throw new ArgumentOutOfRangeException("direction");
            }
        }

        public static AxisDirection AxisDirection(this Direction direction)
        {
            switch (direction)
            {
                case Direction.Left:
                    return Math.AxisDirection.Negative;
                case Direction.Right:
                    return Math.AxisDirection.Positive;
                case Direction.Top:
                    return Math.AxisDirection.Positive;
                case Direction.Bottom:
                    return Math.AxisDirection.Negative;
                default:
                    throw new ArgumentOutOfRangeException("direction");
            }
        }

        public static Axis Axis(this Direction direction)
        {
            switch (direction)
            {
                case Direction.Left:
                    return Math.Axis.X;
                case Direction.Right:
                    return Math.Axis.X;
                case Direction.Top:
                    return Math.Axis.Y;
                case Direction.Bottom:
                    return Math.Axis.Y;
                default:
                    throw new ArgumentOutOfRangeException("direction");
            }
        }

        public static AxisDirection Other(this AxisDirection direction)
        {
            switch (direction)
            {
                case Math.AxisDirection.Negative:
                    return Math.AxisDirection.Positive;
                case Math.AxisDirection.Positive:
                    return Math.AxisDirection.Negative;
                default:
                    throw new ArgumentOutOfRangeException("direction");
            }
        }

        public static Direction Other(this Direction direction)
        {
            switch (direction)
            {
                case Direction.Left:
                    return Direction.Right;
                case Direction.Right:
                    return Direction.Left;
                case Direction.Top:
                    return Direction.Bottom;
                case Direction.Bottom:
                    return Direction.Top;
                default:
                    throw new ArgumentOutOfRangeException("direction");
            }
        }

        public static int GetIncrement(this AxisDirection direction)
        {
            switch (direction)
            {

                case Math.AxisDirection.None:
                    return 0;
                case Math.AxisDirection.Negative:
                    return -1;
                case Math.AxisDirection.Positive:
                    return  1;
                default:
                    throw new ArgumentOutOfRangeException("direction");
            }
        }

        public static int GetIncrement(this Direction direction)
        {
            switch (direction)
            {
                case Direction.Left:
                    return -1;
                case Direction.Right:
                    return  1;
                case Direction.Top:
                    return  1;
                case Direction.Bottom:
                    return -1;
                default:
                    throw new ArgumentOutOfRangeException("direction");
            }
        }

        public static Direction AbsoluteDirection(this Axis axis, AxisDirection direction)
        {
            if (direction == Math.AxisDirection.None)
            {
                return Direction.None;
            }

            if (axis == Math.Axis.X)
            {
                return 
                direction == Math.AxisDirection.Positive 
                ? Direction.Right 
                : Direction.Left;
            }

            return
            direction == Math.AxisDirection.Positive
            ? Direction.Top
            : Direction.Bottom;
        }

    }
}