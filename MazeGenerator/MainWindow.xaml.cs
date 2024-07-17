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
