﻿using UnityEngine;

public static class MeshGenerator
{
    /// <summary>
    /// create a cylinder
    /// </summary>
    /// <param name="radius"></param>
    /// <param name="length"></param>
    /// <param name="side"></param>
    /// <param name="segment"></param>
    /// <param name="normalMode"></param>
    /// <param name="name"></param>
    /// <returns></returns>
    public static Mesh CreateCylinder(float radius, float length, int side, int segment, NormalMode normalMode,
        string name = "Default Cylinder")
    {
        // calculating
        int vertexCount = (side + 1) * (segment + 1);
        int triangleCount = 2 * segment * side;
        float angleStep = (float) 360 / side;

        // initialize mesh data
        Vector3[] vertices = new Vector3[vertexCount];
        Vector3[] vertexNormals = new Vector3[vertexCount];
        Vector2[] uvs = new Vector2[vertexCount];
        int[] triangles = new int[triangleCount * 3];

        // creating...
        // vertex, uvs calculation
        for (int i = 0; i < segment + 1; i++)
        {
            float anglePointer = 0;
            for (int j = 0; j < side + 1; j++)
            {
                var vertexIndex = i * (side + 1) + j;
                Vector3 vertexPos = new Vector3(
                    Mathf.Cos(anglePointer * Mathf.Deg2Rad) * radius,
                    (float) i / segment * length,
                    Mathf.Sin(anglePointer * Mathf.Deg2Rad) * radius);

                vertices[vertexIndex] = vertexPos;
                uvs[vertexIndex] = new Vector2((float) j / (side + 1), (float) i / (segment + 1));

                anglePointer += angleStep;
            }
        }

        // triangle calculation
        for (int ti = 0, vi = 0, y = 0; y < segment; y++, vi++)
        {
            for (int x = 0; x < side; x++, ti += 6, vi++)
            {
                triangles[ti] = vi;
                triangles[ti + 3] = triangles[ti + 2] = vi + 1;
                triangles[ti + 4] = triangles[ti + 1] = vi + side + 1;
                triangles[ti + 5] = vi + side + 2;
            }
        }

        // normal calculation
        for (int i = 0; i < triangleCount; i++)
        {
            int normalTriangleIndex = i * 3;
            int vertexIndexA = triangles[normalTriangleIndex];
            int vertexIndexB = triangles[normalTriangleIndex + 1];
            int vertexIndexC = triangles[normalTriangleIndex + 2];

            Vector3 pointA = vertices[vertexIndexA];
            Vector3 pointB = vertices[vertexIndexB];
            Vector3 pointC = vertices[vertexIndexC];
            Vector3 AB = pointB - pointA;
            Vector3 AC = pointC - pointA;
            Vector3 triangleNormal = Vector3.Cross(AB, AC).normalized;
            vertexNormals[vertexIndexA] += triangleNormal;
            vertexNormals[vertexIndexB] += triangleNormal;
            vertexNormals[vertexIndexC] += triangleNormal;
        }

        for (int i = 0; i < vertexNormals.Length; i++)
        {
            vertexNormals[i].Normalize();
        }

        // creating mesh with mesh data
        Mesh mesh = new Mesh
        {
            name = name
        };

        if (normalMode == NormalMode.Flat)
        {
            Vector3[] flatShadedVertices = new Vector3[triangles.Length];
            Vector2[] flatShadedUVs = new Vector2[triangles.Length];
            for (int i = 0; i < triangles.Length; i++)
            {
                flatShadedVertices[i] = vertices[triangles[i]];
                flatShadedUVs[i] = uvs[triangles[i]];
                triangles[i] = i;
            }

            vertices = flatShadedVertices;
            uvs = flatShadedUVs;
        }


        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.uv = uvs;

        if (normalMode == NormalMode.Flat)
        {
            mesh.RecalculateNormals();
        }
        else if (normalMode == NormalMode.Smooth)
        {
            mesh.normals = vertexNormals;
        }

        return mesh;
    }

    public enum NormalMode
    {
        Flat = 0,
        Smooth = 1
    }
}