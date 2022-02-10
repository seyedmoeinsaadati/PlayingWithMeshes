using System;
using UnityEngine;

public class MeshGenerator : MonoBehaviour
{
    public bool autoUpdate;

    [SerializeField] private MeshFilter viewMeshFilter;
    private Mesh viewMesh;


    [Header("Mesh Setting:")] public AnimationCurve graph;

    [Min(2)] public int resolution = 2;
    

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

        CreateMesh();
    }

    public void CreateMesh()
    {
        int vertexCount = 2 * resolution;
        int triangleCount = 2 * (resolution - 1);

        Vector3[] vertices = new Vector3[vertexCount];
        int[] triangles = new int[triangleCount * 3];

        float step = (float) 1 / resolution;

        vertices[0] = new Vector3(0, 0, 0);
        for (int i = 1; i < vertices.Length - 1; i += 2)
        {
            float value = graph.Evaluate(step * i);
            vertices[i] = new Vector3(0, i * step * height, 0);
            vertices[i + 1] = new Vector3(value * width, i * step * height, value * width);
        }

        //vertices[vertices.Length - 1] = new Vector3(0, height, 0);

        for (int i = 0; i < triangleCount; i++)
        {
            triangles[i * 3] = i;
            if (i % 2 == 0)
            {
                triangles[i * 3 + 1] = i + 1;
                triangles[i * 3 + 2] = i + 2;
            }
            else
            {
                triangles[i * 3 + 1] = i + 2;
                triangles[i * 3 + 2] = i + 1;
            }
        }


        viewMesh.Clear();
        viewMesh.vertices = vertices;
        viewMesh.triangles = triangles;
        viewMesh.RecalculateNormals();
    }

    private void OnValidate()
    {
        if (autoUpdate)
        {
            Generate();
        }
    }
}