using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Diagnostics;

namespace MGSimpelFysik
{
    public class Entity
    {
        public Vector2 position = new Vector2(50,50);
        public float rotation = 0;
        public Vector2 origin = Vector2.Zero;
        public float scale = 1;
        public Texture2D texture;
        public AnimatedSprite animatedSprite;
        public SpriteEffects spriteEffects = SpriteEffects.None;
        public float layerDepth = 0;

        public Point debug_colllision_tile;
        public Entity(Texture2D texture, Vector2? position = null, float rotation = 0, float scale = 1, AnimatedSprite animatedSprite = null, SpriteEffects spriteEffects = SpriteEffects.None, float layerDepth = 0)
        {
            this.texture = texture;
            this.position = position ?? new Vector2(50, 50);
            this.rotation = rotation;
            this.scale = scale;
            this.animatedSprite = animatedSprite;
            this.spriteEffects = spriteEffects;
            this.layerDepth = layerDepth;
            this.origin = animatedSprite != null ? new Vector2(animatedSprite.CurrentTextureRegion.Width, animatedSprite.CurrentTextureRegion.Width)/2f : new Vector2(texture.Width, texture.Width)/2f;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            
            if (animatedSprite != null)
            {
                spriteBatch.Draw(animatedSprite.texture, position, animatedSprite.CurrentTextureRegion, Color.White, rotation, origin, scale, spriteEffects, layerDepth);
            }
            else if (texture != null) 
            {
                spriteBatch.Draw(texture, position, null, Color.White, rotation, new Vector2(texture.Width / 2, texture.Height / 2), scale, spriteEffects, layerDepth);
            }
            if(debug_colllision_tile.X != -1238)
            {
                spriteBatch.Draw(texture, new Rectangle(debug_colllision_tile.X*50, debug_colllision_tile.Y*50, 50, 50), Color.PaleVioletRed);
            }
        }
    }
    public class PhysicalEntity : Entity
    {
        public Vector2 velocity = new Vector2(100f, -10f);
        public Vector2 gravity = new Vector2(0, 800f); // 16 tiles per sekund^2
        float collisionradious = 10;
        private Tilemap tilemap;
        float tempBounceSpeed = 100f;
        private bool was_in_solid = false;
        public PhysicalEntity(Tilemap tilemap, Texture2D texture, float collisionradious = 10) : base(texture)
        {
            this.collisionradious = collisionradious;
            this.tilemap = tilemap;
        }
        public PhysicalEntity(Tilemap tilemap, Texture2D texture, float collisionradious = 10, Vector2? position = null, float rotation = 0, float scale = 1, AnimatedSprite animatedSprite = null, SpriteEffects spriteEffects = SpriteEffects.None, float layerDepth = 0)
        : base(texture, position, rotation, scale, animatedSprite, spriteEffects, layerDepth)
        {
            this.collisionradious = collisionradious; //får inte vara mer än 25 (collrad/2)
            this.tilemap = tilemap;
        }

        public void Update(GameTime gameTime)
        {
            
        }
        public void PhysicsUpdate(GameTime gameTime)
        {
            float delta = (float)gameTime.ElapsedGameTime.TotalMilliseconds / 1000;
            const float tileSize = 50;
            //debug_colllision_tile.X = -123;

            velocity += gravity * delta;
             velocity = new Vector2(MathF.Min(MathF.Max(velocity.X,-collisionradious/delta),collisionradious / delta), MathF.Min(MathF.Max(velocity.Y, -collisionradious / delta), collisionradious / delta));
            position += velocity * delta;

            Point tpos = tilemap.PosToTile(position);
            if (tilemap.GetTileType(new Point(tpos.X, tpos.Y )) >= 0) //inside solid reaction
            { 
                velocity = velocity * 0.9f;
                if (was_in_solid == false)
                {
                    Debug.WriteLine($"pos {position} tpos {tpos} vel {velocity} delta-second {delta}");
                }
                was_in_solid = true;
                return; 
            } was_in_solid = false;

            if (tilemap.GetTileType(new Point(tpos.X, tpos.Y +1)) >= 0) //down
            {
                float boundry = (tpos.Y + 1) * tileSize - collisionradious;
                if (position.Y > boundry)
                {
                    position.Y = boundry;
                    velocity.Y = MathF.Min(velocity.Y, -velocity.Y*0.6f);
                    debug_colllision_tile = new Point(tpos.X, tpos.Y + 1); //debug coll
                }
            }
            if (tilemap.GetTileType(new Point(tpos.X -1, tpos.Y)) >= 0) //left
            {
                float boundry = (tpos.X + 0) * tileSize + collisionradious;
                if (position.X < boundry)
                {
                    position.X = boundry;
                    velocity.X = MathF.Max(velocity.X, tempBounceSpeed);
                    
                }
            }
            if (tilemap.GetTileType(new Point(tpos.X, tpos.Y - 1)) >= 0) //up
            {
                float boundry = (tpos.Y + 0) * tileSize + collisionradious;
                if (position.Y < boundry)
                {
                    position.Y = boundry;
                    velocity.Y = MathF.Max(velocity.Y, tempBounceSpeed);
                }
            }
            if (tilemap.GetTileType(new Point(tpos.X + 1, tpos.Y)) >= 0) //right
            {
                float boundry = (tpos.X + 1) * tileSize - collisionradious;
                if (position.X > boundry)
                {
                    position.X = boundry;
                    velocity.X = MathF.Min(velocity.X, -tempBounceSpeed);
                }
            }






        }
    }
}