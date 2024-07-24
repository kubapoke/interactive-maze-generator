using MazeGenerator.Drawing;
using MazeGenerator.Maze.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MazeGenerator.Maze.Generators
{
    public class WilsonGenerator : Generator
    {
        public override (List<int>[] maze, Drawer drawer) GenerateMaze(int width, int height, (int x, int y) start, (int x, int y) finish)
        {
            List<int>[] mazeGraph = InitializeNeighborLists(width, height);
            List<int>[] fullMazeGraph = GenerateFullMazeGraph(width, height);
            List<int> unusedVertices = new List<int>();
            LoopErasureDrawer drawer = new LoopErasureDrawer(MainWindow.Canvas, width, height);
            bool[] visited = new bool[fullMazeGraph.Length];
            bool[] inCurrentPath = new bool[fullMazeGraph.Length];
            Stack<int> vertexStack = new Stack<int>();

            int startVertex = MainWindow.Rng.Next(height * width);
            int finishVertex = CoordinateConverters.CoordsToVertex(finish, width);
            int visitedVertices = 1;
            visited[startVertex] = true;

            for(int i = 0; i < height * width; i++)
            {
                if (i != startVertex)
                    unusedVertices.Add(i);
            }

            drawer.AddEdgeToDraw(CoordinateConverters.VertexToCoords(startVertex, width), (-1, -1));

            while(visitedVertices < width * height)
            {
                int r = MainWindow.Rng.Next(unusedVertices.Count);
                int currentVertex = unusedVertices[r];
                int prevVertex = -1;

                vertexStack.Push(currentVertex);
                inCurrentPath[currentVertex] = true;

                while(!visited[vertexStack.Peek()])
                {
                    r = MainWindow.Rng.Next(fullMazeGraph[currentVertex].Count - 1);
                    int nextVertex = fullMazeGraph[currentVertex][r];

                    if (nextVertex == prevVertex)
                        nextVertex = fullMazeGraph[currentVertex][fullMazeGraph[currentVertex].Count - 1];

                    if (inCurrentPath[nextVertex])
                    {
                        while (vertexStack.Peek() != nextVertex)
                        {
                            inCurrentPath[vertexStack.Pop()] = false;
                        }
                        
                        vertexStack.Pop();
                        if (vertexStack.Count > 0)
                        {
                            prevVertex = vertexStack.Peek();
                        }
                        else
                        {
                            Shuffler.FisherYatesShuffle(fullMazeGraph[nextVertex]);
                            prevVertex = -1;
                        }
                        vertexStack.Push(nextVertex);
                    }
                    else
                    {
                        vertexStack.Push(nextVertex);
                        inCurrentPath[nextVertex] = true;
                    }

                    drawer.AddEdgeToDraw(CoordinateConverters.VertexToCoords(currentVertex, width), CoordinateConverters.VertexToCoords(nextVertex, width));

                    prevVertex = currentVertex;
                    currentVertex = nextVertex;
                }

                visitedVertices += vertexStack.Count - 1;

                while(vertexStack.Count > 1)
                {
                    int topVertex = vertexStack.Pop();
                    int bottomVertex = vertexStack.Peek();

                    visited[topVertex] = true;
                    inCurrentPath[topVertex] = false;
                    unusedVertices.Remove(topVertex);

                    mazeGraph[topVertex].Add(bottomVertex);
                    mazeGraph[bottomVertex].Add(topVertex);
                }

                int lastVertex = vertexStack.Pop();
                visited[lastVertex] = true;
                inCurrentPath[lastVertex] = false;
                unusedVertices.Remove(lastVertex);
            }

            return (mazeGraph, drawer);
        }
    }
}
