using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Game1000
{
    public class Camera
    {
        private float scale;
        public static Matrix transform;

        private int screenWidth, screenHeight;

        public Camera()
        {
            screenWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
            screenHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
            
            // the 1080 probably shouldn't be a constant? [TODO]
            scale = (float)C.screenHeight / 1080;
            transform = Matrix.CreateScale(scale) * Matrix.CreateTranslation(screenWidth * 0.5f, screenHeight * 0.5f, 0);
        }

        public void BeginDraw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, null, null, null, null, transform);
        }
    }
}
