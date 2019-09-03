using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Game1000
{
    public class Player
    {
        public const float radius = 32;
        public Vector2 position, velocity;
        public bool isAlive;
        private const float maxSpeed = 500, accel = 500;
        private Vector2 origin;
        private Texture2D image;
        private float scale;
        private Keys up, down, left, right;
        private Color color;

        public Player(Vector2 position, Keys up, Keys down, Keys left, Keys right, Color color, ContentManager Content)
        {
            this.position = position;
            this.up = up;
            this.down = down;
            this.left = left;
            this.right = right;
            this.color = color;
            isAlive = true;
            velocity = Vector2.Zero;
            image = Content.Load<Texture2D>("disk");
            origin = new Vector2(image.Width * 0.5f, image.Height * 0.5f);
            scale = 2 * radius / image.Width;
        }

        public void Update(float elapsed, float arenaRadius)
        {
            KeyboardState keyState = Keyboard.GetState();
            if (keyState.IsKeyDown(up))
                velocity.Y -= accel * elapsed;
            if (keyState.IsKeyDown(down))
                velocity.Y += accel * elapsed;
            if (keyState.IsKeyDown(left))
                velocity.X -= accel * elapsed;
            if (keyState.IsKeyDown(right))
                velocity.X += accel * elapsed;

            if (velocity.Length() > maxSpeed)
            {
                velocity.Normalize();
                velocity *= maxSpeed;
            }

            position += velocity * elapsed;

            if (position.Length() > arenaRadius + radius)
                isAlive = false;
        }

        public static void Collide(Player player1, Player player2)
        {
            if (Vector2.Distance(player1.position, player2.position) >= 2 * radius)
                return;
            Vector2 center = (player1.position + player2.position) / 2, direction1 = player1.position - center, direction2 = player2.position - center;
            direction1.Normalize();
            direction2.Normalize();
            player1.position = center + direction1 * radius;
            player2.position = center + direction2 * radius;
            float importnantSpeed1 = -Vector2.Dot(direction1, player1.velocity), importantSpeed2 = -Vector2.Dot(direction2, player2.velocity);
            player1.velocity += (importnantSpeed1 + importantSpeed2) * direction1;
            player2.velocity += (importnantSpeed1 + importantSpeed2) * direction2;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(image, position, null, color, 0, origin, scale, SpriteEffects.None, 0);
        }
    }
}
