using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Game1000
{
    public class Camera
    {
        private Matrix transform;
        private float scale;

        public Camera(int screenWidth, int screenHeight)
        {
            scale = (float)screenHeight / 1080;
            transform = Matrix.CreateScale(scale) * Matrix.CreateTranslation(screenWidth * 0.5f, screenHeight * 0.5f, 0);
        }

        public void BeginDraw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, null, null, transform);
        }
    }
}
