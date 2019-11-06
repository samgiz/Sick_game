﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace Game1000
{
    public class GameState
    {
        private List<Obstacle> obstacles;
        private List<Player> players;
        private List<Bullet> bullets;
        private Arena arena;
        
        public GameState(List<Player> ps)
        {
            //diskObstacles.Add(new DiskObstacle(new Vector2(0, 100), 20, Color.Black));
            // TODO: players should be passed as an argument and initialized in the constructor
            // Adding players one by one is generally undesirable and is only temporary behaviour
            players = ps;
            bullets = new List<Bullet>();
            arena = new Arena(Color.White);
            arena.AssignPositions(players);
            List<Vector2> vertices = new List<Vector2>();
            vertices.Add(new Vector2(0, 0));
            vertices.Add(new Vector2(20, 100));
            vertices.Add(new Vector2(100, 0));
            vertices.Add(new Vector2(0, -100));
            vertices.Add(new Vector2(-100, 0));
            obstacles = new List<Obstacle>();
            obstacles.Add(new Polygon(vertices, 32, Color.Yellow));
        }

        public void Update(GameTime gameTime)
        {
            // Elapsed time since the last update
            float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;

            // Update the position of each player based on the time that has elapsed
            foreach (Player player in players)
                player.Update(elapsed, bullets);

            // Update position of bullets
            foreach (Bullet bullet in bullets)
                bullet.Update(elapsed);

            // Handle collisions of 2 players
            // (Current implementation may cause problems for >2 players)
            // TODO: test this out
            for (int i = 0; i < players.Count; i++)
                for (int j = 0; j < i; j++)
                    players[i].Collide(players[j]);

            // Handle collisions between Player and Bullet
            foreach (Player player in players)
                foreach (Bullet bullet in bullets)
                    bullet.Collide(player);

            // Handle collisions of 2 bullets
            for (int i = 0; i < bullets.Count; i++)
                for (int j = 0; j < i; j++)
                    bullets[i].Collide(bullets[j]);

            // Handle collisions between Obstacle and Player
            foreach (Obstacle obstacle in obstacles)
                foreach (Player player in players)
                    obstacle.Collide(player);

            // Handle collisions between Obstacle and Bullet
            foreach (Bullet bullet in bullets)
                foreach (Obstacle obstacle in obstacles)
                    obstacle.Collide(bullet);

            // Remove dead players
            for (int i = 0; i < players.Count; i++)
                if (!players[i].isAlive || !arena.InBounds(players[i]))
                {
                    players.RemoveAt(i);
                    i--;
                }

            // Remove bullets that have exploded
            for (int i = 0; i < bullets.Count; i++)
                if (!bullets[i].isAlive || !arena.InBounds(bullets[i]))
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

            foreach (Obstacle obstacle in obstacles)
                obstacle.Draw(spriteBatch);

            spriteBatch.End();
        }

        public void AddPlayer(Player p){
            players.Add(p);
            arena.AssignPositions(players);
        }
    }
}
