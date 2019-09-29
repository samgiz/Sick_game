using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace Game1000
{
    // General controls class to keep track of a players keyboard and mouse state
    // By default the Mouse and Keyboard state can be changed manually to allow remote execution of
    public class Controls
    {
        public virtual MouseState MouseState {get; set;}
        public virtual KeyboardState KeyboardState {get; set;}
    }
}
