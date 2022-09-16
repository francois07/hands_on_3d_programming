using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshGenerator : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
        MeshFilter meshFilter = GetComponent<MeshFilter>();
        meshFilter.mesh = createQuadMesh(2);
    }

    // Update is called once per frame
    void Update()
    {

    }

    Mesh createTriangleMesh()
    {
        Mesh mesh = new Mesh();
        mesh.name = "Triangle";
        Vector3[] vertices = new Vector3[3];
        int[] triangles = new int[3];

        vertices[0] = new Vector3(0, 1, 0);
        vertices[1] = new Vector3(0, 0, 1);
        vertices[2] = new Vector3(1, 0, 0);

        triangles = new int[] { 0, 1, 2 };

        mesh.vertices = vertices;
        mesh.triangles = triangles;

        mesh.RecalculateBounds();

        return mesh;
    }

    Mesh createQuadMesh(float size)
    {
        float halfSize = size / 2;
        Mesh mesh = new Mesh();
        mesh.name = "Quad";
        Vector3[] vertices = new Vector3[4];
        int[] triangles = new int[6];
        Vector3[] normals = new Vector3[4];

        vertices[0] = new Vector3(-halfSize, 0, halfSize);
        vertices[1] = new Vector3(halfSize, 0, halfSize);
        vertices[2] = new Vector3(halfSize, 0, -halfSize);
        vertices[3] = new Vector3(-halfSize, 0, -halfSize);

        triangles = new int[] { 0, 1, 2, 0, 2, 3 };

        normals = new Vector3[4]
        {
            -Vector3.forward,
            -Vector3.forward,
            -Vector3.forward,
            -Vector3.forward
        };

        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.normals = normals;

        mesh.RecalculateBounds();

        return mesh;
    }
}
