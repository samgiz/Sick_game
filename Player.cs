using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace Game1000
{
    public class Player : GameObject
    {
        public readonly float mass;
        public bool isInvisible, wasInvisible;
        private readonly float /*maxMomentum, */force, invisibilityWait, visibilityWait, bulletWait, bulletSpeed;
        private const float gravityConst = 100, frictionCoeff = 0.1f, dragCoeff = 0.5f, airDensity = 0.5f;
        private float tillInvisibility, tillBullet;
        private readonly Keys up, down, left, right;
        private readonly bool canBeInvisible, canShoot;

        public Player(Vector2 position, float radius, Keys up, Keys down, Keys left, Keys right, Color color, bool canBeInvisible, bool canShoot)
            : base(position, Vector2.Zero, radius, color)
        {
            this.up = up;
            this.down = down;
            this.left = left;
            this.right = right;

            //maxMomentum = 500000;
            force = 500000;
            mass = radius * radius;
            velocity = Vector2.Zero;

            this.canBeInvisible = canBeInvisible;
            invisibilityWait = 5;
            visibilityWait = 2;
            tillInvisibility = 0;

            this.canShoot = canShoot;
            bulletWait = 0.5f;
            bulletSpeed = 200;
            tillBullet = 0;
            isInvisible = false;
            
            isAlive = true;
        }

        public void Update(float elapsed, float arenaRadius, List<Bullet> bullets)
        {
            wasInvisible = isInvisible;
            tillInvisibility -= elapsed;
            if (tillInvisibility <= visibilityWait)
            {
                isInvisible = false;
            }
            if (canBeInvisible && C.IsRightButtonPressed() && tillInvisibility <= 0)
            {
                isInvisible = true;
                tillInvisibility = invisibilityWait;
            }

            tillBullet -= elapsed;
            if (!isInvisible && canShoot && C.IsLeftButtonPressed() && tillBullet <= 0)
            {
                Vector2 mouseDir = C.MousePos() - position;
                if (mouseDir == Vector2.Zero)
                    mouseDir = new Vector2(1, 0);
                else
                    mouseDir.Normalize();
                bullets.Add(new Bullet(position + mouseDir * (radius + Bullet.radius), mouseDir * bulletSpeed));
                tillBullet = bulletWait;
            }

            if (C.IsKeyDown(up))
                velocity.Y -= force * elapsed / mass;
            if (C.IsKeyDown(down))
                velocity.Y += force * elapsed / mass;
            if (C.IsKeyDown(left))
                velocity.X -= force * elapsed / mass;
            if (C.IsKeyDown(right))
                velocity.X += force * elapsed / mass;

            //if (velocity.Length() > maxMomentum / mass)
            //{
            //    velocity.Normalize();
            //    velocity *= maxMomentum / mass;
            //}

            Vector2 direction = velocity;
            direction.Normalize();
            float speed = velocity.Length(),
                  maxFriction = mass * gravityConst * frictionCoeff,
                  drag = airDensity * speed * speed * dragCoeff * radius,
                  maxVelReduction = (maxFriction + drag) / mass * elapsed;
            if (speed > maxVelReduction)
                velocity -= maxVelReduction * direction;
            else
                velocity = Vector2.Zero;

            base.Update(elapsed, arenaRadius);
        }

        public static void Collide(Player player1, Player player2)
        {
            if (!GameObject.IfIntersects(player1, player2) || player1.isInvisible || player2.isInvisible)
                return;
            if (player1.wasInvisible)
            {
                player1.isAlive = false;
                if (player2.wasInvisible)
                    player2.isAlive = false;
                return;
            }
            if (player2.wasInvisible)
            {
                player2.isAlive = false;
                return;
            }
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

        public void Draw(SpriteBatch spriteBatch)
        {
            Draw(spriteBatch, isInvisible);
        }
    }
}
