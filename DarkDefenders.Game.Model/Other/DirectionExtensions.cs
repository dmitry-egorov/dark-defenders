﻿using System;

namespace DarkDefenders.Game.Model.Other
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
    }
}