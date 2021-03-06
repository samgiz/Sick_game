﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

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
        }

        public void Update(float elapsed)
        {
            // use line below if we want the Arena to shrink
            // radius -= decreasePerSec * elapsed;
            if (radius < 0)
                radius = 0;
        }

        public void Draw()
        {
            C.drawer.DrawBigDisk(new { 
                position = Vector2.Zero,
                color,
                radius
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
