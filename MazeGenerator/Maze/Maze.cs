﻿using MazeGenerator.Drawing;
using MazeGenerator.Maze.Generators;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace MazeGenerator.Maze
{
    public class Maze
    {
        private int N { get { return Width * Height; } }
        public int Width { get; private set; } = 0;
        public int Height { get; private set; } = 0;
        public (int x, int y) Start { get; private set; } = (0, 0);
        public (int x, int y) Finish { get; private set; } = (0, 0);
        public List<int>[] MazeGraph;
        public Drawer Drawer;

        public Maze(int width, int height)
        {
            if (width <= 0 || height <= 0)
                throw new ArgumentException("Width and height of the maze need to be positive integers");

            Width = width;
            Height = height;
            Start = (0, 0);
            Finish = (width - 1, height - 1);
        }

        public async Task GenerateAsync(Generator generator)
        {
            (MazeGraph, Drawer) = await Task.Run(() => generator.GenerateMaze(Width, Height, Start, Finish));

            await Drawer.DrawMazeAsync();
        }
    }
}
