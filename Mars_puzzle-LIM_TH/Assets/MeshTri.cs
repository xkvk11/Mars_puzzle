using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshTri : MonoBehaviour
{
    private void Start()
    {
        // 게임이 시작될 때와 에디터에서 메쉬를 생성
        CreateTriangleMesh();
    }

    private void CreateTriangleMesh()
    {
        // 이미 생성된 경우 중복 생성 방지
        if (GetComponent<MeshFilter>() != null) return;

        // 삼각형 메쉬를 생성하고 메쉬 필터와 렌더러를 추가합니다.
        MeshFilter meshFilter = gameObject.AddComponent<MeshFilter>();
        MeshRenderer meshRenderer = gameObject.AddComponent<MeshRenderer>();

        // 새로운 메쉬 생성
        Mesh mesh = new Mesh();

        // 삼각형의 비대칭 꼭짓점 좌표 설정
        Vector3[] vertices = new Vector3[]
        {
            new Vector3(0, 0, 0),   // 첫 번째 꼭짓점
            new Vector3(1, 0, 0.2f), // 두 번째 꼭짓점 (비대칭)
            new Vector3(0.5f, 1, 0)  // 세 번째 꼭짓점
        };

        // 삼각형 면 설정
        int[] triangles = new int[]
        {
            0, 1, 2  // 삼각형을 구성하는 인덱스
        };

        // UV 좌표 설정
        Vector2[] uv = new Vector2[]
        {
            new Vector2(0, 0),
            new Vector2(1, 0),
            new Vector2(0.5f, 1)
        };

        // 메쉬에 좌표와 면, UV 정보를 설정
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.uv = uv;

        // 노멀 계산 (조명 반응을 위해)
        mesh.RecalculateNormals();

        // 메쉬 필터에 생성한 메쉬를 할당
        meshFilter.mesh = mesh;

        // 머티리얼 할당 (Standard 셰이더로 먼지 색상 설정)
        if (meshRenderer != null)
        {
            meshRenderer.material = new Material(Shader.Find("Unlit/Color"));
            meshRenderer.material.color = new Color(0.8f, 0.6f, 0.4f, 0.5f); // 먼지 색상
        }

        Debug.Log("비대칭 삼각형 Mesh 생성 완료");
    }

    // 에디터에서 객체가 삭제되거나 초기화될 때 기존 컴포넌트를 정리해줌
    private void OnDestroy()
    {
        if (Application.isEditor)
        {
            DestroyImmediate(GetComponent<MeshFilter>());
            DestroyImmediate(GetComponent<MeshRenderer>());
        }
    }
}
