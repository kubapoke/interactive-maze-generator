using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace MazeGenerator.Drawing.SolutionDrawing
{
    public class TracebackSolutionDrawer : Drawer
    {
        public TracebackSolutionDrawer(Canvas canvas, int width, int height, bool? showPreviouslyTraversedPath = true) : base(canvas, width, height)
        {
            IsSolutionDrawer = true;
        }

        public override async Task DrawAsync(int sleepTime = 50)
        {
            RemoveLinesFromCanvas(Canvas);
            await Application.Current.Dispatcher.InvokeAsync(() => { }, DispatcherPriority.Render);

            Stack<((int x, int y) u, (int x, int y) v)> edgeStack = new Stack<((int x, int y) u, (int x, int y) v)>();
            Stack<Line> lineStack = new Stack<Line>();

            foreach (var edge in EdgesToDraw)
            {
                if (ShouldFinishDrawing)
                {
                    ShouldFinishDrawing = false;
                    return;
                }

                Line line = GetConnectionLine(edge.u, edge.v);
                line.Stroke = new SolidColorBrush(Colors.Orange);

                while (edgeStack.Count > 0 && edgeStack.Peek().v != edge.u)
                {
                    if (ShouldFinishDrawing)
                    {
                        ShouldFinishDrawing = false;
                        return;
                    }

                    edgeStack.Pop();
                    lineStack.Pop().Stroke = new SolidColorBrush(Colors.LightGray);

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
