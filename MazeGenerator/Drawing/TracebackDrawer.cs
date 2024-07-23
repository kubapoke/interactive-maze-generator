using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media.Media3D;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace MazeGenerator.Drawing
{
    public class TracebackDrawer : Drawer
    {
        public TracebackDrawer(Canvas canvas, int width, int height) : base(canvas, width, height) { }

        public override void DrawMaze(int sleepTime = 50)
        {
            Canvas.Children.Clear();
            ResizeCanvas();

            List<((int x, int y) u, (int x, int y) v)> edgeStack = new List<((int x, int y) u, (int x, int y) v)>();
            List<Rectangle> rectangleStack = new List<Rectangle>();
            int idx = 0;

            edgeStack.Add(EdgesToDraw[idx]);

            rectangleStack.Add(GetConnectionRectangle(EdgesToDraw[idx].u, EdgesToDraw[idx].v));
            rectangleStack[rectangleStack.Count - 1].Stroke = new SolidColorBrush(Colors.Blue);
            rectangleStack[rectangleStack.Count - 1].Fill = new SolidColorBrush(Colors.Blue);
            Canvas.Children.Add(rectangleStack[rectangleStack.Count - 1]);
            Canvas.Dispatcher.Invoke(DispatcherPriority.Render, new Action(() => { }));

            idx++;

            while (edgeStack.Count > 0 && idx < EdgesToDraw.Count)
            {
                while (edgeStack.Count > 0 && edgeStack[edgeStack.Count - 1].v != EdgesToDraw[idx].u)
                {
                    edgeStack.RemoveAt(edgeStack.Count - 1);

                    rectangleStack[rectangleStack.Count - 1].Stroke = new SolidColorBrush(Colors.White);
                    rectangleStack[rectangleStack.Count - 1].Fill = new SolidColorBrush(Colors.White);
                    Canvas.Dispatcher.Invoke(DispatcherPriority.Render, new Action(() => { }));

                    rectangleStack.RemoveAt(rectangleStack.Count - 1);
                    Thread.Sleep(sleepTime);
                }


                edgeStack.Add(EdgesToDraw[idx]);

                rectangleStack.Add(GetConnectionRectangle(EdgesToDraw[idx].u, EdgesToDraw[idx].v));
                rectangleStack[rectangleStack.Count - 1].Stroke = new SolidColorBrush(Colors.Blue);
                rectangleStack[rectangleStack.Count - 1].Fill = new SolidColorBrush(Colors.Blue);
                Canvas.Children.Add(rectangleStack[rectangleStack.Count - 1]);
                Canvas.Dispatcher.Invoke(DispatcherPriority.Render, new Action(() => { }));

                idx++;

                Thread.Sleep(sleepTime);
            }

            while (edgeStack.Count > 0)
            {
                edgeStack.RemoveAt(edgeStack.Count - 1);

                rectangleStack[rectangleStack.Count - 1].Stroke = new SolidColorBrush(Colors.White);
                rectangleStack[rectangleStack.Count - 1].Fill = new SolidColorBrush(Colors.White);
                Canvas.Dispatcher.Invoke(DispatcherPriority.Render, new Action(() => { }));

                rectangleStack.RemoveAt(rectangleStack.Count - 1);

                Thread.Sleep(sleepTime);
            }
        }
    }
}
