using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Game1000
{
    // Custom Controls object that always returns the current state of the local mouse and keyboard
    // Setting the controls has no effect
    public class LocalControls : Controls
    {
        private Keys upKey, downKey, leftKey, rightKey;
        public LocalControls(Keys up = Keys.W, Keys left = Keys.A, Keys down = Keys.S, Keys right = Keys.D){
            this.upKey = up;
            this.leftKey = left;
            this.downKey = down;
            this.rightKey = right;
        }
        public override bool up {get {return Keyboard.GetState().IsKeyDown(upKey);} }
        public override bool down {get {return Keyboard.GetState().IsKeyDown(downKey);} }
        public override bool right {get {return Keyboard.GetState().IsKeyDown(rightKey);} }
        public override bool left {get {return Keyboard.GetState().IsKeyDown(leftKey);} }
        // TODO: there is no need to keep the MousePos function inside the C class at this point
        // Move it here
        public override Vector2 mousePos {get {return C.MousePos(Mouse.GetState());} }
        public override bool mouseRight {get {return Mouse.GetState().RightButton == ButtonState.Pressed;} }
        public override bool mouseLeft {get {return Mouse.GetState().LeftButton == ButtonState.Pressed;} }
    }
}
