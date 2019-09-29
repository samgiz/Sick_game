﻿using Microsoft.Xna.Framework;
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
        public static Vector2 MousePos(MouseState mouseState)
            => Vector2.Transform(new Vector2(mouseState.X, mouseState.Y), Matrix.Invert(transform));
    }
}
