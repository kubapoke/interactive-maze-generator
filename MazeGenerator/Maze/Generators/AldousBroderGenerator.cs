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
            List<((int x, int y) u, (int x, int y) v)> edgesToDraw = new List<((int x, int y) u, (int x, int y) v)>();
            bool[] visited = new bool[fullMazeGraph.Length];

            int startVertex = MainWindow.Rng.Next(height * width);
            int finishVertex = CoordinateConverters.CoordsToVertex(finish, width);
            visited[startVertex] = true;
            int visitedVertices = 1;

            int currentVertex = startVertex, prevVertex = -1;

            while(visitedVertices < height * width)
            {
                int r = MainWindow.Rng.Next(fullMazeGraph[currentVertex].Count);
                int nextVertex = fullMazeGraph[currentVertex][r];

                if (currentVertex == finishVertex && prevVertex != -1)
                    nextVertex = prevVertex;

                if (!visited[nextVertex])
                {
                    visited[nextVertex] = true;
                    mazeGraph[currentVertex].Add(nextVertex);
                    mazeGraph[nextVertex].Add(currentVertex);
                    visitedVertices++;
                }

                if (draw)
                    edgesToDraw.Add((CoordinateConverters.VertexToCoords(currentVertex, width), CoordinateConverters.VertexToCoords(nextVertex, width)));

                prevVertex = currentVertex;
                currentVertex = nextVertex;
            }

            if (draw)
                MazeDrawer.DrawMazeInOrderWithMisses(MainWindow.Canvas, width, height, finish, edgesToDraw,0);

            return mazeGraph;
        }
    }
}
