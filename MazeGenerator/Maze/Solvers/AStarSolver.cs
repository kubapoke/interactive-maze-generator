using MazeGenerator.Drawing;
using MazeGenerator.Drawing.SolutionDrawing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MazeGenerator.Maze.Solvers
{
    public class AStarSolver : Solver
    {
        public override (List<int> solution, Drawer drawer) SolveMaze(List<int>[] mazeGraph, int width, int height, (int x, int y) start, (int x, int y) finish)
        {
            List<int> solution = new List<int>();
            // PriorityQueue<int, int>
            bool[] visited = new bool[width * height];
            int[] prev = new int[width * height];
            Drawer drawer = new ParallelPathSolutionDrawer(MainWindow.Canvas, width, height);

            throw new NotImplementedException();
        }
    }
}
