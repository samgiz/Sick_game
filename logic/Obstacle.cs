using Microsoft.Xna.Framework.Graphics;
using Drawing;

namespace Game1000
{
    public interface Obstacle
    {
        void Collide(Disk disk);

        void Draw();
    }
}
