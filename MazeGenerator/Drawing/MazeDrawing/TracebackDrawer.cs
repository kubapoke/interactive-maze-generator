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
using System.Windows;

namespace MazeGenerator.Drawing
{
    public class TracebackDrawer : Drawer
    {
        public TracebackDrawer(Canvas canvas, int width, int height) : base(canvas, width, height) { }

        public override async Task DrawAsync(int sleepTime = 50)
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
            await Application.Current.Dispatcher.InvokeAsync(() => { }, DispatcherPriority.Render);

            idx++;

            while (edgeStack.Count > 0 && idx < EdgesToDraw.Count)
            {
                if (ShouldFinishDrawing)
                {
                    ShouldFinishDrawing = false;
                    return;
                }

                while (edgeStack.Count > 0 && edgeStack[edgeStack.Count - 1].v != EdgesToDraw[idx].u)
                {
                    if (ShouldFinishDrawing)
                    {
                        await FinishMazeDrawing();
                        return;
                    }

                    edgeStack.RemoveAt(edgeStack.Count - 1);

                    rectangleStack[rectangleStack.Count - 1].Stroke = new SolidColorBrush(Colors.White);
                    rectangleStack[rectangleStack.Count - 1].Fill = new SolidColorBrush(Colors.White);
                    await Application.Current.Dispatcher.InvokeAsync(() => { }, DispatcherPriority.Render);

                    rectangleStack.RemoveAt(rectangleStack.Count - 1);
                    await Task.Delay(sleepTime);
                }


                edgeStack.Add(EdgesToDraw[idx]);

                rectangleStack.Add(GetConnectionRectangle(EdgesToDraw[idx].u, EdgesToDraw[idx].v));
                rectangleStack[rectangleStack.Count - 1].Stroke = new SolidColorBrush(Colors.Blue);
                rectangleStack[rectangleStack.Count - 1].Fill = new SolidColorBrush(Colors.Blue);
                Canvas.Children.Add(rectangleStack[rectangleStack.Count - 1]);
                await Application.Current.Dispatcher.InvokeAsync(() => { }, DispatcherPriority.Render);

                idx++;

                await Task.Delay(sleepTime);
            }

            while (edgeStack.Count > 0)
            {
                if (ShouldFinishDrawing)
                {
                    await FinishMazeDrawing();
                    return;
                }

                edgeStack.RemoveAt(edgeStack.Count - 1);

                rectangleStack[rectangleStack.Count - 1].Stroke = new SolidColorBrush(Colors.White);
                rectangleStack[rectangleStack.Count - 1].Fill = new SolidColorBrush(Colors.White);
                await Application.Current.Dispatcher.InvokeAsync(() => { }, DispatcherPriority.Render);

                rectangleStack.RemoveAt(rectangleStack.Count - 1);

                await Task.Delay(sleepTime);
            }
        }
    }
}
