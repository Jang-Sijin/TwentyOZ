using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class Nomal : MonoBehaviour
{
    public int nowSizeX, nowSizeY; // x, y 개수 설정
    
    private int _prevSizeX, _prevSizeY; // 이전 설정
    private Vector3[] _vertices; // 정점
    private Mesh _mesh; // 메시
    
    
    // [한번에 렌더링하기: 시작]
    // private void Awake () {
    //     Generate();
    // }
    //
    // private void Generate () {
    //     GetComponent<MeshFilter>().mesh = mesh = new Mesh();
    //     mesh.name = "Procedural Grid";
    //
    //     vertices = new Vector3[(xSize + 1) * (ySize + 1)];
    //     for (int i = 0, y = 0; y <= ySize; y++) {
    //         for (int x = 0; x <= xSize; x++, i++) {
    //             vertices[i] = new Vector3(x, y);
    //         }
    //     }
    //     mesh.vertices = vertices;
    //
    //     int[] triangles = new int[xSize * ySize * 6];
    //     for (int ti = 0, vi = 0, y = 0; y < ySize; y++, vi++) {
    //         for (int x = 0; x < xSize; x++, ti += 6, vi++) {
    //             triangles[ti] = vi;
    //             triangles[ti + 3] = triangles[ti + 2] = vi + 1;
    //             triangles[ti + 4] = triangles[ti + 1] = vi + xSize + 1;
    //             triangles[ti + 5] = vi + xSize + 2;
    //         }
    //     }
    //     mesh.triangles = triangles;
    // }
    // [한번에 렌더링하기: 끝]

    // [딜레이를 주면서 렌더링하기: 시작]
    private void Awake()
    {
        StartCoroutine(Generate());
        _prevSizeX = nowSizeX;
        _prevSizeY = nowSizeY;
    }

    private void Update()
    {
        if ((_prevSizeX != nowSizeX || _prevSizeY != nowSizeY))
        {
            _prevSizeX = nowSizeX;
            _prevSizeY = nowSizeY;
            
        
            WaitForSeconds wait = new WaitForSeconds(0.05f); // 0.05 딜레이
        
            GetComponent<MeshFilter>().mesh = _mesh = new Mesh();
            _mesh.name = "Procedural Grid";
        
            _vertices = new Vector3[(nowSizeX + 1) * (nowSizeY + 1)]; // 외부 vertices 설정 값 만큼 배열 초기화
            Vector2[] uv = new Vector2[_vertices.Length]; // uv 좌표 설정
            Vector4[] tangents = new Vector4[_vertices.Length];
            Vector4 tangent = new Vector4(1f, 0f, 0f, -1f); // z값 앞면
            for (int i = 0, y = 0; y <= nowSizeY; y++) 
            {
                for (int x = 0; x <= nowSizeX; x++, i++)
                {
                    _vertices[i] = new Vector3(x, y); // 정점(vertex) 좌표 값 세팅
                    uv[i] = new Vector2((float)x / nowSizeX, (float)y / nowSizeY); // uv 좌표 값 세팅
                    tangents[i] = tangent;

                    print($"위치: {x}, {y}");
                }
            }
            _mesh.vertices = _vertices;
            _mesh.uv = uv;
            _mesh.tangents = tangents;

            int[] triangles = new int[nowSizeX * nowSizeY * 6];
            for (int ti = 0, vi = 0, y = 0; y < nowSizeY; y++, vi++) 
            {
                for (int x = 0; x < nowSizeX; x++, ti += 6, vi++)
                {
                    triangles[ti] = vi;
                    triangles[ti + 3] = triangles[ti + 2] = vi + 1;
                    triangles[ti + 4] = triangles[ti + 1] = vi + nowSizeX + 1;
                    triangles[ti + 5] = vi + nowSizeX + 2;
    
                    _mesh.triangles = triangles;
                    _mesh.RecalculateNormals(); // 메쉬에 노말벡터 설정 // 빛 반사를 위한 용도 // 노말도 같이 생성된다. (몰랐음 나름 중요?) (노말 벡터를 계산해주는 마법의 함수)
                }
            }
        }
    }

    private IEnumerator Generate ()
    {
        WaitForSeconds wait = new WaitForSeconds(0.05f); // 0.05 딜레이
        
        GetComponent<MeshFilter>().mesh = _mesh = new Mesh();
        _mesh.name = "Procedural Grid";
        
        _vertices = new Vector3[(nowSizeX + 1) * (nowSizeY + 1)]; // 외부 vertices 설정 값 만큼 배열 초기화
        Vector2[] uv = new Vector2[_vertices.Length]; // uv 좌표 설정
        Vector4[] tangents = new Vector4[_vertices.Length];
        Vector4 tangent = new Vector4(1f, 0f, 0f, -1f); // z값 앞면
        for (int i = 0, y = 0; y <= nowSizeY; y++) 
        {
            for (int x = 0; x <= nowSizeX; x++, i++)
            {
                _vertices[i] = new Vector3(x, y); // 정점(vertex) 좌표 값 세팅
                uv[i] = new Vector2((float)x / nowSizeX, (float)y / nowSizeY); // uv 좌표 값 세팅
                tangents[i] = tangent;

                print($"위치: {x}, {y}");
            }
        }
        _mesh.vertices = _vertices;
        _mesh.uv = uv;
        _mesh.tangents = tangents;

        int[] triangles = new int[nowSizeX * nowSizeY * 6];
        for (int ti = 0, vi = 0, y = 0; y < nowSizeY; y++, vi++) 
        {
            for (int x = 0; x < nowSizeX; x++, ti += 6, vi++)
            {
                triangles[ti] = vi;
                triangles[ti + 3] = triangles[ti + 2] = vi + 1;
                triangles[ti + 4] = triangles[ti + 1] = vi + nowSizeX + 1;
                triangles[ti + 5] = vi + nowSizeX + 2;
    
                _mesh.triangles = triangles;
                _mesh.RecalculateNormals(); // 메쉬에 노말벡터 설정 // 빛 반사를 위한 용도 // 노말도 같이 생성된다. (몰랐음 나름 중요?) (노말 벡터를 계산해주는 마법의 함수)

                yield return wait;
            }
        }
        
        print($"{_mesh.normals[0]}");
    }
    // [딜레이를 주면서 렌더링하기: 끝]

    private void OnDrawGizmos () // 정점 렌더? 
    {
        // 정점이 없으면 리턴
        if (_vertices == null) 
        {
            return;
        }
        
        // 기즈모 색상은 검정색
        Gizmos.color = Color.black;
        for (int i = 0; i < _vertices.Length; i++) // 생선된 점정 개수만큼 반복
        {
            // 배열에 저장되어 있는 정점 위치에 반지름 길이가 0.1 구를 그린다.
            Gizmos.DrawSphere(_vertices[i], 0.1f);
        }
    }
}