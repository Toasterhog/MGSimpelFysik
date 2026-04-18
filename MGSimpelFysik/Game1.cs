using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace MGSimpelFysik
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        private int windowWidth = 1000;
        private int windowHeight = 600;
        
        private KeyboardState prevks;
        private MouseState prevms;
        private Point blueAimStart;
        private Point yellowAimStart;

        private Texture2D tileSetTexture;
        private Texture2D dungeonTexture;
        private Texture2D goombaTexture;
        private Texture2D goalTexture;
        private Texture2D blueProjectileTexture;
        //private Texture2D yellowProjectileTexture;


        private AnimatedSprite goombaAnim;
        private AnimatedSprite goalAnim;
        public AdvancedSprite blueProjectileAnim;
        public AdvancedSprite yellowProjectileAnim;

        private LevelBuilder levelBuilder;
        private PhysicalEntity goomba;
        public Tilemap tilemap;
        public Physics physicsWorld;
        public Portal portalSystem;

        public List<IDrawable> visuals = new List<IDrawable>();

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            _graphics.PreferredBackBufferWidth = windowWidth;
            _graphics.PreferredBackBufferHeight = windowHeight;
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            tileSetTexture = Content.Load<Texture2D>("tilemap_8px");
            dungeonTexture = Content.Load<Texture2D>("dungeon");
            goombaTexture = Content.Load<Texture2D>("goomba");
            goalTexture = Content.Load<Texture2D>("goalblob_8px");
            blueProjectileTexture = Content.Load<Texture2D>("projectile"); 
            //yellowProjectileTexture = Content.Load<Texture2D>("dungeon");
        }

        protected override void Initialize()
        {
            
            base.Initialize(); //base.init calls LoadContent

            blueProjectileAnim = new AdvancedSprite(blueProjectileTexture, new Point(12,7));
            yellowProjectileAnim = new AdvancedSprite(blueProjectileTexture, new Point(12, 7));
            blueProjectileAnim.Delay = 50;
            yellowProjectileAnim.Delay = 140;
            goalAnim = new AnimatedSprite(goalTexture, 8);
            goalAnim.Delay = 120;
            tilemap = new Tilemap(tileSetTexture,8);
            tilemap.goalsprite = goalAnim;
            goombaAnim = new AnimatedSprite(goombaTexture, 16);
            goomba = new PhysicalEntity(tilemap, null, goombaAnim, 16, position: new Vector2(300,300), scale: 4);
            goomba.origin = new Vector2(8, 12);
            physicsWorld = new Physics(windowWidth, windowHeight, tilemap);
            physicsWorld.entities.Add(goomba);
            levelBuilder = new LevelBuilder(tilemap, dungeonTexture);
            portalSystem = new Portal(this);

            levelBuilder.SetTilesFromImage(GraphicsDevice, tilemap);
        }

        

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            InputUppdate();
            
            goombaAnim.Update(gameTime);
            goalAnim.Update(gameTime);
            blueProjectileAnim.Update(gameTime);
            yellowProjectileAnim.Update(gameTime);

            physicsWorld.Update(gameTime);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.FromNonPremultiplied(16,16,16,255));
            _spriteBatch.Begin(samplerState: SamplerState.PointClamp);

            tilemap.Draw(_spriteBatch);
            goomba.Draw(_spriteBatch);

            foreach (IDrawable visual in visuals)
            {
                visual.Draw(_spriteBatch);
            }

            _spriteBatch.End();
            base.Draw(gameTime);
        }


        private void InputUppdate()
        {
            KeyboardState ks = Keyboard.GetState();
            MouseState ms = Mouse.GetState();
            
            if (ks.IsKeyDown(Keys.Space)) //move goomba cheat
            {
                if (ms.LeftButton == ButtonState.Pressed)
                {
                    goomba.position = new Vector2(ms.Position.X, ms.Position.Y);
                }
                else if (prevms.LeftButton == ButtonState.Pressed)
                {
                    goomba.position = new Vector2(ms.Position.X, ms.Position.Y);
                    goomba.velocity = new Vector2(ms.Position.X, ms.Position.Y) - new Vector2(prevms.Position.X, prevms.Position.Y);
                    goomba.velocity *= 100f;
                }
            }
            //----- direction from goomba ----
            if (prevms.LeftButton == ButtonState.Pressed && ms.LeftButton == ButtonState.Released)
            {
                
                Vector2 dir = new Vector2(ms.Position.X, ms.Position.Y ) - goomba.position;
                portalSystem.SpawnProjectile(true, goomba.position, dir);
                //Debug.WriteLine("blue proj spawned");
            }

            if (prevms.RightButton == ButtonState.Pressed && ms.RightButton == ButtonState.Released)
            {
                Vector2 dir = new Vector2(ms.Position.X, ms.Position.Y) - goomba.position;
                portalSystem.SpawnProjectile(false, goomba.position, dir);
                //Debug.WriteLine("yellow proj spawned");
            }
            //---- direction from drag press and hold ----
            //if (ms.LeftButton == ButtonState.Pressed && prevms.LeftButton == ButtonState.Released) { blueAimStart = ms.Position; }
            //else if (prevms.LeftButton == ButtonState.Pressed && ms.LeftButton == ButtonState.Released)
            //{
            //    Point pdir = ms.Position - blueAimStart;
            //    Vector2 dir = new Vector2(pdir.X, pdir.Y);
            //    portalSystem.SpawnProjectile(true, goomba.position, dir);
            //    Debug.WriteLine("blue proj spawned");
            //}

            //if (ms.RightButton == ButtonState.Pressed && prevms.RightButton == ButtonState.Released) { yellowAimStart = ms.Position; }
            //else if (prevms.RightButton == ButtonState.Pressed && ms.RightButton == ButtonState.Released)
            //{
            //    Point pdir = ms.Position - yellowAimStart;
            //    Vector2 dir = new Vector2(pdir.X, pdir.Y);
            //    portalSystem.SpawnProjectile(false, goomba.position, dir);
            //    Debug.WriteLine("yellow proj spawned");
            //}


            prevks = ks;
            prevms = ms;
        }
    }

    public static class RandomInfo
    {
        public static readonly int windowWidth = 1000;
        public static readonly int windowHeight = 600;
        public static readonly int tileMapWidth = 20;
        public static readonly int tileMapHeight = 12;
        public static readonly int tileSize = 50;
    }
    

}
