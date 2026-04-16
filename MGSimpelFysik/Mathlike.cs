using Microsoft.Xna.Framework;

namespace MGSimpelFysik
{
    internal static class Mathlike
    {
        public static float ClampF(float v, float min, float max) {
            if (v < min) return min;
            if (v > max) return max;
            return v;
        }
        public static int ClampI(int v, int min, int max)
        {
            if (v < min) return min;
            if (v > max) return max;
            return v;
        }
        public static Vector2 ClampV(Vector2 v, float maxl)
        {
            float vl = v.Length();
            if (vl > maxl) return v * maxl / vl;
            return v;
        }
        public static Point ClampP(Point v, Point u)
        {
            int x = ClampI(v.X, 0, u.X);
            int y = ClampI(v.Y, 0, u.X);
            return new Point(x, y);
        }
        

    }
}
