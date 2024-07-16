using System;
using System.Collections.Generic;

namespace MazeGenerator.Maze
{
    public class Maze
    {
        private int N { get { return Width * Height; } }
        public int Width { get; private set; } = 0;
        public int Height { get; private set; } = 0;
        public (int x, int y) Start { get; private set; } = (0, 0);
        public (int x, int y) Finish { get; private set; } = (0, 0);
        private List<int>[] MazeGraph;

        public Maze(int width, int height)
        {
            if (width <= 0 || height <= 0)
                throw new ArgumentException("Width and height of the maze need to be positive integers");

            Width = width;
            Height = height;
            Start = (0, 0);
            Finish = (width - 1, height - 1);
        }
    }
}
