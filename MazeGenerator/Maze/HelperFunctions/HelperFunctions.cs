namespace MazeGenerator.Maze.Helpers
{
    internal class HelperFunctions
    {
        private static int CoordsToVertex(int x, int y, int width)
        {
            return x + y * width;
        }

        private static (int x, int y) VertexToCoords(int v, int width)
        {
            return (v % width, v / width);
        }
    }
}
