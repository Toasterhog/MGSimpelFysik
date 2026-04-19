using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Diagnostics;

namespace MGSimpelFysik
{
    public class PortalHandler
    {
        private Game1 game;
        private int tileSize = 50;
        private Color blueColor = new Color(70, 85, 255);
        private Color yellowColor = new Color(255, 240, 95);
        public Projectile blueProjectile;
        public Projectile yellowProjectile;
        public Portal portalB;
        public Portal portalY;
        
        public PortalHandler(Game1 game)
        {
            this.game = game;
        }
        public void SetPortal(Point tile, int orientation, bool flipped, bool isBlue)
        {
            Debug.WriteLine("portal place");
            if (isBlue)
            {
                if (portalB != null) { DestroyPortal(portalB); }
                portalB = new Portal(tile, orientation, flipped);
            }
            else
            {
                if (portalY != null) { DestroyPortal(portalY); }
                portalY = new Portal(tile, orientation, flipped);
            }
        }
        public void DestroyPortal(Portal portalToDestroy)
        {
            portalToDestroy = null;
        }
        public Portal GetLinkedPortal(Portal p)
        {
            if(p == portalB) { return portalY; }
            if(p == portalY) {  return portalB; }
            return null;
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
        public bool TileHasDisabledCollision(Point tile) {  return false; } //not implemented!!!!!!!!!!!!!!!!!!

        public void Draw(SpriteBatch spriteBatch)
        {
            Portal[] toDraw = [portalB, portalY ];
            foreach (Portal portal in toDraw)
            {
                if (portal == null) continue;
                Point origin = Tilemap.TileTOPosCenterP(portal.tile);

                spriteBatch.Draw(
                   game.bluePortalFrameTexture,
                   new Rectangle(origin.X, origin.Y, tileSize, tileSize),
                   null,
                   blueColor,
                   portal.orientation,
                   new Vector2(4, 4),
                   portal.flipped ? SpriteEffects.FlipHorizontally : SpriteEffects.None,
                   0
               );
            }

        }

    }
}
