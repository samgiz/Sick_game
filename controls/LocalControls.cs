using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace Game1000
{
    // Custom Controls object that always returns the current state of the local mouse and keyboard
    // Setting the controls has no effect
    public class LocalControls : Controls
    {
        public override MouseState MouseState {get {return Mouse.GetState();} }
        public override KeyboardState KeyboardState {get {return Keyboard.GetState();} }
    }
}
