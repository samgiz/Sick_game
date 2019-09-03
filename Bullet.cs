using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace Game1000
{
    public class Bullet : GameObject
    {
        Bullet(Vector2 position, Vector2 velocity, float radius, Color color, ContentManager Content)
            : base(position, velocity, radius, color, Content)
        {
        }

        void Collide(Player player, Bullet bullet)
        {
            if (Vector2.Distance(player.position, bullet.position) >= player.radius + bullet.radius)
                return;
            Vector2 direction = player.position - bullet.position;
            direction.Normalize();
            player.velocity += direction * 1000;
            bullet.isAlive = false;
        }
    }
}
