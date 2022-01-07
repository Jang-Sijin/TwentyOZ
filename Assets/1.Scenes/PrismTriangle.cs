using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// [참고]
// https://uemonwe.tistory.com/4
public class PrismTriangle : MonoBehaviour
{
    public int v = 3, r = 2, h = 4;
    private int _prevV, _prevR, _prevH;

    private float _sizeX, _sizeY, _sizeZ, _sizeCenter;
    
    private Vector3[] _vertices;
    private Mesh _mesh;
    
    private void Awake ()
    {
        _prevV = v;
        _prevR = r;
        _prevH = h;

        // x, y, z, center 최대 값 설정
        _sizeX = r;
        _sizeY = h;
        _sizeZ = MathF.Sqrt(MathF.Pow(r, 2) - MathF.Pow(0.5f * r, 2));
        _sizeCenter = MathF.Sqrt(MathF.Pow(r, 2) - MathF.Pow(0.5f * r, 2)) * (1f / 3f); 

        StartCoroutine(Generate());
    }

    private IEnumerator Generate()
    {
        WaitForSeconds wait = new WaitForSeconds(0.5f); // 딜레이
        
        GetComponent<MeshFilter>().mesh = _mesh = new Mesh();
        _vertices = new Vector3[]
        {
            // 밑면
            new Vector3(0f, 0f, 0f), // 0
            new Vector3(_sizeX, 0f, 0f), // 2
            new Vector3(_sizeX * 0.5f, 0f, _sizeZ), // 1
            
            
            // 윗면
            new Vector3(0f, h, 0f), // 3
            new Vector3(_sizeX * 0.5f, h, _sizeZ), // 4
            new Vector3(_sizeX, h, 0f), // 5
            
            
            // 옆면 1
            new Vector3(0f, 0f, 0f), // 0
            new Vector3(0f, h, 0f), // 3
            new Vector3(_sizeX, 0f, 0f), // 2
            
            new Vector3(_sizeX, 0f, 0f), // 2
            new Vector3(0f, h, 0f), // 3
            new Vector3(_sizeX, h, 0f), // 5
            
            
            // 옆면 2
            new Vector3(_sizeX, 0f, 0f), // 2
            new Vector3(_sizeX, h, 0f), // 5
            new Vector3(_sizeX * 0.5f, 0f, _sizeZ), // 1
            
            new Vector3(_sizeX * 0.5f, 0f, _sizeZ), // 1
            new Vector3(_sizeX, h, 0f), // 5
            new Vector3(_sizeX * 0.5f, h, _sizeZ), // 4
            
            
            // 옆면 2
            new Vector3(_sizeX * 0.5f, 0f, _sizeZ), // 1
            new Vector3(_sizeX * 0.5f, h, _sizeZ), // 4
            new Vector3(0f, 0f, 0f), // 0
            
            new Vector3(0f, 0f, 0f), // 0
            new Vector3(_sizeX * 0.5f, h, _sizeZ), // 4
            new Vector3(0f, h, 0f), // 3
        };
        _mesh.vertices = _vertices;

        Vector2[] uvs = new Vector2[_vertices.Length];
        for (int i = 0; i < uvs.Length;)
        {
            if (i < 6)
            {
                uvs[i] = new Vector2(0f, 0f); // uv 좌상단
                uvs[i + 1] = new Vector2(0.5f * r, 0.5f * r * 2.0f); // uv 우하단
                uvs[i + 2] = new Vector2(1f * r, 0f); // uv 좌하단

                i += 3;
            }
            else
            {
                uvs[i] = new Vector2(0f, 0f); // uv 좌상단
                uvs[i + 1] = new Vector2(0f, 1f * h); // uv 우하단
                uvs[i + 2] = new Vector2(1f * r, 0f); // uv 좌하단
                
                uvs[i + 3] = new Vector2(1f * r, 0); // uv 좌상단
                uvs[i + 4] = new Vector2(0f, 1f * h); // uv 우하단
                uvs[i + 5] = new Vector2(1f * r, 1f * h); // uv 좌하단
                
                i += 6;
            }
        }
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
