using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Game1000
{
    public class Spring
    {
        private readonly Disk disk1, disk2;
        private Segment segment;
        private readonly float length, springConst;//, maxForce;
        private Color color;

        public Spring(Disk disk1, Disk disk2, float length, Color color)
        {
            this.disk1 = disk1;
            this.disk2 = disk2;
            this.length = length;
            this.color = color;
            springConst = 100000;
            //maxForce = 10000;
        }

        public void Update(float elapsed)
        {
            Vector2 direction = disk1.position - disk2.position;
            direction.Normalize();
            Vector2 force = direction * (Vector2.Distance(disk1.position, disk2.position) - length) * springConst * elapsed;
            //if (force.Length() > maxForce)
            //{
            //    force.Normalize();
            //    force *= maxForce;
            //}
            disk1.force -= force;
            disk2.force += force;
        }

        //public void Collide(Disk disk)
        //{
        //    disk1.Collide(disk);
        //    disk2.Collide(disk);
        //}

        public void Draw()
        {
            segment = new Segment(disk1.position, disk2.position, disk1.radius * 2, color);
            segment.Draw();
            disk1.Draw();
            disk2.Draw();
        }
    }
}
