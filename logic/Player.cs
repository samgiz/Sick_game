using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace Game1000
{
    public class Player : Disk
    {
        protected bool wasInvisible;
        private readonly float power, invisibilityWait, visibilityWait, bulletWait, bulletSpeed;
        private float tillInvisibility, tillBullet;
        private readonly bool canBeInvisible, canShoot;
        private readonly Controls controls;

        // Default contructor
        public Player(Controls controls, Vector2 position, float radius, Color color, bool canBeInvisible, bool canShoot)
            : base(position, Vector2.Zero, radius, color, radius * radius)
        {
            this.controls = controls;

            power = 500000;

            this.canBeInvisible = canBeInvisible;
            invisibilityWait = 5;
            visibilityWait = 2;
            tillInvisibility = 0;

            this.canShoot = canShoot;
            bulletWait = 0.5f;
            bulletSpeed = 200;
            tillBullet = 0;
            
            isAlive = true;
        }

        // Second constructor in case we don't want to set the coordinates during initialization
        public Player(Controls controls, float radius, Color color, bool canBeInvisible, bool canShoot)
            : this(controls, Vector2.Zero, radius, color, canBeInvisible, canShoot){}

        public void Update(float elapsed, List<Bullet> bullets)
        {
            wasInvisible = !ifCollides;
            tillInvisibility -= elapsed;
            if (tillInvisibility <= visibilityWait)
            {
                ifCollides = true;
            }
            if (canBeInvisible && controls.mouseRight && tillInvisibility <= 0)
            {
                ifCollides = false;
                tillInvisibility = invisibilityWait;
            }

            tillBullet -= elapsed;
            if (ifCollides && canShoot && controls.mouseLeft && tillBullet <= 0)
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

            force += accelDir * power * elapsed;

            base.UpdateWithDrag(elapsed);
        }

        public override void Collide()
        {
            if (wasInvisible)
            {
                isAlive = false;
                return;
            }
        }
    }
}
