using System.Collections.Generic;
using UnityEngine;


namespace Moein
{
    public static class MeshEx
    {
        private static List<Vector3> points = new List<Vector3>();
        private static List<Vector3> normals = new List<Vector3>();
        private static List<Vector2> uvs = new List<Vector2>();
        private static List<int> triangles = new List<int>();

        public static MeshFilter CreateRoad(this MeshFilter self, Spline.Spline spline, int resolution, int side, float width, string meshName = null)
        {
            if (self.sharedMesh == null)
                self.sharedMesh = new Mesh() { name = string.IsNullOrEmpty(meshName) ? $"Road_{self.name}" : meshName };

            points.Clear();
            normals.Clear();
            uvs.Clear();
            triangles.Clear();

            float angleStep = (float)360 / side;
            var radius = width * 0.5f;
            var yCount = resolution;
            var yLenght = spline.Length / yCount;
            var xCount = side;// Mathf.CeilToInt(width / yLenght);
            var xLength = width / xCount;

            var uvY = (spline.Length / width) / yCount;
            var uvX = 1.0f / xCount;

            for (int i = 0; i <= yCount; i++)
            {
                var point = spline.GetLocalPoint(i * yLenght);
                var cross = Vector3.Cross(point.forward, point.upward).normalized;
                var right = (point.position - cross).normalized;

                //points.Add(right);
                // normals.Add(point.upward);
                var t = i * uvY;
                // uvs.Add(new Vector2(1, t));

                var tileL = (cross * xLength);
                float angle = 0;
                for (int c = 0; c <= xCount; c++)
                {
                    var pos = Quaternion.Euler(0, 0, angle) * right;

                    // Translate the point to be rotated so that the center is at the origin
                    Vector3 translatedPoint = right - point.position;

                    // Create the rotation quaternion
                    // Apply the rotation to the translated point
                    Vector3 rotatedPoint = Quaternion.AngleAxis(angle, spline.transform.InverseTransformDirection(point.forward)) * translatedPoint;

                    // Translate the rotated point back to its original position relative to the center
                    pos = rotatedPoint;

                    points.Add(point.position + pos * radius);
                    //points.Add(point.position + right.RotateXY(angle * Mathf.Deg2Rad, true) * radius);
                    // points.Add(right + tileL * c);
                    normals.Add((point.position - right).normalized);
                    uvs.Add(new Vector2(1 - uvX * c, t));
                    angle += angleStep;
                }
            }


            for (int y = 0; y < yCount; y++)
            {
                var a = y * (xCount + 1);
                for (int x = 0; x < xCount; x++)
                {
                    int i = x + a;
                    triangles.Add(i);
                    triangles.Add(i + 1);
                    triangles.Add(i + xCount + 1);

                    triangles.Add(i + 1);
                    triangles.Add(i + xCount + 2);
                    triangles.Add(i + xCount + 1);
                }
            }

            self.sharedMesh.Clear();
            if (points.Count > 3)
            {
                self.sharedMesh.vertices = points.ToArray();
                self.sharedMesh.normals = normals.ToArray();
                self.sharedMesh.uv = uvs.ToArray();
                self.sharedMesh.triangles = triangles.ToArray();
                self.sharedMesh.RecalculateNormals();
            }

            return self;
        }
    }
}
