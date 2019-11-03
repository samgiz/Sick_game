using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace Game1000
{
    public class Player : Disk
    {
        public Vector2 velocity;
        public readonly float mass;
        public bool isInvisible, wasInvisible;
        private readonly float maxMomentum, maxSpeed, force, invisibilityWait, visibilityWait, bulletWait, bulletSpeed;
        private const float gravityConst = 100, frictionCoeff = 0.1f, dragCoeff = 0.5f, airDensity = 0.5f;
        private float tillInvisibility, tillBullet;
        private readonly bool canBeInvisible, canShoot;
        private Controls controls;

        // Default contructor
        public Player(Controls controls, Vector2 position, float radius, Color color, bool canBeInvisible, bool canShoot)
            : base(position, radius, color)
        {
            velocity = Vector2.Zero;
            this.controls = controls;

            maxMomentum = 500000;
            force = 500000;
            mass = radius * radius;
            maxSpeed = maxMomentum / mass;
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

        // Second constructor in case we don't want to set the coordinates during initialization
        public Player(Controls controls, float radius, Color color, bool canBeInvisible, bool canShoot)
            : this(controls, Vector2.Zero, radius, color, canBeInvisible, canShoot){}

        public void Update(float elapsed, List<Bullet> bullets)
        {
            wasInvisible = isInvisible;
            tillInvisibility -= elapsed;
            if (tillInvisibility <= visibilityWait)
            {
                isInvisible = false;
            }
            if (canBeInvisible && controls.mouseRight && tillInvisibility <= 0)
            {
                isInvisible = true;
                tillInvisibility = invisibilityWait;
            }

            tillBullet -= elapsed;
            if (!isInvisible && canShoot && controls.mouseLeft && tillBullet <= 0)
            {
                Vector2 mouseDir = controls.mousePos - position;
                if (mouseDir == Vector2.Zero)
                    mouseDir = new Vector2(1, 0);
                else
                    mouseDir.Normalize();
                bullets.Add(new Bullet(position + mouseDir * (radius + Bullet.radius), mouseDir * bulletSpeed));
                tillBullet = bulletWait;
            }

            Vector2 accelDir = Vector2.Zero;

            if (controls.up)
                accelDir.Y--;
            if (controls.down)
                accelDir.Y++;
            if (controls.left)
                accelDir.X--;
            if (controls.right)
                accelDir.X++;

            if (accelDir != Vector2.Zero)
                accelDir.Normalize();

            Vector2 accel = accelDir * force / mass;

            //Move(elapsed, accel);
            DeterministicMove(elapsed, accel);
        }

        public void DeterministicMove(float elapsed, Vector2 accel)
        {
            if ((velocity + accel * elapsed).Length() > maxSpeed)
            {
                float elapsed1 = 0;
                if (velocity.Length() < maxSpeed)
                {
                    float dotProduct = Vector2.Dot(velocity, accel);
                    elapsed1 = (-dotProduct + (float)Math.Sqrt(dotProduct * dotProduct - accel.LengthSquared() * (velocity.LengthSquared() - maxSpeed * maxSpeed))) / accel.LengthSquared();
                    position += velocity * elapsed1 + accel * elapsed1 * elapsed1 / 2;
                    velocity += accel * elapsed1;
                }
                velocity.Normalize();
                velocity *= maxSpeed;
                position += velocity * (elapsed - elapsed1);
            }
            else
            {
                position += velocity * elapsed + accel * elapsed * elapsed / 2;
                velocity += accel * elapsed;
            }

            //if (velocity.Length() > maxMomentum / mass)
            //{
            //    velocity.Normalize();
            //    velocity *= maxMomentum / mass;
            //}
        }

        public void Move(float elapsed, Vector2 accel)
        {
            velocity += accel * elapsed;
            Vector2 direction = velocity;
            if (direction != Vector2.Zero)
                direction.Normalize();
            float speed = velocity.Length(),
                  maxFriction = mass * gravityConst * frictionCoeff,
                  drag = airDensity * speed * speed * dragCoeff * radius,
                  maxVelReduction = (maxFriction + drag) / mass * elapsed;
            if (speed > maxVelReduction)
                velocity -= maxVelReduction * direction;
            else
                velocity = Vector2.Zero;
            position += velocity * elapsed;
        }

        public static void Collide(Player player1, Player player2)
        {
            if (!Disk.IfIntersects(player1, player2) || player1.isInvisible || player2.isInvisible)
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
