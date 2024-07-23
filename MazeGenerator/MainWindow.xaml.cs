using MazeGenerator.Drawing;
using MazeGenerator.Maze.Generators;
using System;
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
        public static DockPanel DockPanel;
        public static Random Rng = new Random();

        public MainWindow()
        {
            InitializeComponent();

            Canvas = MazeCanvas;
            DockPanel = TopDockPanel;

            GeneratorList = new List<NamedGenerator>
            { new NamedGenerator { Name = "Kruskal", Generator = new KruskalGenerator() },
            new NamedGenerator { Name = "DFS", Generator = new DFSGenerator() },
            new NamedGenerator { Name = "Prim", Generator = new PrimGenerator() },
            new NamedGenerator { Name = "Wilson", Generator = new WilsonGenerator() },
            new NamedGenerator { Name = "Aldous-Broder", Generator = new AldousBroderGenerator() } };

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

                Maze.Generate(GeneratorComboBox.SelectedValue as Generator);
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
