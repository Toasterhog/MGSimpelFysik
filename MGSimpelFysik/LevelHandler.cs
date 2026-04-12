using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Diagnostics;
using System.IO;
using System.Reflection.Metadata;

namespace MGSimpelFysik
{
    public class LevelHandler
    {
        public Texture2D fallBackTexture;
        private Tilemap tilemap;
        //private Tilemap tilemap;
        public LevelHandler(Tilemap tilemap, Texture2D fallBackTexture) //_tilemap or Tilemap
        {
            this.fallBackTexture = fallBackTexture;
            this.tilemap = tilemap;
        }


        public Texture2D GetLevelImage(GraphicsDevice GD) //todo maybe be able to load multiple images/levels
        {
            try
            {
                string appDataPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.ApplicationData);
                string gameFolder = Path.Combine(appDataPath, "MonoGameTileMapTest");

                if (!Directory.Exists("MonoGameTileMapTest"))
                {
                    Directory.CreateDirectory("MonoGameTileMapTest");
                }

                string levelImgPath = Path.Combine(gameFolder, "level1.png");
                if (!File.Exists(levelImgPath))
                {
                    Debug.WriteLine("levelhandler level1 not exist");
                    Debug.WriteLine(levelImgPath);
                    return null;
                }
                Texture2D tex;
                using (FileStream stream = new FileStream(levelImgPath, FileMode.Open))
                {
                    tex = Texture2D.FromStream(GD, stream);
                }
                return tex;
            }
            catch { Debug.WriteLine("levelhandler error"); }
            return null;
        }

        public void SetTilesFromImage(GraphicsDevice GD, Tilemap tilemap)
        {
            Texture2D tileMapTexture = GetLevelImage(GD) ?? fallBackTexture;
            if (tileMapTexture == null) { Debug.WriteLine("LevelHandler cannot find image and has no falbacktexture"); return; }
            Color[] colorData = new Color[tileMapTexture.Width * tileMapTexture.Height];
            tileMapTexture.GetData<Color>(colorData);

            Point tilemapSize = tilemap.GetTileMapSize();
            int[,] tempTiles = new int[tilemapSize.X, tilemapSize.Y];
             for (int y = 0; y < tilemapSize.Y; y++)
                {
                for (int x = 0; x < tilemapSize.X; x++)
                {
                    tempTiles[x, y] = ColorToTileType(colorData[x + tilemapSize.X * y]);
                }
            }
            tilemap.SetTiles(tempTiles);
        }

        private int ColorToTileType(Color color)
        {
            return color.R < 128 ? 0 : -1;
        }
    }
}
