CT-4101-A2-Star-Map-Navigation-Similation-Kieran-Kristian
Free Camera Controls:
WASD - Movement
Hold down right click to look around
Shift to speed up
Space to go upwards
Control to go downwards

References:
DataGenetics (no date), "Random Points In A Sphere", [online] DataGenetics, Available At: http://datagenetics.com/blog/january32020/index.html, (Accessed: 14/05/2025)
(Used for Hollow Sphere Galaxy shape, KMaths.RandomDistanceHollowSphere)

SpeedTutor (2024), "GHOST FLYCAM Camera In Unity (New Input System Tutorial)", [online], YouTube, Available at: https://www.youtube.com/watch?v=_j7Rh27ccqk&t=630s, (Accessed: April 2025)
(Used for free move camera, FreeLookFlyCamera.cs)

Dogmatic (2020) , "*FREE* Skyboxes-Space", [online], Unity Asset Store, Available at: https://assetstore.unity.com/packages/2d/textures-materials/sky/free-skyboxes-space-178953, (Accessed: May 2025)
(Used for the skybox in the scene)

hasanbayatme, (2019), "Unity-Dijkstras-Pathfinding", [online], GitHub, Available at: https://github.com/hasanbayatme/unity-dijkstras-pathfinding, (Accessed: April 2025)
(StarPath, Star, and Path Finding scripts were all heavily inspired by this project, with necessary tweaks made to fit into my system, StarPath.cs, Star.cs, DijkstraPathfinding.cs)
(I adapted the system to work with my own mesh generation, so that the paths contained in the route were coloured yellow to highlight which routes need to be traversed)
(I also added extra functionality in the form of pathfinding constraints, such as enemy territories which could be untraversable by setting the distance between them to infinity)
(I also added a fuel constraint which would limit the maximum distance that the player could travel, however, there are also ally territories which are half the distance they should be, which influences the pathfinding algorithm)

Reused Scripts: 
The Lerping, KMaths and Easings scripts are all reused from CT-4101-A1-Modes-Of-Motion
The lerping script has since been adapted to have extra functionality, it now has a float available for lerp
The lerping script also has a new function which will lerp an objects rotation to look at a certain point
The KMaths library includes a method for Truncation which will cut off excess decimal places of a float, this does this to 2 decimal places

Assessment Requirements:
The application is expected to procedurally generate a selection of stars in runtime, from a random selection of prepared data. 
The StarSpawner script procedurally generates a galaxy full of stars, based on a selection of galaxy shapes which can be changed at the start of the simulation

There should be a UI for users to select origin and destination stars from those in scene, and to adjust any pathfinding constraints (e.g., max jump distance, fuel, time, etc). 
The StarSelector script allows the user to select a star using a raycast from the mouse position, there are constraints which can be adjusted at the start, fuel capacity which limits the maximum distance you can travel, player and enemy factions which influence pathfinding

The simulation will need to use pathfinding methods to calculate the optimal route to the destination star, within any constraining parameters.  
The simulation makes use of Dijkstra Pathfinding to calculate the shortest possible route from the start star to the end star

There should be a basic User Interface in place that indicates the route of stars created from your pathfinding solver and the current pathfinding constraints. 
A custom mesh generation script is utilised to create all of the routes between planets in the game scene, the path from the start star to the end star is coloured in yellow to demonstrate which routes to travel between

The simulation should run until the user presses the escape key to halt execution of the program. 
Pressing escape closes the simulation

All source code must be fully commented according to the coding standards stipulated on the Moodle page for this module. 
All code is commented according with the coding standards

Additional Features:
Presentation of Work:
UI has been made to look futuristic to match the aesthetic of the simulation, there is also a free skybox asset in use which makes the scene look a lot nicer

Navigation Agents Following the Discovered Path In Runtime:
There is a spaceship which traverses the desired path, it travels froms star to star and changes the color of the routes it travels on to indicate it has travelled on that route
The spaceship makes use of the lerp script from my previous assessment, as it lerps its position between stars and also lerps to look at its next destination star

Sound: 
There are sound effects in place for UI to improve feedback to the user
There is also a sound that is played by the spaceship, to indicate that it is boosting, the volume is lerped when it is moving to further immerse the user
