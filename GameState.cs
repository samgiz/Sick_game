using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace Game1000
{
    public class GameState
    {
        Camera camera;
        List<Player> players;
        List<Bullet> bullets;
        Arena arena;
        
        public GameState()
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
            // Elapsed time since the last update
            float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;

            // Update the player positions if they are colliding
            // (Current implementation may cause problems for >2 players)
            // TODO: test this out
            for (int i = 0; i < players.Count; i++)
                for (int j = 0; j < i; j++)
                    Player.Collide(players[i], players[j]);

            // Check if any player has been shot
            foreach (Player player in players)
                foreach (Bullet bullet in bullets)
                    Bullet.Collide(player, bullet);

            // Check if any 2 bullets have collided
            for (int i = 0; i < bullets.Count; i++)
                for (int j = 0; j < i; j++)
                    Bullet.Collide(bullets[i], bullets[j]);

            // Update the position of each player based on the time that has elapsed
            // TODO: pass the controls of a player as a default  
            foreach (Player player in players)
                player.Update(elapsed, arena.radius, bullets);

            // Update position of bullets
            foreach (Bullet bullet in bullets)
                bullet.Update(elapsed, arena.radius);

            // Remove dead players
            for (int i = 0; i < players.Count; i++)
                if (!players[i].isAlive)
                {
                    players.RemoveAt(i);
                    i--;
                }

            // Remove bullets that have exploded
            for (int i = 0; i < bullets.Count; i++)
                if (!bullets[i].isAlive)
                {
                    bullets.RemoveAt(i);
                    i--;
                }

            // Update the arena
            // Currently this means shrinking the size
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
