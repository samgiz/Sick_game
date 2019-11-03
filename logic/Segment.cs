using Microsoft.Xna.Framework;

namespace Game1000
{
    public class Segment
    {
        // line is a * x + b * y = c
        protected float a, b, c;
        protected DiskObstacle diskObst1, diskObst2;
        protected Color color;

        protected Segment(Vector2 pos1, Vector2 pos2, Color color)
        {
            diskObst1 = new DiskObstacle(pos1, 0, Color.Transparent);
            diskObst2 = new DiskObstacle(pos2, 0, Color.Transparent);
            this.color = color;
        }

        //public static bool IfCollides(Disk disk, Segment segment)
        //{

        //}

        public static bool IfEndpointsCollide(Disk disk, Segment segment)
        {
            return Disk.IfIntersects(disk, segment.diskObst1) || Disk.IfIntersects(disk, segment.diskObst2);
        }

        public static void Collide(Player player, Segment segment)
        {

        }

        public static void CollideEndpoints(Player player, Segment segment)
        {
            DiskObstacle.Collide(player, segment.diskObst1);
            DiskObstacle.Collide(player, segment.diskObst2);
        }

        public static void CollideEndpoints(Bullet bullet, Segment segment)
        {
            DiskObstacle.Collide(bullet, segment.diskObst1);
            DiskObstacle.Collide(bullet, segment.diskObst2);
        }
    }
}
