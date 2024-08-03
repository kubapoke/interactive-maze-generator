using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace MazeGenerator.Drawing.SolutionDrawing
{
    public class ParallelPathSolutionDrawer : Drawer
    {
        public ParallelPathSolutionDrawer(Canvas canvas, int width, int height) : base(canvas, width, height)
        {
            IsSolutionDrawer = true;
        }

        public override async Task DrawAsync(int sleepTime = 100)
        {
            RemoveLinesFromCanvas(Canvas);
            await Application.Current.Dispatcher.InvokeAsync(() => { }, DispatcherPriority.Render);

            (int x, int y)[,] prev = new (int x, int y)[Width, Height];
            Line[,] lines = new Line[Width, Height];

            foreach (var edge in EdgesToDraw)
            {
                if (ShouldFinishDrawing)
                {
                    ShouldFinishDrawing = false;
                    return;
                }

                Line line = GetConnectionLine(edge.u, edge.v);
                prev[edge.v.x, edge.v.y] = edge.u;
                lines[edge.v.x, edge.v.y] = line;

                Canvas.Children.Add(line);
                GrayOutCurrentLines();

                (int x, int y) vertex = edge.v;

                while (lines[vertex.x, vertex.y] != null)
                {
                    if (ShouldFinishDrawing)
                    {
                        ShouldFinishDrawing = false;
                        return;
                    }

                    line = lines[vertex.x, vertex.y];

                    Canvas.Children.Remove(line);
                    Canvas.Children.Add(line);

                    line.Stroke = new SolidColorBrush(Colors.Orange);

                    vertex = prev[vertex.x, vertex.y];
                }

                await Application.Current.Dispatcher.InvokeAsync(() => { }, DispatcherPriority.Render);
                await Task.Delay(sleepTime);
            }
        }
    }
}
