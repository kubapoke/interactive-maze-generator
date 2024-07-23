using MazeGenerator.Drawing;
using MazeGenerator.Maze.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MazeGenerator.Maze.Generators
{
    public class PrimGenerator : Generator
    {
        public override List<int>[] GenerateMaze(int width, int height, (int x, int y) start, (int x, int y) finish, bool draw = false)
        {
            List<int>[] mazeGraph = InitializeNeighborLists(width, height);
            List<int>[] fullMazeGraph = GenerateFullMazeGraph(width, height);
            SequentialDrawer drawer = new SequentialDrawer(MainWindow.Canvas, width, height);
            bool[] visited = new bool[fullMazeGraph.Length];
            List<(int u, int v)> candidateEdges = new List<(int u, int v)>();

            int startVertex = MainWindow.Rng.Next(height * width);
            int finishVertex = CoordinateConverters.CoordsToVertex(finish, width);
            visited[startVertex] = true;

            foreach(int neighbor in fullMazeGraph[startVertex])
            {
                candidateEdges.Add((startVertex, neighbor));
                if (startVertex == finishVertex)
                    break;
            }  

            while(candidateEdges.Count > 0)
            {
                int r = MainWindow.Rng.Next(candidateEdges.Count);
                var candidateEdge = candidateEdges[r];

                if (!visited[candidateEdge.v])
                {
                    visited[candidateEdge.v] = true;
                    mazeGraph[candidateEdge.u].Add(candidateEdge.v);
                    mazeGraph[candidateEdge.v].Add(candidateEdge.u);

                    if (draw)
                        drawer.AddEdgeToDraw(CoordinateConverters.VertexToCoords(candidateEdge.u, width), CoordinateConverters.VertexToCoords(candidateEdge.v, width));

                    foreach(var neighbor in fullMazeGraph[candidateEdge.v])
                    {
                        if (!visited[neighbor])
                        {
                            candidateEdges.Add((candidateEdge.v, neighbor));
                        }     
                    }
                }

                candidateEdges.Remove(candidateEdge);
            }

            if (draw)
                drawer.DrawMaze();

            return mazeGraph;
        }
    }
}
