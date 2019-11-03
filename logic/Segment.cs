using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace Game1000
{
    public class Segment
    {
        // line is a.X * x + a.Y * y = b
        // length of a is 1
        private readonly Vector2 a;
        private readonly float b;
        private readonly DiskObstacle diskObst1, diskObst2;
        private readonly Color color;

        // for drawing
        private readonly Texture2D image;
        private readonly Vector2 midPos, origin, scale;
        private readonly float angle;

        public Segment(Vector2 pos1, Vector2 pos2, Color color)
        {
            diskObst1 = new DiskObstacle(pos1, 0, Color.Transparent);
            diskObst2 = new DiskObstacle(pos2, 0, Color.Transparent);
            Vector2 direction = pos1 - pos2;
            direction.Normalize();
            a = Perp(direction);
            b = Vector2.Dot(a, pos1);
            this.color = color;

            image = C.LoadImage("pixel");
            midPos = (pos1 + pos2) / 2;
            origin = new Vector2(image.Width * 0.5f, image.Height * 0.5f);
            scale = new Vector2(1, Vector2.Distance(diskObst1.position, diskObst2.position));
            angle = (float)Math.Atan2(a.Y, a.X);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(image, midPos, null, color, angle, origin, scale, SpriteEffects.None, 0);
        }
        
        // Returns the distance between line and disk
        public float Dist(Disk disk)
        {
            // closest point on the segment to the disk
            Vector2 closePoint = ClosePoint(disk);

            return Vector2.Distance(disk.position, closePoint);
        }

        public bool IfCollides(Disk disk)
        {
            // if disk did not move, it does not collide
            if (disk.velocity == Vector2.Zero)
            {
                return false;
            }

            // direction of disk movement
            Vector2 moveDir = disk.velocity;
            moveDir.Normalize();

            // perpendicular to segment
            Vector2 perp = Perp(a);

            // closest point on the segment to the disk
            Vector2 closePoint = ClosePoint(disk);

            return IsBetween(closePoint, diskObst1.position, diskObst2.position) && Vector2.Distance(closePoint, disk.position) < disk.radius;
        }

        public bool IfEndpointsCollide(Disk disk)
        {
            return diskObst1.IfIntersects(disk) || diskObst2.IfIntersects(disk);
        }

        public void Collide(Player player)
        {
            // closest point on the segment to the player
            Vector2 closePoint = ClosePoint(player);

            // closePoint is the one to do all the collision
            DiskObstacle temp = new DiskObstacle(closePoint, 0, Color.Transparent);

            temp.Collide(player);
        }

        public void Collide(Bullet bullet)
        {
            bullet.isAlive = false;
        }
        
        public void CollideEndpoints(Player player)
        {
            diskObst1.Collide(player);
            diskObst2.Collide(player);
        }

        public void CollideEndpoints(Bullet bullet)
        {
            diskObst1.Collide(bullet);
            diskObst2.Collide(bullet);
        }

        // returns vector perpendicular to dir
        private Vector2 Perp(Vector2 dir)
        {
            return new Vector2(-dir.Y, dir.X);
        }

        // Closest point on the line to the disk
        private Vector2 ClosePoint(Disk disk)
        {
            // perpendicular to segment, 
            Vector2 perp = Perp(a);

            return LineIntersection(a, b, perp, Vector2.Dot(perp, disk.position)).Value;
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
