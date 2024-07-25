using MazeGenerator.Maze.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Threading;
using System.Threading.Tasks;

namespace MazeGenerator.Drawing
{
    public abstract class Drawer
    {
        protected Canvas Canvas;
        protected int Width, Height;
        protected List<((int x, int y) u, (int x, int y) v)> EdgesToDraw = new List<((int x, int y) u, (int x, int y) v)>();
        protected bool ShouldFinishDrawing = false;

        public static event EventHandler FinishDrawingEventHandler;

        public Drawer(Canvas canvas, int width, int height)
        {
            Canvas = canvas;
            Width = width;
            Height = height;

            FinishDrawingEventHandler += OnFinishDrawing;
        }

        public void AddEdgeToDraw((int x, int y) u, (int x, int y) v)
        {
            EdgesToDraw.Add((u, v));
        }

        protected void ResizeCanvas()
        {
            FrameworkElement parent = Canvas.Parent as FrameworkElement;
            int parentWidth = (int)Math.Floor(parent.ActualWidth * 2.0 / 3.0 - Canvas.Margin.Left - Canvas.Margin.Right);
            int parentHeight = (int)Math.Floor(parent.ActualHeight - MainWindow.DockPanel.ActualHeight - Canvas.Margin.Top - Canvas.Margin.Bottom);

            if (parent != null)
            {
                int proposedWidth = parentWidth;
                int proposedHeight = (int)((double)proposedWidth * (double)Height / (double)Width);

                if (proposedHeight > parentHeight)
                {
                    proposedHeight = parentHeight;
                    proposedWidth = (int)((double)proposedHeight * (double)Width / (double)Height);
                }

                Canvas.Width = proposedWidth;
                Canvas.Height = proposedHeight;
            }
        }

        protected Rectangle GetFieldRectangle(int x, int y)
        {
            Rectangle rect = new Rectangle();
            double outlinedWidthHeight = Canvas.Width / (double)Width;
            rect.Width = rect.Height = outlinedWidthHeight * 0.9;
            Canvas.SetLeft(rect, outlinedWidthHeight * (x + 0.05));
            Canvas.SetTop(rect, outlinedWidthHeight * (y + 0.05));

            return rect;
        }

        protected Rectangle GetConnectionRectangle((int x, int y) u, (int x, int y) v)
        {
            Rectangle rect = new Rectangle();

            double outlinedWidthHeight = Canvas.Width / (double)Width;

            if (u.x == v.x && Math.Abs(u.y - v.y) == 1)
            {
                rect.Width = outlinedWidthHeight * 0.9;
                rect.Height = outlinedWidthHeight * 1.9;
                Canvas.SetLeft(rect, outlinedWidthHeight * (u.x + 0.05));
                Canvas.SetTop(rect, outlinedWidthHeight * (Math.Min(u.y, v.y) + 0.05));
            }
            else if (u.y == v.y && Math.Abs(u.x - v.x) == 1)
            {
                rect.Width = outlinedWidthHeight * 1.9;
                rect.Height = outlinedWidthHeight * 0.9;
                Canvas.SetLeft(rect, outlinedWidthHeight * (Math.Min(u.x, v.x) + 0.05));
                Canvas.SetTop(rect, outlinedWidthHeight * (u.y + 0.05));
            }
            else
            {
                throw new ArgumentException();
            }

            return rect;
        }

        protected Line GetConnectionLine((int x, int y) u, (int x, int y) v)
        {
            Line line = new Line();

            double outlinedWidthHeight = Canvas.Width / (double)Width;
            line.StrokeThickness = outlinedWidthHeight / 4;

            if (u.x == v.x && Math.Abs(u.y - v.y) == 1)
            {
                line.X1 = line.X2 = ((double)u.x + 0.5) * outlinedWidthHeight;
                line.Y1 = ((double)u.y + 0.5) * outlinedWidthHeight;
                line.Y2 = ((double)v.y + 0.5) * outlinedWidthHeight;
            }
            else if (u.y == v.y && Math.Abs(u.x - v.x) == 1)
            {
                line.Y1 = line.Y2 = ((double)u.y + 0.5) * outlinedWidthHeight;
                line.X1 = ((double)u.x + 0.5) * outlinedWidthHeight;
                line.X2 = ((double)v.x + 0.5) * outlinedWidthHeight;
            }
            else
            {
                throw new ArgumentException();
            }

            return line;
        }

        protected void ConfirmPendingRectangles()
        {
            foreach(var child in Canvas.Children)
            {
                if(child is Rectangle)
                {
                    var rect = child as Rectangle;
                    rect.Stroke = new SolidColorBrush(Colors.White);
                    rect.Fill = new SolidColorBrush(Colors.White);

                }
            }
        }
        public abstract Task DrawAsync(int sleepTime = 100);

        protected virtual async Task FinishMazeDrawing()
        {
            Canvas.Children.Clear();
            ResizeCanvas();

            foreach(var edge in EdgesToDraw)
            {
                var rect = GetConnectionRectangle(edge.u, edge.v);
                rect.Fill = new SolidColorBrush(Colors.White);
                rect.Stroke = new SolidColorBrush(Colors.White);
                Canvas.Children.Add(rect);
            }

            await Application.Current.Dispatcher.InvokeAsync(() => { }, DispatcherPriority.Render);
        }

        public static async Task FinishDrawing()
        {
            await Task.Run(() => FinishDrawingEventHandler.Invoke(new object(), new EventArgs()));
        }

        protected async void OnFinishDrawing(object sender, EventArgs e)
        {
            ShouldFinishDrawing = true;
            await Application.Current.Dispatcher.InvokeAsync(async () => await FinishMazeDrawing());
        }

        protected static void RemoveLinesFromCanvas(Canvas canvas)
        {
            List<Line> linesToRemove = new List<Line>();

            foreach(var child in canvas.Children)
            {
                if(child is Line)
                {
                    linesToRemove.Add(child as Line);
                }
            }

            foreach(var line in linesToRemove)
            {
                canvas.Children.Remove(line);
            }
        }
    }
}
