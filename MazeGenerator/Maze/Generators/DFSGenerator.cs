using MazeGenerator.Drawing;
using MazeGenerator.Maze.Helpers;
using System.Collections.Generic;

namespace MazeGenerator.Maze.Generators
{
    public class DFSGenerator : Generator
    {
        public override (List<int>[] maze, Drawer drawer) GenerateMaze(int width, int height)
        {
            List<int>[] mazeGraph = InitializeNeighborLists(width, height);
            List<int>[] fullMazeGraph = GenerateFullMazeGraph(width, height);
            TracebackMazeDrawer drawer = new TracebackMazeDrawer(MainWindow.Canvas, width, height);
            bool[] visited = new bool[fullMazeGraph.Length];
            Stack<int> vertexStack = new Stack<int>();

            int startVertex = MainWindow.Rng.Next(height * width);

            vertexStack.Push(startVertex);
            visited[startVertex] = true;

            while (vertexStack.Count > 0)
            {
                int currentVertex = vertexStack.Pop();

                foreach (int nextVertex in fullMazeGraph[currentVertex])
                {
                    if (!visited[nextVertex])
                    {
                        vertexStack.Push(currentVertex);
                        vertexStack.Push(nextVertex);
                        visited[nextVertex] = true;

                        mazeGraph[currentVertex].Add(nextVertex);
                        mazeGraph[nextVertex].Add(currentVertex);

                        drawer.AddEdgeToDraw(CoordinateConverters.VertexToCoords(currentVertex, width), CoordinateConverters.VertexToCoords(nextVertex, width));

                        break;
                    }
                }
            }

            return (mazeGraph, drawer);
        }
    }
}
