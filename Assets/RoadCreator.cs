using System.Collections.Generic;
using System.Net.NetworkInformation;
using Moein;
using Moein.Spline;
using UnityEditor;
using UnityEngine;


[ExecuteInEditMode, RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class RoadCreator : MonoBehaviour
{
    [SerializeField] private Spline spline = null;
    [SerializeField] private MeshFilter meshFilter = null;
    [SerializeField] private MeshCollider meshCollider = null;
    [SerializeField] private float width = 10;
    [SerializeField, Min(3)] private int side = 3;
    [SerializeField] private int resolution = 10;

    public void Create()
    {
        if (spline == null || meshFilter == null) return;

        meshFilter.CreateRoad(spline, resolution, side, width);

        if (meshCollider != null)
        {
            meshCollider.sharedMesh = meshFilter.sharedMesh;
        }
    }

    private void Start()
    {
        Create();
    }

#if UNITY_EDITOR

    // private void OnDrawGizmos()
    // {
    //     if (meshFilter.mesh == null) return;
    //
    //     Vector3[] vertices = meshFilter.mesh.vertices;
    //
    //     Gizmos.color = Color.black;
    //     for (var i = 0; i < vertices.Length; i++)
    //     {
    //         Handles.Label(vertices[i], i.ToString());
    //         Gizmos.DrawSphere(vertices[i], 0.1f);
    //     }
    // }

    private void Reset()
    {
        spline = transform.GetComponent<Spline>();
        meshFilter = transform.GetComponent<MeshFilter>();
    }

    private void Update()
    {
        if (UnityEditor.EditorApplication.isPlaying == false)
            Create();
    }
#endif
}
