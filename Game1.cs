using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Lidgren.Network;
using static Networking.Utilities;

namespace Game1000
{
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        GameState game;

        NetServer server;

        Dictionary<long, Controls> controls;
        Dictionary<long, Player> players;

        // Denotes the max acceptable time interval without updating position of a player
        // In milliseconds
        int updateInterval = 250;

        // Denotes the last update of all players positions
        DateTime lastUpdate;

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
            // Initialize and start server
            NetPeerConfiguration config = new NetPeerConfiguration("sick_game");
            config.Port = 14242;
            // For some reason this does not work when ConnectionApproval is enabled
            // config.EnableMessageType(NetIncomingMessageType.ConnectionApproval);
            server = new NetServer(config);
            server.Start();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            C.Content = Content;
            game = new GameState();
            controls = new Dictionary<long, Controls>();
            players = new Dictionary<long, Player>();
            lastUpdate = DateTime.MinValue;
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
                        Console.WriteLine("Connection change");

                        string reason = msg.ReadString();
                        Console.WriteLine(NetUtility.ToHexString(msg.SenderConnection.RemoteUniqueIdentifier) + " " + status + ": " + reason);

                        if (status == NetConnectionStatus.Connected){
                            // Create new controls for person
                            var c = new Controls();
                            controls[msg.SenderConnection.RemoteUniqueIdentifier] = c;

                            // Add player to game
                            Player p = new Player(c, 32, Color.Red, false, false);
                            players[msg.SenderConnection.RemoteUniqueIdentifier] = p;
                            game.AddPlayer(p);

                            // Inform new player of other players in the game
                            NetOutgoingMessage om1 = server.CreateMessage();
                            om1.Write((byte)ServerToClient.NewPlayers);

                            // Encode player position and velocity
                            // Number of players
                            om1.Write(1);
                            // Player identifier
                            om1.Write(msg.SenderConnection.RemoteUniqueIdentifier);
                            // Player encoding
                            EncodePlayer(om1, p);

                            List<NetConnection> all = server.Connections; // get copy

                            // Remove the sender from recipients
                            all.Remove(msg.SenderConnection);


                            // Send the update to the clients
                            if(all.Count > 0)
                                server.SendMessage(om1, all, NetDeliveryMethod.ReliableOrdered, 0);

                            // Inform other player of all new players
                            NetOutgoingMessage om2 = server.CreateMessage();
                            om2.Write((byte)ServerToClient.NewPlayers);

                            // Write the number of players
                            om2.Write(players.Count);
                            // Encode each player
                            foreach(var item in players)
                            {
                                om2.Write(item.Key);
                                EncodePlayer(om2, item.Value);
                                Player pl = item.Value;
                            }
                            server.SendMessage(om2, msg.SenderConnection, NetDeliveryMethod.ReliableOrdered);
                        }
                        break;
                    case NetIncomingMessageType.Data:
                        // Message type sent from client
                        ClientToServer type = (ClientToServer)msg.ReadByte();
                        // Console.WriteLine(msg.SenderConnection.RemoteUniqueIdentifier);
                        
                        switch(type){
                            case ClientToServer.UpdateControls:
                                // Prepare the response message
                                NetOutgoingMessage om = server.CreateMessage();

                                // Response message type
                                om.Write((byte)ServerToClient.UpdateControls);

                                // Id of the player whose controls changed
                                om.Write(msg.SenderConnection.RemoteUniqueIdentifier);

                                // Fetch the controls of the required player
                                Controls control = controls[msg.SenderConnection.RemoteUniqueIdentifier];
                                
                                // Read control to be updated
                                DecodeControls(msg, control);
                                EncodeControls(om, control);

                                // TODO: add a timestamp to the message

                                // Send the update to all clients
                                server.SendMessage(om, server.Connections, NetDeliveryMethod.ReliableOrdered, 0);
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
            // Console.WriteLine((DateTime.Now - lastUpdate).TotalMilliseconds > updateInterval);
            if(server.Connections.Count > 0 && (DateTime.Now - lastUpdate).TotalMilliseconds > updateInterval){
                lastUpdate = DateTime.Now;
                NetOutgoingMessage om = server.CreateMessage();
                om.Write((byte)ServerToClient.UpdatePlayers);
                om.Write(players.Count);
                foreach(var kp in players){
                    om.Write(kp.Key);
                    EncodePlayer(om, kp.Value);
                }
                server.SendMessage(om, server.Connections, NetDeliveryMethod.ReliableOrdered, 0);
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
