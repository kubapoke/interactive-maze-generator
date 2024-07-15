using System.Collections.Generic;

namespace MazeGenerator.Maze.Generators
{
    public abstract class Generator
    {
        public abstract List<int>[] GenerateMaze(int width, int height, (int x, int y) start, (int x, int y) finish);
        private static List<int>[] InitializeNeighborLists(int width, int height)
        {
            int n = width * height;

            List<int>[] resultArray = new List<int>[n];

            for (int i = 0; i < n; i++)
            {
                resultArray[i] = new List<int>();
            }

            return resultArray;
        }
    }
}
