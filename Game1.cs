using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Lidgren.Network;

namespace Game1000
{
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        GameState game;

        NetClient client;

        Dictionary<long, Controls> controls;
        Dictionary<long, Player> players;

        LocalControls localControls;
        Controls previousControls;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            graphics.PreferredBackBufferWidth = C.screenWidth;
            graphics.PreferredBackBufferHeight = C.screenHeight;
            graphics.IsFullScreen = true;
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            base.Initialize();

            // Initialize and start client
            NetPeerConfiguration config = new NetPeerConfiguration("sick_game");
            client = new NetClient(config);
            client.Start();

            // Connect to server
            client.Connect("localhost", 14242);
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            C.Content = Content;
            game = new GameState();
            controls = new Dictionary<long, Controls>();
            players = new Dictionary<long, Player>(); 

            // Default local controls, uses WASD
            localControls = new LocalControls();
            
            previousControls = new Controls();
        }

        protected override void UnloadContent()
        {
        }

        protected override void Update(GameTime gameTime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // UP
            if(localControls.up != previousControls.up){
                NetOutgoingMessage om = client.CreateMessage();
                om.Write((byte) ClientToServer.UpdateControls);
                om.Write((byte) ControlKeys.Up);
                om.Write(localControls.up);
                client.SendMessage(om, NetDeliveryMethod.ReliableOrdered);
            }
            previousControls.up = localControls.up;
            // DOWN
            if(localControls.down != previousControls.down){
                NetOutgoingMessage om = client.CreateMessage();
                om.Write((byte) ClientToServer.UpdateControls);
                om.Write((byte) ControlKeys.Down);
                om.Write(localControls.down);
                client.SendMessage(om, NetDeliveryMethod.ReliableOrdered);
            }
            previousControls.down = localControls.down;
            // RIGHT
            if(localControls.right != previousControls.right){
                NetOutgoingMessage om = client.CreateMessage();
                om.Write((byte) ClientToServer.UpdateControls);
                om.Write((byte) ControlKeys.Right);
                om.Write(localControls.right);
                client.SendMessage(om, NetDeliveryMethod.ReliableOrdered);
            }
            previousControls.right = localControls.right;
            // LEFT
            if(localControls.left != previousControls.left){
                NetOutgoingMessage om = client.CreateMessage();
                om.Write((byte) ClientToServer.UpdateControls);
                om.Write((byte) ControlKeys.Left);
                om.Write(localControls.left);
                client.SendMessage(om, NetDeliveryMethod.ReliableOrdered);
            }
            previousControls.left = localControls.left;
            // LEFT MOUSE
            if(localControls.mouseLeft != previousControls.mouseLeft){
                NetOutgoingMessage om = client.CreateMessage();
                om.Write((byte) ClientToServer.UpdateControls);
                om.Write((byte) ControlKeys.MouseLeft);
                om.Write(localControls.mouseLeft);
                om.Write(localControls.mousePos.X);
                om.Write(localControls.mousePos.Y);
                client.SendMessage(om, NetDeliveryMethod.ReliableOrdered);
            }
            previousControls.mouseLeft = localControls.mouseLeft;
            // RIGHT MOUSE
            if(localControls.mouseRight != previousControls.mouseRight){
                NetOutgoingMessage om = client.CreateMessage();
                om.Write((byte) ClientToServer.UpdateControls);
                om.Write((byte) ControlKeys.MouseRight);
                om.Write(localControls.mouseRight);
                om.Write(localControls.mousePos.X);
                om.Write(localControls.mousePos.Y);
                client.SendMessage(om, NetDeliveryMethod.ReliableOrdered);
            }
            previousControls.mouseRight = localControls.mouseRight;
            
            NetIncomingMessage msg;
            while ((msg = client.ReadMessage()) != null)
            {
                switch (msg.MessageType)
                {
                    case NetIncomingMessageType.VerboseDebugMessage:
                    case NetIncomingMessageType.DebugMessage:
                    case NetIncomingMessageType.WarningMessage:
                    case NetIncomingMessageType.ErrorMessage:
                        Console.WriteLine(msg.ReadString());
                        break;
                    case NetIncomingMessageType.StatusChanged:
                        break;
                    case NetIncomingMessageType.Data:
                        ServerToClient type = (ServerToClient) msg.ReadByte();
                        switch(type){
                            case ServerToClient.NewPlayers:
                                Console.WriteLine("Received new players");
                                int n = msg.ReadInt32();
                                Console.WriteLine(n);
                                // Parse the n players and add them to the game
                                for(int i=0; i<n; i++){
                                    Controls c = new Controls();
                                    Player p  = new Player(c, 32, Color.Red, false, false);
                                    long id = msg.ReadInt64();
                                    int px = msg.ReadInt32();
                                    int py = msg.ReadInt32();
                                    int vx = msg.ReadInt32();
                                    int vy = msg.ReadInt32();
                                    p.position = new Vector2(px, py);
                                    p.velocity = new Vector2(vx, vy);
                                    Console.WriteLine(p.position);
                                    Console.WriteLine(p.velocity);
                                    players[id] = p;
                                    controls[id] = c;
                                    game.AddPlayer(p);
                                }
                                break;
                            default:
                                break;
                        }
                        break;
                    default:
                        Console.WriteLine("Unhandled type: " + msg.MessageType);
                        break;
                }
                client.Recycle(msg);
            }

            // Update game after receiving input changes
            game.Update(gameTime);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            game.Draw(spriteBatch);
            
            base.Draw(gameTime);
        }
    }
}
