namespace Client.Scripts.Algorithms.Legacy
{
    public class Hashing
    {
        private const int XPrime = 1619;
        private const int YPrime = 31337;
        private const int ZPrime = 6971;
        private const int WPrime = 1013;

        private static int Hash2D(int seed, int x, int y)
        {
            int hash = seed;
            hash ^= XPrime * x;
            hash ^= YPrime * y;

            hash = hash * hash * hash * 60493;
            hash = (hash >> 13) ^ hash;

            return hash;
        }

        private static int Hash3D(int seed, int x, int y, int z)
        {
            int hash = seed;
            hash ^= XPrime * x;
            hash ^= YPrime * y;
            hash ^= ZPrime * z;

            hash = hash * hash * hash * 60493;
            hash = (hash >> 13) ^ hash;

            return hash;
        }

        private static int Hash4D(int seed, int x, int y, int z, int w)
        {
            int hash = seed;
            hash ^= XPrime * x;
            hash ^= YPrime * y;
            hash ^= ZPrime * z;
            hash ^= WPrime * w;

            hash = hash * hash * hash * 60493;
            hash = (hash >> 13) ^ hash;

            return hash;
        }
    }
}