using Microsoft.Xna.Framework;

namespace Game1000
{
    // General controls class to keep track of a players keyboard and mouse state
    // By default the Mouse and Keyboard state can be changed manually to allow remote execution of
    public class Controls
    {
        public virtual bool up {get; set;} = false;
        public virtual bool down {get; set;} = false;
        public virtual bool right {get; set;} = false;
        public virtual bool left {get; set;} = false;
        public virtual Vector2 mousePos {get; set;} = Vector2.Zero;
        public virtual bool mouseRight {get; set;} = false;
        public virtual bool mouseLeft {get; set;} = false;
    }
}
