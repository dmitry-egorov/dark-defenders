using System;
using Infrastructure.Math;

namespace DarkDefenders.Game.Model.Other
{
    public static class MovementExtensions
    {
        public static HorizontalDirection ToDirection(this Movement movement)
        {
            switch (movement)
            {
                case Movement.Left:
                    return HorizontalDirection.Left;
                case Movement.Right:
                    return HorizontalDirection.Right;
                default:
                    throw new ArgumentOutOfRangeException("movement");
            }
        }

        public static Movement Other(this Movement movement)
        {
            switch (movement)
            {
                case Movement.Left:
                    return Movement.Right;
                case Movement.Right:
                    return Movement.Left;
                default:
                    throw new ArgumentOutOfRangeException("movement");
            }
        }
    }
}