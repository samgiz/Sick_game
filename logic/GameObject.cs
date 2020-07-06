using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Drawing;

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
            image = C.LoadImage(imageName);
            origin = C.ImageOrigin(image);
            if (width.HasValue)
                scale = (float)width / image.Width;
            else
                scale = 1;
        }

        protected void Draw(bool ifCollides = true)
        {
            Color curColor = color;
            if (!ifCollides)
                curColor *= 0.5f;
            C.drawer.DrawDisk(new {position, color=curColor, origin, scale});
        }
    }
}
