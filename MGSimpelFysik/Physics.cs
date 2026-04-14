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
                entity.PhysicsUpdate(gameTime);

             
                float posX = entity.position.X; //wrapa
                if (posX > gameWindowWidth)
                {
                    entity.position.X = posX % gameWindowWidth;
                }
                else if (posX < 0)
                {
                    entity.position.X = (posX + 5 * gameWindowWidth) % gameWindowWidth; // 5 = godtyckligt, räcker med 1
                }
                float posY = entity.position.Y;
                if (posY > gameWindowHeight)
                {
                    entity.position.Y = posY % gameWindowHeight;
                }
                else if (posY < 0)
                {
                    entity.position.Y = (posY + 5 * gameWindowHeight) % gameWindowHeight;
                }
            }
        }
    }
}
