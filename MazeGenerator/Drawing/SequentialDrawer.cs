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

namespace MazeGenerator.Drawing
{
    public class SequentialDrawer : Drawer
    {
        public SequentialDrawer(Canvas canvas, int width, int height) : base(canvas, width, height) { }

        public override void DrawMaze(int sleepTime = 100)
        {
            Canvas.Children.Clear();
            ResizeCanvas();

            foreach (var edge in EdgesToDraw)
            {
                var rect = GetConnectionRectangle(edge.u, edge.v);

                rect.Stroke = new SolidColorBrush(Colors.Blue);
                rect.Fill = new SolidColorBrush(Colors.Blue);
                Canvas.Children.Add(rect);
                Canvas.Dispatcher.Invoke(DispatcherPriority.Render, new Action(() => { }));

                Thread.Sleep(sleepTime);

                rect.Stroke = new SolidColorBrush(Colors.White);
                rect.Fill = new SolidColorBrush(Colors.White);
                Canvas.Dispatcher.Invoke(DispatcherPriority.Render, new Action(() => { }));
            }
        }
    }
}
