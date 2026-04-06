using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;


namespace MGSimpelFysik
{
    public class AnimatedSprite
    {
        public Texture2D texture;
        private Rectangle[] textureRegions;
        private TimeSpan elapsed = TimeSpan.Zero;
        private TimeSpan delay = TimeSpan.FromMilliseconds(200);
        public bool Playing = true;
        private int frame = 0;
        public int Frame {  
            get { return frame; } 
            set {
                if (value > textureRegions.Length) {frame = textureRegions.Length;}
                else if (value < 0) { frame = 0; }
                else { frame = value; }
            } 
        }
        public Rectangle CurrentTextureRegion => textureRegions[frame];

        public AnimatedSprite(Texture2D Texture)
        {
            texture = Texture;
            textureRegions = new Rectangle[1];
            textureRegions[0] = new Rectangle(0,0, texture.Width, texture.Height);
            Playing = false;
        }
        public AnimatedSprite(Texture2D Texture, int regionWidth)
        {
            texture = Texture;
            textureRegions = new Rectangle[Texture.Width / regionWidth];
            for (int i = 0; i * regionWidth < texture.Width; i++)
            {
                textureRegions[i] = new Rectangle(i * regionWidth, 0, regionWidth, texture.Height);
            }
        }

        public void Update(GameTime gameTime)
        {
            if (!Playing) return;

            elapsed += gameTime.ElapsedGameTime;
            if (elapsed >= delay)
            {
                elapsed -= delay;
                frame++;
                if (frame >= textureRegions.Length)
                {
                    frame = 0;
                }

            }
        }

    }
}
