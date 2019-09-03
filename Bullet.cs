using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace Game1000
{
    public class Bullet : GameObject
    {
        private const float impactForce = 1000000;
        public new const float radius = 10;

        public Bullet(Vector2 position, Vector2 velocity)
            : base(position, velocity, radius, Color.Black)
        {
        }

        public new void Update(float elapsed, float arenaRadius)
        {
            base.Update(elapsed, arenaRadius);
        }

        public static void Collide(Player player, Bullet bullet)
        {
            if (!GameObject.IfIntersects(player, bullet))
                return;
            Vector2 direction = player.position - bullet.position;
            direction.Normalize();
            player.velocity += direction * impactForce / player.mass;
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
