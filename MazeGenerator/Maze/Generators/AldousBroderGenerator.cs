using MazeGenerator.Drawing;
using MazeGenerator.Maze.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace MazeGenerator.Maze.Generators
{
    public class AldousBroderGenerator : Generator
    {
        public override List<int>[] GenerateMaze(int width, int height, (int x, int y) start, (int x, int y) finish, bool draw = false)
        {
            List<int>[] mazeGraph = InitializeNeighborLists(width, height);
            List<int>[] fullMazeGraph = GenerateFullMazeGraph(width, height);
            SequentialDrawerWithMisses drawer = new SequentialDrawerWithMisses(MainWindow.Canvas, width, height);
            bool[] visited = new bool[fullMazeGraph.Length];

            int startVertex = MainWindow.Rng.Next(height * width);
            int finishVertex = CoordinateConverters.CoordsToVertex(finish, width);
            visited[startVertex] = true;
            int visitedVertices = 1;

            int currentVertex = startVertex, prevVertex = -1;

            while(visitedVertices < height * width)
            {
                int r = MainWindow.Rng.Next(fullMazeGraph[currentVertex].Count - 1);
                int nextVertex = fullMazeGraph[currentVertex][r];

                if (nextVertex == prevVertex)
                    nextVertex = fullMazeGraph[currentVertex][fullMazeGraph[currentVertex].Count - 1];

                if (!visited[nextVertex])
                {
                    visited[nextVertex] = true;
                    mazeGraph[currentVertex].Add(nextVertex);
                    mazeGraph[nextVertex].Add(currentVertex);
                    visitedVertices++;
                }

                if (draw)
                    drawer.AddEdgeToDraw(CoordinateConverters.VertexToCoords(currentVertex, width), CoordinateConverters.VertexToCoords(nextVertex, width));

                prevVertex = currentVertex;
                currentVertex = nextVertex;
            }

            if (draw)
                drawer.DrawMaze();

            return mazeGraph;
        }
    }
}
