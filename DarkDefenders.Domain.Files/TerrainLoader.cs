using System;
using System.IO;
using Infrastructure.Math;

namespace DarkDefenders.Domain.Files
{
    public static class TerrainLoader
    {
        public static Map LoadFromTextFile(string path)
        {
            var lines = File.ReadAllLines(path);

            if (lines.Length == 0)
            {
                throw new InvalidOperationException("No lines found");
            }

            var firstLine = lines[0];
            var width = firstLine.Length;
            var height = lines.Length;

            var dimensions = new Dimensions(width, height);

            var map = new Map(dimensions);

            var y = 0;
            foreach (var line in lines)
            {
                if (line.Length != width)
                {
                    throw new InvalidOperationException("All lines must be of the same width");
                }

                for (var x = 0; x < width; x++)
                {
                    map[x, height - 1 - y] = (byte) (line[x] == ' ' ? 0 : 1);
                }

                y++;
            }

            return map;
        }
    }
}
