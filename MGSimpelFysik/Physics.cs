using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MGSimpelFysik
{
    internal class Physics
    {
        public List<PhysicalEntity> entities = new List<PhysicalEntity>();
        private Tilemap tilemap;
        private int gameWindowWidth = 500;
        private int gameWindowHeight = 500;
        public Physics(int gameWindowWidth, int gameWindowHeight, Tilemap tilemap)
        {
            this.gameWindowWidth = gameWindowWidth;
            this.gameWindowHeight = gameWindowHeight;
            this.tilemap = tilemap;
        }

        public void Update(GameTime gameTime)
        {
            foreach (PhysicalEntity entity in entities)
            {
                entity.Update(gameTime);

                if (tilemap.GetTileType(entity.position) != 0)
                {
                    entity.velocity.Y -= 0.01f;
                }
                else
                {
                    entity.velocity  .Y += 0.01f;
                }

                    float posX = entity.position.X;
                if (posX < 0 || posX > gameWindowWidth)
                {
                    entity.position.X = posX % gameWindowWidth;
                }
                float posY = entity.position.Y;
                if (posY < 0 || posY > gameWindowHeight)
                {
                    entity.position.Y = posY % gameWindowHeight;
                }
            }
        }
    }
}
