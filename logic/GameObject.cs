using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace Game1000
{
    public class GameObject
    {
        public Vector2 position;
        public bool isAlive;
        protected float angle;
        protected readonly Vector2 origin;
        protected readonly Texture2D image;
        protected readonly float scale;
        protected readonly Color color;

        protected GameObject(Vector2 position, Color color, string imageName, float? width = null, float angle = 0)
        {
            this.position = position;
            this.angle = angle;
            this.color = color;
            isAlive = true;
            Console.WriteLine(imageName);
            image = C.Content.Load<Texture2D>(imageName);
            origin = new Vector2(image.Width * 0.5f, image.Height * 0.5f);
            if (width.HasValue)
                scale = (float)width / image.Width;
            else
                scale = 1;
        }

        protected void Draw(SpriteBatch spriteBatch, bool isInvisible = false)
        {
            Color curColor = color;
            if (isInvisible)
                curColor *= 0.5f;
            spriteBatch.Draw(image, position, null, curColor, 0, origin, scale, SpriteEffects.None, 0);
        }
    }
}
