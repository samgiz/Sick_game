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

        public void Collide(Player player)
        {
            bool ifSideCollides = false;
            Segment closeSegment = new Segment(Vector2.Zero, Vector2.Zero, Color.Transparent);

            foreach (Segment segment in segments)
            {
                if (segment.IfCollides(player) && (!ifSideCollides || segment.Dist(player) < closeSegment.Dist(player)))
                {
                    ifSideCollides = true;
                    closeSegment = segment;
                }
            }

            if (ifSideCollides)
            {
                closeSegment.Collide(player);
                return;
            }

            foreach (Segment segment in segments)
            {
                if (segment.IfEndpointsCollide(player))
                {
                    segment.CollideEndpoints(player);
                    break;
                }
            }
        }

        public void Collide(Bullet bullet)
        {
            bool ifSideCollides = false;
            Segment closeSegment = new Segment(Vector2.Zero, Vector2.Zero, Color.Transparent);

            foreach (Segment segment in segments)
            {
                if (segment.IfCollides(bullet) && (!ifSideCollides || segment.Dist(bullet) < closeSegment.Dist(bullet)))
                {
                    ifSideCollides = true;
                    closeSegment = segment;
                }
            }

            if (ifSideCollides)
            {
                closeSegment.Collide(bullet);
                return;
            }

            foreach (Segment segment in segments)
            {
                if (segment.IfEndpointsCollide(bullet))
                {
                    segment.CollideEndpoints(bullet);
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
