// CUSTOM GIZMOS SCRIPT BY - Wilfre - 

/*
   - Add this to an object and you can virtualize shapes in the editor
   - Use the "Gizmos" button on the top right corner of the game view to debug in-game
   - Gizmos doesn't appear in game build
 */

using System.Collections.Generic;
using UnityEngine;

public enum GizmoType
{
    Cube,
    WireCube,
    Sphere,
    WireSphere,
    Mesh,
    WireMesh
}

public class DebugGizmos : MonoBehaviour
{
    #region Variables
    [Header("Gizmo")]
    public GizmoType type;

    [Header("Parameters"), Space]
    public Color color = Color.green;
    public Vector3 offset = Vector3.zero;
    public Vector3 size = Vector3.one;
    public float sphereRadius = 1.0f;

    //Mesh
    public Mesh mesh;
    public Vector3 meshRotation = Vector3.zero;

    [Header("Texture"), Space]
    public string icon = "";
    public Material material;

    [Header("Links"), Space]
    public Color linkColor = Color.cyan;
    public List<Transform> linkedObjects = new List<Transform>(1);

    public static bool active = true;

    public bool showName;
    public float textSize = 2.0f;

    #endregion
    #region Draw
    //Draw basic gizmos in editor only
#if (UNITY_EDITOR)
    [ExecuteInEditMode]
    void OnDrawGizmos()
    {
        if (!active) { return; }

        Gizmos.color = color;

        switch (type)
        {
            case GizmoType.Cube:
                Gizmos.DrawCube(transform.position + offset, size);
                Color _colB = color;
                _colB.a = 1.0f;
                Gizmos.color = _colB;
                Gizmos.DrawWireCube(transform.position + offset, size);
                break;
            case GizmoType.WireCube:
                Gizmos.DrawWireCube(transform.position + offset, size);
                break;
            case GizmoType.Sphere:
                Gizmos.DrawSphere(transform.position + offset, sphereRadius);
                Color _colS = color;
                _colS.a = 1.0f;
                Gizmos.color = _colS;
                Gizmos.DrawWireSphere(transform.position + offset, sphereRadius);
                break;
            case GizmoType.WireSphere:
                Gizmos.DrawWireSphere(transform.position + offset, sphereRadius);
                break;
            case GizmoType.Mesh:
                meshRotation = transform.rotation.eulerAngles;
                Gizmos.DrawMesh(mesh, 0, transform.position + offset, Quaternion.Euler(meshRotation), size);
                break;
            case GizmoType.WireMesh:
                meshRotation = transform.rotation.eulerAngles;
                Gizmos.DrawWireMesh(mesh, 0, transform.position + offset, Quaternion.Euler(meshRotation), size);
                break;
        }
        //Gizmos.DrawIcon(transform.position, icon, true);

        //Draw links
        try
        {
            Gizmos.color = linkColor;

            //Link this to first linked object
            Gizmos.DrawLine(transform.position, linkedObjects[0].position);

            for (int i = 0; i < linkedObjects.Count; i++)
            {
                Gizmos.DrawLine(linkedObjects[i].position, linkedObjects[i + 1].position);
            }
        }
        catch { } //Errors (Out of index) are expected but not an issue, we can ignore them
    }
#endif
    #endregion
}