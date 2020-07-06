using Microsoft.Xna.Framework;
using System;

namespace Game1000
{
    // General controls class to keep track of a players keyboard and mouse state
    // By default the Mouse and Keyboard state can be changed manually to allow remote execution of
    // [TODO: fix annoying warnings]
    public class Controls
    {
        public virtual bool up {get; set;} = false;
        public virtual bool down {get; set;} = false;
        public virtual bool right {get; set;} = false;
        public virtual bool left {get; set;} = false;
        public virtual Vector2 mousePos {get; set;} = Vector2.Zero;
        public virtual bool mouseRight {get; set;} = false;
        public virtual bool mouseLeft {get; set;} = false;
        public static bool operator ==(Controls lhs, Controls rhs)
        {
            // Check if this is the same object
            if(Object.ReferenceEquals(lhs, rhs))
                return true;
            // Check if one of the elements is null
            if (Object.ReferenceEquals(lhs, null) || Object.ReferenceEquals(rhs, null))
                return false;
            // Check if all button states match
            return lhs.up == rhs.up &&
                   lhs.left == rhs.left &&
                   lhs.down == rhs.down &&
                   lhs.right == rhs.right &&
                   lhs.mouseLeft == rhs.mouseLeft &&
                   lhs.mouseRight == rhs.mouseRight;
        }
        public static bool operator !=(Controls lhs, Controls rhs){
            return !(lhs == rhs);
        }
        public void AssignValues(Controls c){
            up = c.up;
            left = c.left;
            down = c.down;
            right = c.right;
            mouseLeft = c.mouseLeft;
            mouseRight = c.mouseRight;
            mousePos = c.mousePos;
        }
    }
}
