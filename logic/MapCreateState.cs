using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.IO;

namespace Game1000
{
    public class MapCreateState
    {
        public bool isReady, ifWrites;
        private readonly Controls controls;
        private readonly List<Obstacle> obstacles, tempObst;
        private readonly List<Vector2> vertices;
        private bool wasLeftPressed, wasRightPressed, wasD, wasP;
        private StreamWriter writeFile;
        private StreamReader readFile;
        private readonly Color diskCol, lineCol, rubCol;

        String mapPath = AppDomain.CurrentDomain.BaseDirectory + "../../../../map.txt";

        // If you want lo load the map from file, press Enter
        // If not, press w to enable changing the map
        // Press to create DiskObst
        // Press P to create a point (it will be automatically joined to the last created point
        // Press F to finish the Polygon (otherwise your points won't be saved)
        // When done drawing, press Enter

        public MapCreateState(Controls controls, bool ifCreate)
        {
            this.controls = controls;
            obstacles = new List<Obstacle>();
            tempObst = new List<Obstacle>();
            vertices = new List<Vector2>();
            wasLeftPressed = false;
            wasRightPressed = false;
            wasD = false;
            wasP = false;
            isReady = false;
            isReady = !ifCreate;
            diskCol = Color.Yellow;
            lineCol = Color.Blue;
            rubCol = Color.Orange;
        }

        public List<Obstacle> Read()
        {
            if (ifWrites)
            {
                return obstacles;
            }
            List<Obstacle> obst = new List<Obstacle>();
            Console.WriteLine(AppDomain.CurrentDomain.BaseDirectory);
            readFile = new StreamReader(mapPath);

            while (true)
            {
                string line;
                line = readFile.ReadLine();
                string[] parts = line.Split(' ');
                if (parts[0] == "E")
                    break;
                if (parts[0] == "D")
                {
                    Vector2 pos = new Vector2(Convert.ToInt32(parts[1]), Convert.ToInt32(parts[2]));
                    float radius = Convert.ToInt32(parts[3]);
                    obst.Add(new DiskObst(pos, radius, diskCol));
                }
                if (parts[0] == "P")
                {
                    List<Vector2> vertices = new List<Vector2>();
                    for (int i = 1; i < parts.Length - 1; i += 2)
                    {
                        vertices.Add(new Vector2(Convert.ToInt32(parts[i]), Convert.ToInt32(parts[i + 1])));
                    }
                    float lineWidth = Convert.ToInt32(parts[parts.Length - 1]);
                    obst.Add(new Polygon(vertices, lineWidth, lineCol));
                }
            }
            readFile.Close();
            return obst;
        }

        public void Write()
        {
            ifWrites = true;
            writeFile = new StreamWriter(mapPath);
        }

        public void Update()
        {
            KeyboardState keyState = Keyboard.GetState();
            Vector2 pos = controls.mousePos;

            if (!ifWrites && keyState.IsKeyDown(Keys.W))
            {
                Write();
            }

            if (keyState.IsKeyDown(Keys.Enter))
            {
                if (ifWrites)
                {
                    writeFile.WriteLine("E");
                    writeFile.Close();
                }
                isReady = true;
            }

            if (!ifWrites)
                return;

            if (!wasD && keyState.IsKeyDown(Keys.D))
            {
                obstacles.Add(new DiskObst(pos, 32, diskCol));
                writeFile.WriteLine("D " + (int)pos.X + " " + (int)pos.Y + " " + 32);
            }

            if (!wasP && keyState.IsKeyDown(Keys.P))
            {
                if (vertices.Count == 0)
                {
                    writeFile.Write("P ");
                }
                vertices.Add(pos);

                if (vertices.Count == 1)
                {
                    tempObst.Add(new DiskObst(pos, 4, lineCol));
                }
                else
                {
                    tempObst.Add(new Segment(vertices[vertices.Count - 2], vertices[vertices.Count - 1], 8, lineCol));
                }

                writeFile.Write((int)pos.X + " " + (int)pos.Y + " ");
            }

            if (keyState.IsKeyDown(Keys.F))
            {
                if (vertices.Count >= 2)
                {
                    obstacles.Add(new Polygon(vertices, 8, lineCol));
                    writeFile.WriteLine(8);
                }
                vertices.Clear();
            }
            wasLeftPressed = controls.mouseLeft;
            wasRightPressed = controls.mouseRight;
            wasD = keyState.IsKeyDown(Keys.D);
            wasP = keyState.IsKeyDown(Keys.P);
        }

        //public void IsReady(List<Obstacle> obst)
        //{
        //    if (isReady)
        //    {
        //        foreach (Obstacle obstacle in obstacles)
        //            obst.Add(obstacle);
        //    }
        //}

        public void Draw()
        {
            foreach (Obstacle obstacle in tempObst)
                obstacle.Draw();
            foreach (Obstacle obstacle in obstacles)
                obstacle.Draw();

        }
    }
}