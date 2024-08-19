using MazeGenerator.Drawing;
using MazeGenerator.Maze.Helpers;

namespace MazeGenerator.Maze.Generators
{
    public class KruskalGenerator : Generator
    {
        public override (List<int>[] maze, Drawer drawer) GenerateMaze(int width, int height)
        {
            List<int>[] mazeGraph = InitializeNeighborLists(width, height);
            List<(int u, int v)> potentialEdges = GeneratePotentialEdges(width, height);
            SequentialMazeDrawer drawer = new SequentialMazeDrawer(MainWindow.Canvas, width, height);

            Shuffler.FisherYatesShuffle(potentialEdges);
            UnionFind unionFind = new UnionFind(width * height);

            foreach (var potentialEdge in potentialEdges)
            {
                if (unionFind.Union(potentialEdge.u, potentialEdge.v))
                {
                    mazeGraph[potentialEdge.u].Add(potentialEdge.v);
                    mazeGraph[potentialEdge.v].Add(potentialEdge.u);

                    drawer.AddEdgeToDraw(CoordinateConverters.VertexToCoords(potentialEdge.u, width), CoordinateConverters.VertexToCoords(potentialEdge.v, width));
                }
            }

            return (mazeGraph, drawer);
        }
    }
}
