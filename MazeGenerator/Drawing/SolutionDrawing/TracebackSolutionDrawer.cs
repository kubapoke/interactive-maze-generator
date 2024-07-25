using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Threading;
using System.Windows;

namespace MazeGenerator.Drawing.SolutionDrawing
{
    public class TracebackSolutionDrawer : Drawer
    {
        public TracebackSolutionDrawer(Canvas canvas, int width, int height) : base(canvas, width, height) { }

        public override async Task DrawAsync(int sleepTime = 50)
        {
            RemoveLinesFromCanvas(Canvas);
            await Application.Current.Dispatcher.InvokeAsync(() => { }, DispatcherPriority.Render);

            Stack<((int x, int y) u, (int x, int y) v)> edgeStack = new Stack<((int x, int y) u, (int x, int y) v)>();
            Stack<Line> lineStack = new Stack<Line>();

            foreach (var edge in EdgesToDraw)
            {
                Line line = GetConnectionLine(edge.u, edge.v);
                line.Stroke = new SolidColorBrush(Colors.Orange);

                while(edgeStack.Count > 0 && edgeStack.Peek().v != edge.u)
                {
                    edgeStack.Pop();
                    Canvas.Children.Remove(lineStack.Pop());

                    await Application.Current.Dispatcher.InvokeAsync(() => { }, DispatcherPriority.Render);
                    await Task.Delay(sleepTime);
                }

                Canvas.Children.Add(line);
                edgeStack.Push(edge);
                lineStack.Push(line);

                await Application.Current.Dispatcher.InvokeAsync(() => { }, DispatcherPriority.Render);
                await Task.Delay(sleepTime);
            }
        }
    }
}
