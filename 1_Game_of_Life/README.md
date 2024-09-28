# TD1 - Les Automates Cellulaires

The Game of Life

## Unity interface guidelines

- Open the project in Unity.
![](/AI_Simulations/1_Game_of_Life/ReadMeImages/1_open_project.png)

- Click on "Grid" in the top left hand side "Hierarchy", in order to display the "Game Board" object in the right hand side.  
The different patterns displayed at the bottom of the window can be dragged and dropped into the "Pattern" attribute of the "Game Board" object.
![](/AI_Simulations/1_Game_of_Life/ReadMeImages/2_grid.png)

- Click on "Scripts" in the bottom left hand side "Project", in order to display the different C# scripts.  
The GameBoard_Classic et GameBoard_MoreDeadCells scripts can be dragged and dropped into the "Script" attribute of the "Game Board" object.
![](/AI_Simulations/1_Game_of_Life/ReadMeImages/3_scripts.png)

- Click on the play button at the top center of the window, in order to start the simulation.
![](/AI_Simulations/1_Game_of_Life/ReadMeImages/4_play.png)

## Research for the project

I tested many patterns and sorted them into categories (different folders in the Patterns folder) I thought could be interesting later on for the WORLDBUILDING project. I also tested a variation in the Game of Life rules (the GameBoard_MoreDeadCells script). This variation checks and updated all the neighbors of the alive cells, but the alive cells themselves.  

- Last state of the "Thunderbird" pattern while using the GameBoard_Classic script.
![](/AI_Simulations/1_Game_of_Life/ReadMeImages/5_end_classic.png)

- Last state of the "Thunderbird" pattern while using the GameBoard_MoreDeadCells script.
![](/AI_Simulations/1_Game_of_Life/ReadMeImages/6_end_more_dead_cells.png)