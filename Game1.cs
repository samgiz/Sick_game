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

        Camera camera;
        List<Player> players;
        List<Bullet> bullets;
        Arena arena;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            graphics.PreferredBackBufferWidth = C.screenWidth;
            graphics.PreferredBackBufferHeight = C.screenHeight;
            graphics.IsFullScreen = false;
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {

            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            C.Content = Content;
            players = new List<Player>();
            //for (int i = 0; i < 20; i++)
            //    for (int j = 0; j < 10; j++)
            //        players.Add(new Player(new Vector2(100 * (i + 1), 100 * (j + 1)), Keys.I, Keys.K, Keys.J, Keys.L, Color.Yellow));
            players.Add(new Player(new Vector2(100, 100), 32, Keys.Up, Keys.Down, Keys.Left, Keys.Right, Color.Red));
            players.Add(new Player(new Vector2(-100, -100), 64, Keys.W, Keys.S, Keys.A, Keys.D, Color.Green));
            bullets = new List<Bullet>();
            arena = new Arena(Color.White);
            camera = new Camera();
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
                foreach (Bullet bullet in bullets)
                    Bullet.Collide(player, bullet);

            for (int i = 0; i < bullets.Count; i++)
                for (int j = 0; j < i; j++)
                    Bullet.Collide(bullets[i], bullets[j]);

            foreach (Player player in players)
                player.Update(elapsed, arena.radius, bullets);

            foreach (Bullet bullet in bullets)
                bullet.Update(elapsed, arena.radius);

            for (int i = 0; i < players.Count; i++)
                if (!players[i].isAlive)
                {
                    players.RemoveAt(i);
                    i--;
                }

            for (int i = 0; i < bullets.Count; i++)
                if (!bullets[i].isAlive)
                {
                    bullets.RemoveAt(i);
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

            foreach (Bullet bullet in bullets)
                bullet.Draw(spriteBatch);

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
