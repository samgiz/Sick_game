using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Game1000
{
    public class Camera
    {
        private float scale;

        public Camera()
        {
            scale = (float)C.screenHeight / 1080;
            C.transform = Matrix.CreateScale(scale) * Matrix.CreateTranslation(C.screenWidth * 0.5f, C.screenHeight * 0.5f, 0);
        }

        public void BeginDraw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, null, null, null, null, C.transform);
        }
    }
}
