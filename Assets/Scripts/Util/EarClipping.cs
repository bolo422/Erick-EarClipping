using System.Collections;
using UnityEngine;
using System.Collections.Generic;

namespace Util
{
    public static class EarClipping
    {
        public static bool Triangulate(Vector2[] vertices, out int[] triangles, out string errorMessage)
        {
            triangles = null;
            errorMessage = string.Empty;

            #region Basic validations
            if (vertices is null)
            {
                errorMessage = "A lista de vértices é nula.";
                return false;
            }
            if (vertices.Length <= 3)
            {
                errorMessage = "A lista de vértices deve ter mais de 3 vértices.";
                return false;
            }
            if (vertices.Length > 10000)
            {
                errorMessage = "A lista de vértices suporte até 10.000 vértices.";
                return false;
            }
            #endregion

            List<int> indexList = new List<int>();
            for (int i = 0; i < vertices.Length; i++)
            {
                indexList.Add(i);
            }

            int totalTriangleCount = vertices.Length - 2;
            int totalTriangleIndexCount = totalTriangleCount * 3;

            triangles = new int[totalTriangleIndexCount];
            int triangleIndexCount = 0;

            while (indexList.Count > 3)
            {
                for (int i = 0; i < indexList.Count; i++)
                {
                    int a = indexList[i];
                    int b = Utils.GetItem(indexList, i - 1);
                    int c = Utils.GetItem(indexList, i + 1);

                    Vector2 va = vertices[a];
                    Vector2 vb = vertices[b];
                    Vector2 vc = vertices[c];

                    Vector2 va_to_vb = vb - va;
                    Vector2 va_to_vc = vc - va;

                    
                    if (Utils.Cross2D(va_to_vb, va_to_vc) < 0f)
                    {
                        continue;
                    }

                    bool isEar = true;

                    
                    for (int j = 0; j < vertices.Length; j++)
                    {
                        

                        if (j == a || j == b || j == c)
                        {
                            continue;
                        }

                        Vector2 p = vertices[j];

                        if (IsPointInTriangle(p, vb, va, vc))
                        {
                            isEar = false;
                            break;
                        }

                    }

                    if (isEar)
                    {
                        triangles[triangleIndexCount++] = b;
                        triangles[triangleIndexCount++] = a;
                        triangles[triangleIndexCount++] = c;

                        indexList.RemoveAt(i);
                        break;
                    }

                }

            }

            triangles[triangleIndexCount++] = indexList[0];
            triangles[triangleIndexCount++] = indexList[1];
            triangles[triangleIndexCount++] = indexList[2];

            return true;
        }

        public static bool IsPointInTriangle(Vector2 p, Vector2 a, Vector2 b, Vector2 c)
        {
            Vector2 ab = b - a;
            Vector2 bc = c - b;
            Vector2 ca = a - c;

            Vector2 ap = p - a;
            Vector2 bp = p - b;
            Vector2 cp = p - c;

            float cross1 = Utils.Cross2D(ab, ap);
            float cross2 = Utils.Cross2D(bc, bp);
            float cross3 = Utils.Cross2D(ca, cp);

            if (cross1 > 0f || cross2 > 0f || cross3 > 0f)
            {
                return false;
            }

            return true;
        }

    }
}