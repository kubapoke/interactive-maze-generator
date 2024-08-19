using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;

namespace MazeGenerator.Drawing
{
    public class SequentialMazeDrawer : Drawer
    {
        public SequentialMazeDrawer(Canvas canvas, int width, int height) : base(canvas, width, height) { }

        public override async Task DrawAsync(int sleepTime = 100)
        {
            Canvas.Children.Clear();
            ResizeCanvas();

            foreach (var edge in EdgesToDraw)
            {
                if (ShouldFinishDrawing)
                {
                    ShouldFinishDrawing = false;
                    return;
                }

                var rect = GetConnectionRectangle(edge.u, edge.v);

                rect.Stroke = new SolidColorBrush(Colors.Blue);
                rect.Fill = new SolidColorBrush(Colors.Blue);
                Canvas.Children.Add(rect);
                await Application.Current.Dispatcher.InvokeAsync(() => { }, DispatcherPriority.Render);

                await Task.Delay(sleepTime);

                rect.Stroke = new SolidColorBrush(Colors.White);
                rect.Fill = new SolidColorBrush(Colors.White);
                await Application.Current.Dispatcher.InvokeAsync(() => { }, DispatcherPriority.Render);
            }
        }
    }
}
