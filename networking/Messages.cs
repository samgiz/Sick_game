namespace Game1000
{
    public enum ClientToServer {
        // Sends a list of controls that have changed
        // Message starts with a byte ControlKey
        // Additionally send position and velocity of player?
        UpdateControls,
        // Inform server that we want to start a game
        StartGame
    }

    public enum ServerToClient {
        // Update all players of control changes for a list of players
        // Message starts with a byte ControlKey
        // Additionally send position and velocity of player?
        UpdateControls,
        // Updates info of all players, once every unit of time
        // Also include velocity
        UpdatePlayers,
        // Inform players that somebody died
        // Followed by player id
        Death,
        // Used when someone starts a game to inform everyone that the game has started
        StartGame,
        // Announce to clients that new players joined 
        NewPlayers
    }
}