﻿using System;
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


        // function should be changed, used for testing
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
    }
}