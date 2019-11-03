using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace Game1000
{
    public class Polygon
    {               
        private readonly List<Segment> segments;

        public Polygon(List<Vector2> vertices, Color color)
        {
            segments = new List<Segment>();
            for (int i = 0; i < vertices.Count; i++)
            {
                segments.Add(new Segment(vertices[i], vertices[(i + 1) % vertices.Count], color));
            }
        }

        public static void Collide(Player player, Polygon polygon)
        {
            int minDistInd = -1;

            for (int i = 0; i < polygon.segments.Count; i++)
            {
                if (Segment.IfCollides(player, polygon.segments[i]))
                {
                    float dist = Segment.Dist(player, polygon.segments[i]);
                    if (minDistInd == -1 || dist < Segment.Dist(player, polygon.segments[minDistInd]))
                    {
                        minDistInd = i;
                    }
                }
            }

            if (minDistInd != -1)
            {
                Segment.Collide(player, polygon.segments[minDistInd]);
                return;
            }

            foreach (Segment segment in polygon.segments)
            {
                if (Segment.IfEndpointsCollide(player, segment))
                {
                    Segment.CollideEndpoints(player, segment);
                    break;
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (Segment segment in segments)
                segment.Draw(spriteBatch);
        }
    }
}
