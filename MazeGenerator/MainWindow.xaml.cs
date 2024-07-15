using MazeGenerator.Maze.Generators;
using System.Collections.Generic;
using System.Windows;

namespace MazeGenerator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public Maze.Maze Maze;
        public List<NamedGenerator> GeneratorList { get; set; }

        public MainWindow()
        {
            InitializeComponent();

            GeneratorList = new List<NamedGenerator>
            { new NamedGenerator { Name = "Randomized Kruskal", Generator = new KruskalGenerator()} };

            DataContext = this;
        }

        public struct NamedGenerator
        {
            public string Name { get; set; }
            public Generator Generator { get; set; }
        }
    }
}
