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

        NetClient client;

        Dictionary<long, Controls> controls;
        Dictionary<long, Player> players;

        LocalControls localControls;
        Controls previousControls;

        Camera camera;

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
            client.Connect("localhost", 14242, client.CreateMessage());
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            C.Content = Content;
            game = new GameState(new List<Player>());
            controls = new Dictionary<long, Controls>();
            players = new Dictionary<long, Player>(); 

            // Default local controls, uses WASD
            localControls = new LocalControls();
            
            previousControls = new Controls();

            camera = new Camera();
        }

        protected override void UnloadContent()
        {
        }

        protected override void Update(GameTime gameTime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            if(localControls != previousControls){
                NetOutgoingMessage om = client.CreateMessage();
                om.Write((byte) ClientToServer.UpdateControls);
                EncodeControls(om, localControls);
                client.SendMessage(om, NetDeliveryMethod.ReliableOrdered);
            }
            previousControls.AssignValues(localControls);
            
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
                            {
                                Console.WriteLine("Received new players");
                                // Read number of players
                                int n = msg.ReadInt32();
                                Console.WriteLine(n);
                                // Parse the n players and add them to the game
                                for(int i=0; i<n; i++){
                                    Controls c = new Controls();
                                    Player p  = new Player(c, 32, Color.Red, false, false);
                                    long id = msg.ReadInt64();
                                    DecodePlayer(msg, p);
                                    players[id] = p;
                                    controls[id] = c;
                                    game.AddPlayer(p);
                                }
                            }
                                break;
                            case ServerToClient.UpdateControls:
                            {
                                Console.WriteLine("Received a control update");
                                long id = msg.ReadInt64();
                                Controls control = controls[id];
                                DecodeControls(msg, control);
                            }
                                break;
                            case ServerToClient.UpdatePlayers:
                            {
                                Console.WriteLine("Received players update");
                                int n = msg.ReadInt32();
                                Console.WriteLine(n);
                                for(int i=0; i<n; i++){
                                    long id = msg.ReadInt64();
                                    DecodePlayer(msg, players[id]);
                                    Console.WriteLine(players[id].position);
                                }
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
            camera.BeginDraw(spriteBatch);
            game.Draw(spriteBatch);
            
            base.Draw(gameTime);
            spriteBatch.End();
        }
    }
}
