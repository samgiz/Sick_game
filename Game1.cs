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

        int screenWidth, screenHeight;
        Camera camera;
        List<Player> players;
        Arena arena;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            screenWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
            screenHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
            graphics.PreferredBackBufferWidth = screenWidth;
            graphics.PreferredBackBufferHeight = screenHeight;
            graphics.IsFullScreen = true;
        }

        protected override void Initialize()
        {

            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            players = new List<Player>();
            //for (int i = 0; i < 20; i++)
            //    for (int j = 0; j < 10; j++)
            //        players.Add(new Player(new Vector2(100 * (i + 1), 100 * (j + 1)), Keys.I, Keys.K, Keys.J, Keys.L, Color.Yellow, Content));
            players.Add(new Player(new Vector2(100, 100), Keys.Up, Keys.Down, Keys.Left, Keys.Right, Color.Red, Content));
            players.Add(new Player(new Vector2(-100, -100), Keys.W, Keys.S, Keys.A, Keys.D, Color.Green, Content));
            arena = new Arena(Color.White, Content);
            camera = new Camera(screenWidth, screenHeight);
        }

        protected override void UnloadContent()
        {
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;
            for (int i = 0; i < players.Count; i++)
                for (int j = 0; j < i; j++)
                    Player.Collide(players[i], players[j]);

            foreach (Player player in players)
                player.Update(elapsed, arena.radius);

            for (int i = 0; i < players.Count; i++)
                if (!players[i].isAlive)
                {
                    players.RemoveAt(i);
                    i--;
                }

            arena.Update(elapsed);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            camera.BeginDraw(spriteBatch);

            arena.Draw(spriteBatch);

            foreach (Player player in players)
                player.Draw(spriteBatch);

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
