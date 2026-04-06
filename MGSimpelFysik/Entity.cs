using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
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
            
        }
    }
    public class PhysicalEntity : Entity
    {
        public Vector2 velocity = new Vector2(1f, 1f)*0.1f;
        public Vector2 gravity = new Vector2(0, 0);
        float collisionradious = 10;
        public PhysicalEntity(Texture2D texture, float collisionradious = 10) : base(texture)
        {
            this.collisionradious = collisionradious;
        }
        public PhysicalEntity(Texture2D texture, float collisionradious = 10, Vector2? position = null, float rotation = 0, float scale = 1, AnimatedSprite animatedSprite = null, SpriteEffects spriteEffects = SpriteEffects.None, float layerDepth = 0)
        : base(texture, position, rotation, scale, animatedSprite, spriteEffects, layerDepth)
        {
            this.collisionradious = collisionradious;
        }

        public void Update(GameTime gameTime)
        {
            
        }
        public void PhysicsUpdate(GameTime gameTime)
        {
            velocity += gravity * gameTime.ElapsedGameTime.Milliseconds;
            position += velocity * gameTime.ElapsedGameTime.Milliseconds;

        }
    }
}