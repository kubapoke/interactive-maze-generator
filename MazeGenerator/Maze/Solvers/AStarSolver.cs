using MazeGenerator.Drawing;
using MazeGenerator.Drawing.SolutionDrawing;
using MazeGenerator.Maze.Helpers;

namespace MazeGenerator.Maze.Solvers
{
    public class AStarSolver : Solver
    {
        public override (List<int> solution, Drawer drawer) SolveMaze(List<int>[] mazeGraph, int width, int height, (int x, int y) start, (int x, int y) finish)
        {
            List<int> solution = new List<int>();
            PriorityQueue<int, int> solutionQueue = new PriorityQueue<int, int>();
            bool[] visited = new bool[width * height];
            int[] prev = new int[width * height];
            int[] dist = new int[width * height];
            Drawer drawer = new ParallelPathSolutionDrawer(MainWindow.Canvas, width, height);

            int startVertex = CoordinateConverters.CoordsToVertex(start, width);
            int finishVertex = CoordinateConverters.CoordsToVertex(finish, width);

            solutionQueue.Enqueue(startVertex, ManhattanDistance(start, finish));

            visited[startVertex] = true;
            prev[startVertex] = -1;
            dist[startVertex] = 0;

            Shuffler.ArrayFisherYatesShuffle(mazeGraph);

            while (solutionQueue.Count > 0 && !visited[finishVertex])
            {
                int currentVertex = solutionQueue.Dequeue();
                visited[currentVertex] = true;

                if (currentVertex != startVertex)
                    drawer.AddEdgeToDraw(CoordinateConverters.VertexToCoords(prev[currentVertex], width), CoordinateConverters.VertexToCoords(currentVertex, width));


                foreach (int nextVertex in mazeGraph[currentVertex])
                {
                    if (visited[nextVertex])
                    { continue; }

                    prev[nextVertex] = currentVertex;
                    dist[nextVertex] = dist[currentVertex] + 1;
                    solutionQueue.Enqueue(nextVertex, dist[nextVertex] + ManhattanDistance(CoordinateConverters.VertexToCoords(nextVertex, width), finish));
                }
            }

            int vertexToAdd = finishVertex;

            while (vertexToAdd != -1)
            {
                solution.Add(vertexToAdd);
                vertexToAdd = prev[vertexToAdd];
            }

            solution.Reverse();

            return (solution, drawer);
        }

        private int ManhattanDistance((int x, int y) u, (int x, int y) v)
        {
            return Math.Abs(u.x - v.x) + Math.Abs(u.y - v.y);
        }
    }
}
