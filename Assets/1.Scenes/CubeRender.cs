using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// [참고]
// https://uemonwe.tistory.com/4
public class CubeRender : MonoBehaviour
{
    public int x = 2, y = 2, z = 2;
    private int _prevX, _prevY, _prevZ;
    
    private Vector3[] _vertices;
    private Mesh _mesh;
    
    private void Awake () 
    {
        _prevX = x;
        _prevY = y;
        _prevZ = z;
        
        StartCoroutine(Generate());
    }

    private IEnumerator Generate()
    {
        WaitForSeconds wait = new WaitForSeconds(0.5f); // 딜레이
        
        GetComponent<MeshFilter>().mesh = _mesh = new Mesh();

        _vertices = new Vector3[]
        {
            new Vector3(-x, +y, +z), // 카메라 기준 가장 뒷면의 좌상단 // 3
            new Vector3(+x, +y, -z), // 카메라 기준 가장 앞면의 우상단 // 1
            new Vector3(-x, +y, -z), // 카메라 기준 가장 앞면의 좌상단 // 0
            
            new Vector3(+x, +y, +z), // 카메라 기준 가장 뒷면의 좌상단 // 2
            new Vector3(+x, +y, -z), // 카메라 기준 가장 앞면의 우상단 // 1
            new Vector3(-x, +y, +z), // 카메라 기준 가장 뒷면의 우상단 // 3
            
            
            new Vector3(+x, -y, +z), // 카메라 기준 가장 뒷면의 우하단 // 6
            new Vector3(-x, -y, -z), // 카메라 기준 가장 앞면의 좌하단 // 4
            new Vector3(+x, -y, -z), // 카메라 기준 가장 앞면의 우하단 // 5
            
            new Vector3(-x, -y, +z), // 카메라 기준 가장 뒷면의 좌하단 // 7
            new Vector3(-x, -y, -z), // 카메라 기준 가장 앞면의 좌하단 // 4
            new Vector3(+x, -y, +z), // 카메라 기준 가장 뒷면의 우하단 // 6
            
            
            new Vector3(-x, +y, -z), // 카메라 기준 가장 앞면의 좌상단 // 0
            new Vector3(+x, -y, -z), // 카메라 기준 가장 앞면의 우하단 // 5
            new Vector3(-x, -y, -z), // 카메라 기준 가장 앞면의 좌하단 // 4
            
            new Vector3(+x, +y, -z), // 카메라 기준 가장 앞면의 우상단 // 1
            new Vector3(+x, -y, -z), // 카메라 기준 가장 앞면의 우하단 // 5
            new Vector3(-x, +y, -z), // 카메라 기준 가장 앞면의 좌상단 // 0
                        
            
            new Vector3(+x, +y, +z), // 카메라 기준 가장 뒷면의 좌상단 // 2
            new Vector3(-x, -y, +z), // 카메라 기준 가장 뒷면의 좌하단 // 7
            new Vector3(+x, -y, +z), // 카메라 기준 가장 뒷면의 우하단 // 6
            
            new Vector3(-x, +y, +z), // 카메라 기준 가장 뒷면의 우상단 // 3
            new Vector3(-x, -y, +z), // 카메라 기준 가장 뒷면의 좌하단 // 7
            new Vector3(+x, +y, +z), // 카메라 기준 가장 뒷면의 좌상단 // 2
            
            
            new Vector3(-x, +y, +z), // 카메라 기준 가장 뒷면의 우상단 // 3
            new Vector3(-x, -y, -z), // 카메라 기준 가장 앞면의 좌하단 // 4
            new Vector3(-x, -y, +z), // 카메라 기준 가장 뒷면의 좌하단 // 7
            
            new Vector3(-x, +y, -z), // 카메라 기준 가장 앞면의 좌상단 // 0
            new Vector3(-x, -y, -z), // 카메라 기준 가장 앞면의 좌하단 // 4
            new Vector3(-x, +y, +z), // 카메라 기준 가장 뒷면의 우상단 // 3
            
            
            new Vector3(+x, +y, -z), // 카메라 기준 가장 앞면의 우상단 // 1
            new Vector3(+x, -y, +z), // 카메라 기준 가장 뒷면의 우하단 // 6
            new Vector3(+x, -y, -z), // 카메라 기준 가장 앞면의 우하단 // 5
            
            new Vector3(+x, +y, +z), // 카메라 기준 가장 뒷면의 좌상단 // 2
            new Vector3(+x, -y, +z), // 카메라 기준 가장 뒷면의 우하단 // 6
            new Vector3(+x, +y, -z), // 카메라 기준 가장 앞면의 우상단 // 1
        };
        _mesh.vertices = _vertices;

        Vector2[] uvs = new Vector2[_vertices.Length];
        for(int i = 0; i < uvs.Length; i+=6)
        {
            for (int j = 0; j <= i / 6; j++)
            {
                if (j < 2)
                {
                    uvs[i] = new Vector2(0f, 1f * z); // uv 좌상단
                    uvs[i + 1] = new Vector2(1f * x, 0f); // uv 우하단
                    uvs[i + 2] = new Vector2(0f, 0f); // uv 좌하단

                    uvs[i + 3] = new Vector2(1f * x, 1f * z); // uv 우상단
                    uvs[i + 4] = new Vector2(1f * x, 0f); // uv 우하단
                    uvs[i + 5] = new Vector2(0f, 1f * z); // uv 좌상단
                }
                else if (j < 4)
                {
                    uvs[i] = new Vector2(0f, 1f * y); // uv 좌상단
                    uvs[i + 1] = new Vector2(1f * x, 0f); // uv 우하단
                    uvs[i + 2] = new Vector2(0f, 0f); // uv 좌하단

                    uvs[i + 3] = new Vector2(1f * x, 1f * y); // uv 우상단
                    uvs[i + 4] = new Vector2(1f * x, 0f); // uv 우하단
                    uvs[i + 5] = new Vector2(0f, 1f * y); // uv 좌상단
                }
                else if (j < 6)
                {
                    uvs[i] = new Vector2(0f, 1f * y); // uv 좌상단
                    uvs[i + 1] = new Vector2(1f * z, 0f); // uv 우하단
                    uvs[i + 2] = new Vector2(0f, 0f); // uv 좌하단

                    uvs[i + 3] = new Vector2(1f * z, 1f * y); // uv 우상단
                    uvs[i + 4] = new Vector2(1f * z, 0f); // uv 우하단
                    uvs[i + 5] = new Vector2(0f, 1f * y); // uv 좌상단
                }
            }
        }; 
        _mesh.uv = uvs;

        int[] triangles = new int[_vertices.Length];
        for (int i = 0; i < triangles.Length; i++)
        {
            triangles[i] = i;
        }
        _mesh.triangles = triangles;

        int[] trianglesLoop = new int[triangles.Length];
        for (int i = 0; i < trianglesLoop.Length; i+=3)
        {
            for (int j = i; j < i + 3; j++)
            {
                trianglesLoop[j] = triangles[j];
                _mesh.triangles = trianglesLoop;
                _mesh.RecalculateBounds(); 
                _mesh.RecalculateNormals(); // 메쉬에 노말벡터 설정 // 빛 반사를 위한 용도

                yield return wait;
            }
        }
    }

    private void Update()
    {
        if (_prevX == x && _prevY == y && _prevZ == z)
        {
            return;
        }

        Debug.Log($"Debug: 장시진, update{x},{y},{z}");
        _prevX = x;
        _prevY = y;
        _prevZ = z;
        
        GetComponent<MeshFilter>().mesh = _mesh = new Mesh();

        _vertices = new Vector3[]
        {
            new Vector3(-x, +y, +z), // 카메라 기준 가장 뒷면의 좌상단 // 3
            new Vector3(+x, +y, -z), // 카메라 기준 가장 앞면의 우상단 // 1
            new Vector3(-x, +y, -z), // 카메라 기준 가장 앞면의 좌상단 // 0
            
            new Vector3(+x, +y, +z), // 카메라 기준 가장 뒷면의 좌상단 // 2
            new Vector3(+x, +y, -z), // 카메라 기준 가장 앞면의 우상단 // 1
            new Vector3(-x, +y, +z), // 카메라 기준 가장 뒷면의 우상단 // 3
            
            
            new Vector3(+x, -y, +z), // 카메라 기준 가장 뒷면의 우하단 // 6
            new Vector3(-x, -y, -z), // 카메라 기준 가장 앞면의 좌하단 // 4
            new Vector3(+x, -y, -z), // 카메라 기준 가장 앞면의 우하단 // 5
            
            new Vector3(-x, -y, +z), // 카메라 기준 가장 뒷면의 좌하단 // 7
            new Vector3(-x, -y, -z), // 카메라 기준 가장 앞면의 좌하단 // 4
            new Vector3(+x, -y, +z), // 카메라 기준 가장 뒷면의 우하단 // 6
            
            
            new Vector3(-x, +y, -z), // 카메라 기준 가장 앞면의 좌상단 // 0
            new Vector3(+x, -y, -z), // 카메라 기준 가장 앞면의 우하단 // 5
            new Vector3(-x, -y, -z), // 카메라 기준 가장 앞면의 좌하단 // 4
            
            new Vector3(+x, +y, -z), // 카메라 기준 가장 앞면의 우상단 // 1
            new Vector3(+x, -y, -z), // 카메라 기준 가장 앞면의 우하단 // 5
            new Vector3(-x, +y, -z), // 카메라 기준 가장 앞면의 좌상단 // 0
            
            
            new Vector3(-x, +y, +z), // 카메라 기준 가장 뒷면의 우상단 // 3
            new Vector3(-x, -y, -z), // 카메라 기준 가장 앞면의 좌하단 // 4
            new Vector3(-x, -y, +z), // 카메라 기준 가장 뒷면의 좌하단 // 7
            
            new Vector3(-x, +y, -z), // 카메라 기준 가장 앞면의 좌상단 // 0
            new Vector3(-x, -y, -z), // 카메라 기준 가장 앞면의 좌하단 // 4
            new Vector3(-x, +y, +z), // 카메라 기준 가장 뒷면의 우상단 // 3
            
            
            new Vector3(+x, +y, -z), // 카메라 기준 가장 앞면의 우상단 // 1
            new Vector3(+x, -y, +z), // 카메라 기준 가장 뒷면의 우하단 // 6
            new Vector3(+x, -y, -z), // 카메라 기준 가장 앞면의 우하단 // 5
            
            new Vector3(+x, +y, +z), // 카메라 기준 가장 뒷면의 좌상단 // 2
            new Vector3(+x, -y, +z), // 카메라 기준 가장 뒷면의 우하단 // 6
            new Vector3(+x, +y, -z), // 카메라 기준 가장 앞면의 우상단 // 1
            
            
            new Vector3(+x, +y, +z), // 카메라 기준 가장 뒷면의 좌상단 // 2
            new Vector3(-x, -y, +z), // 카메라 기준 가장 뒷면의 좌하단 // 7
            new Vector3(+x, -y, +z), // 카메라 기준 가장 뒷면의 우하단 // 6
            
            new Vector3(-x, +y, +z), // 카메라 기준 가장 뒷면의 우상단 // 3
            new Vector3(-x, -y, +z), // 카메라 기준 가장 뒷면의 좌하단 // 7
            new Vector3(+x, +y, +z), // 카메라 기준 가장 뒷면의 좌상단 // 2
        };
        _mesh.vertices = _vertices;

        Vector2[] uvs = new Vector2[_vertices.Length];
        for(int i = 0; i < uvs.Length; i+=6)
        {
            for (int j = 0; j <= i / 6; j++)
            {
                if (j < 2)
                {
                    uvs[i] = new Vector2(0f, 1f * z); // uv 좌상단
                    uvs[i + 1] = new Vector2(1f * x, 0f); // uv 우하단
                    uvs[i + 2] = new Vector2(0f, 0f); // uv 좌하단

                    uvs[i + 3] = new Vector2(1f * x, 1f * z); // uv 우상단
                    uvs[i + 4] = new Vector2(1f * x, 0f); // uv 우하단
                    uvs[i + 5] = new Vector2(0f, 1f * z); // uv 좌상단
                }
                else if (j < 4)
                {
                    uvs[i] = new Vector2(0f, 1f * y); // uv 좌상단
                    uvs[i + 1] = new Vector2(1f * x, 0f); // uv 우하단
                    uvs[i + 2] = new Vector2(0f, 0f); // uv 좌하단

                    uvs[i + 3] = new Vector2(1f * x, 1f * y); // uv 우상단
                    uvs[i + 4] = new Vector2(1f * x, 0f); // uv 우하단
                    uvs[i + 5] = new Vector2(0f, 1f * y); // uv 좌상단
                }
                else if (j < 6)
                {
                    uvs[i] = new Vector2(0f, 1f * y); // uv 좌상단
                    uvs[i + 1] = new Vector2(1f * z, 0f); // uv 우하단
                    uvs[i + 2] = new Vector2(0f, 0f); // uv 좌하단

                    uvs[i + 3] = new Vector2(1f * z, 1f * y); // uv 우상단
                    uvs[i + 4] = new Vector2(1f * z, 0f); // uv 우하단
                    uvs[i + 5] = new Vector2(0f, 1f * y); // uv 좌상단
                }
            }
        }; 
        _mesh.uv = uvs;

        int[] triangles = new int[_vertices.Length];
        for (int i = 0; i < triangles.Length; i++)
        {
            triangles[i] = i;
        }
        _mesh.triangles = triangles;

        int[] trianglesLoop = new int[triangles.Length];
        for (int i = 0; i < trianglesLoop.Length; i+=3)
        {
            for (int j = i; j < i + 3; j++)
            {
                trianglesLoop[j] = triangles[j];
                _mesh.triangles = trianglesLoop;
                _mesh.RecalculateBounds(); // 
                _mesh.RecalculateNormals(); // 메쉬에 노말벡터 설정 // 빛 반사를 위한 용도
            }
        }
    }

    private void OnDrawGizmos () // 정점 렌더링 
    {
        // 정점이 없으면 리턴
        if (_vertices == null) 
        {
            return;
        }
        
        // 기즈모 색상은 녹색
        Gizmos.color = Color.green;
        for (int i = 0; i < _vertices.Length; i++) // 생선된 점정 개수만큼 반복
        {
            // 배열에 저장되어 있는 정점 위치에 반지름 길이가 0.2 구를 그린다.
            Gizmos.DrawSphere(_vertices[i], 0.2f);
        }
    }
}
