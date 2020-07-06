using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace Game1000
{
    public class RubberBand : Obstacle
    {
        private readonly List<Disk> disks;
        private readonly List<Spring> springs;

        public RubberBand(Vector2 vert1, Vector2 vert2, float width, float mass, int pointCount, Color color)
        {
            disks = new List<Disk>(pointCount);
            for (int i = 0; i < pointCount; i++)
            {
                float diskMass = mass / (pointCount - 2);
                if (i == 0 || i == pointCount - 1)
                    diskMass = float.PositiveInfinity;
                disks.Add(new Disk((vert1 * (pointCount - 1 - i) + vert2 * i) / (pointCount - 1), Vector2.Zero, width / 2, color, diskMass));
            }

            springs = new List<Spring>(pointCount - 1);
            for (int i = 0; i < pointCount - 1; i++)
            {
                springs.Add(new Spring(disks[i], disks[i + 1], Vector2.Distance(vert1, vert2) / (pointCount - 1) / 3, color));
            }
        }

        public void Update(float elapsed)
        {
            foreach (Spring spring in springs)
                spring.Update(elapsed);

            //foreach (Disk disk in disks)
            //    disk.Update(elapsed);

            foreach (Disk disk in disks)
                disk.UpdateWithDrag(elapsed);
        }

        public void Collide(Disk disk)
        {
            int colCount = 0;
            foreach (Disk d in disks)
            {
                if (d.IfIntersects(disk))
                {
                    colCount++;
                }
                //d.Collide(disk);
            }
            foreach (Disk d in disks)
                d.Collide(disk, colCount);
        }

        public void Draw()
        {
            foreach (Spring spring in springs)
                spring.Draw();
        }
    }
}
