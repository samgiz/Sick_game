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

        NetServer server;

        Dictionary<long, Controls> controls;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            graphics.PreferredBackBufferWidth = C.screenWidth;
            graphics.PreferredBackBufferHeight = C.screenHeight;
            graphics.IsFullScreen = true;
            IsMouseVisible = true;

            // Initialize and start server
            NetPeerConfiguration config = new NetPeerConfiguration("sick_game_server");
            config.Port = 14242;
            server = new NetServer(config);
            server.Start();
        }

        protected override void Initialize()
        {
            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            C.Content = Content;
            game = new GameState();
        }

        protected override void UnloadContent()
        {
        }

        protected override void Update(GameTime gameTime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            
            NetIncomingMessage msg;
            while ((msg = server.ReadMessage()) != null)
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
                        NetConnectionStatus status = (NetConnectionStatus)msg.ReadByte();

                        string reason = msg.ReadString();
                        Console.WriteLine(NetUtility.ToHexString(msg.SenderConnection.RemoteUniqueIdentifier) + " " + status + ": " + reason);

                        if (status == NetConnectionStatus.Connected){
                            Console.WriteLine("Remote hail: " + msg.SenderConnection.RemoteHailMessage.ReadString());
                            // Create new controls for person
                            var c = new Controls();
                            controls[msg.SenderConnection.RemoteUniqueIdentifier] = c;
                            // Add player to game
                            game.AddPlayer(new Player(c, 32, Color.Red, false, false));

                            // TODO: Inform new player of other players in the game

                            // TODO: Inform other player of new player
                        }

                        break;
                    case NetIncomingMessageType.Data:
                        // Message type sent from client
                        ClientToServer type = (ClientToServer)msg.ReadByte();
                        Console.WriteLine(msg.SenderConnection.RemoteUniqueIdentifier);
                        
                        switch(type){
                            case ClientToServer.UpdateControls:
                                // Prepare the response message
                                NetOutgoingMessage om = server.CreateMessage();
                                om.Write((byte)ServerToClient.UpdateControls);

                                // Fetch the controls of the required player
                                Controls control = controls[msg.SenderConnection.RemoteUniqueIdentifier];
                                
                                // Read control to be uppdated
                                ControlKeys key = (ControlKeys) msg.ReadByte();
                                om.Write((byte)key);

                                // Read its state (on or off)
                                bool state = msg.ReadBoolean();
                                switch(key){
                                    case ControlKeys.Up:
                                        control.up = state;
                                        break;
                                    case ControlKeys.Down:
                                        control.down = state;
                                        break;
                                    case ControlKeys.Right:
                                        control.right = state;
                                        break;
                                    case ControlKeys.Left:
                                        control.left = state;
                                        break;
                                    // Update state for a mouse click
                                    default:
                                        if(key == ControlKeys.MouseLeft){
                                            control.mouseLeft = state;
                                        }
                                        if(key == ControlKeys.MouseRight){
                                            control.mouseRight = state;
                                        }
                                        // Read new mouse position (not sure which is supposed to be x and which y)
                                        int x = msg.ReadInt32();
                                        int y = msg.ReadInt32();
                                        om.Write(x);
                                        om.Write(y);
                                        // Set new mouse position
                                        control.mousePos = new Vector2(x, y);
                                        break;
                                }
                                // TODO: Here I should also add a timestamp to the message

                                // broadcast controls change to all connections, including sender
                                List<NetConnection> all = server.Connections; // get copy

                                // Send the update to the clients
                                server.SendMessage(om, all, NetDeliveryMethod.ReliableOrdered, 0);
                                break;
                            // case ClientToServer.StartGame:
                            //     // For each user create a player instance
                            //     break;
                            default:
                                Console.WriteLine("Unknown connection code");
                                break;
                        }

                        
                        break;
                    default:
                        Console.WriteLine("Unhandled type: " + msg.MessageType);
                        break;
                }
                server.Recycle(msg);
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
