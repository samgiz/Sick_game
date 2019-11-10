using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace Game1000
{
    public class Disk : GameObject
    {
        public readonly float mass, radius;
        public Vector2 force;
        public bool ifCollides;

        private Vector2 velocity;
        private readonly float maxSpeed;

        private const float gravityConst = 100, frictionCoeff = 0.1f, dragCoeff = 0.5f, airDensity = 0.5f;

        public Disk(Vector2 position, Vector2 velocity, float radius, Color color, float mass = float.PositiveInfinity)
            : base(position, color, "disk", 2 * radius)
        {
            this.radius = radius;
            this.velocity = velocity;
            this.mass = mass;
            ifCollides = true;
            force = Vector2.Zero;
            maxSpeed = 500000 / mass;
        }

        public void Update(float elapsed)
        {
            velocity += force / mass;
            force = Vector2.Zero;
            position += velocity * elapsed;
        }

        public void UpdateWithDrag(float elapsed)
        {
            velocity += force / mass;
            force = Vector2.Zero;
            ApplyMaxSpeed(elapsed);
            position += velocity * elapsed;
        }

        private void ApplyDrag(float elapsed)
        {
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
        }

        private void ApplyMaxSpeed(float elapsed)
        {
            if (velocity.Length() > maxSpeed)
            {
                velocity.Normalize();
                velocity *= maxSpeed;
            }
        }

        //public void DeterministicMove(float elapsed)
        //{
        //    if ((velocity + accel * elapsed).Length() > maxSpeed)
        //    {
        //        float elapsed1 = 0;
        //        if (velocity.Length() < maxSpeed)
        //        {
        //            float dotProduct = Vector2.Dot(velocity, accel);
        //            elapsed1 = (-dotProduct + (float)Math.Sqrt(dotProduct * dotProduct - accel.LengthSquared() * (velocity.LengthSquared() - maxSpeed * maxSpeed))) / accel.LengthSquared();
        //            position += velocity * elapsed1 + accel * elapsed1 * elapsed1 / 2;
        //            velocity += accel * elapsed1;
        //        }
        //        velocity.Normalize();
        //        velocity *= maxSpeed;
        //        position += velocity * (elapsed - elapsed1);
        //    }
        //    else
        //    {
        //        position += velocity * elapsed + accel * elapsed * elapsed / 2;
        //        velocity += accel * elapsed;
        //    }
        //}

        public void Collide(Disk disk)
        {
            if (!IfIntersects(disk) || !ifCollides || !disk.ifCollides)
                return;

            Vector2 direction = position - disk.position;
            direction.Normalize();

            if (mass == float.PositiveInfinity)
            {
                if (disk.mass != float.PositiveInfinity)
                {
                    disk.position = position - direction * (disk.radius + radius);
                    disk.velocity -= 2 * Vector2.Dot(disk.velocity - velocity, direction) * direction;
                }
            }
            else
            {
                if (disk.mass == float.PositiveInfinity)
                {
                    position = disk.position + direction * (disk.radius + radius);
                    velocity -= 2 * Vector2.Dot(velocity - disk.velocity, direction) * direction;
                }
                else
                {
                    Vector2 center = (position * mass + disk.position * disk.mass) / (mass + disk.mass);
                    float reducedMass = mass * disk.mass / (mass + disk.mass);
                    Vector2 positionExchange = reducedMass * (radius + disk.radius) * direction;
                    position = center + positionExchange / mass;
                    disk.position = center - positionExchange / disk.mass;
                    float impSpeed = Vector2.Dot(direction, velocity), diskImpSpeed = Vector2.Dot(direction, disk.velocity);
                    Vector2 momentumExchange = reducedMass * 2 * (diskImpSpeed - impSpeed) * direction;
                    force += momentumExchange;
                    disk.force -= momentumExchange;
                }
            }

            Collide();
            disk.Collide();
        }

        public virtual void Collide() { }

        public bool IfIntersects(Disk disk)
            => Vector2.Distance(position, disk.position) < radius + disk.radius;

        public void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch, ifCollides);
        }
    }
}
