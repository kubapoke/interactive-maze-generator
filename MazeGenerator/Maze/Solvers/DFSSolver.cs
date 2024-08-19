using MazeGenerator.Drawing;
using MazeGenerator.Drawing.SolutionDrawing;
using MazeGenerator.Maze.Helpers;

namespace MazeGenerator.Maze.Solvers
{
    public class DFSSolver : Solver
    {
        public override (List<int> solution, Drawer drawer) SolveMaze(List<int>[] mazeGraph, int width, int height, (int x, int y) start, (int x, int y) finish)
        {
            List<int> solution = new List<int>();
            Stack<int> solutionStack = new Stack<int>();
            bool[] visited = new bool[width * height];
            int[] prev = new int[width * height];
            TracebackSolutionDrawer drawer = new TracebackSolutionDrawer(MainWindow.Canvas, width, height);

            int startVertex = CoordinateConverters.CoordsToVertex(start, width);
            int finishVertex = CoordinateConverters.CoordsToVertex(finish, width);

            solutionStack.Push(startVertex);
            visited[startVertex] = true;

            Shuffler.ArrayFisherYatesShuffle(mazeGraph);

            while (solutionStack.Peek() != finishVertex)
            {
                int currentVertex = solutionStack.Pop();

                foreach (int nextVertex in mazeGraph[currentVertex])
                {
                    if (!visited[nextVertex])
                    {
                        solutionStack.Push(currentVertex);
                        solutionStack.Push(nextVertex);
                        visited[nextVertex] = true;

                        drawer.AddEdgeToDraw(CoordinateConverters.VertexToCoords(currentVertex, width), CoordinateConverters.VertexToCoords(nextVertex, width));

                        break;
                    }
                }
            }

            solution = solutionStack.ToList();
            solution.Reverse();

            return (solution, drawer);
        }
    }
}
