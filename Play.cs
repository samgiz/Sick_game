using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace Game1000
{
    public class Play
    {
        Camera camera;
        List<Player> players;
        List<Bullet> bullets;
        Arena arena;
        
        public Play()
        {
            players = new List<Player>();
            //for (int i = 0; i < 20; i++)
            //    for (int j = 0; j < 10; j++)
            //        players.Add(new Player(new Vector2(100 * (i + 1), 100 * (j + 1)), Keys.I, Keys.K, Keys.J, Keys.L, Color.Yellow));
            players.Add(new Player(new LocalControls(Keys.Up, Keys.Left, Keys.Down, Keys.Right), new Vector2(100, 0), 32, Color.Red, true, false));
            players.Add(new Player(new LocalControls(Keys.W, Keys.A, Keys.S, Keys.D), new Vector2(-100, 0), 40, Color.Green, false, true));
            bullets = new List<Bullet>();
            arena = new Arena(Color.White);
            camera = new Camera();
        }

        public void Update(GameTime gameTime)
        {
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
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            camera.BeginDraw(spriteBatch);

            arena.Draw(spriteBatch);

            foreach (Player player in players)
                player.Draw(spriteBatch);

            foreach (Bullet bullet in bullets)
                bullet.Draw(spriteBatch);

            spriteBatch.End();
        }
    }
}
