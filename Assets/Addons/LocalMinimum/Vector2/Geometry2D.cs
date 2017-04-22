using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LocalMinimum.Geometry2D
{

    public static class Geometry2D
    {
        public static Vector2 GetCenter(Vector2 a, Vector2 b, Vector2 c)
        {
            return (a + b + c) / 3f;
        }

        public static bool PointInTriangle(Vector2 v0, Vector2 v1, Vector2 v2, Vector2 pt)
        {
            float v1v2 = Cross(v1, v2);
            float a = (Cross(pt, v2) - Cross(v0, v2)) / v1v2;
            float b = (Cross(pt, v1) - Cross(v0, v1)) / v1v2;

            return a >= 0 && a <= 1 && b >= 0 && b <= 1;
        }

        public static float Cross(Vector2 u, Vector2 v)
        {
            return u.x * v.y - u.y * v.x;
        }

        public static Vector2 GetRandomPointInTriangle(Vector2 v0, Vector2 v1, Vector2 v2)
        {
            Vector2 V1 = v1 - v0;
            Vector2 V2 = v2 - v0;
            Vector2 pt;

            do
            {
                pt = V1 * Random.value + V2 * Random.value;
            } while (!PointInTriangle(v0, v1, v2, pt));

            return pt;
        }

        public static IEnumerable<Vector2> GetRandomPointsInTriangle(Vector2 v0, Vector2 v1, Vector2 v2)
        {
            Vector2 V1 = v1 - v0;
            Vector2 V2 = v2 - v0;
            Vector2 pt;
            while (true)
            {
                do
                {
                    pt = V1 * Random.value + V2 * Random.value;
                } while (!PointInTriangle(v0, v1, v2, pt));

                yield return pt;
            }
        }

        public static Vector2 Rotate(this Vector2 input, float radians)
        {
            float cos = Mathf.Cos(radians);
            float sin = Mathf.Sin(radians);
            return new Vector2(input.x * cos - input.y * sin, input.x * sin + input.y * cos);
        }

        public static Vector2 Extend(this Vector2 input, float extension)
        {
            return input.normalized * (input.magnitude + extension);
        }
    }
}