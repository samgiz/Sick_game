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
        // Updates position of all players, once every unit of time
        // Also include velocity
        UpdatePosition,
        // Inform players that somebody died
        // Followed by player id
        Death,
        // Used when someone starts a game to inform everyone that the game has started
        StartGame,
        // Announce to clients that a new player has been added
        NewPlayer,

        // Send over a list of current players to newly connected player
        ListPlayers
    }
    
    // Denotes the control
    public enum ControlKeys {
        // One bit that tells whether button pressed or not
        Up,
        // One bit that tells whether button pressed or not
        Left,
        // One bit that tells whether button pressed or not
        Down,
        // One bit that tells whether button pressed or not
        Right,
        // Two ints that represent the mouse coordinates
        MouseRight,
        // Two ints that represent the mouse coordinates
        MouseLeft
    }
}