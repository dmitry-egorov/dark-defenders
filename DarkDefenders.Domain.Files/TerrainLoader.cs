using System;
using System.Drawing;
using System.IO;
using DarkDefenders.Domain.Other;
using Infrastructure.Math;
using Infrastructure.Util;

namespace DarkDefenders.Domain.Files
{
    public static class TerrainLoader
    {
        public static Map<Tile> LoadFromFile(string path)
        {
            var extension = Path.GetExtension(path);

            if (extension == ".txt")
            {
                return LoadFromTextFile(path);
            }
            
            if (extension == ".bmp")
            {
                return LoadFromMonochromeBmp(path);
            }

            throw new InvalidOperationException("Extension {0} is not supported".FormatWith(extension));
        }

        public static Map<Tile> LoadFromTextFile(string path)
        {
            var lines = File.ReadAllLines(path);

            if (lines.Length == 0)
            {
                throw new InvalidOperationException("No lines found");
            }

            var firstLine = lines[0];
            var width = firstLine.Length;
            var height = lines.Length;

            var map = CreateMap(width, height);

            var y = 0;
            foreach (var line in lines)
            {
                if (line.Length != width)
                {
                    throw new InvalidOperationException("All lines must be of the same width");
                }

                for (var x = 0; x < width; x++)
                {
                    map[x, height - 1 - y] = line[x] == ' ' ? Tile.Open : Tile.Solid;
                }

                y++;
            }

            return map;
        }

        public static Map<Tile> LoadFromMonochromeBmp(string path)
        {
            using (var bitmap = new Bitmap(path))
            {
                var width = bitmap.Width;
                var height = bitmap.Height;

                var map = CreateMap(width, height);

                for (var y = 0; y < height; y++)
                {
                    for (var x = 0; x < width; x++)
                    {
                        map[x, height - 1 - y] = bitmap.GetPixel(x, y) == Color.FromArgb(255, 0, 0, 0) ? Tile.Solid : Tile.Open;
                    }
                }

                return map;
            }
        }

        private static Map<Tile> CreateMap(int width, int height)
        {
            var dimensions = new Dimensions(width, height);

            return new Map<Tile>(dimensions, Tile.Solid);
        }
    }
}
