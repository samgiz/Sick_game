using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace Game1000
{
    public class Segment : Obstacle
    {
        // line is a.X * x + a.Y * y = b
        // length of a is 1
        private readonly Vector2 a;
        private readonly float b;
        private readonly Vector2 vert1, vert2;
        private readonly Color color;
        // radius around the line defining the boundary
        private readonly float radius;

        // for drawing
        private readonly Texture2D pixelImage, diskImage;
        private readonly Vector2 midPos, pixelOrigin, pixelScale, diskOrigin;
        private readonly float angle, diskScale;

        public Segment(Vector2 vert1, Vector2 vert2, float lineWidth, Color color)
        {
            this.vert1 = vert1;
            this.vert2 = vert2;
            radius = lineWidth / 2;
            // since line is described by a verctor perpendicular to any vectorr along the line
            a = new Vector2(-(vert1 - vert2).Y, (vert1 - vert2).X);
            a.Normalize();
            b = Vector2.Dot(a, vert1);
            this.color = color;

            pixelImage = C.LoadImage("pixel");
            pixelOrigin = C.ImageOrigin(pixelImage);
            midPos = (vert1 + vert2) / 2;
            pixelScale = new Vector2(lineWidth, Vector2.Distance(vert1, vert2));
            angle = (float)Math.Atan2(a.Y, a.X);

            diskImage = C.LoadImage("disk");
            diskOrigin = C.ImageOrigin(diskImage);
            diskScale = 2 * radius / diskImage.Width;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(pixelImage, midPos, null, color, angle, pixelOrigin, pixelScale, SpriteEffects.None, 0);
            spriteBatch.Draw(diskImage, vert1, null, color, 0, diskOrigin, diskScale, SpriteEffects.None, 0);
            spriteBatch.Draw(diskImage, vert2, null, color, 0, diskOrigin, diskScale, SpriteEffects.None, 0);
        }

        // Returns the distance between line and disk
        public float Dist(Disk disk)
        {
            // closest point on the segment to the disk
            Vector2 closePoint = ClosePoint(disk);

            return Vector2.Distance(disk.position, closePoint) - radius;
        }

        // Handles collision with disk (usually player or bullet)
        public void Collide(Disk disk)
        {
            // closest point on the segment to the player
            Vector2 closePoint = ClosePoint(disk);

            // closePoint is the one to do all the collision
            Disk temp = new Disk(closePoint, Vector2.Zero, radius, Color.Transparent);

            temp.Collide(disk);
        }

        // Closest point on the line to the disk
        private Vector2 ClosePoint(Disk disk)
        {
            // perpendicular to segment, 
            Vector2 perp = new Vector2(-a.Y, a.X);

            Vector2 closeLinePoint = LineIntersection(a, b, perp, Vector2.Dot(perp, disk.position)).Value;

            if (IsBetween(closeLinePoint, vert1, vert2))
            {
                return closeLinePoint;
            }

            if (Vector2.Distance(disk.position, vert1) < Vector2.Distance(disk.position, vert2))
            {
                return vert1;
            }
            else
            {
                return vert2;
            }
        }

        // returns coordinates where two lines intersect, if they do not intersect, returns null
        private Vector2? LineIntersection(Vector2 a, float b, Vector2 c, float d)
        {
            // we are solving system of equations:
            // a.X * x + a.Y * y = b
            // c.X * x + c.Y * y = d
            float determinant = a.X * c.Y - a.Y * c.X;

            // if they move in parrallel, they do not collide
            if (determinant == 0)
            {
                return null;
            }

            // position of the collision of lines
            return new Vector2(b * c.Y - d * a.Y, d * a.X - b * c.X) / determinant;
        }

        // returns true if a is between b and c on the line
        private bool IsBetween(Vector2 a, Vector2 b, Vector2 c)
        {
            return Vector2.Distance(a, b) < Vector2.Distance(b, c) && Vector2.Distance(a, c) < Vector2.Distance(b, c);
        }
    }
}
