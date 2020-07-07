using Microsoft.Xna.Framework;
using System.Collections.Generic;

// This class probably only makes sense for the server, though I'll have to try out a few things
namespace Game1000
{
    public class Room
    {
        // Name of room
        // Should be unique
        string name;

        // Game instance of the current room
        // If null, then no game is currently running
        // GameState game;
        
        // The players who are playing the current/next game
        List<Player> players;
        
        // List of players who joined the room mid game and can spectate until game ends
        List<Player> spectators;
        
        public Room(string name)
        {
            this.name = name;
            this.players = new List<Player>();
        }

        public void Update(GameTime gameTime)
        {   
            // If game has finished then clean up and remove old game instance
            // if(game != null && !game.IsActive)
            //     updateAfterGame();
            // if(game != null) {
            //     // Update the game state if the game is running
            //     game.Update(gameTime);
            // }
        }

        public void StartGame(){
            // Initialize the game with all the players
            // TODO: figure out where to place each player
            // (actually this should be handled by the game or by the arena of the game)
            // this.game = new GameState(new List<Player>());
        }

        public void AddPlayer(Player p){
            // Check if player is already in the room to avoid adding duplicates
            if(!(players.Contains(p) || spectators.Contains(p)))
                players.Add(p);
        }
        
        public void RemovePlayer(Player p){
            // No need to check whether the player has already been removed
            if(!players.Remove(p))
                // Only try removing player from spectators if they weren't present between the players
                spectators.Remove(p);
        }

        public void updateAfterGame(){
            players.AddRange(spectators);
            // game = null;
        }
    }
}
