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

            KruskalGenerator generator = new KruskalGenerator();

            var maze = generator.GenerateMaze(5, 5, (0, 0), (4, 4));
            ;

            DataContext = this;
        }

        public struct NamedGenerator
        {
            public string Name { get; set; }
            public Generator Generator { get; set; }
        }
    }
}
