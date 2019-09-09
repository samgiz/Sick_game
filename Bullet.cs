﻿using Microsoft.Xna.Framework;

namespace Game1000
{
    public class Bullet : GameObject
    {
        private const float impactForce = 400000;
        public new const float radius = 10;

        public Bullet(Vector2 position, Vector2 velocity)
            : base(position, velocity, radius, Color.Black)
        {
        }

        public void Update(float elapsed, float arenaRadius)
        {
            position += velocity * elapsed;
            base.Update(arenaRadius);
        }

        public static void Collide(Player player, Bullet bullet)
        {
            if (!GameObject.IfIntersects(player, bullet) || player.isInvisible)
                return;
            if (player.wasInvisible)
            {
                player.isAlive = false;
                bullet.isAlive = false;
            }
            Vector2 direction = player.position - bullet.position;
            direction.Normalize();
            player.velocity -= 2 * direction * Vector2.Dot(player.velocity - bullet.velocity, direction);
            //player.velocity += direction * impactForce / player.mass;
            bullet.isAlive = false;
        }

        public static void Collide(Bullet bullet1, Bullet bullet2)
        {
            if (!GameObject.IfIntersects(bullet1, bullet2))
                return;
            bullet1.isAlive = false;
            bullet2.isAlive = false;
        }
    }
}
