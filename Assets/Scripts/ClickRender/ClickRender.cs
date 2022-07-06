using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Util;

namespace ClickRender
{
    public class ClickRender : MonoBehaviour
    {
        const float NORTH_MARGIN = 4;
        const float WEST_MARGIN = -6;

        private List<Vector2> points = new List<Vector2>();
        private LineRenderer lineRenderer;
        private List<GameObject> markers = new List<GameObject>();

        private void Awake()
        {
            if (!(lineRenderer = GetComponent<LineRenderer>()))
            {
                throw new System.Exception("LineRenderer component not found! " + this.name);
            }

        }


        void Update()
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            if (Input.GetMouseButtonDown(0) && mousePos.x >= WEST_MARGIN && mousePos.x <= WEST_MARGIN * -1 && mousePos.y >= NORTH_MARGIN * -1 && mousePos.y <= NORTH_MARGIN)
            {
                points.Add(new Vector2(mousePos.x, mousePos.y));
                markers.Add(Instantiate(Resources.Load<GameObject>("Square"), new Vector3(mousePos.x, mousePos.y, 0), Quaternion.identity));
            }

        }

        public void GeneratePolygon()
        {
            markers.ForEach(m => Destroy(m));
            DrawPolygon(lineRenderer, points.ToArray());
        }



        public void Reset()
        {
            markers.ForEach(m => Destroy(m));
            lineRenderer.positionCount = 0;
            points.Clear();
            markers.Clear();
        }

        public void Triangulate()
        {
            //throw new System.Exception("test");
            if (!EarClipping.Triangulate(points.ToArray(), out int[] triangles, out string errorMessage))
            { 
                throw new System.Exception(errorMessage);
            }

            DrawTriangles(lineRenderer, points.ToArray(), triangles);
        }


        private void DrawPolygon(LineRenderer lineRenderer, Vector2[] points)
        {
            List<Vector3> pointsV3 = new List<Vector3>();
            foreach (Vector2 point in points)
            {
                pointsV3.Add(new Vector3(point.x, point.y, 0));
            }
            DrawPolygon(lineRenderer, pointsV3.ToArray());
        }
        private void DrawPolygon(LineRenderer lineRenderer, Vector3[] points)
        {
            lineRenderer.positionCount += points.Length + 1;
            lineRenderer.SetPositions(points);
            lineRenderer.SetPosition(points.Length, points[0]);
        }

        private void DrawTriangles(LineRenderer lineRenderer, Vector2[] vertices, int[] trinagles)
        {
            List<Vector3> points = new List<Vector3>();

            for (int i = 0; i < trinagles.Length; i+=3)
            {

                Vector3 a = vertices[trinagles[i]];
                Vector3 b = vertices[trinagles[i + 1]];
                Vector3 c = vertices[trinagles[i + 2]];

                points.Add(a);
                points.Add(b);
                points.Add(c);
                points.Add(a);

            }

            lineRenderer.positionCount = points.Count;
            lineRenderer.SetPositions(points.ToArray());
        }
    }
}