using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
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

        public Portal(Game1 game)
        {
            this.game = game;
        }
        public void SetPortal(Point tile, int orientation, bool isMirrored, bool isBlue)
        {
            Debug.WriteLine("portal place");
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

            game.physicsWorld.AddEntity(proj);
            game.visuals.Add(proj as IDrawable);
            if (isBlue) {blueProjectile = proj; }
            else { yellowProjectile = proj; }
            Debug.WriteLine("rotation " + proj.rotation + " dir " + direction);
        }
    }
}
