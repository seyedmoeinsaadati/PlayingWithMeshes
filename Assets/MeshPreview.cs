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
        CylinderAddictive,
        Circle
    }

    public bool autoUpdate;
    public MeshType type;

    [SerializeField] private MeshFilter viewMeshFilter;
    private Mesh viewMesh;

    [Header("Mesh Setting:")] public AnimationCurve graph;
    public float graphFactor = 1;
    public bool isSymmetry;
    [Min(1)] public int segment = 2;
    [Min(3)] public int side;
    public MeshGenerator.NormalMode normalMode;
    public float radius = 1;
    public float length = 1;

    public void DrawMesh(Mesh mesh)
    {
        viewMeshFilter.sharedMesh = mesh;
    }

    public void DrawInEditor()
    {
        switch (type)
        {
            case MeshType.Cylinder:
                DrawMesh(MeshGenerator.CreateCylinder(radius, length, side, segment, normalMode, "preview_cylinder"));
                break;
            case MeshType.CylinderAddictive:
                DrawMesh(MeshGenerator.CreateAddictiveCylinder(radius, length, side, segment, normalMode, graph,
                    graphFactor, isSymmetry, "preview_cylinder"));
                break;
            case MeshType.Circle:
                DrawMesh(MeshGenerator.CreateCircle(radius, segment, "preview_circle"));
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    public void SaveMesh()
    {
        AssetDatabase.CreateAsset(viewMeshFilter.sharedMesh, "Assets/mesh.asset");
        AssetDatabase.SaveAssets();
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(MeshPreview))]
public class MeshPreviewEditor : Editor
{
    public override void OnInspectorGUI()
    {
        MeshPreview meshPreview = (MeshPreview)target;

        DrawDefaultInspector();

        if (meshPreview.autoUpdate)
        {
            meshPreview.DrawInEditor();
        }

        if (GUILayout.Button("Generate"))
        {
            meshPreview.DrawInEditor();
        }

        if (GUILayout.Button("Save Mesh as Asset file"))
        {
            meshPreview.SaveMesh();
        }
    }
}
#endif