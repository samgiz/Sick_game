using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Game1000
{
    public class GameObject
    {
        public readonly float radius;
        public Vector2 position, velocity;
        public bool isAlive;
        protected readonly Vector2 origin;
        protected readonly Texture2D image;
        protected readonly float scale;
        protected readonly Color color;

        public GameObject(Vector2 position, Vector2 velocity, float radius, Color color)
        {
            this.position = position;
            this.radius = radius;
            this.color = color;
            isAlive = true;
            this.velocity = velocity;
            image = C.Content.Load<Texture2D>("disk");
            origin = new Vector2(image.Width * 0.5f, image.Height * 0.5f);
            scale = 2 * radius / image.Width;
        }

        protected void Update(float elapsed, float arenaRadius)
        {
            position += velocity * elapsed;

            if (position.Length() > arenaRadius + radius)
                isAlive = false;
        }

        public static bool IfIntersects(GameObject object1, GameObject object2)
            => Vector2.Distance(object1.position, object2.position) < object1.radius + object2.radius;

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(image, position, null, color, 0, origin, scale, SpriteEffects.None, 0);
        }
    }
}
