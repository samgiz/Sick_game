using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Game1000
{
    public class Player : GameObject
    {
        public readonly float mass;
        private const float maxMomentum = 500000, force = 500000;
        private Keys up, down, left, right;

        public Player(Vector2 position, float radius, Keys up, Keys down, Keys left, Keys right, Color color, ContentManager Content)
            : base(position, Vector2.Zero, radius, color, Content)
        {
            this.up = up;
            this.down = down;
            this.left = left;
            this.right = right;
            mass = radius * radius;
            isAlive = true;
            velocity = Vector2.Zero;
        }

        public new void Update(float elapsed, float arenaRadius)
        {
            KeyboardState keyState = Keyboard.GetState();
            if (keyState.IsKeyDown(up))
                velocity.Y -= force * elapsed / mass;
            if (keyState.IsKeyDown(down))
                velocity.Y += force * elapsed / mass;
            if (keyState.IsKeyDown(left))
                velocity.X -= force * elapsed / mass;
            if (keyState.IsKeyDown(right))
                velocity.X += force * elapsed / mass;

            if (velocity.Length() > maxMomentum / mass)
            {
                velocity.Normalize();
                velocity *= maxMomentum / mass;
            }

            base.Update(elapsed, arenaRadius);
        }

        public static void Collide(Player player1, Player player2)
        {
            if (Vector2.Distance(player1.position, player2.position) >= player1.radius + player2.radius)
                return;
            Vector2 center = (player1.position * player1.mass + player2.position * player2.mass) / (player1.mass + player2.mass), direction = player1.position - player2.position;
            direction.Normalize();
            float reducedMass = player1.mass * player2.mass / (player1.mass + player2.mass);
            Vector2 positionExchange = reducedMass * (player1.radius + player2.radius) * direction;
            player1.position = center + positionExchange / player1.mass;
            player2.position = center - positionExchange / player2.mass;
            float importnantSpeed1 = Vector2.Dot(direction, player1.velocity), importantSpeed2 = Vector2.Dot(direction, player2.velocity);
            Vector2 momentumExchange = reducedMass * 2 * (importantSpeed2 - importnantSpeed1) * direction;
            player1.velocity += momentumExchange / player1.mass;
            player2.velocity -= momentumExchange / player2.mass;
        }
    }
}
