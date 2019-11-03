using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Game1000
{
    public class DiskObstacle : Disk
    {
        public DiskObstacle(Vector2 position, float radius, Color color)
            : base(position, radius, color)
        {
        }

        public new void Update(float arenaRadius)
        {
            base.Update(arenaRadius);
        }

        public static void Collide(Player player, DiskObstacle diskObstacle)
        {
            if (!IfIntersects(player, diskObstacle) || player.isInvisible)
                return;
            if (player.wasInvisible)
            {
                player.isAlive = false;
                return;
            }

            Vector2 direction = player.position - diskObstacle.position;
            direction.Normalize();
            player.position = diskObstacle.position + direction * (player.radius + diskObstacle.radius);
            player.velocity -= 2 * Vector2.Dot(player.velocity, direction) * direction;
        }

        public static void Collide(Bullet bullet, DiskObstacle diskObstacle)
        {
            if (!IfIntersects(bullet, diskObstacle))
                return;

            bullet.isAlive = false;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
        }
    }
}
