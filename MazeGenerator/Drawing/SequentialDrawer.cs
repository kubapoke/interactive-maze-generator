using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media.Media3D;
using System.Windows.Media;
using System.Windows.Threading;
using System.Windows;

namespace MazeGenerator.Drawing
{
    public class SequentialDrawer : Drawer
    {
        public SequentialDrawer(Canvas canvas, int width, int height) : base(canvas, width, height) { }

        public override async Task DrawMazeAsync(int sleepTime = 100)
        {
            Canvas.Children.Clear();
            ResizeCanvas();

            foreach (var edge in EdgesToDraw)
            {
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
