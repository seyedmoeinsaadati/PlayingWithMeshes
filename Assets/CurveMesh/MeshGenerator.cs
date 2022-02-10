using System;
using UnityEngine;

public class MeshGenerator : MonoBehaviour
{
    public bool autoUpdate;

    [SerializeField] private MeshFilter viewMeshFilter;
    private Mesh viewMesh;


    [Header("Mesh Setting:")] public AnimationCurve graph;

    [Min(1)] public int resolution = 2;
    [Min(3)] public int side;

    // [Range(-1, 1)] public float offset = 0;
    public float width = 1;
    public float height = 1;

    public void Generate()
    {
        if (viewMeshFilter == null || viewMesh == null)
        {
            viewMeshFilter = GetComponent<MeshFilter>();

            viewMesh = new Mesh {name = "View Mesh"};
            viewMeshFilter.mesh = viewMesh;
        }

        CreateCylinder();
    }

    private Vector3[] vertices;

    public void CreateCylinder()
    {
        // create cylinder with open top and down
        int vertexCount = (side + 1) * (resolution + 1);
        int triangleCount = 2 * resolution * side;

        vertices = new Vector3[vertexCount];
        int[] triangles = new int[triangleCount * 3];

        // float step = (float) 1 / resolution;
        float angleStep = (float) 360 / side;

        for (int i = 0; i < resolution + 1; i++)
        {
            //reset angle
            float anglePointer = 0;
            for (int j = 0; j < side + 1; j++)
            {
                var vertexIndex = i * (side + 1) + j;
                Vector3 vertexPos = new Vector3(
                    Mathf.Cos(anglePointer * Mathf.Deg2Rad) * width,
                    (float) i / resolution * height,
                    Mathf.Sin(anglePointer * Mathf.Deg2Rad) * width);

                vertices[vertexIndex] = vertexPos;

                anglePointer += angleStep;
            }
        }

        // print(vertexCount + ", " + triangleCount);

        // for (int ti = 0, vi = 0, y = 0; y < resolution + 1; y++, vi++)
        // {
        //     for (int x = 0; x < side + 1; x++, ti += 6, vi++)
        //     {
        //         triangles[ti] = vi;
        //         triangles[ti + 3] = triangles[ti + 2] = vi + 1;
        //         triangles[ti + 4] = triangles[ti + 1] = vi + side + 1;
        //         triangles[ti + 5] = vi + side + 2;
        //     }
        // }

        viewMesh.Clear();
        viewMesh.vertices = vertices;
        viewMesh.triangles = triangles;
        viewMesh.RecalculateNormals();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.black;
        for (int i = 0; i < vertices.Length; i++)
        {
            // if (vertices[i] == Vector3.zero)
            // {
            //     print(i);
            // }

            Gizmos.DrawSphere(vertices[i], 0.01f);
        }
    }

    private void OnValidate()
    {
        if (autoUpdate)
        {
            Generate();
        }
    }
}