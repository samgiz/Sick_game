﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Game1000
{
    public class Disk : GameObject
    {
        public readonly float radius;

        protected Disk(Vector2 position, float radius, Color color)
            : base(position, color, "disk", 2 * radius)
        {
            this.radius = radius;
        }

        protected void Update(float arenaRadius)
        {
            if (position.Length() > arenaRadius + radius)
                isAlive = false;
        }

        protected static bool IfIntersects(Disk disk1, Disk disk2)
            => Vector2.Distance(disk1.position, disk2.position) < disk1.radius + disk2.radius;

        protected new void Draw(SpriteBatch spriteBatch, bool isInvisible = false)
        {
            base.Draw(spriteBatch, isInvisible);
            //Color curColor = color;
            //if (isInvisible)
            //    curColor *= 0.5f;
            //spriteBatch.Draw(image, position, null, curColor, 0, origin, scale, SpriteEffects.None, 0);
        }
    }
}
