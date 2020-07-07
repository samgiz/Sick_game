using Microsoft.Xna.Framework;
using System;

namespace Game1000
{
    public class GameObject
    {
        public Vector2 position;
        public bool isAlive;
        protected float angle;
        protected readonly Vector2 origin;
        protected readonly float scale;
        protected readonly Color color;
        float? width;

        protected GameObject(Vector2 position, Color color, string imageName, float? width = null, float angle = 0)
        {
            this.position = position;
            this.angle = angle;
            this.color = color;
            this.width = width;
            isAlive = true;
            Console.WriteLine(imageName);
        }

        protected void Draw(bool ifCollides = true)
        {
            Color curColor = color;
            if (!ifCollides)
                curColor *= 0.5f;
            C.drawer.DrawDisk(new {position, color=curColor, width});
        }
    }
}
