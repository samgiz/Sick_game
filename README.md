# Overview

This project is an attempt to create a highly customizable and scalable cross-platform multiplayer game with C# using Monogame and Lidgren Network.

The main idea of the game is extremely simple: every player controls a 2d ball and tries to knock out other players from the arena. Different players can also have different special powers

## State of development

The curernt version of the game is technically functional, though not very entertaining. 
- The single_pc version works quite well except for the special powers.
- The client launches the game and waits for a server's response at a hardcoded location (localhost:14242 by default)
- The server launches on localhost:14242. Whenever a new player connects, it restarts the game.

One of the first goals next is to set up the server online so it could be possible to connect to it from anywhere.

# Directory structure

The project consists of 5 directories
* single_pc - a version of the game that works on a single pc. Mainly used for easy testing
* client - the client side of the multiplayer game
* server - server side of the multiplayer game. Currently uses Monogame as well to simulate the game.
* logic - the backbone of the logic of the game. These files are shared by single_pc, client and server
* networking - classes and methods common to both client and server that are related to networking
