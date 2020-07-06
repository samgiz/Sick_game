using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Drawing;

namespace Game1000
{
    public class Arena
    {
        public float radius;
        private Vector2 origin;
        private float scale;
        private readonly float decreasePerSec;
        private readonly Texture2D image;
        private Color color;
        public Arena(Color color)
        {
            this.color = color;
            radius = 540;
            decreasePerSec = 10;
            image = C.Content.Load<Texture2D>("big disk");
            origin = new Vector2(image.Width * 0.5f, image.Height * 0.5f);
            scale = 2 * radius / image.Width;
        }

        public void Update(float elapsed)
        {
            // use line below if we want the Arena to shrink
            // radius -= decreasePerSec * elapsed;
            if (radius < 0)
                radius = 0;
            scale = 2 * radius / image.Width;
        }

        public void Draw()
        {
            C.drawer.DrawDisk(new { 
                position = Vector2.Zero,
                color = color,
                origin = origin,
                scale = scale 
            });
        }

        // Check whether a point is inside the arena
        // public bool InBounds(Vector2 v){
        //     return true;
        // }
        // Check whether a game object is in bounds
        public bool InBounds(Disk go){
            return go.position.Length() - go.radius < radius;
        }

        public void AssignPositions(List<Player> ps){
            for(int i = 0; i < ps.Count; i++){
                // [TODO: ]Make this assign positions in a circle
                ps[i].position = new Vector2(-500+i*200, 0);
                // Override players' speed to avoid strange behaviour
                ps[i].velocity = Vector2.Zero;
            }
        }
    }
}
