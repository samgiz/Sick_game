using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace Game1000
{
    public class Arena
    {
        public float radius;
        private Vector2 origin;
        private float scale, decreasePerSec;
        private Texture2D image;
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
            // radius -= decreasePerSec * elapsed;
            if (radius < 0)
                radius = 0;
            scale = 2 * radius / image.Width;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(image, Vector2.Zero, null, color, 0, origin, scale, SpriteEffects.None, 0);
        }

        // Check whether a point is inside the arena
        public bool InBounds(Vector2 v){
            return true;
        }
        // Check whether a game object is in bounds
        public bool InBounds(GameObject go){
            return true;
        }

        public void AssignPositions(List<Player> ps){
            for(int i = 0; i < ps.Count; i++){
                // Make this assign positions in a circle
                ps[i].position = new Vector2(-500+i*200, 0);
                // Override players' speed to avoid strange behaviour
            }
        }
    }
}
