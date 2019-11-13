using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Game1000
{
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        bool wasReady;
        MapCreateState map;

        GameState game;

        Camera camera;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            graphics.PreferredBackBufferWidth = C.screenWidth;
            graphics.PreferredBackBufferHeight = C.screenHeight;
            graphics.IsFullScreen = false;
            IsMouseVisible = true;
            wasReady = false;
        }

        protected override void Initialize()
        {
            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            C.Content = Content;
            game = new GameState(new List<Player>());
            camera = new Camera();
            game.AddPlayer(new Player(new LocalControls(), 32, Color.Red, true, false));
            game.AddPlayer(new Player(new LocalControls(Keys.Up, Keys.Left, Keys.Down, Keys.Right), 40, Color.Green, false, true));

            map = new MapCreateState(new LocalControls());
        }

        protected override void UnloadContent()
        {
        }

        protected override void Update(GameTime gameTime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            
            if (map.isReady)
            {
                if (!wasReady)
                {
                    game.AddObtacles(map.Read());
                }
                wasReady = map.isReady;
                game.Update(gameTime);
            }
            else
            {
                map.Update();
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            camera.BeginDraw(spriteBatch);

            if (map.isReady)
                game.Draw(spriteBatch);
            else
                map.Draw(spriteBatch);
            
            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
