using System;
using System.Collections.Generic;

namespace MazeGenerator.Maze.Helpers
{
    internal class Shuffler
    {
        public static void FisherYatesShuffle<T>(IList<T> list)
        {
            Random rnd = new Random();

            for (int i = list.Count; i > 0; i--)
            {
                int toSwap = rnd.Next(i);

                T temp = list[i - 1];
                list[i - 1] = list[toSwap];
                list[toSwap] = temp;
            }
        }
    }
}
