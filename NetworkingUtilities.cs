using Lidgren.Network;
using Game1000;
using Microsoft.Xna.Framework;

namespace Networking{
    class Utilities{
        public static void EncodePlayer(NetOutgoingMessage msg, Player p){
            msg.Write(p.position.X);
            msg.Write(p.position.Y);
            msg.Write(p.velocity.X);
            msg.Write(p.velocity.Y);
        }
        public static void DecodePlayer(NetIncomingMessage msg, Player p){
            int px = msg.ReadInt32();
            int py = msg.ReadInt32();
            int vx = msg.ReadInt32();
            int vy = msg.ReadInt32();
            p.position = new Vector2(px, py);
            p.velocity = new Vector2(vx, vy);
        }
        public static void EncodeControls(NetOutgoingMessage msg, Controls c){
            byte b = 0;
            b ^= (byte)((c.up ? 1 : 0) << 0);
            b ^= (byte)((c.left ? 1 : 0) << 1);
            b ^= (byte)((c.down ? 1 : 0) << 2);
            b ^= (byte)((c.right ? 1 : 0) << 3);
            b ^= (byte)((c.mouseLeft ? 1 : 0) << 4);
            b ^= (byte)((c.mouseRight ? 1 : 0) << 5);
            msg.Write(b);
            if(c.mouseLeft || c.mouseRight){
                // Send mouse coordinates as well
                msg.Write(c.mousePos.X);
                msg.Write(c.mousePos.Y);
            }
        }
        public static void DecodeControls(NetIncomingMessage msg, Controls c){
            byte b = msg.ReadByte();
            c.up = (b & 1<<0) == 1 ? true : false;
            c.left = (b & 1<<1) == 1 ? true : false;
            c.down = (b & 1<<2) == 1 ? true : false;
            c.right = (b & 1<<3) == 1 ? true : false;
            c.mouseLeft = (b & 1<<4) == 1 ? true : false;
            c.mouseRight = (b & 1<<5) == 1 ? true : false;
            if(c.mouseLeft || c.mouseRight){
                // Send mouse coordinates as well
                int x = msg.ReadInt32();
                int y = msg.ReadInt32();
                c.mousePos = new Vector2(x, y);
            }
        }
        
    }
}