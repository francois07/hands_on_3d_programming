using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyMathTools;

public class MeshGenerator : MonoBehaviour
{
    delegate Vector3 ComputePositionDelegate(float kX, float kZ);
    delegate Vector3 ComputeNormalDelegate(float kX, float kZ);
    // Start is called before the first frame update
    void Start()
    {
        MeshFilter meshFilter = GetComponent<MeshFilter>();
        meshFilter.mesh = createNormalizedPlaneXZMesh(100, 100,
        (kX, kZ) =>
        {
            float height = Mathf.Sin(kX * Mathf.PI * 3) + Mathf.Cos(kZ * Mathf.PI * 4) + Mathf.Cos(kZ * Mathf.PI * 2) + Mathf.Sin(kX * Mathf.PI);
            return new Vector3(Mathf.Lerp(-5, 5, kX), height, Mathf.Lerp(-5, 5, kZ));
        }
        );

        MeshCollider meshCollider = GetComponent<MeshCollider>();
        meshCollider.sharedMesh = meshFilter.mesh;
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

    Mesh createNormalizedPlaneXZMesh(int nSegmentsX, int nSegmentsZ, ComputePositionDelegate computePos = null, ComputeNormalDelegate computeNorm = null)
    {
        int nVertices = (nSegmentsX + 1) * (nSegmentsZ + 1);

        Mesh mesh = new Mesh();
        mesh.name = "StripXZ";
        Vector3[] vertices = new Vector3[nVertices];
        int[] triangles = new int[nSegmentsX * nSegmentsZ * 6];
        Vector3[] normals = new Vector3[nVertices];
        Vector2[] uv = new Vector2[nVertices];

        // Vertices generation
        for (int i = 0; i < nSegmentsZ + 1; i++)
        {
            float kZ = (float)i / nSegmentsZ;

            for (int j = 0; j < nSegmentsX + 1; j++)
            {
                float kX = ((float)j / nSegmentsX);
                int idx = i * (nSegmentsX + 1) + j;

                vertices[idx] = computePos != null ? computePos(kX, kZ) : (new Vector3(kX, 0, kZ));
                normals[idx] = computeNorm != null ? computeNorm(kX, kZ) : Vector3.up;
                uv[idx] = new Vector2(vertices[idx].x, vertices[idx].z);
            }
        }

        // Create GridXZ Mesh triangles
        for (int i = 0; i < nSegmentsZ; i++)
        {
            for (int j = 0; j < nSegmentsX; j++)
            {
                int p0 = j + i * (nSegmentsX + 1);
                int p1 = p0 + 1;
                int p2 = p0 + nSegmentsX + 1;
                int p3 = p2 + 1;

                int idx = (j * 6) + (6 * nSegmentsX) * i;

                triangles[idx] = p0;
                triangles[idx + 1] = p2;
                triangles[idx + 2] = p1;

                triangles[idx + 3] = p1;
                triangles[idx + 4] = p2;
                triangles[idx + 5] = p3;
            }
        }

        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.normals = normals;
        mesh.uv = uv;

        mesh.RecalculateBounds();

        return mesh;
    }

    Mesh createPlaneXZMesh(Vector3 size, int nSegmentsX, int nSegmentsZ)
    {
        Vector3 halfSize = size / 2;
        int nVertices = (nSegmentsX + 1) * (nSegmentsZ + 1);

        Mesh mesh = new Mesh();
        mesh.name = "StripXZ";
        Vector3[] vertices = new Vector3[nVertices];
        int[] triangles = new int[nSegmentsX * nSegmentsZ * 6];
        Vector3[] normals = new Vector3[nVertices];
        Vector2[] uv = new Vector2[nVertices];

        // Vertices generation
        for (int i = 0; i < nSegmentsZ + 1; i++)
        {
            float kZ = -halfSize.z + (size.z / nSegmentsZ) * i;

            for (int j = 0; j < nSegmentsX + 1; j++)
            {
                float kX = -halfSize.x + (size.x / nSegmentsX) * j;
                int idx = i * (nSegmentsX + 1) + j;

                vertices[idx] = new Vector3(kX, 0, kZ);
                normals[idx] = Vector3.up;
                uv[idx] = new Vector2(vertices[idx].x, vertices[idx].z);
            }
        }

        // Create GridXZ Mesh triangles
        for (int i = 0; i < nSegmentsZ; i++)
        {
            for (int j = 0; j < nSegmentsX; j++)
            {
                int p0 = j + i * (nSegmentsX + 1);
                int p1 = p0 + 1;
                int p2 = p0 + nSegmentsX + 1;
                int p3 = p2 + 1;

                int idx = (j * 6) + (6 * nSegmentsX) * i;

                triangles[idx] = p0;
                triangles[idx + 1] = p2;
                triangles[idx + 2] = p1;

                triangles[idx + 3] = p1;
                triangles[idx + 4] = p2;
                triangles[idx + 5] = p3;
            }
        }

        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.normals = normals;
        mesh.uv = uv;

        mesh.RecalculateBounds();

        return mesh;
    }

    Mesh createCylindricalYMeshFromStripXZ(Vector3 size, int nSegmentsX)
    {
        Mesh stripMesh = createStripXZMesh(size, nSegmentsX);
        Vector3[] vertices = stripMesh.vertices;
        Vector3[] normals = stripMesh.normals;
        Vector3 halfSize = size / 2;

        int nVerticesHalf = vertices.Length / 2;
        float radius = size.x / (2 * Mathf.PI);

        for (int i = 0; i < nVerticesHalf; i++)
        {
            float theta = (1 / radius) * i;
            vertices[i] = CoordConvert.CylindricalToCartesion(new Cylindrical(radius, theta, halfSize.y));
            normals[i] = new Vector3(vertices[i].x, 0, vertices[i].y);

            int offset = vertices.Length - i - 1;
            vertices[offset] = CoordConvert.CylindricalToCartesion(new Cylindrical(radius, theta, -halfSize.y));
            normals[offset] = normals[i];
        }

        stripMesh.vertices = vertices;
        stripMesh.normals = normals;
        stripMesh.RecalculateBounds();

        return stripMesh;
    }

    Mesh createSphericalMeshFromPlaneXZ(float radius, int nSegmentsX, int nSegmentsZ)
    {
        ComputePositionDelegate sphereTrans = (kX, kZ) =>
          {
              float coeff = Mathf.PI * 2;
              return CoordConvert.SphericalToCartesian(new Spherical(radius, coeff * kX, coeff * kZ));
          };

        return createNormalizedPlaneXZMesh(nSegmentsX, nSegmentsZ, sphereTrans);
    }
}
