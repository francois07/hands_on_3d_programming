using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyMathTools;

namespace MeshGenerator
{

    public class MeshGenerator : MonoBehaviour
    {
        public delegate Vector3 ComputePositionDelegate(float kX, float kZ);
        public delegate Vector3 ComputeNormalDelegate(float kX, float kZ);
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

            // MeshCollider meshCollider = GetComponent<MeshCollider>();
            // meshCollider.sharedMesh = meshFilter.mesh;
        }

        // Update is called once per frame
        void Update()
        {

        }

        public Mesh createNormalizedPlaneXZMesh(int nSegmentsX, int nSegmentsZ, ComputePositionDelegate computePos = null, ComputeNormalDelegate computeNorm = null)
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
}
