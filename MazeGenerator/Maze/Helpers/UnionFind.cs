namespace MazeGenerator.Maze.Helpers
{
    public class UnionFind
    {
        private int N;
        private int[] parent;
        private int[] size;
        public UnionFind(int n)
        {
            N = n;
            parent = new int[N];
            size = new int[N];

            for (int i = 0; i < N; i++)
            {
                parent[i] = i;
                size[i] = 1;
            }
        }

        public int Find(int v)
        {
            if (v == parent[v])
                return v;

            return parent[v] = Find(parent[v]);
        }

        public bool IsConnected(int v, int u)
        {
            return Find(v) == Find(u);
        }

        public bool Union(int v, int u)
        {
            if (IsConnected(v, u))
                return false;

            v = Find(v);
            u = Find(u);

            if (size[v] < size[u])
            {
                int t = v;
                v = u;
                u = t;
            }

            parent[u] = v;
            size[v] += size[u];
            size[u] = -1;

            return true;
        }
    }
}
