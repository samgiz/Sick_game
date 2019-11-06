using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Game1000
{
    public class Disk : GameObject
    {
        public readonly float radius;
        public Vector2 velocity;
        public bool ifCollides;

        protected Disk(Vector2 position, Vector2 velocity, float radius, Color color)
            : base(position, color, "disk", 2 * radius)
        {
            this.radius = radius;
            this.velocity = velocity;
            ifCollides = true;
        }

        public virtual void Collide() { }

        public bool IfIntersects(Disk disk)
            => Vector2.Distance(position, disk.position) < radius + disk.radius;

        protected new void Draw(SpriteBatch spriteBatch, bool ifCollides = true)
        {
            base.Draw(spriteBatch, ifCollides);
        }
    }
}
