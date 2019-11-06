using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Game1000
{
    public class DiskObstacle : Disk, Obstacle
    {
        public DiskObstacle(Vector2 position, float radius, Color color)
            : base(position, Vector2.Zero, radius, color)
        {
        }

        public void Collide(Disk disk)
        {
            if (!IfIntersects(disk) || !disk.ifCollides)
                return;

            Vector2 direction = disk.position - position;
            direction.Normalize();
            disk.position = position + direction * (disk.radius + radius);
            disk.velocity -= 2 * Vector2.Dot(disk.velocity, direction) * direction;

            disk.Collide();
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
        }
    }
}
