﻿using System;
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
    public class LoopErasureDrawer : Drawer
    {
        public LoopErasureDrawer(Canvas canvas, int width, int height) : base(canvas, width, height) { }

        public override void DrawMaze(int sleepTime = 50)
        {
            Canvas.Children.Clear();
            ResizeCanvas();

            Stack<((int x, int y) u, (int x, int y) v)> edgeStack = new Stack<((int x, int y) u, (int x, int y) v)>();
            Stack<Rectangle> rectangleStack = new Stack<Rectangle>();
            bool[,] visited = new bool[Width, Height];
            bool[,] inCurrentPath = new bool[Width, Height];

            foreach (var edge in EdgesToDraw)
            {
                if (edge.v == (-1, -1))
                {
                    var rect = GetFieldRectangle(edge.u.x, edge.u.y);
                    rect.Stroke = new SolidColorBrush(Colors.White);
                    rect.Fill = new SolidColorBrush(Colors.White);
                    Canvas.Children.Add(rect);
                    Canvas.Dispatcher.Invoke(DispatcherPriority.Render, new Action(() => { }));

                    visited[edge.u.x, edge.u.y] = true;

                    Thread.Sleep(sleepTime);

                    continue;
                }
                else if (visited[edge.v.x, edge.v.y])
                {
                    edgeStack.Push(edge);
                    inCurrentPath[edge.v.x, edge.v.y] = true;

                    var rect = GetConnectionRectangle(edge.u, edge.v);
                    rect.Fill = new SolidColorBrush(Colors.Blue);
                    rect.Stroke = new SolidColorBrush(Colors.Blue);
                    Canvas.Children.Add(rect);

                    Canvas.Dispatcher.Invoke(DispatcherPriority.Render, new Action(() => { }));

                    rectangleStack.Push(rect);

                    Thread.Sleep(sleepTime);

                    while (edgeStack.Count > 0)
                    {
                        visited[edgeStack.Peek().u.x, edgeStack.Peek().u.y] = true;
                        inCurrentPath[edgeStack.Peek().u.x, edgeStack.Peek().u.y] = false;
                        edgeStack.Pop();
                        rectangleStack.Pop();
                    }

                    ConfirmPendingRectangles();
                    Canvas.Dispatcher.Invoke(DispatcherPriority.Render, new Action(() => { }));

                    Thread.Sleep(sleepTime);
                }
                else if (inCurrentPath[edge.v.x, edge.v.y])
                {
                    List<Rectangle> edgesToRemove = new List<Rectangle>();
                    var edgeToBackTo = edge.v;

                    while (edgeStack.Count > 0 && edgeStack.Peek().v != edgeToBackTo)
                    {
                        inCurrentPath[edgeStack.Peek().v.x, edgeStack.Peek().v.y] = false;
                        edgesToRemove.Add(rectangleStack.Pop());
                        edgeStack.Pop();
                    }

                    foreach (var edgeToRemove in edgesToRemove)
                    {
                        edgeToRemove.Stroke = new SolidColorBrush(Colors.Red);
                        edgeToRemove.Fill = new SolidColorBrush(Colors.Red);
                    }

                    Canvas.Dispatcher.Invoke(DispatcherPriority.Render, new Action(() => { }));

                    Thread.Sleep(sleepTime);

                    foreach (var edgeToRemove in edgesToRemove)
                    {
                        Canvas.Children.Remove(edgeToRemove);
                    }

                    Canvas.Dispatcher.Invoke(DispatcherPriority.Render, new Action(() => { }));
                }
                else
                {
                    edgeStack.Push(edge);
                    inCurrentPath[edge.u.x, edge.u.y] = true;
                    inCurrentPath[edge.v.x, edge.v.y] = true;

                    var rect = GetConnectionRectangle(edge.u, edge.v);
                    rect.Fill = new SolidColorBrush(Colors.Blue);
                    rect.Stroke = new SolidColorBrush(Colors.Blue);
                    Canvas.Children.Add(rect);

                    Canvas.Dispatcher.Invoke(DispatcherPriority.Render, new Action(() => { }));

                    rectangleStack.Push(rect);

                    Thread.Sleep(sleepTime);
                }
            }
        }
    }
}
