using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Util
{
    public static class Utils
    {
        public static float Cross2D(Vector2 a, Vector2 b)
        {
            return Vector2.Dot(a, b);
        }

        public static T GetItem<T>(T[] array, int index)
        {
            if(index >= array.Length)
                return array[index % array.Length];

            if(index < 0)
                return array[index % array.Length + array.Length];

            return array[index];
        }

        public static T GetItem<T>(List<T> list, int index)
        {
            if (index >= list.Count)
                return list[index % list.Count];

            if (index < 0)
                return list[index % list.Count + list.Count];

            return list[index];
        }

    }
}