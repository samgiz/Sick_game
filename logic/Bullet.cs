using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Game1000
{
    public class Bullet : Disk
    {
        private const float impactForce = 1000000;
        public new const float radius = 10;

        public Bullet(Vector2 position, Vector2 velocity)
            : base(position, velocity, radius, Color.Black)
        {
        }

        public void Update(float elapsed)
        {
            position += velocity * elapsed;
        }

        public void Collide(Player player)
        {
            if (!IfIntersects(player) || player.isInvisible)
                return;
            if (player.wasInvisible)
            {
                player.isAlive = false;
                isAlive = false;
            }
            Vector2 direction = player.position - position;
            direction.Normalize();
            player.velocity -= 2 * direction * Vector2.Dot(player.velocity - velocity, direction);
            //player.velocity += direction * impactForce / player.mass;
            isAlive = false;
        }

        public void Collide(Bullet bullet)
        {
            if (!IfIntersects(bullet))
                return;
            isAlive = false;
            bullet.isAlive = false;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
        }
    }
}
