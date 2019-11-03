using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Game1000
{
    public class DiskObstacle : Disk
    {
        public DiskObstacle(Vector2 position, float radius, Color color)
            : base(position, Vector2.Zero, radius, color)
        {
        }

        public new void Update(float arenaRadius)
        {
            base.Update(arenaRadius);
        }

        public void Collide(Player player)
        {
            if (!IfIntersects(player) || player.isInvisible)
                return;
            if (player.wasInvisible)
            {
                player.isAlive = false;
                return;
            }

            Vector2 direction = player.position - position;
            direction.Normalize();
            player.position = position + direction * (player.radius + radius);
            player.velocity -= 2 * Vector2.Dot(player.velocity, direction) * direction;
        }

        public void Collide(Bullet bullet)
        {
            if (!IfIntersects(bullet))
                return;

            bullet.isAlive = false;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
        }
    }
}
