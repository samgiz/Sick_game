using Microsoft.Xna.Framework;

namespace Game1000
{
    // General controls class to keep track of a players keyboard and mouse state
    // By default the Mouse and Keyboard state can be changed manually to allow remote execution of
    public class Controls
    {
        public virtual bool up {get; set;}
        public virtual bool down {get; set;}
        public virtual bool right {get; set;}
        public virtual bool left {get; set;}
        public virtual Vector2 mousePos {get; set;}
        public virtual bool mouseRight {get; set;}
        public virtual bool mouseLeft {get; set;}
    }
}
