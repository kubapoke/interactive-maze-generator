using MazeGenerator.Drawing;
using MazeGenerator.Maze.Generators;
using MazeGenerator.Maze.Solvers;
using System.ComponentModel;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace MazeGenerator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        public Maze.Maze Maze;
        private bool _IsBusy = false;
        private bool _IsGenerated = false;
        private bool _CanFinishGenerating = false;
        private bool _CanFinishSolving = false;
        public static Canvas Canvas;
        public static DockPanel DockPanel;
        public static Random Rng = new Random();
        public event PropertyChangedEventHandler PropertyChanged;

        public List<NamedGenerator> GeneratorList { get; set; }
        public List<NamedSolver> SolverList { get; set; }

        public bool IsBusy
        {
            get => _IsBusy;
            set
            {
                if (_IsBusy != value)
                {
                    _IsBusy = value;
                    OnPropertyChanged(nameof(IsBusy));
                }
            }
        }

        public bool IsGenerated
        {
            get => _IsGenerated;
            set
            {
                if (_IsGenerated != value)
                {
                    _IsGenerated = value;
                    OnPropertyChanged(nameof(IsGenerated));
                }
            }
        }

        public bool CanFinishGenerating
        {
            get => _CanFinishGenerating;
            set
            {
                if (_CanFinishGenerating != value)
                {
                    _CanFinishGenerating = value;
                    OnPropertyChanged(nameof(CanFinishGenerating));
                }
            }
        }

        public bool CanFinishSolving
        {
            get => _CanFinishSolving;
            set
            {
                if (_CanFinishSolving != value)
                {
                    _CanFinishSolving = value;
                    OnPropertyChanged(nameof(CanFinishSolving));
                }
            }
        }

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

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

            SolverList = new List<NamedSolver>
            { new NamedSolver { Name = "DFS", Solver = new DFSSolver() },
            new NamedSolver { Name = "BFS", Solver = new BFSSolver() },
            new NamedSolver { Name = "A*", Solver = new AStarSolver() } };

            DataContext = this;
        }

        public struct NamedGenerator
        {
            public string Name { get; set; }
            public Generator Generator { get; set; }
        }

        public struct NamedSolver
        {
            public string Name { get; set; }
            public Solver Solver { get; set; }
        }

        private async void GenerateButton_Click(object sender, RoutedEventArgs e)
        {
            try // temporary solution, input validation should be done separately
            {
                Maze = new Maze.Maze(int.Parse(WidthTextBox.Text), int.Parse(HeightTextBox.Text));

                IsBusy = true;
                IsGenerated = false;
                CanFinishGenerating = true;

                await Maze.GenerateAsync(GeneratorComboBox.SelectedValue as Generator);

                IsBusy = false;
                IsGenerated = true;
                CanFinishGenerating = false;

                StartXTextBox.Text = "1";
                StartYTextBox.Text = "1";

                FinishXTextBox.Text = WidthTextBox.Text;
                FinishYTextBox.Text = HeightTextBox.Text;
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.Message);

                IsBusy = false;
            }
        }

        private async void FinishGeneratingButton_Click(object sender, RoutedEventArgs e)
        {
            CanFinishGenerating = false;
            await Drawer.FinishDrawing();
        }

        private async void SolveButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Maze.UpdateStart(int.Parse(StartXTextBox.Text) - 1, int.Parse(StartYTextBox.Text) - 1);
                Maze.UpdateFinish(int.Parse(FinishXTextBox.Text) - 1, int.Parse(FinishYTextBox.Text) - 1);

                IsBusy = true;
                IsGenerated = false;
                CanFinishSolving = true;

                await Maze.SolveAsync(SolverComboBox.SelectedValue as Solver);

                IsBusy = false;
                IsGenerated = true;
                CanFinishSolving = false;
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private async void FinishSolvingButton_Click(object sender, RoutedEventArgs e)
        {
            CanFinishSolving = false;
            await Drawer.FinishDrawing();
        }
    }

    public class ReverseBoolConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return !(bool)value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
