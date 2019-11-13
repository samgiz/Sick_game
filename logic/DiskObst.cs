using Microsoft.Xna.Framework;


namespace Game1000
{
    public class DiskObst : Disk, Obstacle
    {
        public DiskObst(Vector2 position, float radius, Color color)
            : base(position, Vector2.Zero, radius, color)
        { }

        public void Collide(Disk disk)
        {
            base.Collide(disk);
        }
    }
}
