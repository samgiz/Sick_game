using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Drawing;

namespace Game1000
{
    public class StandardGraphicsHandler : GraphicsHandler
    {
        GameState game;
        Camera camera;
        private Texture2D circle_image;

        private Vector2 origin;
        public StandardGraphicsHandler(GameState game) : base(game)
        {
            C.drawer = new NormalDrawer();
            this.game = game;
            camera = new Camera();
            circle_image = C.Content.Load<Texture2D>("big disk");
            
            origin = new Vector2(circle_image.Width * 0.5f, circle_image.Height * 0.5f);
        }

        // global drawing logic goes here
        public override void Draw()
        {
            // map to the correct coordinate system
            camera.BeginDraw(C.spriteBatch);

            // draw the arena
            game.arena.Draw();

            // draw the players
            foreach (Player player in game.players)
                player.Draw();

            // draw the bullets
            foreach (Bullet bullet in game.bullets)
                bullet.Draw();

            // draw the obstacles
            foreach (Obstacle obstacle in game.obstacles)
                obstacle.Draw();

            // draw anything else
            // rubberBand.Draw(spriteBatch);

            C.spriteBatch.End();
        }
    }
}
