using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace Game1000
{
    public class Player : GameObject
    {
        public readonly float mass;
        private readonly float maxMomentum, force, wait, bulletSpeed;
        private float timeTillShot;
        private Keys up, down, left, right;

        public Player(Vector2 position, float radius, Keys up, Keys down, Keys left, Keys right, Color color)
            : base(position, Vector2.Zero, radius, color)
        {
            this.up = up;
            this.down = down;
            this.left = left;
            this.right = right;
            maxMomentum = 500000;
            force = 500000;
            wait = 0.5f;
            bulletSpeed = 200;
            timeTillShot = 0;
            mass = radius * radius;
            isAlive = true;
            velocity = Vector2.Zero;
        }

        public void Update(float elapsed, float arenaRadius, List<Bullet> bullets)
        {
            timeTillShot -= elapsed;
            if (C.IsKeyDown(up))
                velocity.Y -= force * elapsed / mass;
            if (C.IsKeyDown(down))
                velocity.Y += force * elapsed / mass;
            if (C.IsKeyDown(left))
                velocity.X -= force * elapsed / mass;
            if (C.IsKeyDown(right))
                velocity.X += force * elapsed / mass;

            if (C.IsLeftButtonPressed() && timeTillShot <= 0)
            {
                Vector2 mouseDir = C.MousePos() - position;
                if (mouseDir == Vector2.Zero)
                    mouseDir = new Vector2(1, 0);
                else
                    mouseDir.Normalize();
                bullets.Add(new Bullet(position + mouseDir * (radius + Bullet.radius), mouseDir * bulletSpeed));
                timeTillShot = wait;
            }


            if (velocity.Length() > maxMomentum / mass)
            {
                velocity.Normalize();
                velocity *= maxMomentum / mass;
            }

            base.Update(elapsed, arenaRadius);
        }

        public static void Collide(Player player1, Player player2)
        {
            if (!GameObject.IfIntersects(player1, player2))
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
