# GameOfLife

A cool console game framework, grid game framework, and implementation of Conway's Game of Life.

# Build / Run
Load into your favorite C# IDE or use MSBuild.

# Testing
No unit tests (yet?) but there are many examples which can be loaded from the /examples folder and verified visually against similar examples found on Wikipedia.

# More Info
The console game engine can easily be plugged into other games and provide some basic support for things like keyboard commands, timer-initiated operations, and the concept of a Game which has rounds as well as some kind of startup/teardown activities.

The grid game engine can easily be plugged into other UIs (or used in a headless fashion) to enable things such as a 3D variant of the game or rendering to a file.  

It can also be used to create other types of games using similar algorithms like Langton's ant/loops, Paterson's worms, Turmites, etc. - as well as interactive things such as a text-based dungeon/maze.
