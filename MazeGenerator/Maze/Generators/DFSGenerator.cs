using MazeGenerator.Drawing;
using MazeGenerator.Maze.Helpers;
using System.Collections.Generic;

namespace MazeGenerator.Maze.Generators
{
    public class DFSGenerator : Generator
    {
        public override List<int>[] GenerateMaze(int width, int height, (int x, int y) start, (int x, int y) finish, bool draw = false)
        {
            List<int>[] mazeGraph = InitializeNeighborLists(width, height);
            List<int>[] fullMazeGraph = GenerateFullMazeGraph(width, height);
            List<((int x, int y) u, (int x, int y) v)> edgesToDraw = new List<((int x, int y) u, (int x, int y) v)>();
            bool[] visited = new bool[fullMazeGraph.Length];
            Stack<int> vertexStack = new Stack<int>();

            vertexStack.Push(CoordinateConverters.CoordsToVertex(start, width));
            visited[CoordinateConverters.CoordsToVertex(start, width)] = true;

            while (vertexStack.Count > 0)
            {
                int currentVertex = vertexStack.Pop();

                if (currentVertex == CoordinateConverters.CoordsToVertex(finish, width))
                    continue;

                foreach (int nextVertex in fullMazeGraph[currentVertex])
                {
                    if (!visited[nextVertex])
                    {
                        vertexStack.Push(currentVertex);
                        vertexStack.Push(nextVertex);
                        visited[nextVertex] = true;

                        mazeGraph[currentVertex].Add(nextVertex);
                        mazeGraph[nextVertex].Add(currentVertex);

                        if (draw)
                            edgesToDraw.Add((CoordinateConverters.VertexToCoords(currentVertex, width), CoordinateConverters.VertexToCoords(nextVertex, width)));

                        break;
                    }
                }
            }

            if (draw)
                MazeDrawer.DrawMazeWithTraceback(MainWindow.Canvas, width, height, edgesToDraw);

            return mazeGraph;
        }
    }
}
