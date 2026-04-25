using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MGSimpelFysik
{
    public class Physics
    {
        public List<PhysicalEntity> entities = new List<PhysicalEntity>();
        private Tilemap tilemap;
        public int gameWindowWidth = 500;
        public int gameWindowHeight = 500;
        public List<PhysicalEntity> pEntitiesToAdd = new List<PhysicalEntity>();
        public List<PhysicalEntity> pEntitiesToRemove = new List<PhysicalEntity>();

        public Physics(int gameWindowWidth, int gameWindowHeight, Tilemap tilemap)
        {
            this.gameWindowWidth = gameWindowWidth;
            this.gameWindowHeight = gameWindowHeight;
            this.tilemap = tilemap;
        }

        public void AddEntity(PhysicalEntity entity)
        {
            pEntitiesToAdd.Add(entity);
        }
        public void RemoveEntity(PhysicalEntity entity) 
        {
            pEntitiesToRemove.Add(entity); 
        }

        public void Update(GameTime gameTime)
        {
            foreach (PhysicalEntity PE in pEntitiesToAdd) //modifiera entities listan innan istället för medans foreach körs = inte error
            {
                entities.Add(PE);
            }
            pEntitiesToAdd.Clear();
            foreach (PhysicalEntity PE in pEntitiesToRemove)
            {
                entities.Remove(PE);
            }
            pEntitiesToRemove.Clear();

            float delta = (float)gameTime.ElapsedGameTime.TotalSeconds;
            foreach (PhysicalEntity entity in entities)
            {
                entity.PhysicsUpdate(delta);

             
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
