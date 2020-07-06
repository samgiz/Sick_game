// the basic class for drawing a disk
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Game1000;
using System;
namespace Drawing{
    public class NormalDrawer : Drawer {
        Texture2D bigDiskImage, diskImage, pixelImage;
        Vector2 diskOrigin, pixelOrigin;

        public NormalDrawer(){
            bigDiskImage = C.LoadImage("big disk");
            diskImage = C.LoadImage("disk");
            pixelImage = C.LoadImage("pixel");
            diskOrigin = C.ImageOrigin(diskImage);
            pixelOrigin = C.ImageOrigin(pixelImage);
        }
        public void DrawDisk(dynamic p){
            try{
                C.spriteBatch.Draw(diskImage, p.position, null, p.color, 0, p.origin, p.scale, SpriteEffects.None, 0);
            } catch {
                throw new System.Exception("Disk drawer did not receive the required properties");
            }
        }
        public void DrawSegment(dynamic p){
            try{
                Vector2 midPos = (p.vert1 + p.vert2) / 2;
                C.spriteBatch.Draw(pixelImage, midPos, null, p.color, p.angle, pixelOrigin, p.pixelScale, SpriteEffects.None, 0);
                C.spriteBatch.Draw(diskImage, p.vert1, null, p.color, 0, diskOrigin, p.diskScale, SpriteEffects.None, 0);
                C.spriteBatch.Draw(diskImage, p.vert2, null, p.color, 0, diskOrigin, p.diskScale, SpriteEffects.None, 0);
            } catch {
                throw new System.Exception("Segment drawer did not receive the required properties");
            }
        }
        // [TODO: refactor code so this method is not needed]
        public void DrawBigDisk(dynamic p){
            try{
                C.spriteBatch.Draw(bigDiskImage, p.position, null, p.color, 0, p.origin, p.scale, SpriteEffects.None, 0);
            } catch {
                throw new System.Exception("Disk drawer did not receive the required properties");
            }
        }
    }
}