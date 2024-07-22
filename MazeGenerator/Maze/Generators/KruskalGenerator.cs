using MazeGenerator.Drawing;
using MazeGenerator.Maze.Helpers;
using System.Collections.Generic;

namespace MazeGenerator.Maze.Generators
{
    public class KruskalGenerator : Generator
    {
        public override List<int>[] GenerateMaze(int width, int height, (int x, int y) start, (int x, int y) finish, bool draw = false)
        {
            List<int>[] mazeGraph = InitializeNeighborLists(width, height);
            List<(int u, int v)> potentialEdges = GeneratePotentialEdges(width, height);
            List<((int x, int y) u, (int x, int y) v)> edgesToDraw = new List<((int x, int y) u, (int x, int y) v)>();

            Shuffler.FisherYatesShuffle(potentialEdges);
            UnionFind unionFind = new UnionFind(width * height);

            foreach (var potentialEdge in potentialEdges)
            {
                if (unionFind.Union(potentialEdge.u, potentialEdge.v))
                {
                    mazeGraph[potentialEdge.u].Add(potentialEdge.v);
                    mazeGraph[potentialEdge.v].Add(potentialEdge.u);

                    if (draw)
                        edgesToDraw.Add((CoordinateConverters.VertexToCoords(potentialEdge.u, width), CoordinateConverters.VertexToCoords(potentialEdge.v, width)));
                }
            }

            if (draw)
                MazeDrawer.DrawMazeInOrder(MainWindow.Canvas, width, height, edgesToDraw);

            return mazeGraph;
        }
    }
}
