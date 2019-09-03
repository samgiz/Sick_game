using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Game1000
{
    public static class C
    {
        public static readonly int screenWidth, screenHeight;
        public static Matrix transform;
        public static ContentManager Content;

        static C()
        {
            screenWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
            screenHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
        }

        public static void LoadImage(string name)
            => Content.Load<Texture2D>(name);
        public static bool IsKeyDown(Keys key)
            => Keyboard.GetState().IsKeyDown(key);
        public static bool IsLeftButtonPressed()
            => Mouse.GetState().LeftButton == ButtonState.Pressed;
        public static bool IsRightButtonPressed()
            => Mouse.GetState().RightButton == ButtonState.Pressed;
        public static Vector2 MousePos()
            => Vector2.Transform(new Vector2(Mouse.GetState().X, Mouse.GetState().Y), Matrix.Invert(transform));
    }
}
