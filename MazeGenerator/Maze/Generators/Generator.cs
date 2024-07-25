using MazeGenerator.Drawing;
using MazeGenerator.Maze.Helpers;
using System.Collections.Generic;

namespace MazeGenerator.Maze.Generators
{
    public abstract class Generator
    {
        public abstract (List<int>[] maze, Drawer drawer) GenerateMaze(int width, int height);
        protected static List<int>[] InitializeNeighborLists(int width, int height)
        {
            int n = width * height;

            List<int>[] resultArray = new List<int>[n];

            for (int i = 0; i < n; i++)
            {
                resultArray[i] = new List<int>();
            }

            return resultArray;
        }

        protected static List<(int u, int v)> GeneratePotentialEdges(int width, int height)
        {
            List<(int u, int v)> potentialEdges = new List<(int, int)>();

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

            return potentialEdges;
        }

        protected static List<int>[] GenerateFullMazeGraph(int width, int height, bool randomizeOrder = true)
        {
            List<int>[] fullMazeGraph = new List<int>[width * height];

            for (int i = 0; i < fullMazeGraph.Length; i++)
                fullMazeGraph[i] = new List<int>();

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    if (x != width - 1)
                    {
                        fullMazeGraph[CoordinateConverters.CoordsToVertex(x, y, width)].Add(CoordinateConverters.CoordsToVertex(x + 1, y, width));
                        fullMazeGraph[CoordinateConverters.CoordsToVertex(x + 1, y, width)].Add(CoordinateConverters.CoordsToVertex(x, y, width));
                    }
                    if (y != height - 1)
                    {
                        fullMazeGraph[CoordinateConverters.CoordsToVertex(x, y, width)].Add(CoordinateConverters.CoordsToVertex(x, y + 1, width));
                        fullMazeGraph[CoordinateConverters.CoordsToVertex(x, y + 1, width)].Add(CoordinateConverters.CoordsToVertex(x, y, width));
                    }
                }
            }

            if (randomizeOrder)
            {
                Shuffler.ArrayFisherYatesShuffle(fullMazeGraph);
            }

            return fullMazeGraph;
        }
    }
}
