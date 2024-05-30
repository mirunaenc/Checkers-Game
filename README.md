# Checkers-Game

## Description
This project involves creating a checkers game application using C# and WPF, following the MVVM design pattern. The game features two sets of pieces (white and red) and an 8x8 board. The player with the red pieces moves first, and players take turns making moves. The application visually indicates the current player and displays the number of remaining pieces for each player.

## Features

### Gameplay Mechanics
- **Simple Move**: A piece moves diagonally forward by one square. If a piece reaches the opposite end of the board, it becomes a "king" and can move diagonally both forward and backward.
- **Jumping Over Opponent**: A piece can jump over an opponent's piece to capture it. Multiple jumps are possible if the option is enabled at the start of the game.
- **Multiple Jumps**: Players can perform multiple jumps over opponent pieces consecutively if the option is enabled.

### Game End
- The game ends when a player has no more pieces left on the board, and the opponent is declared the winner.
- The application displays a message indicating the winner at the end of the game.
- The number of wins for each player (white and red) is tracked and displayed, stored in a text file.

### Additional Features
- **Save and Load Game**: The game state can be saved to a file and loaded later. 
- **Statistics**: The application tracks and displays the number of wins for each player and the maximum number of remaining pieces for the winner.
- **Configuration**: The game can be started with different configurations, such as enabling kings, setting up scenarios for testing the endgame, and enabling multiple jumps.
- **Help and About**: The application includes a Help menu with an About section that contains the developer's name, institutional email, group, and a brief description of the game.

## User Interface
- **Main Board**: Displays the checkers board and the current game state.
- **Menus**: 
  - **File Menu**: Options for starting a new game, saving the current game, loading a saved game, enabling multiple jumps, and viewing statistics.
  - **Help Menu**: Contains an About section with information about the developer and the game.

## Technologies Used
- **Programming Language**: C#
- **Framework**: .NET Framework
- **User Interface**: WPF (Windows Presentation Foundation)
- **Design Pattern**: MVVM (Model-View-ViewModel)
- **Data Storage**: Text files (TXT)
