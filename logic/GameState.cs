using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace Game1000
{
    public class GameState
    {
        List<DiskObstacle> diskObstacles;
        List<Player> players;
        List<Bullet> bullets;
        List<Polygon> polygons;
        Arena arena;
        
        public GameState(List<Player> ps)
        {
            diskObstacles = new List<DiskObstacle>();
            //diskObstacles.Add(new DiskObstacle(new Vector2(0, 100), 20, Color.Black));
            // TODO: players should be passed as an argument and initialized in the constructor
            // Adding players one by one is generally undesirable and is only temporary behaviour
            players = ps;
            bullets = new List<Bullet>();
            arena = new Arena(Color.White);
            arena.AssignPositions(players);
            //segments.Add(new Segment(new Vector2(0, 0), new Vector2(100, 100), Color.Black));
            polygons = new List<Polygon>();
            List<Vector2> vertices = new List<Vector2>();
            vertices.Add(new Vector2(0, 0));
            vertices.Add(new Vector2(20, 100));
            vertices.Add(new Vector2(100, 0));
            vertices.Add(new Vector2(0, -100));
            vertices.Add(new Vector2(-100, 0));
            polygons.Add(new Polygon(vertices, Color.Black));
        }

        public void Update(GameTime gameTime)
        {
            // Elapsed time since the last update
            float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;

            // Handle collisions of 2 players
            // (Current implementation may cause problems for >2 players)
            // TODO: test this out
            for (int i = 0; i < players.Count; i++)
                for (int j = 0; j < i; j++)
                    players[i].Collide(players[j]);

            // Handle collisions of Player
            foreach (Player player in players)
            {
                // Handle collisions with Bullet
                foreach (Bullet bullet in bullets)
                    bullet.Collide(player);

                // Handle collisions with DiskObstacle
                foreach (DiskObstacle diskObstacle in diskObstacles)
                    diskObstacle.Collide(player);

                // Handle collisions with Polygon
                foreach (Polygon polygon in polygons)
                    polygon.Collide(player);
            }

            // Handle collisions of 2 bullets
            for (int i = 0; i < bullets.Count; i++)
                for (int j = 0; j < i; j++)
                    bullets[i].Collide(bullets[j]);

            // Handle collisions of Bullet
            foreach (Bullet bullet in bullets)
            {
                // Handle collisions with DiskObstacle
                foreach (DiskObstacle diskObstacle in diskObstacles)
                    diskObstacle.Collide(bullet);

                // Handle collisions with Polygon
                foreach (Polygon polygon in polygons)
                    polygon.Collide(bullet);
            }                

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
            arena.Draw(spriteBatch);

            foreach (Player player in players)
                player.Draw(spriteBatch);

            foreach (Bullet bullet in bullets)
                bullet.Draw(spriteBatch);

            foreach (DiskObstacle diskObstacle in diskObstacles)
                diskObstacle.Draw(spriteBatch);

            foreach (Polygon polygon in polygons)
                polygon.Draw(spriteBatch);

            spriteBatch.End();
        }

        public void AddPlayer(Player p){
            players.Add(p);
            arena.AssignPositions(players);
        }
    }
}
