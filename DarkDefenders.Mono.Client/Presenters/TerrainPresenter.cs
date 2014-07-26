using DarkDefenders.Game.Model.Other;
using Infrastructure.Math;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DarkDefenders.Mono.Client.Presenters
{
    public class TerrainPresenter
    {
        private readonly Texture2D _groundTexture;
        private readonly SpriteBatch _spriteBatch;
        private Map<Tile> _map;

        public TerrainPresenter(Map<Tile> map, Texture2D groundTexture, SpriteBatch spriteBatch)
        {
            _groundTexture = groundTexture;
            _spriteBatch = spriteBatch;
            _map = map;
        }

        public void DrawTerrain()
        {
            for (var x = 0; x < _map.Dimensions.Width; x++)
            {
                for (var y = 0; y < _map.Dimensions.Height; y++)
                {
                    var color = GetTileColor(_map, x, y);

                    _spriteBatch.Draw(_groundTexture, new Rectangle(x, y, 1, 1), color);
                }
            }
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