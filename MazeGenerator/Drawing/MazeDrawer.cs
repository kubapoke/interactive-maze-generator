using MazeGenerator.Maze.Helpers;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace MazeGenerator.Drawing
{
    public class MazeDrawer
    {
        public static void ResizeCanvas(Canvas canvas, int width, int height)
        {
            FrameworkElement parent = canvas.Parent as FrameworkElement;
            int parentWidth = (int)Math.Floor(parent.ActualWidth * 2.0 / 3.0 - canvas.Margin.Left - canvas.Margin.Right);
            int parentHeight = (int)Math.Floor(parent.ActualHeight - MainWindow.DockPanel.ActualHeight - canvas.Margin.Top - canvas.Margin.Bottom);

            if (parent != null)
            {
                int proposedWidth = parentWidth;
                int proposedHeight = (int)((double)proposedWidth * (double)height / (double)width);

                if (proposedHeight > parentHeight)
                {
                    proposedHeight = parentHeight;
                    proposedWidth = (int)((double)proposedHeight * (double)width / (double)height);
                }

                canvas.Width = proposedWidth;
                canvas.Height = proposedHeight;
            }
        }

        public static Rectangle GetFieldRectangle(Canvas canvas, int width, int height, int x, int y)
        {
            Rectangle rect = new Rectangle();
            double outlinedWidthHeight = canvas.Width / (double)width;
            rect.Width = rect.Height = outlinedWidthHeight * 0.9;
            Canvas.SetLeft(rect, outlinedWidthHeight * (x + 0.05));
            Canvas.SetTop(rect, outlinedWidthHeight * (y + 0.05));

            return rect;
        }

        public static Rectangle GetConnectionRectangle(Canvas canvas, int width, int height, (int x, int y) u, (int x, int y) v)
        {
            Rectangle rect = new Rectangle();

            double outlinedWidthHeight = canvas.Width / (double)width;

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

        public static void DrawMazeInOrder(Canvas canvas, int width, int height, List<((int x, int y) u, (int x, int y) v)> edgesToDraw, int sleepTime = 100)
        {
            foreach (var edge in edgesToDraw)
            {
                var rect = GetConnectionRectangle(canvas, width, height, edge.u, edge.v);

                rect.Stroke = new SolidColorBrush(Colors.Blue);
                rect.Fill = new SolidColorBrush(Colors.Blue);
                canvas.Children.Add(rect);
                canvas.Dispatcher.Invoke(DispatcherPriority.Render, new Action(() => { }));

                Thread.Sleep(sleepTime);

                rect.Stroke = new SolidColorBrush(Colors.White);
                rect.Fill = new SolidColorBrush(Colors.White);
                canvas.Dispatcher.Invoke(DispatcherPriority.Render, new Action(() => { }));
            }
        }

        public static void DrawMazeInOrderWithMisses(Canvas canvas, int width, int height, (int x, int y) finish, List<((int x, int y) u, (int x, int y) v)> edgesToDraw, int sleepTime = 100)
        {
            bool finishConnected = false;
            bool[] visited = new bool[width * height];

            visited[CoordinateConverters.CoordsToVertex(edgesToDraw[0].u, width)] = true;

            foreach (var edge in edgesToDraw)
            {
                var rect = GetConnectionRectangle(canvas, width, height, edge.u, edge.v);

                if (visited[CoordinateConverters.CoordsToVertex(edge.v, width)] || (finishConnected && (edge.u == finish || edge.v == finish)))
                {
                    rect.Stroke = new SolidColorBrush(Colors.Red);
                    rect.Fill = new SolidColorBrush(Colors.Red);
                    canvas.Children.Add(rect);
                    canvas.Dispatcher.Invoke(DispatcherPriority.Render, new Action(() => { }));

                    Thread.Sleep(sleepTime);

                    canvas.Children.Remove(rect);
                    canvas.Dispatcher.Invoke(DispatcherPriority.Render, new Action(() => { }));
                }
                else
                {
                    visited[CoordinateConverters.CoordsToVertex(edge.u, width)] = true;
                    visited[CoordinateConverters.CoordsToVertex(edge.v, width)] = true;

                    if (edge.u == finish || edge.v == finish)
                        finishConnected = true;

                    rect.Stroke = new SolidColorBrush(Colors.Blue);
                    rect.Fill = new SolidColorBrush(Colors.Blue);
                    canvas.Children.Add(rect);
                    canvas.Dispatcher.Invoke(DispatcherPriority.Render, new Action(() => { }));

                    Thread.Sleep(sleepTime);

                    rect.Stroke = new SolidColorBrush(Colors.White);
                    rect.Fill = new SolidColorBrush(Colors.White);
                    canvas.Dispatcher.Invoke(DispatcherPriority.Render, new Action(() => { }));
                }     
            }
        }

        public static void DrawMazeWithTraceback(Canvas canvas, int width, int height, List<((int x, int y) u, (int x, int y) v)> edgesToDraw, int sleepTime = 50)
        {
            List<((int x, int y) u, (int x, int y) v)> edgeStack = new List<((int x, int y) u, (int x, int y) v)>();
            List<Rectangle> rectangleStack = new List<Rectangle>();
            int idx = 0;

            edgeStack.Add(edgesToDraw[idx]);

            rectangleStack.Add(GetConnectionRectangle(canvas, width, height, edgesToDraw[idx].u, edgesToDraw[idx].v));
            rectangleStack[rectangleStack.Count - 1].Stroke = new SolidColorBrush(Colors.Blue);
            rectangleStack[rectangleStack.Count - 1].Fill = new SolidColorBrush(Colors.Blue);
            canvas.Children.Add(rectangleStack[rectangleStack.Count - 1]);
            canvas.Dispatcher.Invoke(DispatcherPriority.Render, new Action(() => { }));

            idx++;

            while (edgeStack.Count > 0 && idx < edgesToDraw.Count)
            {
                while (edgeStack.Count > 0 && edgeStack[edgeStack.Count - 1].v != edgesToDraw[idx].u)
                {
                    edgeStack.RemoveAt(edgeStack.Count - 1);

                    rectangleStack[rectangleStack.Count - 1].Stroke = new SolidColorBrush(Colors.White);
                    rectangleStack[rectangleStack.Count - 1].Fill = new SolidColorBrush(Colors.White);
                    canvas.Dispatcher.Invoke(DispatcherPriority.Render, new Action(() => { }));

                    rectangleStack.RemoveAt(rectangleStack.Count - 1);

                    Thread.Sleep(sleepTime);
                }


                edgeStack.Add(edgesToDraw[idx]);

                rectangleStack.Add(GetConnectionRectangle(canvas, width, height, edgesToDraw[idx].u, edgesToDraw[idx].v));
                rectangleStack[rectangleStack.Count - 1].Stroke = new SolidColorBrush(Colors.Blue);
                rectangleStack[rectangleStack.Count - 1].Fill = new SolidColorBrush(Colors.Blue);
                canvas.Children.Add(rectangleStack[rectangleStack.Count - 1]);
                canvas.Dispatcher.Invoke(DispatcherPriority.Render, new Action(() => { }));

                idx++;

                Thread.Sleep(sleepTime);
            }

            while (edgeStack.Count > 0)
            {
                edgeStack.RemoveAt(edgeStack.Count - 1);

                rectangleStack[rectangleStack.Count - 1].Stroke = new SolidColorBrush(Colors.White);
                rectangleStack[rectangleStack.Count - 1].Fill = new SolidColorBrush(Colors.White);
                canvas.Dispatcher.Invoke(DispatcherPriority.Render, new Action(() => { }));

                rectangleStack.RemoveAt(rectangleStack.Count - 1);

                Thread.Sleep(sleepTime);
            }
        }
    }
}
