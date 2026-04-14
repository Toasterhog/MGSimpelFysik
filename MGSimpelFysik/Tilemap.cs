using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Diagnostics;

namespace MGSimpelFysik
{
   
    public class Tilemap
    {
        private int[,] tiles = new int[20, 12];
        private int tileSize = 50;
        private Texture2D tileset;
        private Rectangle[] sourceRects;
        public AnimatedSprite goalsprite;

        public Tilemap(Texture2D _tileSet, int? _tileSetSourceSize = null)
        {
            tileset = _tileSet;
            int tileSetSourceSize = _tileSetSourceSize ?? _tileSet.Height; //assume square
            sourceRects = new Rectangle[tileset.Width / tileSetSourceSize];
            for (int i = 0; i * _tileSetSourceSize < _tileSet.Width; i++)
            {
                sourceRects[i] = new Rectangle(i * tileSetSourceSize, 0, tileSetSourceSize, tileset.Height);
            }
        }

        public Point PosToTile(Vector2 pos) 
        {
            return new Point((int)pos.X / tileSize, (int)pos.Y / tileSize); 
        }
        public int GetTileType(Vector2 pos)
        {
            Point coord = PosToTile(pos);
            return GetTileType(coord);
        }
        public int GetTileType(Point coord) //should wrap
        {
            if (coord.X < 0 || coord.Y < 0 || coord.X >= tiles.GetLength(0) || coord.Y >= tiles.GetLength(1)) return -1;
            return tiles[coord.X, coord.Y];
        }
        public void SetTiles(int[,] newTiles)
        {
            if (tiles.GetLength(0) == newTiles.GetLength(0) && tiles.GetLength(1) == newTiles.GetLength(1))
            {
                tiles = newTiles;
            }
        }
        public Point GetTileMapSize() { return new Point(tiles.GetLength(0), tiles.GetLength(1)); }

        public void Draw(SpriteBatch spriteBatch)
        {
            for (int x = 0; x < tiles.GetLength(0); x++)
            {
                for (int y = 0; y < tiles.GetLength(1); y++)
                {
                    int tile = tiles[x, y];
                    if (tile >= 0 && tile < sourceRects.Length)
                    {
                        if(tile == 3)
                        {
                            spriteBatch.Draw(goalsprite.texture, new Rectangle(x * tileSize, y * tileSize, tileSize, tileSize), goalsprite.CurrentTextureRegion, Color.White);
                        }
                        else
                        {
                            spriteBatch.Draw(tileset, new Rectangle(x * tileSize, y * tileSize, tileSize, tileSize), sourceRects[tile], Color.White);
                        }
                    }
                }
            }
        }
    }
}
