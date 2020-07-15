using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

[CustomEditor(typeof(DebugGizmos))]
public class DebugGizmosEditor : Editor
{
    private DebugGizmos currentGizmos;
    private static bool hideEditor = false;

    public override void OnInspectorGUI()
    {
        currentGizmos = (DebugGizmos)target;

        //If the editor is hidden, do not draw the next content
        if (GUILayout.Button("Hide editor")) { hideEditor = !hideEditor; }
        if(hideEditor) { return; }

        DebugGizmoErrors();

        GUILayout.Space(20.0f);
        currentGizmos.type = (GizmoType)EditorGUILayout.EnumPopup("Gizmo type", currentGizmos.type);
        GUILayout.Space(20.0f);

        DrawGizmoDefaultProperties();
        
        switch (currentGizmos.type)
        {
            case GizmoType.Cube:
                DrawGizmoCubeProperties();
                break;

            case GizmoType.WireCube:
                DrawGizmoWireCubeProperties();
                break;

            case GizmoType.Sphere:
                DrawGizmoSphereProperties();
                break;

            case GizmoType.WireSphere:
                DrawGizmoWireSphereProperties();
                break;

            case GizmoType.Mesh:
                DrawGizmoMeshProperties();
                break;

            case GizmoType.WireMesh:
                DrawGizmoWireMeshProperties();
                break;
        }
    }

    #region Property drawers
    private void DrawGizmoDefaultProperties()
    {
        currentGizmos.color = EditorGUILayout.ColorField("Color", currentGizmos.color);
        currentGizmos.offset = EditorGUILayout.Vector3Field("Offset position", currentGizmos.offset);
    }

    private void DrawGizmoCubeProperties()
    {
        currentGizmos.size = EditorGUILayout.Vector3Field("Cube size", currentGizmos.size);
    }

    private void DrawGizmoWireCubeProperties()
    {
        currentGizmos.size = EditorGUILayout.Vector3Field("Cube size", currentGizmos.size);
    }

    private void DrawGizmoSphereProperties()
    {
        currentGizmos.sphereRadius = EditorGUILayout.FloatField("Sphere radius", currentGizmos.sphereRadius);
    }

    private void DrawGizmoWireSphereProperties()
    {
        currentGizmos.sphereRadius = EditorGUILayout.FloatField("Sphere radius", currentGizmos.sphereRadius);
    }

    private void DrawGizmoMeshProperties()
    {
        currentGizmos.size = EditorGUILayout.Vector3Field("Mesh size", currentGizmos.size);
        currentGizmos.mesh = (Mesh)EditorGUILayout.ObjectField("Selected mesh", currentGizmos.mesh, typeof(Mesh), false);
    }

    private void DrawGizmoWireMeshProperties()
    {
        currentGizmos.size = EditorGUILayout.Vector3Field("Mesh size", currentGizmos.size);
        currentGizmos.mesh = (Mesh)EditorGUILayout.ObjectField("Selected mesh", currentGizmos.mesh, typeof(Mesh), false);
    }
    #endregion

    private void DebugGizmoErrors()
    {
        if((currentGizmos.type == GizmoType.Sphere || currentGizmos.type == GizmoType.WireSphere) && currentGizmos.sphereRadius == 0) 
        {
            Debug.LogWarning($"{ currentGizmos.gameObject.name }: Gizmo's sphere radius is equal to zero. It will be invisible.");
        }

        if ((currentGizmos.type == GizmoType.Mesh || currentGizmos.type == GizmoType.WireMesh) && currentGizmos.mesh == null)
        {
            Debug.LogWarning($"{ currentGizmos.gameObject.name }: Gizmo's mesh object reference not set. It will be invisible.");
        }
    }
}
