using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Game1000
{
    public class Arena
    {
        public float radius;
        private Vector2 origin;
        private float scale, decreasePerSec;
        private Texture2D image;
        private Color color;

        public Arena(Color color)
        {
            this.color = color;
            radius = 540;
            decreasePerSec = 10;
            image = C.Content.Load<Texture2D>("big disk");
            origin = new Vector2(image.Width * 0.5f, image.Height * 0.5f);
            scale = 2 * radius / image.Width;
        }

        public void Update(float elapsed)
        {
            // radius -= decreasePerSec * elapsed;
            if (radius < 0)
                radius = 0;
            scale = 2 * radius / image.Width;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(image, Vector2.Zero, null, color, 0, origin, scale, SpriteEffects.None, 0);
        }
    }
}
