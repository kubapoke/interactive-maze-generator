using MazeGenerator.Maze.Helpers;
using System.Collections.Generic;

namespace MazeGenerator.Maze.Generators
{
    public class KruskalGenerator : Generator
    {
        public override List<int>[] GenerateMaze(int width, int height, (int x, int y) start, (int x, int y) finish)
        {
            List<int>[] mazeGraph = InitializeNeighborLists(width, height);
            List<(int u, int v)> potentialEdges = new List<(int, int)>();
            bool startConnected = false, finishConnected = false;

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    if (x != width - 1)
                        potentialEdges.Add((CoordinateConverters.CoordsToVertex(x, y, width), CoordinateConverters.CoordsToVertex(x + 1, y, width)));
                    if (y != height - 1)
                        potentialEdges.Add((CoordinateConverters.CoordsToVertex(x, y, width), CoordinateConverters.CoordsToVertex(x, y + 1, width)));
                }
            }

            Shuffler.FisherYatesShuffle(potentialEdges);
            UnionFind unionFind = new UnionFind(width * height);

            foreach (var potentialEdge in potentialEdges)
            {
                if (potentialEdge.u == CoordinateConverters.CoordsToVertex(finish, width) || potentialEdge.v == CoordinateConverters.CoordsToVertex(finish, width))
                {
                    if (finishConnected == false)
                        finishConnected = true;
                    else
                        continue;

                }

                if (unionFind.Union(potentialEdge.u, potentialEdge.v))
                {
                    mazeGraph[potentialEdge.u].Add(potentialEdge.v);
                    mazeGraph[potentialEdge.v].Add(potentialEdge.u);
                }
            }

            return mazeGraph;
        }
    }
}
