using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshGenerator : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        MeshFilter meshFilter = GetComponent<MeshFilter>();
        meshFilter.mesh = createStripXZMesh(new Vector3(8, 0, 8), 8);
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

    Mesh createStripXZMesh(Vector3 size, int nSegmentsX)
    {
        Vector3 halfSize = size / 2;
        int nVertices = (nSegmentsX + 1) * 2;

        Mesh mesh = new Mesh();
        mesh.name = "StripXZ";
        Vector3[] vertices = new Vector3[nVertices];
        int[] triangles = new int[nSegmentsX * 6];
        Vector3[] normals = new Vector3[nVertices];
        Vector2[] uv = new Vector2[nVertices];

        // Vertices generation
        for (int i = 0; i < nVertices / 2; i++)
        {
            vertices[i] = new Vector3(-halfSize.x + (size.x / nSegmentsX) * i, 0, halfSize.z);
            uv[i] = new Vector2(vertices[i].x, vertices[i].y);
            normals[i] = Vector3.up;

            int offset = nVertices - i - 1;
            vertices[offset] = vertices[i] - Vector3.forward * size.x;
            uv[offset] = uv[i] - Vector2.up * size.x;
            normals[offset] = Vector3.up;
        }

        // Create StripXZ Mesh triangles
        for (int i = 0; i < nSegmentsX; i++)
        {
            int p0 = i;
            int p1 = i + 1;
            int p2 = nVertices - (i + 1);
            int p3 = nVertices - (i + 2);

            int offset = i * 6;

            triangles[offset] = p0;
            triangles[offset + 1] = p1;
            triangles[offset + 2] = p2;

            triangles[offset + 3] = p1;
            triangles[offset + 4] = p3;
            triangles[offset + 5] = p2;
        }

        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.normals = normals;
        mesh.uv = uv;

        mesh.RecalculateBounds();

        return mesh;
    }
}
