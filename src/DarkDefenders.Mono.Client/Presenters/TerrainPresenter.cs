using DarkDefenders.Game.Model.Other;
using Infrastructure.Math;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DarkDefenders.Mono.Client.Presenters
{
    public class TerrainPresenter
    {
        private readonly Texture2D _whiteTexture;
        private readonly SpriteBatch _spriteBatch;
        private Map<Color> _colorMap;

        public TerrainPresenter(Map<Tile> map, Texture2D whiteTexture, SpriteBatch spriteBatch)
        {
            _whiteTexture = whiteTexture;
            _spriteBatch = spriteBatch;
            _colorMap = ConvertToColorMap(map);
        }

        public void Draw()
        {
            for (var x = 0; x < _colorMap.Dimensions.Width; x++)
            {
                for (var y = 0; y < _colorMap.Dimensions.Height; y++)
                {
                    _spriteBatch.Draw(_whiteTexture, new Rectangle(x, y, 1, 1), _colorMap[x, y]);
                }
            }
        }

        private static Map<Color> ConvertToColorMap(Map<Tile> tiles)
        {
            var colorMap = new Map<Color>(tiles.Dimensions, Color.SaddleBrown);

            for (var x = 0; x < tiles.Dimensions.Width; x++)
            {
                for (var y = 0; y < tiles.Dimensions.Height; y++)
                {
                    colorMap[x, y] = GetTileColor(tiles, x, y);
                }
            }

            return colorMap;
        }

        private static Color GetTileColor(Map<Tile> map, int x, int y)
        {
            var color = Color.SaddleBrown;
            var count = 0;

            if (map[x, y] == Tile.Open)
            {
                return new Color(150, 130, 86);
            }

            for (var i = x - 1; i <= x + 1; i++)
                for (var j = y - 1; j <= y + 1; j++)
                {
                    if (i == x && j == y)
                    {
                        continue;
                    }

                    if (map[i, j] == Tile.Solid)
                    {
                        continue;
                    }

                    count++;

                    if (count == 2)
                    {
                        return color;
                    }
                }

            return count == 0 ? Color.Black : color * 0.6f;
        }
    }
}