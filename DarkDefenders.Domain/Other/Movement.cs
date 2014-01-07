﻿using System;

namespace DarkDefenders.Domain.Other
{
    public enum Movement
    {
        Stop = 0,
        Left = 1,
        Right = 2,
    }

    public static class MovementExtensions
    {

        public static Direction ToDirection(this Movement movement)
        {
            switch (movement)
            {
                case Movement.Left:
                    return Direction.Left;
                case Movement.Right:
                    return Direction.Right;
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