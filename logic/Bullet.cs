using Microsoft.Xna.Framework;

namespace Game1000
{
    public class Bullet : Disk
    {
        public new const float radius = 10;

        public Bullet(Vector2 position, Vector2 velocity)
            : base(position, velocity, radius, Color.Black)
        { }

        public override void Collide()
        {
            isAlive = false;
        }
    }
}
