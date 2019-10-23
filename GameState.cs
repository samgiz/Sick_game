using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace Game1000
{
    public class GameState
    {
        Camera camera;
        List<DiskObstacle> diskObstacles;
        List<Player> players;
        List<Bullet> bullets;
        Arena arena;
        
        public GameState(List<Player> ps)
        {
            diskObstacles = new List<DiskObstacle>();
            diskObstacles.Add(new DiskObstacle(new Vector2(0, 100), 20, Color.Black));
            // TODO: players should be passed as an argument and initialized in the constructor
            // Adding players one by one is generally undesirable and is only temporary behaviour
            players = ps;
            bullets = new List<Bullet>();
            arena = new Arena(Color.White);
            arena.AssignPositions(players);
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

            // Update player positions if they have been shot
            foreach (Player player in players)
                foreach (Bullet bullet in bullets)
                    Bullet.Collide(player, bullet);

            // Update player positions if they bounce off disk obstacle
            foreach (Player player in players)
                foreach (DiskObstacle diskObstacle in diskObstacles)
                    DiskObstacle.Collide(player, diskObstacle);

            // Check if bullet collides with disk obstacle
            foreach (Bullet bullet in bullets)
                foreach (DiskObstacle diskObstacle in diskObstacles)
                    DiskObstacle.Collide(bullet, diskObstacle);

            // Check if any 2 bullets have collided
            for (int i = 0; i < bullets.Count; i++)
                for (int j = 0; j < i; j++)
                    Bullet.Collide(bullets[i], bullets[j]);


            // Update the position of each player based on the time that has elapsed
            foreach (Player player in players){
                player.Update(elapsed, bullets);
                if(!arena.InBounds(player)){
                    player.isAlive = false;
                }
            }

            // Update position of bullets
            foreach (Bullet bullet in bullets){
                bullet.Update(elapsed);
                if(!arena.InBounds(bullet)){
                    bullet.isAlive = false;
                }
            }

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

            foreach (DiskObstacle diskObstacle in diskObstacles)
                diskObstacle.Draw(spriteBatch);

            spriteBatch.End();
        }
        public void AddPlayer(Player p){
            players.Add(p);
            arena.AssignPositions(players);
        }
    }
}
