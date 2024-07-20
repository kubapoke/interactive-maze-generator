# Interactive maze generator
## Introduction
The project is an interactive, visual maze generator written using C# and the WPF framework. It provides an UI allowing for users to generate and (TBA) solve mazes, while visualising the process, allowing for insight into the algorithms behind the scenes.

**Please note, that the project is in an early WIP stage and therefore the majority of its functionality is not yet implemented.**

The main aim of this project is to practice visual programming skills (specifically the aforementioned WPF framework) and learn to create applications with dynamically generated graphics. Moreover the project is supposed to help me practice general object-oriented programming as well as alghoritmics skills.

## Installation and compilation
For now, the project has been worked on and compiled using Microsoft's Visual Studio, using WPF Framework and .NET 4.8 on a Windows device (as a WPF application, it is suited for Windows devices only).

In the futture I'll provide a more accessible way to download just the binary file.

## Usage
After running the application, you will see a generation menu. After selecting the maze parameters as well as the maze generation algorithm, pressing the "Generate" button shall create the desired maze, using the chosen algorithm, visualizing the alghoritm's maze creation process.

A similar menu shall be added for maze solving in the near future.

## To-do list
1. User interface
    - [X] Provide a basic maze generation UI
    - [ ] Provide a basic maze solving UI
    - [ ] Extend the above menus with more complex options
2. Maze visualization
    - [X] Create the basic maze vizualization tools
    - [X] Create the tools for accurately visualizing the creation process of different maze generating algorithms
    - [ ] Create the tools for accurately visualizing the creation process of different maze solving algorithms
    - [ ] Implement dynamic maze scaling, allow for window manipulation for better user experience
    - [ ] Refine the maze graphics, get rid of the unwanted artifacts
3. Maze generation algorithms
    - [X] DFS
    - [X] Kruskal
    - [ ] Prim
    - [ ] Wilson (loop-erased random walk)
    - [ ] Aldous-Broder
    - [ ] ...
4. Maze solving algorithms
    - [ ] DFS
    - [ ] BFS
    - [ ] A*
    - [ ] ...
5. Miscalleanous
    - [ ] Add more variety to mazes, allowing to properly showcase shortest path algorithms
    - [ ] Add maze file import/export mechanism
    - [ ] Add a way to manually create mazes to be solved
