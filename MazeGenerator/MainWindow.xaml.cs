﻿using MazeGenerator.Drawing;
using MazeGenerator.Maze.Generators;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace MazeGenerator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public Maze.Maze Maze;
        public List<NamedGenerator> GeneratorList { get; set; }
        public static Canvas Canvas;

        public MainWindow()
        {
            InitializeComponent();

            Canvas = MazeCanvas;

            GeneratorList = new List<NamedGenerator>
            { new NamedGenerator { Name = "Randomized Kruskal", Generator = new KruskalGenerator()} };

            DataContext = this;
        }

        public struct NamedGenerator
        {
            public string Name { get; set; }
            public Generator Generator { get; set; }
        }

        private void GenerateButton_Click(object sender, RoutedEventArgs e)
        {
            try // temporary solution, input validation should be done separately
            {
                Maze = new Maze.Maze(int.Parse(WidthTextBox.Text), int.Parse(HeightTextBox.Text));

                MazeDrawer.ResizeCanvas(MazeCanvas, Maze.Width, Maze.Height);

                MazeCanvas.Children.Clear();

                Maze.Generate(GeneratorComboBox.SelectedValue as Generator);
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
