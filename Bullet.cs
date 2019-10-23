using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Game1000
{
    public class Bullet : Disk
    {
        public Vector2 velocity;
        private const float impactForce = 1000000;
        public new const float radius = 10;

        public Bullet(Vector2 position, Vector2 velocity)
            : base(position, radius, Color.Black)
        {
            this.velocity = velocity;
        }

        public void Update(float elapsed)
        {
            position += velocity * elapsed;
        }

        public static void Collide(Player player, Bullet bullet)
        {
            if (!Disk.IfIntersects(player, bullet) || player.isInvisible)
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
            if (!Disk.IfIntersects(bullet1, bullet2))
                return;
            bullet1.isAlive = false;
            bullet2.isAlive = false;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
        }
    }
}
