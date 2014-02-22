using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using DarkDefenders.Game.Model.Other;
using Infrastructure.Math;
using Infrastructure.Util;

namespace DarkDefenders.Game.Resources.Internals
{
    public static class WorldLoader
    {
        public static WorldData LoadFromFile(string mapId)
        {
            var pathToFiles = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "WorldsData");
            var path = Path.Combine(pathToFiles, mapId);

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

        private static WorldData LoadFromTextFile(string path)
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
            var playerSpawns = new List<Vector>();
            var heroSpawns = new List<Vector>();

            var y = 0;
            foreach (var line in lines)
            {
                if (line.Length != width)
                {
                    throw new InvalidOperationException("All lines must be of the same width");
                }

                for (var x = 0; x < width; x++)
                {
                    var c = line[x];
                    var worldY = height - 1 - y;
                    switch (c)
                    {
                        case ' ':
                            map[x, worldY] = Tile.Open;
                            break;
                        case '@':
                            playerSpawns.Add(new Vector(x + 0.5, worldY + 0.5));
                            break;
                        case 'H':
                            heroSpawns.Add(new Vector(x + 0.5, worldY + 0.5));
                            break;
                        default:
                            map[x, worldY] = Tile.Solid;
                            break;
                    }
                }

                y++;
            }

            return new WorldData(map, playerSpawns, heroSpawns);
        }

        private static WorldData LoadFromMonochromeBmp(string path)
        {
            using (var bitmap = new Bitmap(path))
            {
                var width = bitmap.Width;
                var height = bitmap.Height;

                var map = CreateMap(width, height);
                var playerSpawns = new List<Vector>();
                var heroSpawns = new List<Vector>();

                var solidColor = Color.FromArgb(255, 0, 0, 0);
                var playerSpawnColor = Color.FromArgb(255, 0, 0, 255);
                var heroSpawnColor = Color.FromArgb(255, 255, 0, 0);

                for (var y = 0; y < height; y++)
                {
                    for (var x = 0; x < width; x++)
                    {
                        var pixel = bitmap.GetPixel(x, y);
                        var worldY = height - 1 - y;

                        if (pixel == solidColor)
                        {
                            map[x, worldY] = Tile.Solid;
                        }
                        else 
                        {
                            map[x, worldY] = Tile.Open;

                            if (pixel == playerSpawnColor)
                            {
                                playerSpawns.Add(new Vector(x + 0.5, worldY + 0.5));
                            }
                            else if (pixel == heroSpawnColor)
                            {
                                heroSpawns.Add(new Vector(x + 0.5, worldY + 0.5));
                            }
                        }
                    }
                }

                return new WorldData(map, playerSpawns, heroSpawns);
            }
        }

        private static Map<Tile> CreateMap(int width, int height)
        {
            var dimensions = new Dimensions(width, height);

            return new Map<Tile>(dimensions, Tile.Solid);
        }
    }
}
