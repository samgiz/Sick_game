using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace Game1000
{
    public class Polygon : Obstacle
    {               
        private readonly List<Segment> segments;

        public Polygon(List<Vector2> vertices, float lineWidth, Color color)
        {
            segments = new List<Segment>();
            for (int i = 0; i < vertices.Count; i++)
            {
                segments.Add(new Segment(vertices[i], vertices[(i + 1) % vertices.Count], lineWidth, color));
            }
        }

        public void Collide(Disk disk)
        {
            foreach (Segment segment in segments)
                segment.Collide(disk);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (Segment segment in segments)
                segment.Draw(spriteBatch);
        }
    }
}
