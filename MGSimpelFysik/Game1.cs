using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
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

        private Texture2D dirt;
        private Texture2D tileSetTexture;
        private Texture2D dungeonTexture;
        private Texture2D goombaTexture;
        private Texture2D goalTexture;

        private LevelBuilder levelBuilder;
        private AnimatedSprite goombaAnim;
        private AnimatedSprite goalAnim;
        private PhysicalEntity goomba;
        private Tilemap tilemap;
        private Physics physicsWorld;
        

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

            dirt = Content.Load<Texture2D>("8 dirt");
            tileSetTexture = Content.Load<Texture2D>("tilemap_8px");
            dungeonTexture = Content.Load<Texture2D>("dungeon");
            goombaTexture = Content.Load<Texture2D>("goomba");
            goalTexture = Content.Load<Texture2D>("goalblob_8px");

        }

        protected override void Initialize()
        {
            
            base.Initialize(); //base.init calls LoadContent

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
        }

        

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            InputUppdate();
            
            goombaAnim.Update(gameTime);
            goalAnim.Update(gameTime);
            physicsWorld.Update(gameTime);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.FromNonPremultiplied(16,16,16,255));
            _spriteBatch.Begin(samplerState: SamplerState.PointClamp);

            tilemap.Draw(_spriteBatch);
            goomba.Draw(_spriteBatch);

            _spriteBatch.End();
            base.Draw(gameTime);
        }


        private void InputUppdate()
        {
            KeyboardState ks = Keyboard.GetState();
            MouseState ms = Mouse.GetState();
            
            if (ks.IsKeyDown(Keys.Space) && prevks.IsKeyUp(Keys.Space))
            {
                levelBuilder.SetTilesFromImage(GraphicsDevice, tilemap);
                
            }

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
