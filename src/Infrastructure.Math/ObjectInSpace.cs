using System;
using System.Runtime.CompilerServices;
using Infrastructure.Util;

namespace Infrastructure.Math
{
    public class ObjectInSpace
    {
        private readonly Vector _position;
        private readonly Box _boundingBox;

        public ObjectInSpace(Vector position, Box boundingBox)
        {
            _position = position;
            _boundingBox = boundingBox;
        }

        public ObjectInSpace MovedTo(Vector newPosition)
        {
            return new ObjectInSpace(newPosition, _boundingBox);
        }

        public Vector GetPosition()
        {
            return _position;
        }

        public Box GetBoundingBox()
        {
            return _boundingBox;
        }

        public double GetTopBoundY()
        {
            return _position.Y + _boundingBox.HeightRadius;
        }

        public double GetBottomBoundY()
        {
            return _position.Y - _boundingBox.HeightRadius;
        }

        public double GetRightBoundX()
        {
            return _position.X + _boundingBox.WidthRadius;
        }

        public double GetLeftBoundX()
        {
            return _position.X - _boundingBox.WidthRadius;
        }

        public int Level()
        {
            return GetBottomBoundY().TolerantFloor().ToInt();
        }

        public int SlotToTheRightX()
        {
            var right = GetRightBoundX();

            return right.NextInteger().ToInt();
        }

        public int SlotToTheLeftX()
        {
            var left = GetLeftBoundX();

            return left.TolerantFloor().ToInt() - 1;
        }

        public int NextSlotX(Direction direction)
        {
            switch (direction)
            {
                case Direction.Left:
                    return SlotToTheLeftX();
                case Direction.Right:
                    return SlotToTheRightX();
                default:
                    throw new ArgumentOutOfRangeException("direction");
            }
        }

        public int SlotYUnder()
        {
            var bottom = GetBottomBoundY();

            return bottom.PrevInteger().ToInt();
        }

        public int BoundSlot(Axis axis, AxisDirection direction)
        {
            switch (axis)
            {
                case Axis.X:
                    return BoundSlotX(direction);
                case Axis.Y:
                    return BoundSlotY(direction);
                default:
                    throw new ArgumentOutOfRangeException("axis");
            }
        }
        public int BoundSlotY(AxisDirection direction)
        {
            switch (direction)
            {
                case AxisDirection.Positive:
                    return TopBoundSlotY();
                case AxisDirection.Negative:
                    return BottomBoundSlotY();
                default:
                    throw new ArgumentOutOfRangeException("direction");
            }
        }

        public int BoundSlotX(AxisDirection direction)
        {
            switch (direction)
            {
                case AxisDirection.Negative:
                    return LeftBoundSlotX();
                case AxisDirection.Positive:
                    return RightBoundSlotX();
                default:
                    throw new ArgumentOutOfRangeException("direction");
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int TopBoundSlotY()
        {
            var top = GetTopBoundY();

            return top.TolerantFloor().ToInt();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int BottomBoundSlotY()
        {
            var bottom = GetBottomBoundY();

            return bottom.PrevInteger().ToInt();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int LeftBoundSlotX()
        {
            var left = GetLeftBoundX();

            return left.PrevInteger().ToInt();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int RightBoundSlotX()
        {
            var right = GetRightBoundX();

            return right.TolerantFloor().ToInt();
        }
    }
}