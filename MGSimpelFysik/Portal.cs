using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SharpDX.Direct2D1.Effects;
using SharpDX.Direct3D11;
using SharpDX.Direct3D9;
using System;
using System.Diagnostics;

namespace MGSimpelFysik
{
    public class Portal
    {
        public Projectile blueProjectile;
        private Projectile yellowProjectile;
        private Game1 game;
        private Color blueColor = new Color(70, 85, 255);
        private Color yellowColor = new Color(255, 240, 95);
        public struct generalPortalFrame
        {
            public generalPortalFrame(Point coord_, int side_, bool flipped_)
            {
                coord = coord_;
                side = side_;
                flipped = flipped_;
            }
            public Point coord { get; }
            public int side { get; }
            public bool flipped { get; }
        }
        public generalPortalFrame bluePortalFrame;
        public generalPortalFrame yellowPortalFrame;
        public bool bluePortalsExists = false;
        public bool yellowPortalsExists = false;
        public Entity drawingEntityBlue;
        public Entity drawingEntityYellow;
        int tileSize = 50;
        public Portal(Game1 game)
        {
            this.game = game;
        }
        public void SetPortal(Point tile, int orientation, bool isMirrored, bool isBlue)
        {
            Debug.WriteLine("portal place");
            if (isBlue)
            {
                if (bluePortalsExists) { DestroyPortal(isBlue); }
                bluePortalsExists = true;
                bluePortalFrame = new generalPortalFrame(tile, orientation, isMirrored);
                //drawingEntityBlue = new Entity(game.bluePortalFrameTexture, null, new Vector2(bluePortalFrame.coord.X * tileSize + tileSize / 2, yellowPortalFrame.coord.X * tileSize + tileSize / 2),);
            }
            else
            {
                if (yellowPortalsExists) { DestroyPortal(isBlue); }
                yellowPortalsExists = true;
                yellowPortalFrame = new generalPortalFrame(tile, orientation, isMirrored);
            }
        }
        public void DestroyPortal(bool isBlue)
        {
            if (isBlue) { bluePortalsExists = false; }
            else { yellowPortalsExists = false; }
        }

        public void RemoveProjectile(bool isBlue)
        {
            Projectile proj = isBlue ? blueProjectile : yellowProjectile;
            game.physicsWorld.RemoveEntity(proj);
            game.visuals.Remove(proj as IDrawable);
            if(isBlue ) {blueProjectile = null;}
            else {yellowProjectile = null;}
        }
        public void SpawnProjectile(bool isBlue, Vector2 spawnPoint, Vector2 direction)
        {
            direction = Vector2.Normalize(direction);

            if (isBlue && blueProjectile != null)
            {
                RemoveProjectile(isBlue);
            }
            else if (!isBlue && yellowProjectile != null)
            {
                RemoveProjectile(isBlue);
            }
            
            Projectile proj = new Projectile(
                this, 
                isBlue,
                direction,
                game.tilemap, 
                animatedSprite: isBlue ? game.blueProjectileAnim : game.yellowProjectileAnim, 
                position: spawnPoint, 
                rotation: MathF.Atan2(direction.Y, direction.X), 
                spriteEffects: direction.X < 0 ? SpriteEffects.FlipVertically : SpriteEffects.None
            );
            proj.colorMultiplier = isBlue ? blueColor : yellowColor;
            proj.isLeft = direction.X < 0 ? true : false;

            game.physicsWorld.AddEntity(proj);
            game.visuals.Add(proj as IDrawable);
            if (isBlue) {blueProjectile = proj; }
            else { yellowProjectile = proj; }
            Debug.WriteLine("rotation " + proj.rotation + " dir " + direction);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            
            if (bluePortalsExists)
            {
                float rotation = bluePortalFrame.side * MathF.PI / 2f;
                
                spriteBatch.Draw(
                    game.bluePortalFrameTexture,
                    new Rectangle(bluePortalFrame.coord.X * tileSize + tileSize/2, bluePortalFrame.coord.Y * tileSize + tileSize / 2, tileSize, tileSize),
                    null, 
                    blueColor, 
                    rotation, 
                    new Vector2(4,4), 
                    bluePortalFrame.flipped? SpriteEffects.FlipHorizontally : SpriteEffects.None, 
                    0
                );
            
            
            }
            if (yellowPortalsExists)
            {
                float rotation = yellowPortalFrame.side * MathF.PI / 2f;
                spriteBatch.Draw(
                    game.bluePortalFrameTexture,
                    new Rectangle(yellowPortalFrame.coord.X * tileSize + tileSize / 2, yellowPortalFrame.coord.Y * tileSize + tileSize / 2, tileSize, tileSize),
                    null,
                    yellowColor,
                    rotation,
                    new Vector2(4, 4),
                    yellowPortalFrame.flipped ? SpriteEffects.FlipHorizontally : SpriteEffects.None,
                    0
                );
            }
        }

    }
}
