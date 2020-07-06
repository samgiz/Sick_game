using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Drawing;

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
        GraphicsHandler gHandler;

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
            C.spriteBatch = spriteBatch;
            C.Content = Content;
            game = new GameState(new List<Player>());
            gHandler = new StandardGraphicsHandler(game);
            camera = new Camera();
            game.AddPlayer(new Player(new LocalControls(), 32, Color.Red, true, false));
            game.AddPlayer(new Player(new LocalControls(Keys.Up, Keys.Left, Keys.Down, Keys.Right), 40, Color.Green, false, true));

            // if you want to create your map, second variable must be 'true'
            map = new MapCreateState(new LocalControls(), false);
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
            

            if (map.isReady)
                gHandler.Draw();
            else{
                camera.BeginDraw(spriteBatch);
                map.Draw();
                spriteBatch.End();
            }
            base.Draw(gameTime);
        }
    }
}
