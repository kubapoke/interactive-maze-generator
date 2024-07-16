namespace MazeGenerator.Maze.Helpers
{
    public class CoordinateConverters
    {
        public static int CoordsToVertex(int x, int y, int width)
        {
            return x + y * width;
        }

        public static int CoordsToVertex((int x, int y) coords, int width)
        {
            return coords.x + coords.y * width;
        }

        public static (int x, int y) VertexToCoords(int v, int width)
        {
            return (v % width, v / width);
        }
    }
}
