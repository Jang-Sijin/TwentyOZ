using UnityEngine;
using System.Collections;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class RoundCube : MonoBehaviour
{
    public int xSize, ySize, zSize;
    public int roundness;

    private Mesh mesh;
    private Vector3[] vertices;
    private Vector3[] normals;


    private void Awake()
    {
        Generate();
    }

    private void Generate()
    {
        GetComponent<MeshFilter>().mesh = mesh = new Mesh();
        mesh.name = "Procedural Cube";
        CreateVertices();
        CreateTriangles();
    }

    private void CreateVertices()
    {
        int cornerVertices = 8; // 육면체 가장 사이드 꼭지점 총 개수
        int edgeVertices = (xSize + ySize + zSize - 3) * 4; // 육면체 가장 사이드 꼭지점 총 개수를 제외한 육면체 모서리 라인의 꼭지점 총 개수
        int faceVertices = 2 * ((xSize - 1) * (ySize - 1) +
                                (xSize - 1) * (zSize - 1) +
                                (ySize - 1) *
                                (zSize - 1)); // 육면체 가장 사이드 꼭지점 총 개수와 육면체 모서리 라인의 꼭지점 총 개수를 제외한 육면체의 내부 꼭지점 총 개수 
        vertices = new Vector3[cornerVertices + edgeVertices + faceVertices]; // 육면체 사이드 정점의 총 개수
        normals = new Vector3[vertices.Length]; // 노말 벡터

        // 1. 정점 그리기 (4*4*4 육면체라고 가정하면) [xz평면 앞쪽, 오른쪽, 뒷쪽, 왼쪽 엣지 순으로 그리기 * 높이(아래에서 위방향으로)]
        int v = 0;
        for (int y = 0; y <= ySize; y++)
        {
            // 1-1. 앞쪽 엣지 x 정점 0 1 2 3 그리기
            for (int x = 0; x <= xSize; x++)
            {
                SetVertex(v++, x, y, 0);
            }

            // 1-2. 오른쪽 엣지 z 정점 1 2 3 그리기 (z = 0) == (x = 3)
            for (int z = 1; z <= zSize; z++)
            {
                SetVertex(v++, xSize, y, z);
            }

            // 1-3. 뒷쪽 엣지 x 정점 2 1 0 그리기 (x = size) == (z = zSize) 
            for (int x = xSize - 1; x >= 0; x--)
            {
                SetVertex(v++, x, y, zSize);
            }

            // 1-4. 왼쪽 엣지 z 정점 2 1 그리기 (z = zSize) == (x = 0) && (z = 0) == (x = 0)
            for (int z = zSize - 1; z > 0; z--)
            {
                SetVertex(v++, 0, y, z);
            }
        }

        //  2. 윗면, 아랫면에 비어있는 나머지 정점들 그리기
        //  2.1 윗면(ySize) 정점 (1, 1)에서 부터 시작한다. [x축 1라인 그리고 z축+1 순회]
        for (int z = 1; z < zSize; z++)
        {
            for (int x = 1; x < xSize; x++)
            {
                SetVertex(v++, x, ySize, z);
            }
        }

        //  2.2 밑면(0) 정점 (1, 1)에서 부터 시작한다. [x축 1라인 그리고 z축+1 순회]
        for (int z = 1; z < zSize; z++)
        {
            for (int x = 1; x < xSize; x++)
            {
                SetVertex(v++, x, 0, z);
            }
        }

        mesh.vertices = vertices;
        mesh.normals = normals;
    }

    private void SetVertex (int i, int x, int y, int z) 
    {
        Vector3 inner = vertices[i] = new Vector3(x, y, z); // inner = (x, y, z);
        
        // inner.x 설정
        if (x < roundness) 
        {
            inner.x = roundness;
        }
        else if (x > xSize - roundness) 
        {
            inner.x = xSize - roundness;
        }
        
        // inner.y 설정
        if (y < roundness) 
        {
            inner.y = roundness;
        }
        else if (y > ySize - roundness)
        {
            inner.y = ySize - roundness;
        }
        
        if (z < roundness) 
        {
            inner.z = roundness;
        }
        else if (z > zSize - roundness)
        {
            inner.z = zSize - roundness;
        }
        
        normals[i] = (vertices[i] - inner).normalized; // 노말 위치 재설정
        vertices[i] = inner + normals[i] * roundness;
    }

    private void CreateTriangles()
    {
        int quads = (xSize * ySize + xSize * zSize + ySize * zSize) * 2;
        int[] triangles = new int[quads * 6];
        int ring = (xSize + zSize) * 2;
        int t = 0, v = 0;

        for (int y = 0; y < ySize; y++, v++)
        {
            for (int q = 0; q < ring - 1; q++, v++)
            {
                t = SetQuad(triangles, t, v, v + 1, v + ring, v + ring + 1);
            }

            t = SetQuad(triangles, t, v, v - ring + 1, v + ring, v + 1);
        }

        // 윗면 나머지 부분
        t = CreateTopFace(triangles, t, ring);

        // 아랫면
        t = CreateBottomFace(triangles, t, ring);

        mesh.triangles = triangles;
    }

    private int CreateTopFace(int[] triangles, int t, int ring)
    {
        int v = ring * ySize;
        for (int x = 0; x < xSize - 1; x++, v++)
        {
            t = SetQuad(triangles, t, v, v + 1, v + ring - 1, v + ring);
        }

        t = SetQuad(triangles, t, v, v + 1, v + ring - 1, v + 2); // 마지막 쿼드(삼각형 * 2)

        int vMin = ring * (ySize + 1) - 1;
        int vMid = vMin + 1;
        int vMax = v + 2;

        for (int z = 1; z < zSize - 1; z++, vMin--, vMid++, vMax++)
        {
            t = SetQuad(triangles, t, vMin, vMid, vMin - 1, vMid + xSize - 1);
            for (int x = 1; x < xSize - 1; x++, vMid++)
            {
                t = SetQuad(
                    triangles, t,
                    vMid, vMid + 1, vMid + xSize - 1, vMid + xSize);
            }

            t = SetQuad(triangles, t, vMid, vMax, vMid + xSize - 1, vMax + 1);
        }

        int vTop = vMin - 2;
        t = SetQuad(triangles, t, vMin, vMid, vTop + 1, vTop);
        for (int x = 1; x < xSize - 1; x++, vTop--, vMid++)
        {
            t = SetQuad(triangles, t, vMid, vMid + 1, vTop, vTop - 1);
        }

        t = SetQuad(triangles, t, vMid, vTop - 2, vTop, vTop - 1);

        return t;
    }

    private int CreateBottomFace(int[] triangles, int t, int ring)
    {
        int v = 1;
        int vMid = vertices.Length - (xSize - 1) * (zSize - 1);
        t = SetQuad(triangles, t, ring - 1, vMid, 0, 1);
        for (int x = 1; x < xSize - 1; x++, v++, vMid++)
        {
            t = SetQuad(triangles, t, vMid, vMid + 1, v, v + 1);
        }

        t = SetQuad(triangles, t, vMid, v + 2, v, v + 1);

        int vMin = ring - 2;
        vMid -= xSize - 2;
        int vMax = v + 2;

        for (int z = 1; z < zSize - 1; z++, vMin--, vMid++, vMax++)
        {
            t = SetQuad(triangles, t, vMin, vMid + xSize - 1, vMin + 1, vMid);
            for (int x = 1; x < xSize - 1; x++, vMid++)
            {
                t = SetQuad(
                    triangles, t,
                    vMid + xSize - 1, vMid + xSize, vMid, vMid + 1);
            }

            t = SetQuad(triangles, t, vMid + xSize - 1, vMax + 1, vMid, vMax);
        }

        int vTop = vMin - 1;
        t = SetQuad(triangles, t, vTop + 1, vTop, vTop + 2, vMid);
        for (int x = 1; x < xSize - 1; x++, vTop--, vMid++)
        {
            t = SetQuad(triangles, t, vTop, vTop - 1, vMid, vMid + 1);
        }

        t = SetQuad(triangles, t, vTop, vTop - 1, vMid, vTop - 2);

        return t;
    }

    private static int SetQuad(int[] triangles, int i, int v00, int v10, int v01, int v11)
    {
        triangles[i] = v00;
        triangles[i + 1] = triangles[i + 4] = v01;
        triangles[i + 2] = triangles[i + 3] = v10;
        triangles[i + 5] = v11;
        return i + 6;
    }

    private void OnDrawGizmos()
    {
        if (vertices == null)
        {
            return;
        }
        
        for (int i = 0; i < vertices.Length; i++) 
        {
            Gizmos.color = Color.black;
            Gizmos.DrawSphere(vertices[i], 0.1f);
            Gizmos.color = Color.yellow;
            Gizmos.DrawRay(vertices[i], normals[i]);
        }
    }
}