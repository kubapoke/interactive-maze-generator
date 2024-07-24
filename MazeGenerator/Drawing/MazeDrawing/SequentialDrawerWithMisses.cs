using MazeGenerator.Maze.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media.Media3D;
using System.Windows.Media;
using System.Windows;
using System.Windows.Threading;

namespace MazeGenerator.Drawing
{
    public class SequentialDrawerWithMisses : Drawer
    {
        public SequentialDrawerWithMisses(Canvas canvas, int width, int height) : base(canvas, width, height) { }

        public override async Task DrawAsync(int sleepTime = 100)
        {
            Canvas.Children.Clear();
            ResizeCanvas();

            bool[] visited = new bool[Width * Height];

            visited[CoordinateConverters.CoordsToVertex(EdgesToDraw[0].u, Width)] = true;

            foreach (var edge in EdgesToDraw)
            {
                if (ShouldFinishDrawing)
                {
                    ShouldFinishDrawing = false;
                    return;
                }

                var rect = GetConnectionRectangle(edge.u, edge.v);

                if (visited[CoordinateConverters.CoordsToVertex(edge.v, Width)])
                {
                    rect.Stroke = new SolidColorBrush(Colors.Red);
                    rect.Fill = new SolidColorBrush(Colors.Red);
                    Canvas.Children.Add(rect);
                    await Application.Current.Dispatcher.InvokeAsync(() => { }, DispatcherPriority.Render);

                    await Task.Delay(sleepTime);

                    Canvas.Children.Remove(rect);
                    await Application.Current.Dispatcher.InvokeAsync(() => { }, DispatcherPriority.Render);
                }
                else
                {
                    visited[CoordinateConverters.CoordsToVertex(edge.u, Width)] = true;
                    visited[CoordinateConverters.CoordsToVertex(edge.v, Width)] = true;

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

        protected override async Task FinishMazeDrawing()
        {
            Canvas.Children.Clear();
            ResizeCanvas();

            bool[] visited = new bool[Width * Height];

            visited[CoordinateConverters.CoordsToVertex(EdgesToDraw[0].u, Width)] = true;

            foreach (var edge in EdgesToDraw)
            {
                var rect = GetConnectionRectangle(edge.u, edge.v);

                if (!visited[CoordinateConverters.CoordsToVertex(edge.v, Width)])
                {
                    visited[CoordinateConverters.CoordsToVertex(edge.u, Width)] = true;
                    visited[CoordinateConverters.CoordsToVertex(edge.v, Width)] = true;

                    rect.Stroke = new SolidColorBrush(Colors.White);
                    rect.Fill = new SolidColorBrush(Colors.White);
                    Canvas.Children.Add(rect);
                }
            }

            await Application.Current.Dispatcher.InvokeAsync(() => { }, DispatcherPriority.Render);
        }
    }
}
