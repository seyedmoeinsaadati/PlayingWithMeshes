using System;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif


public class MeshPreview : MonoBehaviour
{
    public enum MeshType
    {
        Cylinder,
        CylinderWithGraph
    }

    public bool autoUpdate;
    public MeshType type;

    [SerializeField] private MeshFilter viewMeshFilter;
    private Mesh viewMesh;

    [Header("Mesh Setting:")] public AnimationCurve graph;
    [Min(1)] public int segment = 2;
    [Min(3)] public int side;
    public MeshGenerator.NormalMode normalMode;

    // [Range(-1, 1)] public float offset = 0;
    public float radius = 1;
    public float length = 1;

    public void DrawMesh(Mesh mesh)
    {
        viewMeshFilter.sharedMesh = mesh;

        // textureRenderer.gameObject.SetActive(false);
        // meshFilter.gameObject.SetActive(true);
    }

    public void DrawInEditor()
    {
        switch (type)
        {
            case MeshType.Cylinder:
                DrawMesh(MeshGenerator.CreateCylinder(radius, length, side, segment, normalMode, "preview_cylinder"));
                break;
            case MeshType.CylinderWithGraph:
                // DrawMesh(MeshGenerator.CreateCylinder());
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(MeshPreview))]
public class MeshPreviewEditor : Editor
{
    public override void OnInspectorGUI()
    {
        MeshPreview meshPreview = (MeshPreview) target;

        DrawDefaultInspector();

        if (meshPreview.autoUpdate)
        {
            meshPreview.DrawInEditor();
        }

        if (GUILayout.Button("Generate"))
        {
            meshPreview.DrawInEditor();
        }

        if (GUILayout.Button("Save Mesh as FBX"))
        {
            // meshPreview.SaveMesh();
        }
    }
}
#endif