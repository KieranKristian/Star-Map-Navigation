using UnityEngine;
[RequireComponent(typeof(MeshFilter))]
[RequireComponent (typeof(MeshRenderer))]

public class RouteMesh : MonoBehaviour {
    public Transform pointA, pointB;

    public float scale = 2;

    Mesh mesh;
    private void Awake() {
        mesh = new Mesh();
        mesh.name = "Route";
        GetComponent<MeshFilter>().sharedMesh = mesh;
    }

    /// <summary>
    /// Generates a mesh connecting two points
    /// </summary>
    public void Generate() { 
        Vector3[] verts = new Vector3[] {
            pointA.position + new Vector3(-0.5f, -0.288f, 0) / scale , // 0
            pointA.position + new Vector3(0.5f, -0.288f, 0) / scale , // 1
            pointB.position + new Vector3(-0.5f, -0.288f, 0) / scale , // 2
            pointB.position + new Vector3(0.5f, -0.288f, 0) / scale , // 3
            pointA.position + new Vector3(0, 0.59f, 0) / scale , // 4
            pointB.position + new Vector3(0, 0.59f, 0) / scale , // 5
        };

        int[] tris = new int[]
        {
            0, 1, 2, 3, 2, 1, //Bottom face
            0, 2, 5, 0, 5, 4, //Left face
            1, 4, 5, 1 ,5, 3 //Right face
        };

        mesh.Clear();
        mesh.vertices = verts;
        mesh.triangles = tris;
        mesh.RecalculateNormals();
    }
}