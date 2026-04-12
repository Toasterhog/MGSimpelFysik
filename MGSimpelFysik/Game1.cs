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

        private Texture2D dirt;
        private KeyboardState previousKeyboardState;
        private Texture2D tileSetTexture;
        private Texture2D dungeonTexture;
        private Texture2D goombaTexture;

        private LevelHandler levelHandler;
        private AnimatedSprite goombaAnim;
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

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            
            base.Initialize(); //base.init calls LoadContent

            tilemap = new Tilemap(tileSetTexture, 8);
            goombaAnim = new AnimatedSprite(goombaTexture, 16);
            goomba = new PhysicalEntity(tilemap, goombaTexture, animatedSprite: goombaAnim, position: new Vector2(300,300), scale: 4);
            goomba.origin = new Vector2(8, 12);
            physicsWorld = new Physics(windowWidth, windowHeight, tilemap);
            physicsWorld.entities.Add(goomba);
            levelHandler = new LevelHandler(tilemap, dungeonTexture);
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            dirt = Content.Load<Texture2D>("8 dirt");
            tileSetTexture = Content.Load<Texture2D>("8 dirt");
            dungeonTexture = Content.Load<Texture2D>("dungeon");
            goombaTexture = Content.Load<Texture2D>("goomba");

           
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            KeyboardState keyboardState = Keyboard.GetState();
            
            if (keyboardState.IsKeyDown(Keys.Space) && previousKeyboardState.IsKeyUp(Keys.Space))
            {
                levelHandler.SetTilesFromImage(GraphicsDevice, tilemap);
                
            }
            MouseState ms = Mouse.GetState();
            if(ms.LeftButton == ButtonState.Pressed) { goomba.position = new Vector2( ms.Position.X, ms.Position.Y); }

            goombaAnim.Update(gameTime);
            physicsWorld.Update(gameTime);

            previousKeyboardState = keyboardState;
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
