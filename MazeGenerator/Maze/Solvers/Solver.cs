using MazeGenerator.Drawing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MazeGenerator.Maze.Solvers
{
    public abstract class Solver
    {
        public abstract (List<int> solution, Drawer drawer) SolveMaze(List<int>[] mazeGraph, int width, int height, (int x, int y) start, (int x, int y) finish);
    }
}
