// the basic class for drawing a disk
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Game1000;

namespace Drawing{
    public class NormalDrawer : Drawer {
        Texture2D bigDiskImage, diskImage, pixelImage;
        Vector2 bigDiskOrigin, diskOrigin, pixelOrigin;

        public NormalDrawer(){
            bigDiskImage = C.LoadImage("big disk");
            diskImage = C.LoadImage("disk");
            pixelImage = C.LoadImage("pixel");
            diskOrigin = C.ImageOrigin(diskImage);
            pixelOrigin = C.ImageOrigin(pixelImage);
            bigDiskOrigin = C.ImageOrigin(bigDiskImage);
        }
        public void DrawDisk(dynamic p){
            try{
                // does not work without the line below
                float? width = p.width;
                // [TODO: make this less ugly]
                float scale;
                if (width.HasValue)
                    scale = (float)width / diskImage.Width;
                else
                    scale = 1;
                C.spriteBatch.Draw(diskImage, p.position, null, p.color, 0, diskOrigin, scale, SpriteEffects.None, 0);
            } catch {
                throw new System.Exception("Disk drawer did not receive the required properties");
            }
        }
        public void DrawSegment(dynamic p){
            try{
                Vector2 midPos = (p.vert1 + p.vert2) / 2;
                float diskScale = 2 * p.radius / diskImage.Width;
                Vector2 pixelScale = new Vector2(p.radius*2, Vector2.Distance(p.vert1, p.vert2));
                C.spriteBatch.Draw(pixelImage, midPos, null, p.color, p.angle, pixelOrigin, pixelScale, SpriteEffects.None, 0);
                C.spriteBatch.Draw(diskImage, p.vert1, null, p.color, 0, diskOrigin, diskScale, SpriteEffects.None, 0);
                C.spriteBatch.Draw(diskImage, p.vert2, null, p.color, 0, diskOrigin, diskScale, SpriteEffects.None, 0);
            } catch {
                throw new System.Exception("Segment drawer did not receive the required properties");
            }
        }
        // [TODO: refactor code so this method is not needed]
        public void DrawBigDisk(dynamic p){
            try{
                // [TODO: find a way to not have this be so ugly]
                float scale = 2 * p.radius / bigDiskImage.Width;
                C.spriteBatch.Draw(bigDiskImage, p.position, null, p.color, 0, bigDiskOrigin, scale, SpriteEffects.None, 0);
            } catch {
                throw new System.Exception("Disk drawer did not receive the required properties");
            }
        }
    }
}