using MazeGenerator.Drawing;

namespace MazeGenerator.Maze.Solvers
{
    public abstract class Solver
    {
        public abstract (List<int> solution, Drawer drawer) SolveMaze(List<int>[] mazeGraph, int width, int height, (int x, int y) start, (int x, int y) finish);
    }
}
